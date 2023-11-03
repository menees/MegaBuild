namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	internal sealed class OutputQueue : IDisposable
	{
		#region Private Data Members

		// Don't use \t because the Text Object Model (in OutputWindow) uses huge tab stops, so indents don't line up with tab stops.
		private const string TimestampPrefixSeparator = "    ";

		// https://jkorpela.fi/chars/spaces.html
		private const char FigureSpace = '\u2007';
		private const char PunctuationSpace = '\u2008';

		// The article below says 100ms feels like direct manipulation, and 1000ms
		// is still a short enough delay for users to stay focused on a continuous flow.
		// So we'll go with a quarter of a second to lean toward responsiveness
		// without firing too many window updates under load.
		// https://medium.com/@slhenty/ui-response-times-acec744f3157
		private const int UpdateMilliseconds = 250;

		private readonly OutputWindow outputWindow;
		private readonly AnsiCodeHandler ansiCodeHandler;
		private readonly ConcurrentQueue<(OutputAddedEventArgs Args, DateTimeOffset Added)> queue;
		private readonly Timer timer;

		private string? previousTimestamp;
		private OutputAddedEventArgs? previousArgs;

		#endregion

		#region Constructors

		public OutputQueue(OutputWindow outputWindow, AnsiCodeHandler ansiCodeHandler)
		{
			this.outputWindow = outputWindow;
			this.ansiCodeHandler = ansiCodeHandler;
			this.queue = new();

			// Use a WM_TIMER message because Windows will combine them so only the last one is sent.
			// https://web.archive.org/web/20130627005845/http://support.microsoft.com/kb/96006
			// https://stackoverflow.com/a/47934541/1882616
			this.timer = new() { Interval = UpdateMilliseconds };
			this.timer.Tick += this.Timer_Tick;
		}

		#endregion

		#region Public Methods

		public static string RemoveTimestampPrefix(string line)
		{
			string result = line;

			if (!string.IsNullOrWhiteSpace(result) && Options.UseTimestampPrefix)
			{
				result = result.Trim();
				int tabIndex = result.IndexOf(TimestampPrefixSeparator);
				if (tabIndex > 0 && (tabIndex + 1) < result.Length)
				{
					string prefix = result.Substring(0, tabIndex);
					if (DateTime.TryParseExact(
						prefix,
						Options.TimestampFormat,
						CultureInfo.CurrentCulture,
						DateTimeStyles.NoCurrentDateDefault,
						out _))
					{
						result = result.Substring(tabIndex + 1);
					}
				}
			}

			return result;
		}

		public static string UseStandardSpaces(string text)
			=> text.Replace(FigureSpace, ' ').Replace(PunctuationSpace, ' ');

		// This method will typically be invoked from a non-UI worker thread, so it can't render immediately.
		public void Add(OutputAddedEventArgs e)
		{
			this.queue.Enqueue((e, DateTimeOffset.Now));
			this.EnableTimer(true);
		}

		public void Clear()
		{
			this.EnableTimer(false);
			this.previousTimestamp = null;
			this.previousArgs = null;

			// .NET 4.8 doesn't implement ConcurrentQueue<T>.Clear().
			while (this.queue.TryDequeue(out _))
			{
			}
		}

		public void Dispose()
		{
			this.timer.Dispose();
		}

		#endregion

		#region Private Methods

		private static string BuildInvisibleTimestamp()
		{
			string invisibleTimestamp;
			StringBuilder sb = new(Options.TimestampFormat.Length);
			foreach (char ch in Options.TimestampFormat)
			{
				// Use special space characters to try to make the invisible timestamp
				// take the same horizontal space as a visible timestamp.
				if (char.IsLetterOrDigit(ch))
				{
					sb.Append(FigureSpace);
				}
				else if (char.IsPunctuation(ch))
				{
					sb.Append(PunctuationSpace);
				}
				else
				{
					sb.Append(' ');
				}
			}

			sb.Append(TimestampPrefixSeparator);
			invisibleTimestamp = sb.ToString();
			return invisibleTimestamp;
		}

		private void EnableTimer(bool enable)
		{
			// Don't force a PostMessage unless we know a change is required.
			if (this.timer.Enabled != enable)
			{
				if (this.outputWindow.InvokeRequired)
				{
					// The Timer control creates a hidden window, so it can only be started and stopped on the UI thread.
					this.outputWindow.BeginInvoke(EnableTimer, new object[] { enable });
				}
				else
				{
					this.timer.Enabled = enable;
				}
			}
		}

		// This handler is always invoked on the UI thread.
		private void Timer_Tick(object? sender, EventArgs ea)
		{
			this.EnableTimer(false);

			if (!this.queue.IsEmpty)
			{
				List<OutputAddedEventArgs> segments = this.GetOutputSegments();

				List<OutputAddedEventArgs> formatEqual = new(segments.Count);
				OutputAddedEventArgs? previousSegment = null;
				foreach (OutputAddedEventArgs segment in segments)
				{
					// This has to "split" at differences instead of using "group by" because
					// segments can't move past others in the output order.
					if (!segment.IsFormatEqual(previousSegment))
					{
						this.CoalesceAndAppendSegments(formatEqual);
						formatEqual.Clear();
					}

					formatEqual.Add(segment);
					previousSegment = segment;
				}

				this.CoalesceAndAppendSegments(formatEqual);
			}
		}

		private List<OutputAddedEventArgs> GetOutputSegments()
		{
			List<OutputAddedEventArgs> segments = new();

			bool useTimestamps = Options.UseTimestampPrefix;
			string? invisibleTimestamp = null;

			while (this.queue.TryDequeue(out (OutputAddedEventArgs Args, DateTimeOffset Added) output))
			{
				OutputAddedEventArgs args = output.Args;
				if (useTimestamps)
				{
					string timestamp = output.Added.ToString(Options.TimestampFormat) + TimestampPrefixSeparator;

					// Track when the timestamp (or format) changes so we can show a blank timestamp whenever it would be duplicated.
					// This makes a big rendering performance difference because we can show the blanks using the same color as the
					// segment after it, which allows us to concatenate and append big strings all at once to the OutputWindow.
					// That's much more efficient than adding little strings that change color twice per line (timestamp and text).
					if (timestamp == this.previousTimestamp && args.IsFormatEqual(this.previousArgs))
					{
						invisibleTimestamp ??= BuildInvisibleTimestamp();

						// We never want to highlight invisible whitespace.
						segments.Add(new(invisibleTimestamp, args.Indent, args.Color, false, Guid.Empty));
					}
					else
					{
						segments.Add(new(timestamp, args.Indent, Color.LightSteelBlue));
					}

					// We have to store the previous values in member variables to avoid showing duplicate timestamps
					// across Timer_Tick events. For example, if we only kept local variables for the previous values, then
					// output like the following could (and did) occur (because line 650 came in on a different Tick event).
					// 		13:48:51.886	This is long output line number 648.
					// 							This is long output line number 649.
					// 		13:48:51.886	This is long output line number 650.
					this.previousTimestamp = timestamp;
					this.previousArgs = args;
				}

				// Only highlight the first segment (if any). We don't want every color change to be a separately highlighted region.
				// Also, only use the unique output Guid (if any) at the beginning of the output.
				bool highlight = args.Highlight;
				Guid outputId = args.OutputId;
				foreach ((string text, Color color) in this.ansiCodeHandler.Split(args.Message, args.Color, () => SystemColors.Window))
				{
					segments.Add(new(text, args.Indent, color, highlight, outputId));
					highlight = false;
					outputId = Guid.Empty;
				}
			}

			return segments;
		}

		private void CoalesceAndAppendSegments(IReadOnlyList<OutputAddedEventArgs> segments)
		{
			if (segments.Count > 0)
			{
				// Coalesce similarly formatted items so we can concatenate their text messages for more efficient Appends.
				string message = string.Concat(segments.Select(g => g.Message));
				OutputAddedEventArgs first = segments[0];
				this.outputWindow.Append(message, first.Color, first.Indent, first.Highlight, first.OutputId);
			}
		}

		#endregion
	}
}

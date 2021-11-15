namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Drawing;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using Menees.Windows.Forms;

	#endregion

	/// <summary>
	/// Processes ANSI escape codes for foreground colors.
	/// </summary>
	/// <remarks>
	/// This only handles a subset of the ANSI escape codes for colors since our <see cref="OutputWindow"/>
	/// only supports setting the foreground color and not background color or font effects.
	/// <para/>
	/// Some good references are:
	/// <list type="bullet">
	/// <item>https://gist.github.com/fnky/458719343aabd01cfb17a3a4f7296797#colors--graphics-mode</item>
	/// <item>https://en.wikipedia.org/wiki/ANSI_escape_code#CSI_(Control_Sequence_Introducer)_sequences</item>
	/// </list>
	/// </remarks>
	internal sealed class AnsiCodeHandler
	{
		#region Private Data Members

		private const double DimBrightTolerance = 0.1;

		// According to https://en.wikipedia.org/wiki/ANSI_escape_code#CSI_(Control_Sequence_Introducer)_sequences, all args are optional.
		private static readonly Regex ColorExpression = new(@"(?n)\e\[(?<args>\d*(;\d*)*)m", RegexOptions.Compiled | RegexOptions.CultureInvariant);

		private Mode mode;
		private Color? foregroundColor;

		#endregion

		#region Private Enums

		private enum Mode
		{
			Normal,
			Bright,
			Dim,
		}

		#endregion

		#region Public Methods

		public void Reset()
		{
			this.mode = default;
			this.foregroundColor = null;
		}

		public IEnumerable<Tuple<string, Color>> Split(string text, Color defaultColor, Func<Color> getCurrentBackground)
		{
			// Most text won't contain ANSI escape codes, so try to short circuit and return quickly.
			MatchCollection matches;
			if (string.IsNullOrWhiteSpace(text) || !text.Contains('\u001B') || (matches = ColorExpression.Matches(text)).Count == 0)
			{
				yield return new Tuple<string, Color>(text, this.foregroundColor ?? defaultColor);
			}
			else
			{
				int startIndex = 0;
				foreach (Match match in matches)
				{
					if (match.Index > startIndex)
					{
						yield return new Tuple<string, Color>(text.Substring(startIndex, match.Index - startIndex), this.foregroundColor ?? defaultColor);
					}

					// According to https://en.wikipedia.org/wiki/ANSI_escape_code#CSI_(Control_Sequence_Introducer)_sequences,
					// missing args should be treated as 0 (e.g., in \e[1;;3m the middle arg is 0). Also, no arg (i.e., \e[m) should be treated as 0.
					// For simplicity, we'll treat any unparsable arg as 0 (e.g., 999 => 0).
					IEnumerable<byte> args = match.Groups[1].Value.Split(';').Select(arg => byte.TryParse(arg, out byte value) ? value : byte.MinValue);
					foreach (byte arg in args)
					{
						// If we encounter an unsupported arg, we'll quit processing this escape sequence
						// (e.g., for background colors, 256 color ids, 24-bit color components, or other font effects).
						if (!this.TryProcessArg(arg, defaultColor, getCurrentBackground))
						{
							break;
						}
					}

					startIndex = match.Index + match.Length;
				}

				if (startIndex < text.Length)
				{
					yield return new Tuple<string, Color>(text.Substring(startIndex), this.foregroundColor ?? defaultColor);
				}
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Processes a single color code argument.
		/// </summary>
		/// <remarks>
		/// This supported subset of codes are documented at the following references:
		/// https://gist.github.com/fnky/458719343aabd01cfb17a3a4f7296797#colors--graphics-mode
		/// https://en.wikipedia.org/wiki/ANSI_escape_code#SGR_(Select_Graphic_Rendition)_parameters
		/// <para/>
		/// A good .NET color chart is at https://sites.google.com/site/cdeveloperresources/
		/// and https://www.codeproject.com/Articles/34328/A-Hue-Brightness-Color-Wheel-Style-Chart-for-Syste
		/// </remarks>
		[SuppressMessage("Design", "MEN010:Avoid magic numbers", Justification = "ANSI SGR codes don't have standard names, just effects.")]
		private bool TryProcessArg(byte arg, Color defaultColor, Func<Color> getCurrentBackground)
		{
			bool result = true;

			Color? requestedColor = null;
			switch (arg)
			{
				// --- Mode Codes ---
				case 0: // Reset all modes (styles and colors)
					this.Reset();
					this.foregroundColor = defaultColor;
					break;
				case 1:
					// If the background is already bright (e.g., White), then we'll ignore requests for brighter colors and treat them as normal.
					double backgroundBrightness = (getCurrentBackground?.Invoke())?.GetBrightness() ?? 0;
					this.mode = backgroundBrightness < (1.0 - DimBrightTolerance) ? Mode.Bright : Mode.Normal;
					break;
				case 2:
					// If the background is already dim (e.g., Black), then we'll ignore requests for dimmer colors and treat them as normal.
					backgroundBrightness = (getCurrentBackground?.Invoke())?.GetBrightness() ?? 1;
					this.mode = backgroundBrightness > DimBrightTolerance ? Mode.Dim : Mode.Normal;
					break;
				case 22:
					this.mode = Mode.Normal;
					break;

				// --- Normal Color Codes ---
				case >= 30 and <= 37:
					requestedColor = AnsiColor.GetColor(arg, this.mode);
					break;

				case 39: // Reset foreground color only (ignore mode)
					this.foregroundColor = defaultColor;
					break;

				// --- Bright Color Codes (Non-standard; from aixterm) ---
				case >= 90 and <= 97:
					requestedColor = AnsiColor.GetColor(arg, Mode.Bright);
					break;

				default:
					result = false;
					break;
			}

			if (requestedColor != null)
			{
				this.TryColor(requestedColor.Value, getCurrentBackground);
			}

			return result;
		}

		private void TryColor(Color requestedColor, Func<Color> getCurrentBackground)
		{
			// If the requestedColor matches the current background color in ARGB value, then make it darker or lighter.
			// We only compare by ARGB values because .NET Color values will be considered non-equal if a named color
			// like White is compared to a system color like Window even if their ARGB values are the same.
			Color? background = getCurrentBackground?.Invoke();
			if (background != null && requestedColor.ToArgb() == background.Value.ToArgb())
			{
				double backgroundBrightness = background.Value.GetBrightness();
				const double BrightnessMidPoint = 0.5;
				if (backgroundBrightness >= BrightnessMidPoint)
				{
					requestedColor = ControlPaint.Dark(requestedColor, (float)DimBrightTolerance);
				}
				else
				{
					requestedColor = ControlPaint.Light(requestedColor, (float)DimBrightTolerance);
				}
			}

			this.foregroundColor = requestedColor;
		}

		#endregion

		#region Private Types

		private struct AnsiColor
		{
			#region Private Data Members

			private const int BrightBlackLevel = 64;

			#endregion

			#region Constructors

			public AnsiColor(Color normal, Color? bright = null, Color? dim = null)
			{
				this.Normal = normal;
				this.Bright = bright ?? normal;
				this.Dim = dim ?? normal;
			}

			#endregion

			#region Public Properties

			public static AnsiColor Black { get; } = new AnsiColor(Color.Black, bright: Color.FromArgb(BrightBlackLevel, BrightBlackLevel, BrightBlackLevel));

			public static AnsiColor Red { get; } = new AnsiColor(Color.Red, dim: Color.DarkRed);

			public static AnsiColor Green { get; } = new AnsiColor(Color.Green, bright: Color.Lime, dim: Color.DarkGreen);

			public static AnsiColor Yellow { get; } = new AnsiColor(Color.Gold, bright: Color.Yellow, dim: Color.DarkGoldenrod);

			public static AnsiColor Blue { get; } = new AnsiColor(Color.Blue, dim: Color.DarkBlue);

			public static AnsiColor Magenta { get; } = new AnsiColor(Color.Magenta, dim: Color.DarkMagenta);

			public static AnsiColor Cyan { get; } = new AnsiColor(Color.Cyan, dim: Color.DarkCyan);

			public static AnsiColor White { get; } = new AnsiColor(Color.White, dim: Color.Gainsboro);

			public Color Normal { get; }

			public Color Bright { get; }

			public Color Dim { get; }

			#endregion

			#region Public Methods

			[SuppressMessage("Design", "MEN010:Avoid magic numbers", Justification = "ANSI SGR codes don't have standard names, just effects.")]
			public static Color GetColor(byte code, Mode mode)
			{
				AnsiColor ansiColor;

				const byte LeastSignificantDigitModulus = 10;
				switch (code % LeastSignificantDigitModulus)
				{
					case 0:
						ansiColor = Black;
						break;
					case 1:
						ansiColor = Red;
						break;
					case 2:
						ansiColor = Green;
						break;
					case 3:
						ansiColor = Yellow;
						break;
					case 4:
						ansiColor = Blue;
						break;
					case 5:
						ansiColor = Magenta;
						break;
					case 6:
						ansiColor = Cyan;
						break;
					case 7:
						ansiColor = White;
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(code), code, "Unsupported color code.");
				}

				Color result = ansiColor.GetColor(mode);
				return result;
			}

			public Color GetColor(Mode mode)
				=> mode switch
				{
					Mode.Bright => this.Bright,
					Mode.Dim => this.Dim,
					_ => this.Normal,
				};

			#endregion
		}

		#endregion
	}
}

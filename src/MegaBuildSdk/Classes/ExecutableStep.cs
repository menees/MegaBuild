namespace MegaBuild;

#region Using Directives

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Menees;

#endregion

public abstract class ExecutableStep : Step
{
	#region Private Data Members

	private static readonly List<Regex> ErrorRegexes = ParseRegexes(Properties.Settings.Default.ExecutableStep_ErrorRegexs);
	private static readonly List<Regex> WarningRegexes = ParseRegexes(Properties.Settings.Default.ExecutableStep_WarningRegexs);
	private static readonly ConcurrentExclusiveSchedulerPair Schedulers = new();

	private readonly ExecSupports supportFlags;
	private bool autoColorErrorsAndWarnings = true;
	private bool ignoreFailure;
	private bool inCurrentBuild;
	private bool isAdministratorRequired;
	private bool onlyIfParentSucceeded = true;
	private bool promptFirst;
	private bool timeout;
	private bool waitForCompletion = true;
	private StepStatus status = StepStatus.None;
	private int timeoutMinutes = 10;
	private TimeSpan totalTime;
	private List<(OutputStyle Style, Regex Pattern)>? customOutputStyles;
	private Encoding standardStreamEncoding;

	#endregion

	#region Constructors

	protected ExecutableStep(Project project, StepCategory category, StepTypeInfo info)
		: this(project, category, info, ExecSupports.All)
	{
	}

	protected ExecutableStep(Project project, StepCategory category, StepTypeInfo info, ExecSupports supports)
		: base(project, category, info)
	{
		this.supportFlags = supports;
		this.standardStreamEncoding = Encoding.UTF8;
	}

	#endregion

	#region Public Properties

	public bool AutoColorErrorsAndWarnings
	{
		get => this.SupportsAutoColorErrorsAndWarnings && this.autoColorErrorsAndWarnings;
		set => this.SetValue(ref this.autoColorErrorsAndWarnings, value);
	}

	public bool IgnoreFailure
	{
		get => this.ignoreFailure;
		set => this.SetValue(ref this.ignoreFailure, value);
	}

	public bool InCurrentBuild
	{
		get => this.inCurrentBuild;
		set
		{
			if (this.inCurrentBuild != value)
			{
				// This setting isn't persisted, so it shouldn't call SetModified.
				// But we do need to send notifications that the step was edited.
				this.inCurrentBuild = value;
				this.Project.RuntimeStepValueChanged(this);
			}
		}
	}

	public bool IsAdministratorRequired
	{
		get => this.MayRequireAdministrator && this.isAdministratorRequired;
		set
		{
			if (this.MayRequireAdministrator)
			{
				this.SetValue(ref this.isAdministratorRequired, value);
			}
		}
	}

	public bool OnlyIfParentSucceeded
	{
		get => this.onlyIfParentSucceeded;
		set => this.SetValue(ref this.onlyIfParentSucceeded, value);
	}

	public Guid OutputStartId { get; internal set; }

	public bool ParentSucceeded
	{
		get
		{
			// Check all the way up the parent chain if necessary.
			ExecutableStep? executableParent = this.Parent as ExecutableStep;

			bool result = true;
			while (executableParent != null && executableParent.InCurrentBuild)
			{
				if (executableParent.Status != StepStatus.Succeeded)
				{
					result = false;
					break;
				}

				executableParent = executableParent.Parent as ExecutableStep;
			}

			return result;
		}
	}

	public bool PromptFirst
	{
		get => this.promptFirst;
		set => this.SetValue(ref this.promptFirst, value);
	}

	public StepStatus Status
	{
		get => this.status;
		set
		{
			if (this.status != value)
			{
				// This setting isn't persisted, so it shouldn't call SetModified.
				// But we do need to send notifications that the step was edited.
				this.status = value;
				this.Project.RuntimeStepValueChanged(this);
			}
		}
	}

	public bool Timeout
	{
		get => this.SupportsTimeout && this.timeout;
		set => this.SetValue(ref this.timeout, value);
	}

	public int TimeoutMinutes
	{
		get => this.timeoutMinutes;
		set => this.SetValue(ref this.timeoutMinutes, value);
	}

	public TimeSpan TotalExecutionTime
	{
		get => this.totalTime;
		set
		{
			if (this.totalTime != value)
			{
				// This setting isn't persisted, so it shouldn't call SetModified.
				// But we do need to send notifications that the step was edited.
				this.totalTime = value;
				this.Project.RuntimeStepValueChanged(this);
			}
		}
	}

	public bool WaitForCompletion
	{
		get => !this.SupportsWaitForCompletion || this.waitForCompletion;
		set => this.SetValue(ref this.waitForCompletion, value);
	}

	public Encoding StandardStreamEncoding
	{
		get => this.standardStreamEncoding;
		set => this.SetValue(ref this.standardStreamEncoding, value);
	}

	#endregion

	#region Internal Properties

	internal bool MayRequireAdministrator
	{
		get
		{
			object[] attrs = this.GetType().GetCustomAttributes(typeof(MayRequireAdministratorAttribute), true);
			return attrs.Length > 0;
		}
	}

	internal List<(OutputStyle Style, Regex Pattern)>? CustomOutputStyles
	{
		get => this.customOutputStyles;
		set
		{
			if (!ReferenceEquals(this.CustomOutputStyles, value))
			{
				if ((this.customOutputStyles == null && value != null)
					|| (this.CustomOutputStyles != null && value == null)
					|| (this.CustomOutputStyles!.Count != value!.Count)
					|| this.CustomOutputStyles.Zip(value, (x, y) => (x, y))
						.Any(t => t.x.Style != t.y.Style || t.x.Pattern.ToString() != t.y.Pattern.ToString()))
				{
					this.customOutputStyles = value;
					this.SetModified();
				}
			}
		}
	}

	#endregion

	#region Protected Properties

	protected internal bool SupportsAutoColorErrorsAndWarnings => (this.supportFlags & ExecSupports.AutoColorErrorsAndWarnings) != 0;

	protected internal bool SupportsTimeout => this.SupportsWaitForCompletion && (this.supportFlags & ExecSupports.Timeout) != 0;

	protected internal bool SupportsWaitForCompletion => (this.supportFlags & ExecSupports.WaitForCompletion) != 0;

	protected bool StopBuilding
	{
		get
		{
			bool result = false;

			if (this.Project.StopBuilding)
			{
				this.status = StepStatus.Canceled;
				result = true;
			}

			return result;
		}
	}

	#endregion

	#region Public Methods

	public abstract bool Execute(StepExecuteArgs args);

	[SuppressMessage("Usage", "CC0022:Should dispose object", Justification = "Caller disposes new controls.")]
	public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
	{
		base.GetStepEditorControls(controls);
		controls.Add(new ExecStepCtrl { Step = this });
		if (this.SupportsAutoColorErrorsAndWarnings)
		{
			controls.Add(new ExecOutputCtrl { Step = this });
		}
	}

	public void ResetStatus()
	{
		this.Status = StepStatus.None;
		this.TotalExecutionTime = TimeSpan.Zero;
		this.OutputStartId = Guid.Empty;
	}

	#endregion

	#region Internal Methods

	internal static bool TryParseRegex(string? text, [MaybeNullWhen(false)] out Regex regex)
	{
		bool result = false;
		regex = null;

		try
		{
			if (text.IsNotEmpty())
			{
				// This defaults to case-insensitive to make the built-in patterns simpler.
				// Custom patterns can include (?-i) to force case-sensitive if they want to.
				regex = new Regex(text, RegexOptions.Compiled | RegexOptions.IgnoreCase);
				result = true;
			}
		}
		catch (ArgumentException ex)
		{
			Log.Error(typeof(ExecutableStep), "Unable to parse regex: " + text, ex);
			regex = null;
		}

		return result;
	}

	#endregion

	#region Protected Methods

	protected internal override void Load(XmlKey key)
	{
		base.Load(key);
		this.WaitForCompletion = key.GetValue(nameof(this.WaitForCompletion), this.waitForCompletion);
		this.IgnoreFailure = key.GetValue(nameof(this.IgnoreFailure), this.ignoreFailure);
		this.PromptFirst = key.GetValue(nameof(this.PromptFirst), this.promptFirst);
		this.OnlyIfParentSucceeded = key.GetValue(nameof(this.OnlyIfParentSucceeded), this.onlyIfParentSucceeded);
		this.Timeout = key.GetValue(nameof(this.Timeout), this.timeout);
		this.TimeoutMinutes = key.GetValue(nameof(this.TimeoutMinutes), this.timeoutMinutes);
		this.AutoColorErrorsAndWarnings = key.GetValue(nameof(this.AutoColorErrorsAndWarnings), this.autoColorErrorsAndWarnings);
		this.StandardStreamEncoding = Encoding.GetEncoding(key.GetValue(nameof(this.StandardStreamEncoding), this.standardStreamEncoding.WebName));
		if (this.MayRequireAdministrator)
		{
			this.IsAdministratorRequired = key.GetValue(nameof(this.IsAdministratorRequired), this.isAdministratorRequired);
		}

		XmlKey customOutputStyles = key.GetSubkey(nameof(this.CustomOutputStyles));
		foreach (XmlKey subKey in customOutputStyles.GetSubkeys())
		{
			OutputStyle style = subKey.GetValue("Style", OutputStyle.Normal);
			string pattern = subKey.GetValue("Pattern", string.Empty);
			if (!string.IsNullOrEmpty(pattern) && TryParseRegex(pattern, out Regex? regex))
			{
				this.customOutputStyles ??= [];
				this.customOutputStyles.Add((style, regex));
			}
		}
	}

	protected internal override void Save(XmlKey key)
	{
		base.Save(key);
		key.SetValue(nameof(this.WaitForCompletion), this.waitForCompletion);
		key.SetValue(nameof(this.IgnoreFailure), this.ignoreFailure);
		key.SetValue(nameof(this.PromptFirst), this.promptFirst);
		key.SetValue(nameof(this.OnlyIfParentSucceeded), this.onlyIfParentSucceeded);
		key.SetValue(nameof(this.Timeout), this.timeout);
		key.SetValue(nameof(this.TimeoutMinutes), this.timeoutMinutes);
		key.SetValue(nameof(this.AutoColorErrorsAndWarnings), this.autoColorErrorsAndWarnings);
		key.SetValue(nameof(this.StandardStreamEncoding), this.standardStreamEncoding.WebName);
		key.SetValue(nameof(this.IsAdministratorRequired), this.isAdministratorRequired);

		if (this.customOutputStyles != null && this.customOutputStyles.Count > 0)
		{
			XmlKey container = key.AddSubkey(nameof(this.CustomOutputStyles));
			foreach ((OutputStyle style, Regex pattern) in this.customOutputStyles)
			{
				XmlKey entry = container.AddSubkey("Output");
				entry.SetValue("Style", style);
				entry.SetValue("Pattern", pattern.ToString());
			}
		}
	}

	protected static bool AreFoldersDifferent(string? filePath, string? workingDirectory)
	{
		bool result = false;

		if (filePath.IsNotEmpty() && workingDirectory.IsNotEmpty())
		{
			string? fileDirectory = Path.GetDirectoryName(filePath);
			if (fileDirectory.IsNotEmpty())
			{
				fileDirectory = Manager.ExpandVariables(fileDirectory);
				workingDirectory = Manager.ExpandVariables(workingDirectory);
				result = !string.Equals(fileDirectory, workingDirectory, StringComparison.OrdinalIgnoreCase);
			}
		}

		return result;
	}

	protected bool ExecuteCommand(ExecuteCommandArgs args)
	{
		bool result = true;
		args.ExitCode = 0;

		// Check if the step requires the user to be running as an administrator.  This is mostly
		// an issue on Vista or later when UAC is enabled, but it can affect earlier OSes.
		if (this.IsAdministratorRequired && !ApplicationInfo.IsUserRunningAsAdministrator)
		{
			this.Project.OutputLine("This step requires MegaBuild to be running in the administrator role.", OutputColors.Error, 0, true);
			result = false;
			args.ExitCode = -1;
		}
		else
		{
			ProcessStartInfo startInfo = this.CreateProcessStartInfo(args);

			using (Process process = new())
			{
				process.StartInfo = startInfo;

				// Attach to the asynchronous stream events if necessary.
				if (startInfo.RedirectStandardOutput)
				{
					process.OutputDataReceived += (sender, eData) => this.OutputStreamData(eData, false, args.AllowOutputLine);
				}

				if (startInfo.RedirectStandardError)
				{
					bool errorData = !args.RedirectStandardStreams.HasFlag(RedirectStandardStreams.TreatErrorAsOutput);
					process.ErrorDataReceived += (sender, eData) => this.OutputStreamData(eData, errorData, args.AllowErrorLine);
				}

				// Start the process
				if (this.StartProcess(process))
				{
					// Begin asynchronous reading of the streams if necessary.
					if (startInfo.RedirectStandardOutput)
					{
						process.BeginOutputReadLine();
					}

					if (startInfo.RedirectStandardError)
					{
						process.BeginErrorReadLine();
					}

					if (this.StopCommand(process))
					{
						result = false;
					}
					else if (this.WaitForCompletion)
					{
						DateTime startTime = DateTime.UtcNow;

						// Rather than do a Proc.WaitForExit and block the whole time,
						// we'll just poll.  That way we can be responsive to StopBuild
						// requests.
						try
						{
							while (!process.HasExited)
							{
								Thread.Sleep(TimeSpan.FromSeconds(1));

								if (this.StopCommand(process))
								{
									result = false;
									break;
								}

								if (this.Timeout)
								{
									TimeSpan time = DateTime.UtcNow - startTime;
									if (time.TotalMinutes >= this.TimeoutMinutes)
									{
										this.Status = StepStatus.TimedOut;
										this.StopProcess(process);
										result = false;
										break;
									}
								}
							}
						}
						finally
						{
							// We must wait for exit with an infinite timeout to force all
							// asynchronous output to be read before we return.
							process.WaitForExit();
						}

						args.ExitCode = process.ExitCode;
						result = args.ExitCode >= args.FirstSuccessCode && args.ExitCode <= args.LastSuccessCode;
					}
				}

				// else
				// 	The process was already running.  This happens when UseShellExecute is used,
				// 	and it reuses an existing process (e.g. "C:\Windows" with verb "Explore").
				// 	A consequence of this is that certain processes can't be waited on to finish
				// 	even if the user requested that.  For example, Explorer.exe won't exit until
				// 	the user logs out.
			}
		}

		if (!result)
		{
			this.Status = StepStatus.Failed;
		}

		this.DebugOutput($"Exit Code: {args.ExitCode}; Success: {result}");
		return result;
	}

	protected void DebugOutput(params object[] values)
	{
		if (this.Project.ShowDebugOutput)
		{
			foreach (object value in values ?? Enumerable.Empty<object>())
			{
				string? message = value?.ToString();
				this.Project.Output(message + Environment.NewLine, OutputColors.Debug);
			}
		}
	}

	#endregion

	#region Private Methods

	private static List<Regex> ParseRegexes(StringCollection regexStrings)
	{
		List<Regex> result = [];

		foreach (string? regexString in regexStrings)
		{
			if (TryParseRegex(regexString, out Regex? regex))
			{
				result.Add(regex);
			}
		}

		return result;
	}

	private ProcessStartInfo CreateProcessStartInfo(ExecuteCommandArgs args)
	{
		ProcessStartInfo startInfo = new()
		{
			// Use Manager.ExpandVariables for all strings.
			FileName = Manager.ExpandVariables(args.FileName),
		};
		this.DebugProperty("FileName", startInfo.FileName);
		startInfo.Arguments = Manager.ExpandVariables(args.Arguments);
		this.DebugProperty("Arguments", startInfo.Arguments);
		startInfo.WorkingDirectory = Manager.ExpandVariables(args.WorkingDirectory);
		this.DebugProperty("WorkingDirectory", startInfo.WorkingDirectory);
		startInfo.Verb = Manager.ExpandVariables(args.Verb);
		this.DebugProperty("Verb", startInfo.Verb);

		startInfo.UseShellExecute = args.UseShellExecute;
		this.DebugProperty("UseShellExecute", startInfo.UseShellExecute);

		// If stdout is redirected, then sometimes stdin must be redirected too.
		// Some console programs like Win2000's XCopy will fail if only one
		// stream is redirected.  But other programs like PowerShell 1.0 will
		// hang if both are redirected.
		startInfo.RedirectStandardInput = (args.RedirectStandardStreams & RedirectStandardStreams.Input) != 0;
		this.DebugProperty("RedirectStandardInput", startInfo.RedirectStandardInput);
		startInfo.RedirectStandardOutput = (args.RedirectStandardStreams & RedirectStandardStreams.Output) != 0;
		this.DebugProperty("RedirectStandardOutput", startInfo.RedirectStandardOutput);
		startInfo.RedirectStandardError = (args.RedirectStandardStreams & RedirectStandardStreams.Error) != 0;
		this.DebugProperty("RedirectStandardError", startInfo.RedirectStandardError);

		if (startInfo.RedirectStandardOutput)
		{
			startInfo.StandardOutputEncoding = this.StandardStreamEncoding;
		}

		if (startInfo.RedirectStandardError)
		{
			startInfo.StandardErrorEncoding = this.StandardStreamEncoding;
		}

		if (startInfo.RedirectStandardInput)
		{
			startInfo.StandardInputEncoding = this.StandardStreamEncoding;
		}

		startInfo.WindowStyle = args.WindowStyle;
		this.DebugProperty("WindowStyle", startInfo.WindowStyle);
		if (args.WindowStyle == ProcessWindowStyle.Hidden)
		{
			startInfo.CreateNoWindow = true;
			this.DebugProperty("CreateNoWindow", startInfo.CreateNoWindow);
		}

		if (args.EnvironmentVariables.Count > 0)
		{
			if (args.UseShellExecute)
			{
				throw Exceptions.NewInvalidOperationException("Environment variables can't be set when using ShellExecute.");
			}

			foreach (var pair in args.EnvironmentVariables)
			{
				startInfo.EnvironmentVariables[pair.Key] = pair.Value;
			}
		}

		return startInfo;
	}

	private void DebugProperty(string property, object value)
	{
		string? text = value?.ToString();
		if (!string.IsNullOrEmpty(text))
		{
			this.DebugOutput($"{property}:\t{value}");
		}
	}

	private void StopProcess(Process process)
	{
		if (!process.HasExited)
		{
			try
			{
				// Try to close the process's main window, which will exit most applications.
				this.DebugOutput($"Closing main window for {process.ProcessName}.");
				process.CloseMainWindow();
			}
#pragma warning disable CC0004 // Catch block cannot be empty
			catch (InvalidOperationException)
			{
				// If the process goes away between the time we check
				// HasExited and when we call CloseMainWindow, then an
				// InvalidOperationException will be thrown.  But that's ok
				// because then we know the process has definitely exited.
			}
#pragma warning restore CC0004 // Catch block cannot be empty
		}

		if (!process.HasExited)
		{
			int milliseconds = Properties.Settings.Default.ExecutableStep_KillTimeoutMilliseconds;
			this.DebugOutput($"Waiting {milliseconds} ms for process {process.ProcessName} to exit.");
			process.WaitForExit(milliseconds);
			if (!process.HasExited)
			{
				this.DebugOutput($"Killing process {process.ProcessName}.");
				try
				{
					process.Kill();
				}
#pragma warning disable CC0004 // Catch block cannot be empty
				catch (InvalidOperationException)
				{
					// This can occur if the process finished between WaitForExit and Kill.
				}
#pragma warning restore CC0004 // Catch block cannot be empty
#pragma warning disable CC0004 // Catch block cannot be empty
				catch (Win32Exception)
				{
					// This can occur if the process can't be killed.
				}
#pragma warning restore CC0004 // Catch block cannot be empty
			}
		}
	}

	private bool StartProcess(Process process)
	{
		const int E_FAIL = -2_147_467_259; // 0x80004005
		try
		{
			bool result = process.Start();
			return result;
		}
		catch (Win32Exception ex) when (ex.HResult == E_FAIL)
		{
			// The error message is usually just "The system cannot find the file specified".
			// So we'll write out the file info that we have to make diagnostics easier.
			ProcessStartInfo startInfo = process.StartInfo;
			string message = $"FileName: {startInfo.FileName}";
			if (!string.IsNullOrEmpty(startInfo.Arguments))
			{
				message += $"\r\nArguments: {startInfo.Arguments}";
			}

			if (!string.IsNullOrEmpty(startInfo.WorkingDirectory))
			{
				message += $"\r\nWorking Directory: {startInfo.WorkingDirectory}";
			}

			this.Project.OutputLine(message, OutputColors.Warning, 0, true);

			throw;
		}
	}

	private void OutputStreamData(DataReceivedEventArgs args, bool errorData, Func<string, bool>? allowLine)
	{
		// Fork this off to another thread to try to minimize the chance of deadlocking
		// while reading from the console output streams.  Use the exclusive scheduler
		// so only one of these tasks will run at a time and in the order they're queued.
		Task.Factory.StartNew(
			() => this.OutputStreamDataBackground(args, errorData, allowLine),
			CancellationToken.None,
			TaskCreationOptions.PreferFairness,
			Schedulers.ExclusiveScheduler);
	}

	private void OutputStreamDataBackground(DataReceivedEventArgs args, bool errorData, Func<string, bool>? allowLine)
	{
		// Sometimes we get null data.  I'm just ignoring those cases.  From digging through the Process
		// implementation, we get null data as the last notification when the stream has ended.
		// Some users (e.g., http://stackoverflow.com/a/7608823) depend on that implementation.
		string? output = args.Data;
		if (output != null && (allowLine == null || allowLine(output)))
		{
			// Some lines from VC++ end with an embedded NULL. We have to strip that off or the Output window
			// won't display it correctly, and it will hose up text output logs.
			// Note: .NET 5+ changed the behavior of EndsWith("\0") to always return true:
			// https://github.com/dotnet/runtime/issues/55843#issuecomment-882002006
			if (output.Length > 0 && output[output.Length - 1] == '\0')
			{
				output = output.Substring(0, output.Length - 1);
			}

			// We only allow output to be added not removed.  So we can't support
			// the ASCII Backspace control character (#8).  Some tools like WinZip
			// try to use it to animate their command-line output progress.  We'll
			// quietly remove it.  Otherwise, it shows up as a box with a circle in it
			// when using WinForms.  (WPF doesn't display it at all.)
			output = output.Replace("\u0008", string.Empty);

			OutputStyle style = errorData ? OutputStyle.Error : OutputStyle.Normal;
			bool appliedCustom = false;
			if (this.customOutputStyles != null && this.customOutputStyles.Count > 0)
			{
				foreach ((OutputStyle customStyle, Regex regex) in this.customOutputStyles)
				{
#pragma warning disable CC0081 // Use of Regex.IsMatch might be improved. TryParseRegex creates a compiled Regex.
					if (regex.IsMatch(output))
#pragma warning restore CC0081 // Use of Regex.IsMatch might be improved
					{
						style = customStyle;
						appliedCustom = true;
						break;
					}
				}
			}

			if (!appliedCustom && this.AutoColorErrorsAndWarnings && !errorData)
			{
				if (ErrorRegexes.Any(regex => regex.IsMatch(output)))
				{
					style = OutputStyle.Error;
				}
				else if (WarningRegexes.Any(regex => regex.IsMatch(output)))
				{
					style = OutputStyle.Warning;
				}
			}

			if (style != OutputStyle.None)
			{
				Color color = style switch
				{
					OutputStyle.Error => OutputColors.Error,
					OutputStyle.Warning => OutputColors.Warning,
					OutputStyle.Debug => OutputColors.Debug,
					_ => SystemColors.WindowText,
				};

				bool highlight = style >= OutputStyle.Warning && output.IsNotWhiteSpace();
				this.Project.OutputLine(output, color, 0, highlight);
			}
		}
	}

	private bool StopCommand(Process process)
	{
		bool result = false;

		if (this.StopBuilding)
		{
			this.StopProcess(process);
			result = true;
		}

		return result;
	}

	#endregion
}
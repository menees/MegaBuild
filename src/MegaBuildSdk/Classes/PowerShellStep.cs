namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Text;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	[StepDisplay(nameof(PowerShell), "Runs a PowerShell script or command.", "Images.PowerShellStep.ico")]
	[MayRequireAdministrator]
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Called by Reflection.")]
	internal sealed class PowerShellStep : ExecutableStep
	{
		#region Private Data Members

		private static readonly char[] InvalidPathCharacters = Path.GetInvalidPathChars();

		private string? command;
		private string? workingDirectory;
		private PowerShell shell;
		private bool treatErrorStreamAsOutput;

		#endregion

		#region Constructors

		public PowerShellStep(Project project, StepCategory category, StepTypeInfo info)
			: base(project, category, info)
		{
		}

		#endregion

		#region Public Properties

		public string? Command
		{
			get => this.command;
			set => this.SetValue(ref this.command, value);
		}

		public string? WorkingDirectory
		{
			get => this.workingDirectory;
			set => this.SetValue(ref this.workingDirectory, value);
		}

		public PowerShell Shell
		{
			get => this.shell;
			set => this.SetValue(ref this.shell, value);
		}

		public bool TreatErrorStreamAsOutput
		{
			get => this.treatErrorStreamAsOutput;
			set => this.SetValue(ref this.treatErrorStreamAsOutput, value);
		}

		#endregion

		#region Private Properties

		private bool IsScript
		{
			get
			{
				bool result = false;

				try
				{
					string? unquotedCommand = this.GetExpandedCommand(true);
					if (unquotedCommand != null && !unquotedCommand.Any(ch => InvalidPathCharacters.Contains(ch)))
					{
						string extension = Path.GetExtension(unquotedCommand);
						result = string.Compare(extension, ".ps1", true) == 0;
					}
				}
#pragma warning disable CC0004 // Catch block cannot be empty
				catch (ArgumentException)
				{
					// If they haven't set up a valid command yet, it's safest to just return false.
				}
#pragma warning restore CC0004 // Catch block cannot be empty

				return result;
			}
		}

		#endregion

		#region Public Methods

		public override bool Execute(StepExecuteArgs args)
		{
			string arguments;
			if (this.IsScript)
			{
				// The -file option needs the file name surrounded in double quotes (like DOS) not single quotes like PowerShell.
				arguments = "-file " + TextUtility.EnsureQuotes(this.GetExpandedCommand(true));
			}
			else
			{
				// I'm intentionally not doing EnsureQuotes on the command because
				// it may not be a script or even a single argument.
				arguments = "-command " + this.GetExpandedCommand(false);
			}

			const string WindowsFileName = "PowerShell.exe";
			const string CoreFileName = "pwsh.exe";
			string fileName;
			switch (this.Shell)
			{
				case PowerShell.Windows:
					fileName = WindowsFileName;
					break;

				case PowerShell.Core:
					fileName = CoreFileName;
					break;

				default:
					fileName = SearchPath(CoreFileName) ?? WindowsFileName;
					break;
			}

			ExecuteCommandArgs cmdArgs = new()
			{
				FileName = fileName,
				Arguments = arguments,
				WorkingDirectory = this.WorkingDirectory,
				WindowStyle = ProcessWindowStyle.Hidden,

				// PowerShell 1.0 hangs if we redirect StdIn, so we'll always leave that off.
				RedirectStandardStreams = RedirectStandardStreams.Output | RedirectStandardStreams.Error
					| (this.TreatErrorStreamAsOutput ? RedirectStandardStreams.TreatErrorAsOutput : RedirectStandardStreams.None),
			};

			bool result = this.ExecuteCommand(cmdArgs);
			if (!result)
			{
				this.Project.OutputLine("PowerShell returned exit code: " + cmdArgs.ExitCode, OutputColors.Heading);
			}

			return result;
		}

		public override void ExecuteCustomVerb(string verb)
		{
			switch (verb)
			{
				case "Edit Script":
					WindowsUtility.ShellExecute(null, TextUtility.EnsureQuotes(this.GetExpandedCommand(true)));
					break;
			}
		}

		public override string[]? GetCustomVerbs()
			=> this.IsScript ? new string[] { "Edit Script" } : base.GetCustomVerbs();

		[SuppressMessage("Usage", "CC0022:Should dispose object", Justification = "Caller disposes new controls.")]
		public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
		{
			base.GetStepEditorControls(controls);
			controls.Add(new PowerShellStepCtrl { Step = this });
		}

		#endregion

		#region Protected Methods

		protected internal override void Load(XmlKey key)
		{
			base.Load(key);
			this.Command = key.GetValueN(nameof(this.Command), this.Command);
			this.WorkingDirectory = key.GetValueN(nameof(this.WorkingDirectory), this.WorkingDirectory);
			this.Shell = key.GetValue(nameof(this.Shell), this.Shell);
			this.TreatErrorStreamAsOutput = key.GetValue(nameof(this.TreatErrorStreamAsOutput), this.TreatErrorStreamAsOutput);
		}

		protected internal override void Save(XmlKey key)
		{
			base.Save(key);
			key.SetValue(nameof(this.Command), this.Command);
			key.SetValue(nameof(this.WorkingDirectory), this.WorkingDirectory);
			key.SetValue(nameof(this.Shell), this.Shell);
			key.SetValue(nameof(this.TreatErrorStreamAsOutput), this.TreatErrorStreamAsOutput);
		}

		#endregion

		#region Private Methods

		private static string? SearchPath(string fileName)
		{
			string? result = null;

			string[] pathEntries = (Environment.GetEnvironmentVariable("PATH") ?? string.Empty)
				.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(entry => entry.Trim())
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.ToArray();

			// The check for duplicates saves time here. On my system, 12 of the 55 entries in PATH are duplicates (ignoring case).
			// Unfortunately, we can't use LINQ's Distinct() because it returns an unordered sequence, and we have to preserve order.
			// Note: Windows 10 supports case-sensitive folders, but the PowerShell folders shouldn't be configured that way.
			HashSet<string> checkedPaths = new(StringComparer.OrdinalIgnoreCase);
			foreach (string path in pathEntries)
			{
				if (!checkedPaths.Contains(path))
				{
					checkedPaths.Add(path);
					string fullyQualifiedName = Path.Combine(path, fileName);
					if (File.Exists(fullyQualifiedName))
					{
						result = fullyQualifiedName;
						break;
					}
				}
			}

			return result;
		}

		private string GetExpandedCommand(bool forShellExecute)
		{
			string result = Manager.ExpandVariables(this.Command);

			// When passing a script path containing spaces to PowerShell's -command parameter
			// we have to put single-quotes around the path, so that's how the Command might be
			// configured.  However, Windows's ShellExecute doesn't like that, so we have to remove
			// the single quotes and replace them with double quotes.
			if (forShellExecute)
			{
				result = TextUtility.StripQuotes(result, "'", "'");
			}

			return result;
		}

		#endregion
	}
}
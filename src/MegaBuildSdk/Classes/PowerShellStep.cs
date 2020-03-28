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

	[StepDisplay("PowerShell", "Runs a PowerShell script or command.", "Images.PowerShellStep.ico")]
	[MayRequireAdministrator]
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Called by Reflection.")]
	internal sealed class PowerShellStep : ExecutableStep
	{
		#region Private Data Members

		private static readonly char[] InvalidPathCharacters = Path.GetInvalidPathChars();

		private string command;
		private string workingDirectory;

		#endregion

		#region Constructors

		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Reflection.")]
		public PowerShellStep(Project project, StepCategory category, StepTypeInfo info)
			: base(project, category, info)
		{
		}

		#endregion

		#region Public Properties

		public string Command
		{
			get
			{
				return this.command;
			}

			set
			{
				this.SetValue(ref this.command, value);
			}
		}

		public string WorkingDirectory
		{
			get
			{
				return this.workingDirectory;
			}

			set
			{
				this.SetValue(ref this.workingDirectory, value);
			}
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
					string unquotedCommand = this.GetExpandedCommand(true);
					if (!unquotedCommand.Any(ch => InvalidPathCharacters.Contains(ch)))
					{
						string extension = Path.GetExtension(unquotedCommand);
						result = string.Compare(extension, ".ps1", true) == 0;
					}
				}
				catch (ArgumentException)
				{
				}

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

			ExecuteCommandArgs cmdArgs = new ExecuteCommandArgs()
			{
				FileName = "PowerShell.exe",
				Arguments = arguments,
				WorkingDirectory = this.WorkingDirectory,
				WindowStyle = ProcessWindowStyle.Hidden,

				// PowerShell 1.0 hangs if we redirect StdIn, so we'll always leave that off.
				RedirectStandardStreams = RedirectStandardStreams.Output | RedirectStandardStreams.Error,
			};

			bool result = this.ExecuteCommand(cmdArgs);
			if (!result)
			{
				this.Project.OutputLine("PowerShell returned exit code: " + cmdArgs.ExitCode.ToString(), OutputColors.Heading);
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

		public override string[] GetCustomVerbs()
			=> this.IsScript ? new string[] { "Edit Script" } : base.GetCustomVerbs();

		public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
		{
			base.GetStepEditorControls(controls);
			controls.Add(new PowerShellStepCtrl() { Step = this });
		}

		#endregion

		#region Protected Methods

		protected internal override void Load(XmlKey key)
		{
			base.Load(key);
			this.Command = key.GetValue("Command", this.Command);
			this.WorkingDirectory = key.GetValue("WorkingDirectory", this.WorkingDirectory);
		}

		protected internal override void Save(XmlKey key)
		{
			base.Save(key);
			key.SetValue("Command", this.Command);
			key.SetValue("WorkingDirectory", this.WorkingDirectory);
		}

		#endregion

		#region Private Methods

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
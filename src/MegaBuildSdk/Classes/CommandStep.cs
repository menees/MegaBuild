namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Drawing;
	using System.IO;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	[StepDisplay("Command", "Runs a program, batch file, script, or any other file with an associated program.", "Images.CommandStep.ico")]
	[MayRequireAdministrator]
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Called by Reflection.")]
	internal sealed class CommandStep : ExecutableStep
	{
		#region Private Data Members

		private bool useShellExecute;
		private RedirectStandardStreams redirectStreams;
		private ProcessWindowStyle windowState = ProcessWindowStyle.Hidden;
		private int firstSuccessCode;
		private int lastSuccessCode;
		private string arguments = string.Empty;
		private string command = string.Empty;
		private string verb = string.Empty;
		private string workingDirectory = string.Empty;

		#endregion

		#region Constructors

		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Reflection.")]
		public CommandStep(Project project, StepCategory category, StepTypeInfo info)
			: base(project, category, info)
		{
		}

		#endregion

		#region Public Properties

		public string Arguments
		{
			get
			{
				return this.arguments;
			}

			set
			{
				this.SetValue(ref this.arguments, value);
			}
		}

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

		public int FirstSuccessCode
		{
			get
			{
				return this.firstSuccessCode;
			}

			set
			{
				this.SetValue(ref this.firstSuccessCode, value);
			}
		}

		public int LastSuccessCode
		{
			get
			{
				return this.lastSuccessCode;
			}

			set
			{
				this.SetValue(ref this.lastSuccessCode, value);
			}
		}

		public RedirectStandardStreams RedirectStreams
		{
			get
			{
				return this.redirectStreams;
			}

			set
			{
				this.SetValue(ref this.redirectStreams, value);
			}
		}

		public bool UseShellExecute
		{
			get
			{
				return this.useShellExecute;
			}

			set
			{
				this.SetValue(ref this.useShellExecute, value);
			}
		}

		public string Verb
		{
			get
			{
				return this.verb;
			}

			set
			{
				this.SetValue(ref this.verb, value);
			}
		}

		public ProcessWindowStyle WindowState
		{
			get
			{
				return this.windowState;
			}

			set
			{
				this.SetValue(ref this.windowState, value);
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

		private string ExpandedCommand => Manager.ExpandVariables(this.Command);

		private bool IsBatchFile => HasBatchFileExt(this.ExpandedCommand);

		#endregion

		#region Public Methods

		public override bool Execute(StepExecuteArgs args)
		{
			string command = this.ExpandedCommand;
			string arguments = this.Arguments;

			// If the user wants to redirect the output of a BAT or CMD file,
			// then we'll need to change things a bit.  Redirection can only
			// be done if a Win32 executable (.exe or .com) is launched.  To
			// make it work for a batch file, we have to explicitly launch
			// CMD.EXE and tell it to run the batch file.  UseShellExecute
			// invokes CMD.EXE automatically, but it provides no way to redirect.
			if (this.RedirectStreams != RedirectStandardStreams.None && this.IsBatchFile)
			{
				arguments = string.Format("/c {0} {1}", TextUtility.EnsureQuotes(command), arguments);
				command = "cmd.exe";
			}

			ExecuteCommandArgs cmdArgs = new ExecuteCommandArgs(
				command,
				arguments,
				this.WorkingDirectory,
				this.WindowState,
				this.FirstSuccessCode,
				this.LastSuccessCode,
				this.RedirectStreams,
				this.UseShellExecute,
				this.Verb);

			bool result = this.ExecuteCommand(cmdArgs);

			if (!result)
			{
				this.Project.OutputLine("Command returned exit code: " + cmdArgs.ExitCode.ToString(), OutputColors.Heading);
			}

			return result;
		}

		public override void ExecuteCustomVerb(string verb)
		{
			switch (verb)
			{
				case "Edit Batch File":
					try
					{
						using (WindowsUtility.ShellExecute(null, TextUtility.EnsureQuotes(this.ExpandedCommand), "Edit"))
						{
						}
					}
					catch (Win32Exception)
					{
						// ShellExecute will have already displayed an error dialog.
					}

					break;
			}
		}

		public override string[] GetCustomVerbs()
			=> this.IsBatchFile ? new string[] { "Edit Batch File" } : base.GetCustomVerbs();

		public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
		{
			base.GetStepEditorControls(controls);
			controls.Add(new CmdStepCtrl() { Step = this });
		}

		#endregion

		#region Internal Methods

		internal static bool HasBatchFileExt(string fileName)
		{
			string ext = Path.GetExtension(fileName).ToLower();
			return ext == ".bat" || ext == ".cmd";
		}

		#endregion

		#region Protected Methods

		protected internal override void Load(XmlKey key)
		{
			base.Load(key);
			this.Command = key.GetValue("Command", this.command);
			this.Arguments = key.GetValue("Arguments", this.arguments);
			this.WorkingDirectory = key.GetValue("WorkingDirectory", this.workingDirectory);
			this.WindowState = key.GetValue("WindowState", this.windowState);
			this.FirstSuccessCode = key.GetValue("FirstSuccessCode", this.firstSuccessCode);
			this.LastSuccessCode = key.GetValue("LastSuccessCode", this.lastSuccessCode);
			this.UseShellExecute = key.GetValue("UseShellExecute", this.useShellExecute);
			this.Verb = key.GetValue("Verb", this.verb);

			// In 1.0.5, I changed the redirect to all streams, so this name changed.
			// Then in 2.0.2, I changed it again to support a mask of streams.
			// But I need to still read in old project files correctly.
			if (key.ValueExists("RedirectOutput"))
			{
				this.RedirectStreams = key.GetValue("RedirectOutput", (this.redirectStreams & RedirectStandardStreams.Output) != 0)
					? RedirectStandardStreams.All : RedirectStandardStreams.None;
			}
			else if (key.ValueExists("RedirectStreams"))
			{
				this.RedirectStreams = key.GetValue("RedirectStreams", this.redirectStreams != RedirectStandardStreams.None)
					? RedirectStandardStreams.All : RedirectStandardStreams.None;

				// The ExecutableStep base class used to save a "RedirectStandardError" flag,
				// but as of 2.0.2 I handle it with a stream mask.  So I'll check for the old flag
				// here for backwards compatibility.
				if (this.RedirectStreams != RedirectStandardStreams.None)
				{
					if (!key.GetValue("RedirectStandardError", true))
					{
						this.RedirectStreams &= ~RedirectStandardStreams.Error;
					}
				}
			}
			else
			{
				this.RedirectStreams = key.GetValue("RedirectStreamsMask", this.redirectStreams);
			}
		}

		protected internal override void Save(XmlKey key)
		{
			base.Save(key);
			key.SetValue("Command", this.command);
			key.SetValue("Arguments", this.arguments);
			key.SetValue("WorkingDirectory", this.workingDirectory);
			key.SetValue("WindowState", this.windowState);
			key.SetValue("FirstSuccessCode", this.firstSuccessCode);
			key.SetValue("LastSuccessCode", this.lastSuccessCode);
			key.SetValue("UseShellExecute", this.useShellExecute);
			key.SetValue("Verb", this.verb);
			key.SetValue("RedirectStreamsMask", this.redirectStreams);
		}

		#endregion
	}
}
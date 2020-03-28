namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Drawing;
	using System.IO;
	using System.Text;
	using System.Threading;
	using System.Windows.Forms;
	using Menees;

	#endregion

	[StepDisplay("MegaBuild", "Opens and builds another MegaBuild project.", "Images.MegaBuildStep.ico")]
	[MayRequireAdministrator]
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Called by Reflection.")]
	internal sealed class MegaBuildStep : ExecutableStep
	{
		#region Private Data Members

		private bool exit;
		private bool minimize;
		private bool inProc = true;
		private string projectFile = string.Empty;

		#endregion

		#region Constructors

		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Reflection.")]
		public MegaBuildStep(Project project, StepCategory category, StepTypeInfo info)
			: base(project, category, info)
		{
		}

		#endregion

		#region Public Properties

		public bool Exit
		{
			get
			{
				return this.exit;
			}

			set
			{
				this.SetValue(ref this.exit, value);
			}
		}

		public bool InProc
		{
			get
			{
				return this.inProc;
			}

			set
			{
				this.SetValue(ref this.inProc, value);
			}
		}

		public bool Minimize
		{
			get
			{
				return this.minimize;
			}

			set
			{
				this.SetValue(ref this.minimize, value);
			}
		}

		public string ProjectFile
		{
			get
			{
				return this.projectFile;
			}

			set
			{
				this.SetValue(ref this.projectFile, value);
			}
		}

		public override string StepInformation
		{
			get
			{
				StringBuilder sb = new StringBuilder();

				sb.Append(Path.GetFileName(this.ProjectFile));

				if (!this.InProc)
				{
					sb.Append(", Out-Of-Process");
					if (this.Minimize)
					{
						sb.Append(", Minimize");
					}

					if (this.Exit)
					{
						sb.Append(", Exit");
					}
				}

				return sb.ToString();
			}
		}

		#endregion

		#region Public Methods

		public override bool Execute(StepExecuteArgs args) => this.InProc ? this.ExecuteInProc(args) : this.ExecuteOutOfProc();

		public override void ExecuteCustomVerb(string verb)
		{
			string project = Manager.ExpandVariables(this.ProjectFile);
			string quotedProject = TextUtility.EnsureQuotes(project);

			switch (verb)
			{
				case "Open Project":
					this.Project.Open(project);
					break;

				case "Open Project In New Instance":
					Process.Start(Application.ExecutablePath, quotedProject);
					break;

				case "Build Project In New Instance":
					Process.Start(Application.ExecutablePath, quotedProject + " /Build");
					break;
			}
		}

		public override string[] GetCustomVerbs() => new[] { "Open Project", "Open Project In New Instance", "Build Project In New Instance" };

		public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
		{
			base.GetStepEditorControls(controls);
			controls.Add(new MegaBuildStepCtrl() { Step = this });
		}

		#endregion

		#region Protected Methods

		protected internal override void Load(XmlKey key)
		{
			base.Load(key);
			this.ProjectFile = key.GetValue("ProjectFile", this.ProjectFile);
			this.Exit = key.GetValue("Exit", this.Exit);
			this.Minimize = key.GetValue("Minimize", this.Minimize);

			// We have to default InProc to false to keep the same
			// behavior for old, loaded steps.  (However, it will default
			// to true for added/inserted steps.)
			this.InProc = key.GetValue("InProc", false);
		}

		protected internal override void Save(XmlKey key)
		{
			base.Save(key);
			key.SetValue("ProjectFile", this.ProjectFile);
			key.SetValue("Exit", this.Exit);
			key.SetValue("Minimize", this.Minimize);
			key.SetValue("InProc", this.InProc);
		}

		#endregion

		#region Private Methods

		private bool ExecuteInProc(StepExecuteArgs args)
		{
			bool result = false;

			using (Project subProject = new Project(this.Level + 2))
			{
				// Let the sub-project use the same form, so it can properly
				// synchronize messages with the GUI thread.
				subProject.Form = this.Project.Form;

				string projectFileName = Manager.ExpandVariables(this.ProjectFile);
				if (subProject.Open(projectFileName))
				{
					Step[] steps = new Step[subProject.BuildSteps.Count];
					subProject.BuildSteps.CopyTo(steps);

					// We have to auto-confirm any confirmable steps to prevent a modal dialog from popping up
					// during the build. If they really don't want the confirmable steps to build, then they'll
					// need to turn them off (i.e., disable them) in the project.
					subProject.Build(steps, BuildOptions.AutoConfirmSteps, args);

					while (subProject.Building)
					{
						// Check the outer project to see if the main build has been stopped/canceled.
						// If so, then we need to stop/cancel the inner project too.
						if (this.StopBuilding)
						{
							subProject.StopBuild();
						}

						Thread.Sleep(TimeSpan.FromSeconds(1));
					}

					result = subProject.BuildStatus != BuildStatus.Failed;
				}
			}

			return result;
		}

		private bool ExecuteOutOfProc()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("/build ");
			if (this.Exit)
			{
				sb.Append("/exit ");
			}

			sb.Append(TextUtility.EnsureQuotes(this.ProjectFile));
			string args = sb.ToString();

			// Note: When you're running in the debugger this will NOT minimize the child process.  But it does work outside the debugger.
			ProcessWindowStyle windowStyle = this.Minimize ? ProcessWindowStyle.Minimized : ProcessWindowStyle.Normal;

			ExecuteCommandArgs cmdArgs = new ExecuteCommandArgs()
			{
				FileName = Application.ExecutablePath,
				Arguments = args,
				WindowStyle = windowStyle,
				UseShellExecute = true,
			};

			bool result = this.ExecuteCommand(cmdArgs);

			if (this.WaitForCompletion)
			{
				string message = string.Format(
					"Build of MegaBuild project '{0}' {1}.",
					Path.GetFileName(this.ProjectFile),
					cmdArgs.ExitCode == 0 ? "succeeded" : "failed");
				if (cmdArgs.ExitCode == 0)
				{
					this.Project.OutputLine(message);
				}
				else
				{
					this.Project.OutputLine(message, OutputColors.Error, 0, true);
				}
			}
			else
			{
				this.Project.OutputLine("A new MegaBuild process step was launched asynchronously.");
			}

			return result;
		}

		#endregion
	}
}
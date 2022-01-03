namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Windows.Forms;
	using Menees;

	#endregion

	[StepDisplay("Visual Studio", "Builds one or more configurations for a Visual Studio solution.", "Images.VSStep.ico")]
	[MayRequireAdministrator]
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Called by Reflection.")]
	internal sealed class VSStep : ExecutableStep
	{
		#region Private Data Members

		private readonly VSConfigurationList configurations = new();
		private bool redirectStreams = true;
		private VSAction action = VSAction.Build;
		private VSVersion version = VSVersionInfo.LatestVersion.Version;
		private ProcessWindowStyle windowState = ProcessWindowStyle.Hidden;
		private string devEnvArguments = string.Empty;
		private string solution = string.Empty;
		private string stepInformation = string.Empty;
		private VSStepExecuteArgs? stepExecuteArgs;

		#endregion

		#region Constructors

		public VSStep(Project project, StepCategory category, StepTypeInfo info)
			: base(project, category, info)
		{
			// Make warnings and errors show up in color by default.
			this.AutoColorErrorsAndWarnings = true;
		}

		#endregion

		#region Public Properties

		public VSAction Action
		{
			get => this.action;
			set => this.SetValue(ref this.action, value);
		}

		public string DevEnvArguments
		{
			get => this.devEnvArguments;
			set => this.SetValue(ref this.devEnvArguments, value);
		}

		public bool RedirectStreams
		{
			get => this.redirectStreams;
			set => this.SetValue(ref this.redirectStreams, value);
		}

		public string Solution
		{
			get => this.solution;
			set => this.SetValue(ref this.solution, value);
		}

		public override string StepInformation => this.stepInformation;

		public VSVersion Version
		{
			get => this.version;
			set => this.SetValue(ref this.version, value);
		}

		public ProcessWindowStyle WindowState
		{
			get => this.windowState;
			set => this.SetValue(ref this.windowState, value);
		}

		#endregion

		#region Internal Properties

		internal VSConfigurationList Configurations => this.configurations;

		#endregion

		#region Private Properties

		private string ActionDescription
		{
			get
			{
				string result;

				switch (this.GetExecutableAction())
				{
					case VSAction.Clean:
						result = "Cleaning";
						break;

					case VSAction.Rebuild:
						result = "Rebuilding";
						break;

					case VSAction.Deploy:
						result = "Deploying";
						break;

					default:
						result = "Building";
						break;
				}

				return result;
			}
		}

		#endregion

		#region Public Methods

		public override bool Execute(StepExecuteArgs args)
		{
			bool result = true;

			this.stepExecuteArgs = args as VSStepExecuteArgs;
			try
			{
				// Get the list of configurations to build.
				VSConfigurationList configurations = this.GetExecutableConfigurations();
				bool overrideConfigurations = this.Project.OverrideVSStepConfigurations;

				string? slnName = Path.GetFileNameWithoutExtension(Manager.ExpandVariables(this.Solution));

				// Build each configuration.
				int numConfigurations = configurations.Count;
				string actionDescription = this.ActionDescription;
				for (int i = 0; i < numConfigurations; i++)
				{
					if (this.StopBuilding)
					{
						result = false;
						break;
					}

					bool buildConfiguration = configurations.GetState(i);
					if (buildConfiguration)
					{
						string configuration = configurations.GetName(i);

						// Only build an override config if it is in the current SLN's configurations
						if (!overrideConfigurations || this.Configurations.Contains(configuration))
						{
							string message = string.Format("{0} {1} - {2} Configuration", actionDescription, slnName, configuration);
							this.Project.SendBuildProgressMessage(message);

							VSVersionInfo executableVersion = this.GetExecutableVersion();

							// Don't use the .exe.  Use DevEnv.com so we can redirect the output properly.
							string command = this.GenerateCommand(executableVersion, false);
							string arguments = this.GenerateArguments(executableVersion, configuration);

							ExecuteCommandArgs cmdArgs = new()
							{
								FileName = command,
								Arguments = arguments,
								WindowStyle = this.WindowState,
								RedirectStandardStreams = this.RedirectStreams ? RedirectStandardStreams.All : RedirectStandardStreams.None,
							};

							this.IgnorePackageLoadErrors(cmdArgs);

							using (VSHubWatcher watcher = new(executableVersion))
							{
								if (!this.ExecuteCommand(cmdArgs))
								{
									result = false;
									break;
								}
							}
						}
					}
				}
			}
			finally
			{
				// It technically shouldn't hurt anything to hold onto this,
				// but I want to make sure we're not holding any external
				// objects that we shouldn't be.
				this.stepExecuteArgs = null;
			}

			return result;
		}

		public override void ExecuteCustomVerb(string verb)
		{
			switch (verb)
			{
				case "Open Solution":
					// Use .exe so it won't popup a command window
					using (Process.Start(this.GenerateCommand(this.GetExecutableVersion(), true), this.GenerateSolutionPath()))
					{
						// We'll dispose of the process handle, but we'll leave the process running.
					}

					break;

				case "Build Solution":
					this.ExecuteActionVerb(VSAction.Build);
					break;

				case "Rebuild Solution":
					this.ExecuteActionVerb(VSAction.Rebuild);
					break;

				case "Clean Solution":
					this.ExecuteActionVerb(VSAction.Clean);
					break;

				case "Deploy Solution":
					this.ExecuteActionVerb(VSAction.Deploy);
					break;
			}
		}

		public override string[] GetCustomVerbs()
		{
			string[] result;

			if (this.Project.Building)
			{
				result = new string[] { "Open Solution" };
			}
			else
			{
				result = new string[] { "Open Solution", "Build Solution", "Rebuild Solution", "Clean Solution", "Deploy Solution" };
			}

			return result;
		}

		[SuppressMessage("Usage", "CC0022:Should dispose object", Justification = "Caller disposes new controls.")]
		public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
		{
			base.GetStepEditorControls(controls);
			controls.Add(new VSStepCtrl { Step = this });
		}

		public override void ProjectOptionsChanged()
		{
			this.UpdateStepInformation(true);
		}

		#endregion

		#region Internal Methods

		internal void GetConfigurationsFromListView(ListView listView)
		{
			if (this.configurations.GetFromListView(listView))
			{
				this.SetModified();
			}
		}

		internal void PutConfigurationsInListView(ListView listView)
		{
			this.configurations.PutInListView(listView);
		}

		#endregion

		#region Protected Methods

		protected internal override void Load(XmlKey key)
		{
			base.Load(key);
			this.Solution = key.GetValue(nameof(this.Solution), this.solution);
			this.Action = key.GetValue(nameof(this.Action), this.action);
			this.Version = key.GetValue(nameof(this.Version), this.version);
			this.DevEnvArguments = key.GetValue(nameof(this.DevEnvArguments), this.devEnvArguments);
			this.WindowState = key.GetValue(nameof(this.WindowState), this.windowState);
			this.configurations.Load(key.GetSubkey(nameof(this.Configurations)));

			// In 1.0.5, I changed the redirect to all streams, so this name changed.
			// But I need to still read in old project files correctly.
			this.RedirectStreams = key.GetValue(
				key.ValueExists("RedirectOutput") ? "RedirectOutput" : nameof(this.RedirectStreams),
				this.redirectStreams);
			this.UpdateStepInformation(false);
		}

		protected internal override void Save(XmlKey key)
		{
			base.Save(key);
			key.SetValue(nameof(this.Solution), this.solution);
			key.SetValue(nameof(this.Action), this.action);
			key.SetValue(nameof(this.Version), this.version);
			key.SetValue(nameof(this.RedirectStreams), this.redirectStreams);
			key.SetValue(nameof(this.DevEnvArguments), this.devEnvArguments);
			key.SetValue(nameof(this.WindowState), this.windowState);
			this.configurations.Save(key.GetSubkey(nameof(this.Configurations)));
		}

		protected override void StepEdited()
		{
			this.UpdateStepInformation(false);
		}

		#endregion

		#region Private Members

		private static void AppendShortConfigurationName(StringBuilder sb, string configuration)
		{
			string[] words = configuration.Split(' ', '|');
			int numWords = words.Length;
			for (int i = 0; i < numWords; i++)
			{
				string word = words[i];

				// Only append up to the first 3 characters.  Handle some common words specially.
				const int ShortPrefixLength = 3;
				if (word == "Debug")
				{
					sb.Append("Dbg");
				}
				else if (word == ".NET")
				{
					sb.Append("NET");
				}
				else if (word == "Platforms")
				{
					sb.Append("Plt");
				}
				else if (word.Length > ShortPrefixLength)
				{
					sb.Append(word.Substring(0, ShortPrefixLength));
				}
				else
				{
					sb.Append(word);
				}

				if (i < (numWords - 1))
				{
					sb.Append(' ');
				}
			}
		}

		private void ExecuteActionVerb(VSAction verbAction)
		{
			this.Project.Build(new Step[] { this }, true, new VSStepExecuteArgs(verbAction));
		}

		private string GenerateArguments(VSVersionInfo executableVersion, string configuration)
		{
			const int ExtraBufferLength = 50;
			StringBuilder sb = new(configuration.Length + this.Solution.Length + this.DevEnvArguments.Length + ExtraBufferLength);

			// Hide the logo.  This only works on 2003 and up.
			if (executableVersion.Version != VSVersion.V2002)
			{
				sb.Append("/nologo ");
			}

			// Action
			switch (this.GetExecutableAction())
			{
				case VSAction.Clean:
					sb.Append("/clean ");
					break;

				case VSAction.Rebuild:
					sb.Append("/rebuild ");
					break;

				case VSAction.Deploy:
					sb.Append("/deploy ");
					break;

				default:
					sb.Append("/build ");
					break;
			}

			// Configuration
			sb.Append(TextUtility.EnsureQuotes(configuration)).Append(' ');

			// DevEnvArguments - Don't quote these.  That's the user's job.
			if (!string.IsNullOrEmpty(this.DevEnvArguments))
			{
				sb.Append(Manager.ExpandVariables(this.DevEnvArguments)).Append(' ');
			}

			// Solution
			sb.Append(this.GenerateSolutionPath());

			return sb.ToString();
		}

		private string GenerateCommand(VSVersionInfo executableVersion, bool useExe)
		{
			if (!executableVersion.TryGetDevEnvPath(useExe, out string? devEnvPath))
			{
				string message =
					string.IsNullOrEmpty(devEnvPath)
					? $"{executableVersion.FullDisplayName} does not appear to be installed."
					: $"{executableVersion.FullDisplayName} does not appear to be installed since \"{devEnvPath}\" does not exist.";
				this.Project.OutputLine(message, OutputColors.Warning, 0, true);
			}

			return TextUtility.EnsureQuotes(devEnvPath ?? string.Empty);
		}

		private string GenerateSolutionPath() => TextUtility.EnsureQuotes(Manager.ExpandVariables(this.Solution));

		private VSAction GetExecutableAction()
		{
			VSAction result;

			if (this.stepExecuteArgs != null)
			{
				result = this.stepExecuteArgs.Action;
			}
			else
			{
				result = this.Project.OverrideVSActions ? this.Project.OverrideVSAction : this.Action;
			}

			return result;
		}

		private VSConfigurationList GetExecutableConfigurations()
			=> this.Project.OverrideVSStepConfigurations ? this.Project.OverrideConfigurations : this.Configurations;

		private VSVersionInfo GetExecutableVersion() => VSVersionInfo.GetInfo(this.Project.OverrideVSVersions ? this.Project.OverrideVSVersion : this.Version);

		private void UpdateStepInformation(bool sendChangeNotification)
		{
			StringBuilder sb = new();
			sb.Append(this.GetExecutableVersion().DisplayNumber);
			sb.Append(' ');
			sb.Append(this.GetExecutableAction());
			sb.Append(": ");

			VSConfigurationList configurations = this.GetExecutableConfigurations();
			bool overrideConfigurations = this.Project.OverrideVSStepConfigurations;

			// Build each configuration.
			int numConfigurations = configurations.Count;
			bool firstConfig = true;
			for (int i = 0; i < numConfigurations; i++)
			{
				bool buildConfiguration = configurations.GetState(i);
				if (buildConfiguration)
				{
					string configuration = configurations.GetName(i);

					// Only build an override config if it is in the current SLN's configurations
					if (!overrideConfigurations || this.Configurations.Contains(configuration))
					{
						if (!firstConfig)
						{
							sb.Append(", ");
						}

						firstConfig = false;
						AppendShortConfigurationName(sb, configuration);
					}
				}
			}

			string info = sb.ToString();

			if (info != this.stepInformation)
			{
				this.stepInformation = info;

				if (sendChangeNotification)
				{
					// This setting isn't persisted, so it shouldn't call SetModified.
					// But we do need to send notifications that the step was edited.
					this.Project.RuntimeStepValueChanged(this);
				}
			}
		}

		private void IgnorePackageLoadErrors(ExecuteCommandArgs cmdArgs)
		{
			string[]? packageErrorsToIgnore = null;
			switch (this.Version)
			{
				case VSVersion.V2010:
					// Get rid of an annoying "faux" error in VS 2010's stderr because Microsoft won't fix it,
					// and I'm tired of people asking me about it.
					// http://connect.microsoft.com/VisualStudio/feedback/details/636760/ (continued on next line...
					// 	...) devenv-com-fails-with-visualstudio-qualitytools-testcasemanagement
					// http://social.msdn.microsoft.com/Forums/eu/tfsbuild/thread/cbfb80ed-0c8f-4f2a-889c-635ccca9db8c
					packageErrorsToIgnore = new[]
					{
						"Package 'Microsoft.VisualStudio.TestTools.TestCaseManagement.QualityToolsPackage, " +
						"Microsoft.VisualStudio.QualityTools.TestCaseManagement, Version=10.0.0.0, Culture=neutral, " +
						"PublicKeyToken=b03f5f7f11d50a3a' failed to load.",
					};
					break;

				case VSVersion.V2015:
					packageErrorsToIgnore = new[]
					{
						// As of VS 2015, Microsoft has a new "faux" error, "Package 'Code Analysis Package' failed to load."
						// even though Code Analysis does load and runs fine.
						"Package 'Code Analysis Package' failed to load.",

						// This shows up when building solutions for analyzers (e.g., with vsix projects).
						"Package 'TestWindowPackage' failed to load.",
					};
					break;

				case VSVersion.V2019:
					packageErrorsToIgnore = new[]
					{
						"Package 'Task Status Center' failed to load.",
						"Package 'Operation Progress Service Package' failed to load.",
					};
					break;
			}

			if (packageErrorsToIgnore != null)
			{
				bool hasErrorOutput = false;
				cmdArgs.AllowErrorLine = line =>
				{
					// Don't allow a package error or the blank line that appears before them when they're the first error output.
					bool allowed = !packageErrorsToIgnore.Contains(line) && (hasErrorOutput || !string.IsNullOrEmpty(line));
					hasErrorOutput = allowed;
					return allowed;
				};
			}
		}

		#endregion

		#region Private Types

		private class VSStepExecuteArgs : StepExecuteArgs
		{
			#region Constructors

			public VSStepExecuteArgs(VSAction action)
			{
				this.Action = action;
			}

			#endregion

			#region Public Properties

			public VSAction Action { get; }

			#endregion
		}

		#endregion
	}
}
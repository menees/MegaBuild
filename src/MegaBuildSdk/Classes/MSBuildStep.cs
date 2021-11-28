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
	using System.Runtime.InteropServices;
	using System.Text;
	using Menees;

	#endregion

	[StepDisplay("MSBuild", "Builds one or more targets in an MSBuild project.", "Images.MSBuildStep.ico")]
	[MayRequireAdministrator]
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Called by Reflection.")]
	internal sealed class MSBuildStep : ExecutableStep
	{
		#region Private Data Members

		private string commandLineOptions;
		private string projectFile;
		private Dictionary<string, string> properties;
		private string[] targets;
		private MSBuildToolsVersion toolsVersion;
		private bool use32BitProcess;
		private MSBuildVerbosity verbosity = MSBuildVerbosity.Normal;
		private string workingDirectory;

		#endregion

		#region Constructors

		public MSBuildStep(Project project, StepCategory category, StepTypeInfo info)
			: base(project, category, info)
		{
		}

		#endregion

		#region Public Properties

		public string CommandLineOptions
		{
			get => this.commandLineOptions;
			set => this.SetValue(ref this.commandLineOptions, value);
		}

		public string ProjectFile
		{
			get => this.projectFile;
			set => this.SetValue(ref this.projectFile, value);
		}

		public Dictionary<string, string> Properties
		{
			get => this.properties ?? new Dictionary<string, string>();
			set
			{
				var currentValues = this.Properties;
				var newValues = value ?? new Dictionary<string, string>();
				bool areEqual = currentValues.Count == newValues.Count;
				if (areEqual)
				{
					foreach (KeyValuePair<string, string> pair in currentValues)
					{
						if (!newValues.TryGetValue(pair.Key, out string newValue) || pair.Value != newValue)
						{
							areEqual = false;
							break;
						}
					}
				}

				if (!areEqual)
				{
					this.properties = value;
					this.SetModified();
				}
			}
		}

		public override string StepInformation
		{
			get
			{
				string result = string.Empty;

				if (this.targets != null && this.targets.Length > 0)
				{
					result = string.Join(", ", this.targets);
				}

				return result;
			}
		}

		public string[] Targets
		{
			get => this.targets ?? CollectionUtility.EmptyArray<string>();
			set
			{
				HashSet<string> currentValues = this.targets != null ? new HashSet<string>(this.targets) : new HashSet<string>();
				HashSet<string> newValues = value != null ? new HashSet<string>(value) : new HashSet<string>();
				var areEqual = currentValues.Count == newValues.Count && currentValues.IsSubsetOf(newValues);
				if (!areEqual)
				{
					this.targets = value;
					this.SetModified();
				}
			}
		}

		public MSBuildToolsVersion ToolsVersion
		{
			get => this.toolsVersion;
			set => this.SetValue(ref this.toolsVersion, value);
		}

		public bool Use32BitProcess
		{
			get => this.use32BitProcess;
			set => this.SetValue(ref this.use32BitProcess, value);
		}

		public MSBuildVerbosity Verbosity
		{
			get => this.verbosity;
			set => this.SetValue(ref this.verbosity, value);
		}

		public string WorkingDirectory
		{
			get => this.workingDirectory;
			set => this.SetValue(ref this.workingDirectory, value);
		}

		#endregion

		#region Public Methods

		public override bool Execute(StepExecuteArgs args)
		{
			bool result = false;

			if (!this.StopBuilding)
			{
				string directory;
				switch (this.ToolsVersion)
				{
					case MSBuildToolsVersion.Twelve:
						directory = this.GetProgramFilesMsBuildBinPath("12.0");
						break;

					case MSBuildToolsVersion.Fourteen:
						directory = this.GetProgramFilesMsBuildBinPath("14.0");
						break;

					case MSBuildToolsVersion.Fifteen:
						directory = this.GetVsMsBuildBinPath("15.0", VSVersion.V2017);
						break;

					case MSBuildToolsVersion.Sixteen:
						directory = this.GetVsMsBuildBinPath("Current", VSVersion.V2019);
						break;

					case MSBuildToolsVersion.Seventeen:
						directory = this.GetVsMsBuildBinPath("Current", VSVersion.V2022);
						break;

					case MSBuildToolsVersion.Current:
						directory = this.GetVsMsBuildBinPath("Current", VSVersion.V2022, VSVersion.V2019);
						break;

					default:
						directory = RuntimeEnvironment.GetRuntimeDirectory();
						if (Environment.Is64BitProcess && this.Use32BitProcess)
						{
							directory = directory.Replace(@"\Framework64\", @"\Framework\");
						}

						break;
				}

				string exePath = Path.Combine(directory, "MSBuild.exe");
				string commandLineArguments = this.BuildCommandLineArguments();

				ExecuteCommandArgs cmdArgs = new()
				{
					FileName = exePath,
					Arguments = commandLineArguments,
					WorkingDirectory = this.WorkingDirectory,
					WindowStyle = ProcessWindowStyle.Hidden,
					RedirectStandardStreams = RedirectStandardStreams.All,
				};

				result = this.ExecuteCommand(cmdArgs);

				if (!result)
				{
					this.Project.OutputLine("MSBuild returned exit code: " + cmdArgs.ExitCode, OutputColors.Heading);
				}
			}

			return result;
		}

		[SuppressMessage("Usage", "CC0022:Should dispose object", Justification = "Caller disposes new controls.")]
		public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
		{
			base.GetStepEditorControls(controls);
			controls.Add(new MSBuildStepCtrl { Step = this });
		}

		#endregion

		#region Protected Methods

		protected internal override void Load(XmlKey key)
		{
			base.Load(key);

			this.ProjectFile = key.GetValue(nameof(this.ProjectFile), this.ProjectFile);
			this.WorkingDirectory = key.GetValue(nameof(this.WorkingDirectory), this.WorkingDirectory);
			this.Verbosity = key.GetValue(nameof(this.Verbosity), this.verbosity);
			this.ToolsVersion = key.GetValue(nameof(this.ToolsVersion), this.toolsVersion);
			this.CommandLineOptions = key.GetValue(nameof(this.CommandLineOptions), this.commandLineOptions);
			this.Use32BitProcess = key.GetValue(nameof(this.Use32BitProcess), this.use32BitProcess);

			List<string> targets = new();
			XmlKey targetsKey = key.GetSubkey(nameof(this.Targets));
			foreach (XmlKey targetKey in targetsKey.GetSubkeys())
			{
				Debug.Assert(targetKey.KeyType == "Target", "Key type must be Target.");

				string target = targetKey.GetValue(nameof(this.Name), string.Empty);
				if (!string.IsNullOrEmpty(target))
				{
					targets.Add(target);
				}
			}

			this.Targets = targets.ToArray();

			Dictionary<string, string> properties = new();
			XmlKey propertiesKey = key.GetSubkey(nameof(this.Properties));
			foreach (XmlKey propertyKey in propertiesKey.GetSubkeys())
			{
				Debug.Assert(propertyKey.KeyType == "Property", "Key type must be Property.");

				string name = propertyKey.GetValue(nameof(this.Name), string.Empty);
				if (!string.IsNullOrEmpty(name))
				{
					string value = propertyKey.GetValue("Value", string.Empty);
					properties[name] = value;
				}
			}

			this.Properties = properties;
		}

		protected internal override void Save(XmlKey key)
		{
			base.Save(key);

			key.SetValue(nameof(this.ProjectFile), this.ProjectFile);
			key.SetValue(nameof(this.WorkingDirectory), this.WorkingDirectory);
			key.SetValue(nameof(this.Verbosity), this.Verbosity);
			key.SetValue(nameof(this.ToolsVersion), this.ToolsVersion);
			key.SetValue(nameof(this.CommandLineOptions), this.CommandLineOptions);
			key.SetValue(nameof(this.Use32BitProcess), this.Use32BitProcess);

			var targets = this.Targets;
			XmlKey targetsKey = key.GetSubkey(nameof(this.Targets));
			int numTargets = targets.Length;
			for (int i = 0; i < numTargets; i++)
			{
				XmlKey targetKey = targetsKey.GetSubkey("Target", i.ToString());
				targetKey.SetValue(nameof(this.Name), targets[i]);
			}

			var properties = this.Properties.ToList();
			XmlKey propertiesKey = key.GetSubkey(nameof(this.Properties));
			int numKeys = properties.Count;
			for (int i = 0; i < numKeys; i++)
			{
				XmlKey propertyKey = propertiesKey.GetSubkey("Property", i.ToString());
				var pair = properties[i];
				propertyKey.SetValue(nameof(this.Name), pair.Key);
				propertyKey.SetValue("Value", pair.Value);
			}
		}

		#endregion

		#region Private Methods

		private string BuildCommandLineArguments()
		{
			StringBuilder sb = new();
			sb.Append("/nologo ");

			var targets = this.Targets;
			if (targets.Length > 0)
			{
				foreach (string target in targets)
				{
					// Use quotes in case the target name contains a space.
					sb.Append("\"/t:").Append(target).Append("\" ");
				}
			}

			var properties = this.Properties;
			if (properties.Count > 0)
			{
				foreach (var pair in properties)
				{
					// Use quotes in case the property name or value contains a space.
					sb.Append("\"/p:").Append(pair.Key).Append('=').Append(pair.Value).Append("\" ");
				}
			}

			switch (this.verbosity)
			{
				case MSBuildVerbosity.Quiet:
					sb.Append("/v:q ");
					break;

				case MSBuildVerbosity.Minimal:
					sb.Append("/v:m ");
					break;

				case MSBuildVerbosity.Detailed:
					sb.Append("/v:d ");
					break;

				case MSBuildVerbosity.Diagnostic:
					sb.Append("/v:diag ");
					break;
			}

			switch (this.toolsVersion)
			{
				case MSBuildToolsVersion.Two:
					sb.Append("/ToolsVersion:2.0 ");
					break;

				case MSBuildToolsVersion.Three:
					sb.Append("/ToolsVersion:3.0 ");
					break;

				case MSBuildToolsVersion.ThreeFive:
					sb.Append("/ToolsVersion:3.5 ");
					break;

				case MSBuildToolsVersion.Four:
					sb.Append("/ToolsVersion:4.0 ");
					break;

				case MSBuildToolsVersion.Twelve:
					sb.Append("/ToolsVersion:12.0 ");
					break;

				case MSBuildToolsVersion.Fourteen:
					sb.Append("/ToolsVersion:14.0 ");
					break;

				case MSBuildToolsVersion.Fifteen:
					sb.Append("/ToolsVersion:15.0 ");
					break;

				case MSBuildToolsVersion.Sixteen:
				case MSBuildToolsVersion.Seventeen:
				case MSBuildToolsVersion.Current:
					sb.Append("/ToolsVersion:Current ");
					break;
			}

			if (!string.IsNullOrEmpty(this.commandLineOptions))
			{
				sb.Append(this.commandLineOptions).Append(' ');
			}

			sb.Append(TextUtility.EnsureQuotes(this.ProjectFile));

			return sb.ToString();
		}

		private string GetProgramFilesMsBuildBinPath(string version)
		{
			var programFilesFolder = Environment.Is64BitOperatingSystem ? Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles;
			string programFilesPath = Environment.GetFolderPath(programFilesFolder);
			string msbuildVersionBinPath = Path.Combine(programFilesPath, "MSBuild", version, "Bin");
			string result = Environment.Is64BitProcess && !this.Use32BitProcess ? Path.Combine(msbuildVersionBinPath, "amd64") : msbuildVersionBinPath;
			return result;
		}

		private string GetVsMsBuildBinPath(string msbuildVersion, params VSVersion[] vsVersionsInPreferenceOrder)
		{
			VSVersionInfo info = null;
			bool foundDevEnv = false;
			string devEnvPath = null;
			foreach (VSVersion vsVersion in vsVersionsInPreferenceOrder)
			{
				info = VSVersionInfo.AllVersions.First(ver => ver.Version == vsVersion);

				// This will always return some path, even if it's just an incorrect guess.
				// This will also find preview/prerelease editions (just like the dotnet CLI app does).
				foundDevEnv = info.TryGetDevEnvPath(true, out devEnvPath);
				if (foundDevEnv)
				{
					break;
				}
			}

			string ideFolder = Path.GetDirectoryName(Environment.ExpandEnvironmentVariables(devEnvPath));
			string relativePath = Path.Combine(ideFolder, @"..\..\MSBuild", msbuildVersion, "Bin");
			string result = Path.GetFullPath(relativePath);
			if (!this.Use32BitProcess)
			{
				result = Path.Combine(result, "amd64");
			}

			if (!foundDevEnv)
			{
				string message =
					$"MSBuild {msbuildVersion} cannot be found at {result}.  The {info.FullDisplayName} build tools do not appear to be installed" +
					(string.IsNullOrEmpty(result) ? $"." : $" since \"{devEnvPath}\" does not exist.");
				this.Project.OutputLine(message, OutputColors.Warning, 0, true);
			}

			return result;
		}

		#endregion
	}
}
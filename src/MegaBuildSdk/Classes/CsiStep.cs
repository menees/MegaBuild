namespace MegaBuild;

#region Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Menees;

#endregion

[StepDisplay("C# Interactive", "Runs a C# Interactive script.", "Images.CsiStep.ico")]
[MayRequireAdministrator]
[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Called by Reflection.")]
internal sealed class CsiStep : ExecutableStep
{
	#region Internal Constants

	internal const MSBuildToolsVersion MinSupportedCsiVersion = MSBuildToolsVersion.Sixteen;

	#endregion

	#region Private Data Members

	private string? scriptFile;
	private string? scriptOptions;
	private MSBuildToolsVersion csiVersion = MSBuildToolsVersion.Current;
	private string? workingDirectory;
	private bool treatErrorStreamAsOutput;
	private string? csiOptions;

	#endregion

	#region Constructors

	public CsiStep(Project project, StepCategory category, StepTypeInfo info)
		: base(project, category, info)
	{
	}

	#endregion

	#region Public Properties

	public string? ScriptFile
	{
		get => this.scriptFile;
		set => this.SetValue(ref this.scriptFile, value);
	}

	public string? ScriptArguments
	{
		get => this.scriptOptions;
		set => this.SetValue(ref this.scriptOptions, value);
	}

	public MSBuildToolsVersion CsiVersion
	{
		get => this.csiVersion;
		set => this.SetValue(ref this.csiVersion, value);
	}

	public string? CsiOptions
	{
		get => this.csiOptions;
		set => this.SetValue(ref this.csiOptions, value);
	}

	public string? WorkingDirectory
	{
		get => this.workingDirectory;
		set => this.SetValue(ref this.workingDirectory, value);
	}

	public bool TreatErrorStreamAsOutput
	{
		get => this.treatErrorStreamAsOutput;
		set => this.SetValue(ref this.treatErrorStreamAsOutput, value);
	}

	#endregion

	#region Public Methods

	public override bool Execute(StepExecuteArgs args)
	{
		bool result = false;

		if (!this.StopBuilding)
		{
			string exePath = this.CsiVersion switch
			{
				MinSupportedCsiVersion => this.GetCsiPath(VSVersion.V2019),
				MSBuildToolsVersion.Seventeen => this.GetCsiPath(VSVersion.V2022),
				_ => this.GetCsiPath(VSVersion.V2022, VSVersion.V2019),
			};

			string commandLineArguments = this.BuildCommandLineArguments();

			ExecuteCommandArgs cmdArgs = new()
			{
				FileName = exePath,
				Arguments = commandLineArguments,
				WorkingDirectory = this.WorkingDirectory,
				WindowStyle = ProcessWindowStyle.Hidden,
				RedirectStandardStreams = RedirectStandardStreams.All,
			};

			if (this.TreatErrorStreamAsOutput)
			{
				cmdArgs.RedirectStandardStreams |= RedirectStandardStreams.TreatErrorAsOutput;
			}

			result = this.ExecuteCommand(cmdArgs);

			if (!result)
			{
				this.Project.OutputLine("csi.exe returned exit code: " + cmdArgs.ExitCode, OutputColors.Heading);
			}
		}

		return result;
	}

	[SuppressMessage("Usage", "CC0022:Should dispose object", Justification = "Caller disposes new controls.")]
	public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
	{
		base.GetStepEditorControls(controls);
		controls.Add(new CsiStepCtrl { Step = this });
	}

	public override void ExecuteCustomVerb(string verb)
	{
		switch (verb)
		{
			case "Open Script":
				SystemUtility.TryOpenFile(this.ScriptFile);
				break;

			case "Open Script Folder In Explorer":
				SystemUtility.TryOpenExplorerForFile(this.ScriptFile);
				break;

			case "Open Script Folder In Terminal":
				SystemUtility.TryOpenTerminalForFile(this.ScriptFile);
				break;

			case "Open Working Directory In Explorer":
				SystemUtility.TryOpenExplorerForFolder(this.WorkingDirectory);
				break;

			case "Open Working Directory In Terminal":
				SystemUtility.TryOpenTerminalForFolder(this.WorkingDirectory);
				break;
		}
	}

	public override string[]? GetCustomVerbs()
	{
		List<string> result = ["Open Script", SeparatorVerb, "Open Script Folder In Explorer", "Open Script Folder In Terminal"];

		if (AreFoldersDifferent(this.ScriptFile, this.WorkingDirectory))
		{
			result.AddRange(["Open Working Directory In Explorer", "Open Working Directory In Terminal"]);
		}

		return [.. result];
	}

	#endregion

	#region Protected Methods

	protected internal override void Load(XmlKey key)
	{
		base.Load(key);
		this.ScriptFile = key.GetValueN(nameof(this.ScriptFile), this.ScriptFile);
		this.ScriptArguments = key.GetValueN(nameof(this.ScriptArguments), this.ScriptArguments);
		this.WorkingDirectory = key.GetValueN(nameof(this.WorkingDirectory), this.WorkingDirectory);
		this.CsiOptions = key.GetValueN(nameof(this.CsiOptions), this.CsiOptions);
		this.CsiVersion = key.GetValue(nameof(this.CsiVersion), this.CsiVersion);
		this.TreatErrorStreamAsOutput = key.GetValue(nameof(this.TreatErrorStreamAsOutput), this.TreatErrorStreamAsOutput);
	}

	protected internal override void Save(XmlKey key)
	{
		base.Save(key);
		key.SetValue(nameof(this.ScriptFile), this.ScriptFile);
		key.SetValue(nameof(this.ScriptArguments), this.ScriptArguments);
		key.SetValue(nameof(this.WorkingDirectory), this.WorkingDirectory);
		key.SetValue(nameof(this.CsiOptions), this.CsiOptions);
		key.SetValue(nameof(this.CsiVersion), this.CsiVersion);
		key.SetValue(nameof(this.TreatErrorStreamAsOutput), this.TreatErrorStreamAsOutput);
	}

	#endregion

	#region Private Methods

	private string GetCsiPath(params VSVersion[] vsVersionsInPreferenceOrder)
		=> VSVersionInfo.GetUtilityPath(this.Project, "C# Interactive Current", @"..\..\Msbuild\Current\Bin\Roslyn\csi.exe", vsVersionsInPreferenceOrder);

	private string BuildCommandLineArguments()
	{
		StringBuilder sb = new();

		void Append(string? argument, bool ensureQuotes = false)
		{
			if (argument.IsNotWhiteSpace())
			{
				if (sb.Length > 0)
				{
					sb.Append(' ');
				}

				if (ensureQuotes)
				{
					sb.Append(TextUtility.EnsureQuotes(argument));
				}
				else
				{
					// The step editor UI may allow newlines in options and arguments, but we need to linearize them for the command line.
					string singleLine = argument.Replace(Environment.NewLine, " ").Replace("\n", " ").Replace("\r", string.Empty);
					sb.Append(singleLine);
				}
			}
		}

		Append(this.CsiOptions);
		Append(this.ScriptFile, ensureQuotes: true);
		Append(this.ScriptArguments);

		string result = sb.ToString();
		return result;
	}

	#endregion
}

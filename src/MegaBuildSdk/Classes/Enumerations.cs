namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.ComponentModel;

	#endregion

	#region public BuildOptions

	[Flags]
	public enum BuildOptions
	{
		None = 0,

		ForceStepsToBeIncludedInBuild = 1,

		AutoConfirmSteps = 2
	}

	#endregion

	#region public BuildStatus

	public enum BuildStatus
	{
		None,

		Started,

		Failing,

		Stopped,

		Succeeded,

		Failed
	}

	#endregion

	#region public ExecSupports

	[Flags]
	public enum ExecSupports
	{
		None = 0,

		WaitForCompletion = 1,

		Timeout = 2,

		AutoColorErrorsAndWarnings = 4,

		All = WaitForCompletion | Timeout | AutoColorErrorsAndWarnings
	}

	#endregion

	#region public ProjectStepsChangedType

	public enum ProjectStepsChangedType
	{
		StepInserted,

		StepEdited,

		StepDeleted,

		StepMoved
	}

	#endregion

	#region public RedirectStandardStreams

	[Flags]
	public enum RedirectStandardStreams
	{
		None = 0,

		Input = 1,

		Output = 2,

		Error = 4,

		All = Input | Output | Error
	}

	#endregion

	#region public StepCategory

	public enum StepCategory
	{
		Build,

		Failure
	}

	#endregion

	#region public StepStatus

	public enum StepStatus
	{
		None,

		Executing,

		Succeeded,

		Failed,

		Skipped,

		Canceled,

		TimedOut
	}

	#endregion

	#region internal MSBuildToolsVersion

	// NOTE: When you add support for a new .NET version, you must:
	// 		Check whether the /ToolsVersion switch for MSBuild.exe supports a new version.
	// 			Note: .NET 4.5 and 4.6 didn't add a new ToolsVersion,
	// 			But: VS 2013 and 2015 did (12.0 and 14.0).  See C:\Program Files (x86)\MSBuild.
	// 			http://blogs.msdn.com/b/visualstudio/archive/2013/07/24/msbuild-is-now-part-of-visual-studio.aspx
	// 		Add a new enum field below with its Description attribute set to the ToolsVersion value.
	internal enum MSBuildToolsVersion
	{
		Default,

		[Description("2.0")]
		Two,

		[Description("3.0")]
		Three,

		[Description("3.5")]
		ThreeFive,

		[Description("4.0")]
		Four,

		[Description("12.0")]
		Twelve,

		[Description("14.0")]
		Fourteen,

		[Description("15.0")]
		Fifteen,

		Current,
	}

	#endregion

	#region internal MSBuildVerbosity

	internal enum MSBuildVerbosity
	{
		Quiet,

		Minimal,

		Normal,

		Detailed,

		Diagnostic
	}

	#endregion

	#region internal enum SoundStyle

	internal enum SoundStyle
	{
		SystemSound,

		WavFile,

		Beep
	}

	#endregion

	#region internal SystemSound

	internal enum SystemSound
	{
		// NOTE: These fields have to keep their original integral values,
		// so we can correctly read in values from older .mgb files.
		Default = 0,

		Question = 0x20,

		Error = 0x10,

		Warning = 0x30,

		Information = 0x40,

		Simple = -1
	}

	#endregion

	#region internal VSAction

	internal enum VSAction
	{
		Build,

		Rebuild,

		Clean,

		Deploy
	}

	#endregion

	#region internal VSVersion

	// NOTE: When you add support for a new VS version, you must:
	// 		Add a new VSVersionInfo instance to its AllVersions collection.
	// 		Search for all places that VSVersion is used (Entire Solution; Match whole word only).
	// 		Search for all places 2002 is used.
	// 		Search for all places the last enum field is used (e.g., V2017).
	// 		Update MegaBuild.docx.
	// 		Update MegaBuild.htm on the web site.
	// 		See comments on MSBuildToolsVersion above.
	internal enum VSVersion
	{
		V2002,

		V2003,

		V2005,

		V2008,

		V2010,

		V2012,

		V2013,

		V2015,

		V2017,

		V2019,
	}

	#endregion
}
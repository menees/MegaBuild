namespace MegaBuild;

#region Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Menees;
using Menees.Windows;
using Microsoft.VisualStudio.Setup.Configuration;

#endregion

[DebuggerDisplay("{FullDisplayName}")]
internal sealed class VSVersionInfo
{
	#region Public Fields

	public static readonly IReadOnlyList<VSVersionInfo> AllVersions =
	[
		new VSVersionInfo(VSVersion.V2002, "%VSCOMNTOOLS%", 7, 7),
		new VSVersionInfo(VSVersion.V2003, "%VS71COMNTOOLS%", 7.1m, 8),
		new VSVersionInfo(VSVersion.V2005, "%VS80COMNTOOLS%", 8, 9),
		new VSVersionInfo(VSVersion.V2008, "%VS90COMNTOOLS%", 9, 10),
		new VSVersionInfo(VSVersion.V2010, "%VS100COMNTOOLS%", 10, 11),
		new VSVersionInfo(VSVersion.V2012, "%VS110COMNTOOLS%", 11, 12),
		new VSVersionInfo(VSVersion.V2013, "%VS120COMNTOOLS%", 12, 12),
		new VSVersionInfo(VSVersion.V2015, "%VS140COMNTOOLS%", 14, 12, "Visual Studio 14"),
		new VSVersionInfo(VSVersion.V2017, null, 15, 12, "Visual Studio 15"),
		new VSVersionInfo(VSVersion.V2019, null, 16, 12, "Visual Studio Version 16"),
		new VSVersionInfo(VSVersion.V2022, null, 17, 12, "Visual Studio Version 17"),
	];

	public static readonly VSVersionInfo LatestVersion = AllVersions[AllVersions.Count - 1];

	#endregion

	#region Private Data Members

	private static readonly Dictionary<VSVersion, VSVersionInfo> VersionToInfoMap = AllVersions.ToDictionary(v => v.Version);

	#endregion

	#region Constructors

	private VSVersionInfo(VSVersion version, string? variable, decimal internalVersion, int solutionVersion, string? solutionCommentVersion = null)
	{
		this.Version = version;
		this.ToolsVariable = variable;
		this.InternalVersion = internalVersion;
		this.SolutionVersion = solutionVersion;
		this.DisplayNumber = version.ToString().Substring(1);
		this.FullDisplayName = "Visual Studio " + this.DisplayNumber;
		this.SolutionCommentVersion = solutionCommentVersion ?? this.FullDisplayName;
	}

	#endregion

	#region Public Properties

	public string DisplayNumber { get; }

	public string FullDisplayName { get; }

	public decimal InternalVersion { get; }

	public decimal SolutionVersion { get; }

	public VSVersion Version { get; }

	public string SolutionCommentVersion { get; }

	#endregion

	#region Private Properties

	private string? ToolsVariable { get; }

	private bool Is64Bit => this.Version >= VSVersion.V2022;

	private string ProgramFilesVariable => this.Is64Bit ? "%ProgramFiles%" : "%ProgramFiles(x86)%";

	#endregion

	#region Public Methods

	public static VSVersionInfo GetInfo(VSVersion version)
	{
		VSVersionInfo result = VersionToInfoMap[version];
		return result;
	}

	public static string GetUtilityPath(Project project, string displayName, string relativeToDevEnvPath, params VSVersion[] vsVersionsInPreferenceOrder)
	{
		VSVersionInfo? info = null;
		bool foundDevEnv = false;
		string? devEnvPath = null;
		foreach (VSVersion vsVersion in vsVersionsInPreferenceOrder)
		{
			info = GetInfo(vsVersion);

			// This will always return some path, even if it's just an incorrect guess.
			// This will also find preview/prerelease editions (just like the dotnet CLI app does).
			foundDevEnv = info.TryGetDevEnvPath(true, out devEnvPath);
			if (foundDevEnv)
			{
				break;
			}
		}

		string ideFolder = Path.GetDirectoryName(Environment.ExpandEnvironmentVariables(devEnvPath ?? string.Empty)) ?? string.Empty;
		string relativePath = Path.Combine(ideFolder, relativeToDevEnvPath);
		string result = Path.GetFullPath(relativePath);

		if (!foundDevEnv)
		{
			string message =
				$"{displayName} cannot be found at {result}.  The {info?.FullDisplayName} build tools do not appear to be installed" +
				(string.IsNullOrEmpty(result) ? $"." : $" since \"{devEnvPath}\" does not exist.");
			project.OutputLine(message, OutputColors.Warning, 0, true);
		}

		return result;
	}

	public bool TryGetDevEnvPath(bool useExe, [MaybeNullWhen(false)] out string devEnvPath)
	{
		bool result;

		if (!string.IsNullOrEmpty(this.ToolsVariable))
		{
			result = this.TryGetPathFromToolsVariable(useExe, out devEnvPath);
		}
		else
		{
			result = this.TryGetPathFromSetupConfiguration(useExe, out devEnvPath);
		}

		return result;
	}

	#endregion

	#region Private Methods

	private bool TryGetPathFromToolsVariable(bool useExe, out string devEnvPath)
	{
		devEnvPath = Environment.ExpandEnvironmentVariables(this.ToolsVariable ?? string.Empty);
		bool expandedVariable = devEnvPath != this.ToolsVariable;

		if (expandedVariable && this.Version == VSVersion.V2002)
		{
			// VS 2002 surrounded the tool path with double quotes, so we need to strip them off.
			devEnvPath = TextUtility.StripQuotes(devEnvPath);
		}

		string relativeExecutable = @"..\IDE\DevEnv." + (useExe ? "exe" : "com");
		devEnvPath = Path.Combine(devEnvPath, relativeExecutable);
		if (expandedVariable)
		{
			devEnvPath = Path.GetFullPath(devEnvPath);
		}

		bool result = expandedVariable && File.Exists(devEnvPath);
		return result;
	}

	private bool TryGetPathFromSetupConfiguration(bool useExe, [MaybeNullWhen(false)] out string devEnvPath)
	{
		devEnvPath = VisualStudioUtility.ResolvePath(
			ver => @"Common7\IDE\DevEnv." + (useExe ? "exe" : "com"),
			(int)this.InternalVersion,
			(int)this.InternalVersion);

		bool result = !string.IsNullOrEmpty(devEnvPath);

		// This must return a non-null, non-empty path, even if it doesn't exist. VSStep depends on getting back
		// at least an environment variable-based path that it can display to the user.  So we'll make one up here.
		if (!result)
		{
			// Note: "Edition" should actually be Professional, Communitiy, or Enterprise.
			// But there's no point being specific about a non-installed edition.
			devEnvPath = $@"{this.ProgramFilesVariable}\Microsoft Visual Studio\{this.DisplayNumber}\[Missing Edition]\Common7\IDE\devenv.exe";
		}

		return result;
	}

	#endregion
}
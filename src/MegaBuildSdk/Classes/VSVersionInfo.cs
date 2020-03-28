namespace MegaBuild
{
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
	using Microsoft.VisualStudio.Setup.Configuration;

	#endregion

	[DebuggerDisplay("{FullDisplayName}")]
	internal sealed class VSVersionInfo
	{
		#region Public Fields

		public static readonly IReadOnlyList<VSVersionInfo> AllVersions = new[]
		{
			new VSVersionInfo(VSVersion.V2002, "%VSCOMNTOOLS%", 7, 7),
			new VSVersionInfo(VSVersion.V2003, "%VS71COMNTOOLS%", 7.1m, 8),
			new VSVersionInfo(VSVersion.V2005, "%VS80COMNTOOLS%", 8, 9),
			new VSVersionInfo(VSVersion.V2008, "%VS90COMNTOOLS%", 9, 10),
			new VSVersionInfo(VSVersion.V2010, "%VS100COMNTOOLS%", 10, 11),
			new VSVersionInfo(VSVersion.V2012, "%VS110COMNTOOLS%", 11, 12),
			new VSVersionInfo(VSVersion.V2013, "%VS120COMNTOOLS%", 12, 12), // 2013 and 2012 both use v12 .sln.
			new VSVersionInfo(VSVersion.V2015, "%VS140COMNTOOLS%", 14, 12, "Visual Studio 14"), // 2015 also uses v12 .sln.
			new VSVersionInfo(VSVersion.V2017, null, 15, 12, "Visual Studio 15"), // 2017 also uses v12 .sln, but it doesn't define %VS150COMNTOOLS%.
			new VSVersionInfo(VSVersion.V2019, null, 16, 12, "Visual Studio Version 16"), // 2019 also uses v12 .sln, but it doesn't define %VS160COMNTOOLS%.
		};

		public static readonly VSVersionInfo LatestVersion = AllVersions[AllVersions.Count - 1];

		#endregion

		#region Private Data Members

		private static readonly Dictionary<VSVersion, VSVersionInfo> VersionToInfoMap = AllVersions.ToDictionary(v => v.Version);

		#endregion

		#region Constructors

		private VSVersionInfo(VSVersion version, string variable, decimal internalVersion, int solutionVersion, string solutionCommentVersion = null)
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

		public decimal SolutionVersion { get; }

		public VSVersion Version { get; }

		public string SolutionCommentVersion { get; }

		#endregion

		#region Private Properties

		private string ToolsVariable { get; }

		private decimal InternalVersion { get; }

		#endregion

		#region Public Methods

		public static VSVersionInfo GetInfo(VSVersion version)
		{
			VSVersionInfo result = VersionToInfoMap[version];
			return result;
		}

		public bool TryGetDevEnvPath(bool useExe, out string devEnvPath)
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
			devEnvPath = Environment.ExpandEnvironmentVariables(this.ToolsVariable);
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

		[SuppressMessage(
			"Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Microsoft's COM API and samples say to catch all exceptions for the SetupConfiguration objects.")]
		private bool TryGetPathFromSetupConfiguration(bool useExe, out string devEnvPath)
		{
			bool result = false;
			devEnvPath = null;

			// VS 2017 allows multiple side-by-side editions to be installed, so it doesn't define a %VS150COMNTOOLS% environment variable.
			// Instead we have to use a new COM API to enumerate the installed instances of VS.  So we'll just pick the first matching version we find.
			// https://github.com/mluparu/vs-setup-samples - COM API samples
			// https://code.msdn.microsoft.com/Visual-Studio-Setup-0cedd331 - More Q&A about the COM API samples
			// https://blogs.msdn.microsoft.com/vcblog/2017/03/06/finding-the-visual-c-compiler-tools-in-visual-studio-2017/#comment-273625
			// https://github.com/Microsoft/vswhere - A redistributable .exe for enumerating the VS instances from the command line.
			// https://blogs.msdn.microsoft.com/heaths/2016/09/15/changes-to-visual-studio-15-setup/
			const int REGDB_E_CLASSNOTREG = -2147221164; // 0x80040154
			try
			{
				// From MS example: https://github.com/Microsoft/vs-setup-samples/blob/master/Setup.Configuration.CS/Program.cs
				SetupConfiguration configuration = new SetupConfiguration();

				IEnumSetupInstances instanceEnumerator = configuration.EnumAllInstances();
				int fetched;
				ISetupInstance[] instances = new ISetupInstance[1];
				do
				{
					instanceEnumerator.Next(1, instances, out fetched);
					if (fetched > 0)
					{
						ISetupInstance instance = instances[0];
						if (instance != null
							&& System.Version.TryParse(instance.GetInstallationVersion(), out Version version)
							&& version.Major == (int)this.InternalVersion)
						{
							InstanceState state = ((ISetupInstance2)instance).GetState();
							if (state == InstanceState.Complete)
							{
								string relativeExecutable = @"Common7\IDE\DevEnv." + (useExe ? "exe" : "com");
								devEnvPath = instance.ResolvePath(relativeExecutable);
								result = true;
								break;
							}
						}
					}
				}
				while (fetched > 0);
			}
			catch (COMException ex) when (ex.HResult == REGDB_E_CLASSNOTREG)
			{
				// The SetupConfiguration API is not registered, so assume no instances are installed.
			}
			catch (Exception)
			{
				// Heath Stewart (MSFT), the author of the SetupConfiguration API, says to treat any exception as "no instances installed."
				// https://code.msdn.microsoft.com/windowsdesktop/Visual-Studio-Setup-0cedd331/view/Discussions#content
			}

			// This must return a non-null, non-empty path, even if it doesn't exist. VSStep depends on getting back
			// at least an environment variable-based path that it can display to the user.  So we'll make one up here.
			if (string.IsNullOrEmpty(devEnvPath))
			{
				// Note: "Edition" should actually be Professional, Communitiy, or Enterprise.
				// But there's no point being specific about a non-installed edition.
				devEnvPath = $@"%ProgramFiles(x86)%\Microsoft Visual Studio\{this.DisplayNumber}\[Missing Edition]\Common7\IDE\devenv.exe";
			}

			return result;
		}

		#endregion
	}
}
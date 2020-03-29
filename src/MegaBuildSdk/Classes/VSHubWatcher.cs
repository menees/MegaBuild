namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading;
	using Menees.Diagnostics;
	using Menees.Windows.Diagnostics;

	#endregion

	internal sealed class VSHubWatcher : IDisposable
	{
		#region Private Data Members

		private static readonly Version FixedVsHubVersion = new Version(14, 0, 23304, 0);

		private readonly string devEnvPath;

		#endregion

		#region Constructors

		public VSHubWatcher(VSVersionInfo version)
		{
			// If VS 2015's DevEnv.com is run when VsHub.exe isn't running, then when its hidden DevEnv.exe
			// instance runs to build, it will launch VsHub.exe, and DevEnv.com will hang from 1 to 5 minutes
			// waiting for VsHub.exe to timeout and shutdown.  So we'll pre-emptively launch a VsHub.exe
			// process that can be used by the hidden DevEnv.exe instance, and that will allow DevEnv.com
			// to go away without waiting on a child VsHub.exe process to exit.
			//
			// Note: We'll also check for a MegaBuild.exe process to make sure our WMI queries are actually
			// working.  If a user doesn't have permission to run a WMI query against Win32_Process, then
			// there's no point continuing.
			//
			// Note 2: Microsoft issued a hotfix (KB3092422) for this, so now we only need to detect when
			// the RTM version (14.0.23107.0) of Microsoft.VsHub.Client.dll is used.  Version 14.0.23304.0
			// of VsHub won't hang on to DevEnv.com, so this workaround isn't needed with it.
			// http://connect.microsoft.com/VisualStudio/feedback/details/1648480/devenv-com-command-line-builds-hang-until-vshub-exe-times-out-and-exits
			// https://www.microsoft.com/en-us/download/details.aspx?id=49029
			if (version.Version == VSVersion.V2015
				&& version.TryGetDevEnvPath(true, out this.devEnvPath)
				&& this.IsBuggyVsHubInstalled
				&& FindProcesses("MegaBuild.exe")
				&& !FindProcesses("VsHub.exe"))
			{
				string variable = Environment.Is64BitOperatingSystem ? "%ProgramFiles(x86)%" : "%ProgramFiles%";
				string programFilesx86 = Environment.ExpandEnvironmentVariables(variable);
				string vsHubExe = Path.Combine(programFilesx86, @"Common Files\Microsoft Shared\VsHub\1.0.0.0\vshub.exe");
				if (File.Exists(vsHubExe))
				{
					ProcessStartInfo startInfo = new ProcessStartInfo(vsHubExe, " EXECUTE -n vshub --console false")
					{
						CreateNoWindow = true,
						WindowStyle = ProcessWindowStyle.Hidden,
						UseShellExecute = false,
					};
					using (Process.Start(startInfo))
					{
						// We'll leave the process running, but we need to dispose of our process handle.
					}
				}
			}
		}

		#endregion

		#region Private Properties

		private bool IsBuggyVsHubInstalled
		{
			get
			{
				string vsHubClientDll = Path.Combine(Path.GetDirectoryName(this.devEnvPath), @"PrivateAssemblies\Microsoft.VsHub.Client.dll");
				bool result = File.Exists(vsHubClientDll);
				if (result)
				{
					FileVersionInfo info = FileVersionInfo.GetVersionInfo(vsHubClientDll);
					Version fileVersion = new Version(info.FileMajorPart, info.FileMinorPart, info.FileBuildPart, info.FilePrivatePart);
					result = fileVersion < FixedVsHubVersion;
				}

				return result;
			}
		}

		#endregion

		#region Public Methods

		public void Dispose()
		{
		}

		#endregion

		#region Private Methods

		private static bool FindProcesses(string processName)
		{
			bool result = false;
			string query = $"select * from Win32_Process where Name = '{processName}' and ExecutablePath is not null";
			WmiUtility.TryQueryForRecords(query, record => result = true);
			return result;
		}

		#endregion
	}
}

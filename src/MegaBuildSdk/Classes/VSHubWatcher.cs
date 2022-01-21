namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Diagnostics;
	using System.IO;
	using Menees;

	#endregion

	internal sealed class VSHubWatcher : IDisposable
	{
		#region Private Data Members

		private static readonly Version FixedVsHubVersion = new(14, 0, 23304, 0);

		private readonly string? devEnvPath;

		#endregion

		#region Constructors

		public VSHubWatcher(VSVersionInfo version)
		{
			// If VS 2015 RTM's DevEnv.com is run when VsHub.exe isn't running, then when its hidden DevEnv.exe
			// instance runs to build, it will launch VsHub.exe, and DevEnv.com will hang from 1 to 5 minutes
			// waiting for VsHub.exe to timeout and shutdown.
			//
			// Microsoft issued a hotfix (KB3092422) for this, so now we only need to detect when
			// the RTM version (14.0.23107.0) of Microsoft.VsHub.Client.dll is used.  Version 14.0.23304.0
			// of VsHub won't hang on to DevEnv.com, so this workaround isn't needed with it.
			// http://connect.microsoft.com/VisualStudio/feedback/details/1648480/devenv-com-command-line-builds-hang-until-vshub-exe-times-out-and-exits
			// https://www.microsoft.com/en-us/download/details.aspx?id=49029
			if (version.Version == VSVersion.V2015
				&& version.TryGetDevEnvPath(true, out this.devEnvPath)
				&& this.IsBuggyVsHubInstalled)
			{
				throw Exceptions.NewInvalidOperationException("VS 2015 RTM isn't supported. You must apply at least KB3092422.");
			}
		}

		#endregion

		#region Private Properties

		private bool IsBuggyVsHubInstalled
		{
			get
			{
				string vsHubClientDll = Path.Combine(Path.GetDirectoryName(this.devEnvPath) ?? string.Empty, @"PrivateAssemblies\Microsoft.VsHub.Client.dll");
				bool result = File.Exists(vsHubClientDll);
				if (result)
				{
					FileVersionInfo info = FileVersionInfo.GetVersionInfo(vsHubClientDll);
					Version fileVersion = new(info.FileMajorPart, info.FileMinorPart, info.FileBuildPart, info.FilePrivatePart);
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
	}
}

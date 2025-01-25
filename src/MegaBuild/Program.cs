namespace MegaBuild;

#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menees.Windows.Forms;

#endregion

internal static class Program
{
	#region Main Entry Point

	/// <summary>
	/// The main entry point for the application.
	/// </summary>
	[STAThread]
	private static int Main()
	{
		// Allow modern .NET builds to use all the code pages that .NET Framework builds can.
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

		// I'm intentionally not setting TaskbarManager.Instance.ApplicationId.  We want Windows
		// to assign a default application ID to each executable path, so they can have their own
		// private lists of recent files (just like we store in our .stgx files).  Then it will also make
		// MegaBuild's taskbar icons group together separately by .exe.  For more info:
		// "Introducing The Taskbar APIs" discusses the "default application ID":
		// http://msdn.microsoft.com/en-us/magazine/dd942846.aspx
		// Also, see SetCurrentProcessExplicitAppUserModelID function:
		// http://msdn.microsoft.com/en-us/library/windows/desktop/dd378422(v=vs.85).aspx
		WindowsUtility.InitializeApplication(nameof(MegaBuild), null);

		// Setup idle time processing.
		MainForm frmMain = new();
		Application.Idle += new EventHandler(frmMain.OnIdle);

		// Run the application.
		Application.Run(frmMain);

		// Return the exit code.
		return Environment.ExitCode;
	}

	#endregion
}

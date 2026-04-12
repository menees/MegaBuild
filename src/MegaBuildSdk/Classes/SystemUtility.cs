namespace MegaBuild;

#region Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Menees;
using Menees.Shell;
using Menees.Windows.Forms;

#endregion

public static class SystemUtility
{
	#region Public Methods

	public static string? SearchPath(string fileName)
		=> ShellUtility.SearchPath(fileName);

	public static string? FindWindowsTerminal()
		=> WindowsUtility.FindWindowsTerminal();

	public static bool TryOpenExplorerForFile(string? path)
		=> WindowsUtility.TryOpenExplorerForFile(Manager.ExpandVariables(path));

	public static bool TryOpenExplorerForFolder(string? path)
		=> WindowsUtility.TryOpenExplorerForFolder(Manager.ExpandVariables(path));

	public static bool TryOpenTerminalForFile(string? path)
		=> WindowsUtility.TryOpenTerminalForFile(Manager.ExpandVariables(path));

	public static bool TryOpenTerminalForFolder(string? path)
		=> WindowsUtility.TryOpenTerminalForFolder(Manager.ExpandVariables(path));

	public static bool TryOpenFile(string? filePath)
		=> WindowsUtility.TryOpenFile(Manager.ExpandVariables(filePath));

	#endregion
}

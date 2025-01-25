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
using Menees.Windows.Forms;

#endregion

public static class SystemUtility
{
	#region Public Methods

	public static string? SearchPath(string fileName)
	{
		string? result = null;

		string[] pathEntries = (Environment.GetEnvironmentVariable("PATH") ?? string.Empty)
			.Split([';'], StringSplitOptions.RemoveEmptyEntries)
			.Select(entry => entry.Trim())
			.Where(entry => !string.IsNullOrWhiteSpace(entry))
			.ToArray();

		// The check for duplicates saves time here. On my system, 12 of the 55 entries in PATH are duplicates (ignoring case).
		// Unfortunately, we can't use LINQ's Distinct() because it returns an unordered sequence, and we have to preserve order.
		// Note: Windows 10 supports case-sensitive folders, but the PowerShell folders shouldn't be configured that way.
		HashSet<string> checkedPaths = new(StringComparer.OrdinalIgnoreCase);
		foreach (string path in pathEntries)
		{
			if (checkedPaths.Add(path))
			{
				string fullyQualifiedName = Path.Combine(path, fileName);
				if (File.Exists(fullyQualifiedName))
				{
					result = fullyQualifiedName;
					break;
				}
			}
		}

		return result;
	}

	public static string? FindWindowsTerminal()
	{
		// If Windows Terminal Preview is installed, this will find it as long as
		// "Manage App Execution Aliases" is still enabled in Windows settings for it.
		// https://stackoverflow.com/a/68006153/1882616
		string? result = SearchPath("wt.exe");
		return result;
	}

	public static bool TryOpenExplorerForFile(string? path)
	{
		bool result = false;

		path = GetFullyQualifiedPath(path);
		if (File.Exists(path))
		{
			// Start Explorer with the file selected.
			// https://stackoverflow.com/a/13680458/1882616
			string arguments = $"/select,\"{path}\"";
			result = TryOpenExplorer(arguments);
		}
		else if (path.IsNotEmpty())
		{
			WindowsUtility.ShowError(null, $"The file '{path}' does not exist.");
		}

		return result;
	}

	public static bool TryOpenExplorerForFolder(string? path)
	{
		bool result = false;

		path = GetFullyQualifiedPath(path);
		if (Directory.Exists(path))
		{
			string arguments = $"\"{path}\"";
			result = TryOpenExplorer(arguments);
		}
		else if (path.IsNotEmpty())
		{
			WindowsUtility.ShowError(null, $"The folder '{path}' does not exist.");
		}

		return result;
	}

	public static bool TryOpenTerminalForFile(string? path)
	{
		bool result = false;

		path = GetFullyQualifiedPath(path);
		if (File.Exists(path))
		{
			string? folder = Path.GetDirectoryName(path);
			if (folder.IsNotEmpty())
			{
				result = TryOpenTerminalForFolder(folder);
			}
		}
		else if (path.IsNotEmpty())
		{
			WindowsUtility.ShowError(null, $"The file '{path}' does not exist.");
		}

		return result;
	}

	public static bool TryOpenTerminalForFolder(string? path)
	{
		bool result = false;

		path = GetFullyQualifiedPath(path);
		if (Directory.Exists(path))
		{
			string? terminal = FindWindowsTerminal();
			if (terminal.IsNotEmpty())
			{
				// https://learn.microsoft.com/en-us/windows/terminal/command-line-arguments?tabs=windows#new-tab-command
				string arguments = $"--startingDirectory \"{path}\"";
				result = TryStartProcess(terminal, arguments);
			}
			else
			{
				// https://en.wikipedia.org/wiki/COMSPEC
				string cmdExe = Environment.GetEnvironmentVariable("ComSpec") ?? "cmd.exe";
				string arguments = $"/K cd /d \"{path}\"";
				result = TryStartProcess(cmdExe, arguments);
			}
		}
		else if (path.IsNotEmpty())
		{
			WindowsUtility.ShowError(null, $"The folder '{path}' does not exist.");
		}

		return result;
	}

	#endregion

	#region Private Methods

	private static string? GetFullyQualifiedPath(string? path)
	{
		// If the path is rooted but doesn't exist, we want to return the rooted path as is.
		// If the path is relative or just a file name, then we need to fully qualify it to
		// safely pass it to external processes (e.g., explorer.exe).
		string? result = path is null || Path.IsPathRooted(path) ? path : Path.GetFullPath(path);
		return result;
	}

	private static bool TryOpenExplorer(string arguments)
		=> TryStartProcess("explorer.exe", arguments);

	private static bool TryStartProcess(string fileName, string arguments)
	{
		using (Process? process = Process.Start(fileName, arguments))
		{
			bool result = process != null;
			return result;
		}
	}

	#endregion
}

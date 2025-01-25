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

	public static bool TryOpenFileExplorer(string? startingFolderOrFileLocation)
	{
		bool result = false;

		string arguments = string.Empty;
		if (startingFolderOrFileLocation.IsNotEmpty())
		{
			if (File.Exists(startingFolderOrFileLocation))
			{
				// Start Explorer with the file selected.
				// https://stackoverflow.com/a/13680458/1882616
				arguments = $"/select,\"{Path.GetFullPath(startingFolderOrFileLocation)}\"";
			}
			else
			{
				string? absoluteFolder = Path.GetFullPath(startingFolderOrFileLocation);
				arguments = $"\"{absoluteFolder}\"";
			}
		}

		using (Process? process = Process.Start("explorer.exe", arguments))
		{
			result = process != null;
		}

		return result;
	}

	public static bool TryOpenWindowsTerminal(string? startingFolderOrFileLocation)
	{
		bool result = false;

		string? terminal = FindWindowsTerminal();
		if (terminal.IsNotEmpty())
		{
			string? folder = null;
			if (startingFolderOrFileLocation.IsNotEmpty())
			{
				string? absoluteFolder = File.Exists(startingFolderOrFileLocation)
					? Path.GetDirectoryName(Path.GetFullPath(startingFolderOrFileLocation))
					: Path.GetFullPath(startingFolderOrFileLocation);
				folder = absoluteFolder;
			}

			// https://learn.microsoft.com/en-us/windows/terminal/command-line-arguments?tabs=windows#new-tab-command
			string arguments = folder.IsEmpty() ? string.Empty : $"--startingDirectory \"{folder}\"";

			using (Process? process = Process.Start(terminal, arguments))
			{
				result = process != null;
			}
		}

		return result;
	}

	#endregion
}

namespace MegaBuild;

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

[TestClass]
public class SystemUtilityTests
{
	[TestMethod]
	public void SearchPathTest()
	{
		string? cmdExe = Utility.SearchPath("cmd.exe");
		cmdExe.ShouldNotBeNullOrWhiteSpace();
		cmdExe.ShouldEndWith($"{Path.DirectorySeparatorChar}cmd.exe");
		cmdExe.ShouldStartWith(Environment.ExpandEnvironmentVariables("%SystemRoot%"), Case.Insensitive);

		Utility.SearchPath($"{Guid.NewGuid():N}.exe").ShouldBeNull();
	}

	[TestMethod]
	public void FindWindowsTerminalTest()
	{
		// Windows Terminal may not be available on the GitHub build agent.
		// See https://stackoverflow.com/a/68006153/1882616 for info on wt.exe.
		string? actual = Utility.FindWindowsTerminal();
		string expected = Environment.ExpandEnvironmentVariables(@"%LocalAppData%\Microsoft\WindowsApps\wt.exe");
		if (File.Exists(expected))
		{
			actual.ShouldBe(expected, StringCompareShould.IgnoreCase);
		}
		else
		{
			actual.ShouldBeNull();
		}
	}
}

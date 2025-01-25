namespace MegaBuild;

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

[TestClass]
public class ExecutableStepTests
{
	[TestMethod]
	public void SearchPathTest()
	{
		string? cmdExe = ExecutableStep.SearchPath("cmd.exe");
		cmdExe.ShouldNotBeNullOrWhiteSpace();
		cmdExe.ShouldEndWith($"{Path.DirectorySeparatorChar}cmd.exe");
		cmdExe.ShouldStartWith(Environment.ExpandEnvironmentVariables("%SystemRoot%"), Case.Insensitive);

		ExecutableStep.SearchPath($"{Guid.NewGuid():N}.exe").ShouldBeNull();
	}
}

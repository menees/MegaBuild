namespace MegaBuild;

#region Using Directives

using System;
using System.IO;
using Menees;
using Menees.Shell;

#endregion

internal sealed class CommandLineArgs
{
	#region Private Data Members

	private readonly bool showHelp;
#pragma warning disable CC0052 // Make field readonly. These are modified by lambdas in the constructor, so they can't be readonly.
	private bool build;
	private bool exit;
	private string project = string.Empty;
#pragma warning restore CC0052 // Make field readonly

	#endregion

	#region Constructors

	public CommandLineArgs()
	{
		CommandLine cmdLine = new(false);
		cmdLine.AddSwitch(nameof(this.Build), string.Empty, value => this.build = value);
		cmdLine.AddSwitch(nameof(this.Exit), string.Empty, value => this.exit = value);
		cmdLine.AddValueHandler((value, errors) =>
			{
				this.project = TextUtility.StripQuotes(value);
				if (File.Exists(this.project))
				{
					// Expand a short file name into a long one since the file name will be displayed in the title bar.
					this.project = FileUtility.ExpandFileName(this.project);
				}
				else
				{
					this.project = string.Empty;
				}
			});
		CommandLineParseResult parseResult = cmdLine.Parse();

		// Check for some invalid combinations
		if (parseResult != CommandLineParseResult.Valid ||
			(string.IsNullOrEmpty(this.project) && (this.build || this.exit)) ||
			(this.exit && !this.build))
		{
			this.showHelp = true;

			// If the arguments are stunk up, then reset all the data.
			this.project = string.Empty;
			this.build = false;
			this.exit = false;
		}
	}

	#endregion

	#region Public Properties

	public static string HelpString => "Usage:\tMegaBuild [/build [/exit]] [project]\r\n" +
		"\t/build -\tBuilds the specified project.\r\n" +
		"\t/exit -\tExits the program after building.\r\n" +
		"\tproject -\tThe name of the MegaBuild project to open.";

	public bool Build => this.build;

	public bool Exit => this.exit;

	public string Project => this.project;

	public bool ShowHelp => this.showHelp;

	#endregion
}
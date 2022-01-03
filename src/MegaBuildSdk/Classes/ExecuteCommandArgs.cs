namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;

	#endregion

	public class ExecuteCommandArgs
	{
		#region Constructors

		public ExecuteCommandArgs()
		{
			this.WindowStyle = ProcessWindowStyle.Normal;
			this.Arguments = string.Empty;
			this.FileName = string.Empty;
			this.Verb = string.Empty;
			this.WorkingDirectory = string.Empty;
		}

		public ExecuteCommandArgs(
			string fileName,
			string arguments,
			string workingDirectory,
			ProcessWindowStyle windowStyle,
			int firstSuccessCode,
			int lastSuccessCode,
			RedirectStandardStreams redirectStreams,
			bool useShellExecute,
			string verb)
		{
			this.FileName = fileName;
			this.Arguments = arguments;
			this.WorkingDirectory = workingDirectory;
			this.WindowStyle = windowStyle;
			this.FirstSuccessCode = firstSuccessCode;
			this.LastSuccessCode = lastSuccessCode;
			this.RedirectStandardStreams = redirectStreams;
			this.UseShellExecute = useShellExecute;
			this.Verb = verb;
		}

		#endregion

		#region Public Properties

		public Func<string, bool>? AllowErrorLine { get; set; }

		public Func<string, bool>? AllowOutputLine { get; set; }

		public string Arguments { get; set; }

		public int ExitCode { get; set; }

		public string FileName { get; set; }

		public int FirstSuccessCode { get; set; }

		public int LastSuccessCode { get; set; }

		public RedirectStandardStreams RedirectStandardStreams { get; set; }

		public bool UseShellExecute { get; set; }

		public string Verb { get; set; }

		public ProcessWindowStyle WindowStyle { get; set; }

		public string? WorkingDirectory { get; set; }

		public IDictionary<string, string> EnvironmentVariables { get; } = new Dictionary<string, string>();

		#endregion
	}
}
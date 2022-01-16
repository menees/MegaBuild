namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Diagnostics.CodeAnalysis;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Windows.Forms;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	public static class Manager
	{
		#region Public Fields

		public const string IndentString = "\t";

		#endregion

		#region Private Data Members

		private static readonly char IndentChar = IndentString[0];

		private static readonly object LockToken = new();
		private static readonly ImageList Images = new() { ColorDepth = ColorDepth.Depth32Bit };
		private static readonly Dictionary<string, string> VariablesMap = new(StringComparer.CurrentCultureIgnoreCase);
		private static readonly List<Project> Projects = new(1);
		private static readonly Dictionary<string, StepTypeInfo> TypeNameToStepTypeInfo
			= new(StringComparer.CurrentCultureIgnoreCase);

		private static Form? mainForm;
		private static StringBuilder output = new();

		#endregion

		#region Public Events

		public static event EventHandler<OutputAddedEventArgs>? OutputAdded;

		public static event EventHandler? OutputCleared;

		#endregion

		#region Public Properties

		public static bool ParseOutputCommands { get; set; } = true;

		public static ImageList StepImages => Images;

		public static Dictionary<string, string> Variables => VariablesMap;

		#endregion

		#region Public Methods

		public static void ClearOutput()
		{
			lock (LockToken)
			{
				// Clear the output cache
				output = new StringBuilder();
			}

			var outputCleared = OutputCleared;
			if (outputCleared != null)
			{
				if (mainForm != null)
				{
					mainForm.BeginInvoke(outputCleared, new object?[] { null, EventArgs.Empty });
				}
				else
				{
					outputCleared(null, EventArgs.Empty);
				}
			}
		}

		public static string CollapseVariables(string text)
		{
			lock (LockToken)
			{
				var combinedVariables = GetCombinedVariables();
				foreach (KeyValuePair<string, string> entry in combinedVariables)
				{
					// If the string is initially empty or becomes empty, then we can quit.
					if (string.IsNullOrEmpty(text))
					{
						break;
					}

					string variable = entry.Key;
					string value = entry.Value;

					text = TextUtility.Replace(text, value, variable, StringComparison.CurrentCultureIgnoreCase);
				}

				// NOTE: We can't "unexpand" environment variables because their
				// content is too unpredictable.  For example, when I tried it on the
				// path to PowerShell.exe, I ended up with this on my dual core system:
				// %SystemDrive%\Windows\System3%NUMBER_OF_PROCESSORS%\WindowsPowerShell\v%TRACKER_ATTACHED%.0\powershell.exe
				// What I really wanted was:
				// %SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe
				// So we'll just let ExpandVariables handle environment variables, and
				// we'll rely on the user to manually update their step properties to use
				// the correct environment variables when and if they need them.
				return text;
			}
		}

		public static string ExpandVariables(string? text)
		{
			lock (LockToken)
			{
				var combinedVariables = GetCombinedVariables();
				foreach (KeyValuePair<string, string> entry in combinedVariables)
				{
					// If the string is initially empty or becomes empty, then we can quit.
					if (text.IsEmpty())
					{
						break;
					}

					string variable = entry.Key;
					string value = entry.Value;

					text = TextUtility.Replace(text, variable, value, StringComparison.CurrentCultureIgnoreCase);
				}

				// Now that MegaBuild variables have been replaced,
				// let's expand environment variables too.  That makes
				// it easier to refer to things generically under Windows,
				// Program Files, etc.
				if (!string.IsNullOrEmpty(text))
				{
					text = Environment.ExpandEnvironmentVariables(text);
				}

				return text ?? string.Empty;
			}
		}

		public static string GetOutput()
		{
			lock (LockToken)
			{
				string result = output.ToString();
				return result;
			}
		}

		public static void Load(Form mainForm, ISettingsNode key)
		{
			lock (LockToken)
			{
				Manager.mainForm = mainForm;

				ParseOutputCommands = key.GetValue(nameof(ParseOutputCommands), ParseOutputCommands);

				// Load variables
				VariablesMap.Clear();
				ISettingsNode? node = key.TryGetSubNode(nameof(Variables));
				if (node != null)
				{
					IList<string> names = node.GetSettingNames();
					foreach (string name in names)
					{
						VariablesMap.Add(name, node.GetValue(name, string.Empty));
					}
				}

				// Cache the step types last because this is what is most likely to throw an exception (e.g.,
				// if a user type isn't written correctly or specifies an icon resource incorrectly). Also, there's
				// a small chance that a user type's static constructor might want to call ExpandVariables...
				CacheStepTypes();
			}
		}

		public static void Output(string message, int indent, Color color)
		{
			Output(message, indent, color, false, Guid.Empty);
		}

		public static void Output(string message, int indent, Color color, bool highlight)
		{
			Output(message, indent, color, highlight, Guid.Empty);
		}

		public static void Output(string message, int indent, Color color, bool highlight, Guid outputId)
		{
			// Cache the output so Email steps can get it if necessary.
			lock (LockToken)
			{
				// Parse any commands in the output (e.g. MEGABUILD.SET) if necessary.
				if (ParseOutputCommands)
				{
					ParseCommands(message);
				}

				// Unfortunately, color, highlight, and id information is lost when caching the output this way...
				for (int i = 0; i < indent; i++)
				{
					output.Append(IndentChar);
				}

				output.Append(message);
			}

			// Send these to the main window on the current thread. It'll figure out whether the output needs
			// to be buffered and/or marshaled to the GUI thread.
			var outputAdded = OutputAdded;
			if (outputAdded != null)
			{
				OutputAddedEventArgs e = new(message, indent, color, highlight, outputId);
				outputAdded(null, e);
			}
		}

		public static void Save(ISettingsNode key)
		{
			lock (LockToken)
			{
				key.SetValue(nameof(ParseOutputCommands), ParseOutputCommands);

				// Save variables
				key.DeleteSubNode(nameof(Variables));
				if (VariablesMap.Count > 0)
				{
					ISettingsNode varNode = key.GetSubNode(nameof(Variables));
					foreach (KeyValuePair<string, string> entry in VariablesMap)
					{
						varNode.SetValue(entry.Key, entry.Value);
					}
				}
			}
		}

		#endregion

		#region Internal Methods

		internal static void AddProject(Project project)
		{
			lock (LockToken)
			{
				Projects.Add(project);
			}
		}

		internal static StepTypeInfo? GetStepTypeInfo(string fullTypeName)
		{
			lock (LockToken)
			{
				TypeNameToStepTypeInfo.TryGetValue(fullTypeName, out StepTypeInfo? result);
				return result;
			}
		}

		internal static StepTypeInfo[] GetStepTypeInfos()
		{
			lock (LockToken)
			{
				return TypeNameToStepTypeInfo.Select(pair => pair.Value).ToArray();
			}
		}

		internal static void RemoveProject(Project project)
		{
			lock (LockToken)
			{
				Projects.Remove(project);
			}
		}

		#endregion

		#region Private Methods

		private static void CacheAssemblyStepTypes(Assembly asm, bool cacheAllTypes)
		{
			// Only cache the types if the assembly has a [MegaBuildExtension] attribute.
			object[] attributes = asm.GetCustomAttributes(typeof(MegaBuildExtensionAttribute), true);
			if (attributes.Length > 0)
			{
				Type[] types = cacheAllTypes ? asm.GetTypes() : asm.GetExportedTypes();
				foreach (Type type in types)
				{
					if (type.IsSubclassOf(typeof(Step)) && !type.IsAbstract)
					{
						// Some classes may need to do availability checks.
						bool available = true;
						MethodInfo? checkAvailability = type.GetMethod("CheckAvailability", BindingFlags.Static | BindingFlags.Public);
						if (checkAvailability != null)
						{
							string? reasonUnavailable = (string?)checkAvailability.Invoke(null, CollectionUtility.EmptyArray<object>());
							if (!string.IsNullOrEmpty(reasonUnavailable))
							{
								available = false;
								string message = string.Format("The step type '{0}' will be unavailable: {1}\r\n", type.FullName, reasonUnavailable);
								Output(message, 0, OutputColors.Error, true);
							}
						}

						if (available)
						{
							TypeNameToStepTypeInfo.Add(type.FullName ?? string.Empty, new StepTypeInfo(type));
						}
					}
				}
			}
		}

		private static void CacheStepTypes()
		{
			TypeNameToStepTypeInfo.Clear();

			// Cache the types in this assembly first.
			Assembly thisAsm = Assembly.GetExecutingAssembly();
			CacheAssemblyStepTypes(thisAsm, true);

			// Now scan all the Dlls in the application's directory.
			string[] dllNames = Directory.GetFileSystemEntries(Path.GetDirectoryName(Application.ExecutablePath) ?? string.Empty, "*.dll");
			foreach (string dllName in dllNames)
			{
				Assembly asm = Assembly.LoadFrom(dllName);
				if (asm != thisAsm)
				{
					CacheAssemblyStepTypes(asm, false);
				}
			}
		}

		private static Dictionary<string, string> GetCombinedVariables()
		{
			// Add the Manager variables first.
			Dictionary<string, string> result = new(VariablesMap, StringComparer.CurrentCultureIgnoreCase);

			// Then add each project's variables in project order, so project and sub-project variables
			// can override existing variable values from Manager or parent projects.
			foreach (Project project in Projects)
			{
				foreach (KeyValuePair<string, string> pair in project.Variables)
				{
					result[pair.Key] = pair.Value;
				}
			}

			// Note: We're not caching the result because the project variable
			// values could change between calls (e.g., if someone saves the
			// project to a new folder, then $(ProjectDir) references would change).
			return result;
		}

		private static void ParseCommands(string message)
		{
			int currentIndex = 0;
			string upperMessage = message.ToUpper();

			// Only do the rest of the parsing if the message starts with a MegaBuild command prefix.
			const int PrefixLength = 10; // Length of "MEGABUILD.".
			if (upperMessage.Length > PrefixLength && upperMessage.StartsWith("MEGABUILD."))
			{
				// Strip off the command prefix
				upperMessage = upperMessage.Substring(PrefixLength);
				currentIndex += PrefixLength;

				// We only support the "SET" command for now, so just check for that.
				// 	Format:
				// 	MEGABUILD.SET %zipname%=c:\xyz\foo.zip
				const int SetLength = 3;
				if (upperMessage.StartsWith("SET") && upperMessage.Length > SetLength && char.IsWhiteSpace(upperMessage[SetLength]))
				{
					// Strip off "SET".
					upperMessage = upperMessage.Substring(SetLength);
					currentIndex += SetLength;

					if (upperMessage.Length > 0)
					{
						// Note: At this point upperMessage has at least one leading whitespace character,
						// but I'm not trimming the whitespace because it makes tracking the current
						// position in the original string more difficult.
						int equalIndex = upperMessage.IndexOf('=');
						if (equalIndex >= 0 && message.Length > (currentIndex + equalIndex + 1))
						{
							// Now we pull from the original string (not the uppercase version)
							// so the variables will have the case the user intended.  The
							// ExpandVariables method is case-insensitive, but the variable's
							// value might be used somewhere in a case-sensitive manner.
							string name = message.Substring(currentIndex, equalIndex).Trim();
							if (name.Length > 0)
							{
								string value = message.Substring(currentIndex + equalIndex + 1).Trim();

								// Make sure the variable name is formatted correctly.
								name = TextUtility.EnsureQuotes(name, "%");

								// Put it in the map.  The index operator adds or overwrites.
								VariablesMap[name] = value;
							}
						}
					}
				}
			}
		}

		#endregion
	}
}
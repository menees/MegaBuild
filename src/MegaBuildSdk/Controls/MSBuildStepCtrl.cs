namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Windows.Forms;
	using System.Xml.Linq;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	internal partial class MSBuildStepCtrl : StepEditorControl
	{
		#region Private Data Members

		private static readonly string[] BaseSolutionTargets = { string.Empty, "Rebuild", "Clean" };

		private MSBuildStep? step;

		#endregion

		#region Constructors

		public MSBuildStepCtrl()
		{
			this.InitializeComponent();

			// https://www.codementor.io/cerkit/giving-an-enum-a-string-value-using-the-description-attribute-6b4fwdle0
			Type type = typeof(MSBuildToolsVersion);
			string[] names = Enum.GetNames(type);
			foreach (string name in names)
			{
				string value = name;
				DescriptionAttribute? description = type.GetMember(name)
					.Select(m => (DescriptionAttribute?)m.GetCustomAttribute(typeof(DescriptionAttribute)))
					.FirstOrDefault();
				if (description != null)
				{
					value = description.Description;
				}

				this.cbToolsVersion.Items.Add(value);
			}
		}

		#endregion

		#region Public Properties

		public override string DisplayName => "MSBuild";

		public MSBuildStep Step
		{
			set
			{
				if (this.step != value)
				{
					this.step = value;

					this.edtProject.Text = this.step.ProjectFile;
					this.edtWorkingDirectory.Text = this.step.WorkingDirectory;
					this.txtTargets.Lines = this.step.Targets;
					this.txtProperties.Lines = (from p in this.step.Properties
													orderby p.Key
													select p.Key + "=" + p.Value).ToArray();
					this.cbVerbosity.SelectedIndex = (int)this.step.Verbosity;
					this.cbToolsVersion.SelectedIndex = (int)this.step.ToolsVersion;
					this.edtOtherOptions.Text = this.step.CommandLineOptions;
					this.chk32Bit.Checked = this.step.Use32BitProcess;
				}
			}
		}

		#endregion

		#region Private Properties

		private string ExpandedProjectFileName
		{
			get
			{
				string result = Manager.ExpandVariables(this.edtProject.Text.Trim());
				return result;
			}
		}

		#endregion

		#region Public Methods

		public override bool OnOk()
		{
			bool result = false;

			string projectFile = this.edtProject.Text.Trim();
			if (string.IsNullOrEmpty(projectFile))
			{
				WindowsUtility.ShowError(this, "You must enter a project file name.");
			}
			else
			{
				List<string> targets = new();
				foreach (string line in this.txtTargets.Lines)
				{
					string target = line.Trim();
					if (!string.IsNullOrEmpty(target))
					{
						targets.Add(target);
					}
				}

				result = true;
				Dictionary<string, string> properties = new();
				foreach (string line in this.txtProperties.Lines)
				{
					string[] parts = line.Trim().Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
					if (parts.Length == 2)
					{
						properties[parts[0].Trim()] = parts[1].Trim();
					}
					else if (parts.Length != 0)
					{
						WindowsUtility.ShowError(this, "Unable to parse the following as a Name=Value pair: " + line);
						result = false;
						break;
					}
				}

				if (result && this.step != null)
				{
					this.step.ProjectFile = projectFile;
					this.step.WorkingDirectory = this.edtWorkingDirectory.Text.Trim();
					this.step.Targets = targets.ToArray();
					this.step.Properties = properties;
					this.step.Verbosity = (MSBuildVerbosity)this.cbVerbosity.SelectedIndex;
					this.step.ToolsVersion = (MSBuildToolsVersion)this.cbToolsVersion.SelectedIndex;
					this.step.CommandLineOptions = this.edtOtherOptions.Text;
					this.step.Use32BitProcess = this.chk32Bit.Checked;
				}
			}

			return result;
		}

		#endregion

		#region Private Methods

		private static string? FindSolutionTargets(string fileName)
		{
			// According to the MSBuild Command Line Reference (http://msdn.microsoft.com/en-us/library/ms164311.aspx)
			// the projects referenced by a solution file can be listed as targets if they are colon-suffixed with one of VS's
			// base targets (e.g., Rebuild, Clean): msbuild MegaBuild.sln /t:MegaBuildSDK /t:Menees:Clean
			var projectLines = from line in File.ReadAllLines(fileName)
								where line.StartsWith("Project(\"")
								select line;

			string? result = null;
			if (projectLines.FirstOrDefault() != null)
			{
				StringBuilder sb = new("Solution Targets:");
				sb.AppendLine();

				foreach (string line in projectLines)
				{
					int equalPos = line.IndexOf('=');
					if (equalPos >= 0)
					{
						int openQuote = line.IndexOf('"', equalPos);
						if (openQuote >= 0)
						{
							int closeQuote = line.IndexOf('"', openQuote + 1);
							if (closeQuote >= 0)
							{
								string projectName = line.Substring(openQuote + 1, closeQuote - openQuote - 1);
								if (!string.IsNullOrEmpty(projectName))
								{
									foreach (string baseTarget in BaseSolutionTargets)
									{
										sb.AppendLine().Append(projectName);
										if (!string.IsNullOrEmpty(baseTarget))
										{
											sb.Append(':').Append(baseTarget);
										}
									}

									sb.AppendLine();
								}
							}
						}
					}
				}

				result = sb.ToString();
			}

			return result;
		}

		private static bool IsSolutionFile(string fileName)
		{
			string extension = Path.GetExtension(fileName);
			bool result = string.Equals(extension, ".sln", StringComparison.CurrentCultureIgnoreCase);
			return result;
		}

		private void SelectProject_Click(object sender, EventArgs e)
		{
			this.OpenDlg.FileName = Manager.ExpandVariables(this.edtProject.Text);
			if (this.OpenDlg.ShowDialog(this) == DialogResult.OK)
			{
				this.edtProject.Text = Manager.CollapseVariables(this.OpenDlg.FileName);
			}
		}

		private void SelectWorkingDirectory_Click(object sender, EventArgs e)
		{
			string initialFolder = Manager.ExpandVariables(this.edtWorkingDirectory.Text);
			string? selectedFolder = WindowsUtility.SelectFolder(this, "Select Working Directory", initialFolder);
			if (selectedFolder.IsNotEmpty())
			{
				this.edtWorkingDirectory.Text = Manager.CollapseVariables(selectedFolder);
			}
		}

		private void ShowProperties_Click(object sender, EventArgs e)
		{
			string? message = null;
			string fileName = this.ExpandedProjectFileName;
			if (!File.Exists(fileName))
			{
				message = "The project file was not found.";
			}
			else if (!IsSolutionFile(fileName))
			{
				XElement doc = XElement.Load(fileName);
				XNamespace ns = doc.Name.Namespace;
				var groups = doc.Elements(XName.Get("PropertyGroup", ns.NamespaceName));

				if (groups.FirstOrDefault() != null)
				{
					StringBuilder sb = new();
					foreach (var g in groups)
					{
						sb.Append("Property Group: ");
						string? condition = (string?)g.Attribute("Condition");
						if (condition.IsNotEmpty())
						{
							sb.Append(condition);
						}

						sb.AppendLine();
						var properties = from prop in g.Elements()
											orderby prop.Name.LocalName
											select prop;
						foreach (XElement p in properties)
						{
							sb.AppendLine().Append(p.Name.LocalName).Append('=').Append(p.Value);
						}

						sb.AppendLine().AppendLine();
					}

					message = sb.ToString();
				}
			}

			if (string.IsNullOrEmpty(message))
			{
				message = "No properties were found in the project file.";
			}

			MessageBox.Show(this, message, nameof(Properties), MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void ShowTargets_Click(object sender, EventArgs e)
		{
			string? message = null;
			string fileName = this.ExpandedProjectFileName;
			if (!File.Exists(fileName))
			{
				message = "The project file was not found.";
			}
			else if (IsSolutionFile(fileName))
			{
				message = FindSolutionTargets(fileName);
			}
			else
			{
				XElement doc = XElement.Load(fileName);
				XNamespace ns = doc.Name.Namespace;
				var targets = from t in doc.Elements(XName.Get("Target", ns.NamespaceName))
								where !string.IsNullOrEmpty((string?)t.Attribute("Name"))
								select t;

				StringBuilder sb = new();
				if (targets.FirstOrDefault() != null)
				{
					sb.AppendLine("Project Target(s):");
					foreach (XElement t in targets)
					{
						sb.AppendLine((string?)t.Attribute("Name"));
					}
				}
				else if (doc.Elements(XName.Get("Import", ns.NamespaceName)).FirstOrDefault() != null)
				{
					sb.AppendLine("The project file only contains imported targets.");
				}

				string? defaultTargets = (string?)doc.Attribute("DefaultTargets");
				if (defaultTargets.IsNotEmpty())
				{
					if (sb.Length > 0)
					{
						sb.AppendLine();
					}

					sb.AppendLine("Default Target(s):");
					foreach (string target in defaultTargets.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
					{
						sb.AppendLine(target);
					}
				}

				message = sb.ToString();
			}

			if (string.IsNullOrEmpty(message))
			{
				message = "No targets were found in the project file.";
			}

			MessageBox.Show(this, message, "Targets", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		#endregion
	}
}
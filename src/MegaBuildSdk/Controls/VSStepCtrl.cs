namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Windows.Forms;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	internal sealed partial class VSStepCtrl : StepEditorControl
	{
		#region Private Data Members

		private VSStep step;

		#endregion

		#region Constructors

		public VSStepCtrl()
		{
			this.InitializeComponent();

			this.cbVersion.Items.AddRange(VSVersionInfo.AllVersions.Select(v => v.FullDisplayName).ToArray());
		}

		#endregion

		#region Public Properties

		public override string DisplayName => "Visual Studio";

		public VSStep Step
		{
			set
			{
				if (this.step != value)
				{
					this.step = value;

					this.edtSolution.Text = this.step.Solution;
					this.cbAction.SelectedIndex = (int)this.step.Action;
					this.cbVersion.SelectedIndex = (int)this.step.Version;
					this.cbWindowState.SelectedIndex = (int)this.step.WindowState;
					this.edtDevEnvArgs.Text = this.step.DevEnvArguments;
					this.chkRedirectStreams.Checked = this.step.RedirectStreams;
					this.step.PutConfigurationsInListView(this.lstConfigurations);
				}
			}
		}

		#endregion

		#region Public Methods

		public override bool OnOk()
		{
			bool result = false;

			if (string.IsNullOrEmpty(this.edtSolution.Text.Trim()))
			{
				WindowsUtility.ShowError(this, "You must enter a solution file name.");
			}
			else if (this.lstConfigurations.CheckedItems.Count == 0)
			{
				WindowsUtility.ShowError(this, "You must select at least one configuration to build.");
			}
			else
			{
				this.step.Solution = this.edtSolution.Text;
				this.step.Action = (VSAction)this.cbAction.SelectedIndex;
				this.step.Version = (VSVersion)this.cbVersion.SelectedIndex;
				this.step.WindowState = (ProcessWindowStyle)this.cbWindowState.SelectedIndex;
				this.step.DevEnvArguments = this.edtDevEnvArgs.Text;
				this.step.RedirectStreams = this.chkRedirectStreams.Checked;
				this.step.GetConfigurationsFromListView(this.lstConfigurations);
				result = true;
			}

			return result;
		}

		public override void OnReadyToDisplay()
		{
			if (this.lstConfigurations.Columns.Count > 0)
			{
				this.lstConfigurations.Columns[0].Width = this.lstConfigurations.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 1;
			}
		}

		#endregion

		#region Private Methods

		private static string[] GetConfigurationsFromSolution(string solutionFile, ref VSVersion solutionVersion)
		{
			string[] configurations = null;

			solutionFile = Manager.ExpandVariables(solutionFile);
			if (File.Exists(solutionFile))
			{
				string[] lines = File.ReadAllLines(solutionFile);
				int numLines = lines.Length;

				// We may have to look at lines 0 and 1.
				if (numLines >= 2)
				{
					// In VS2005 Beta 2 the Unicode byte marks are on an otherwise blank
					// first line, and the version string is on the second line.
					string version = lines[0];
					if (string.IsNullOrEmpty(version))
					{
						version = lines[1];
					}

					const string VersionLinePrefix = "Microsoft Visual Studio Solution File, Format Version ";
					if (version.StartsWith(VersionLinePrefix))
					{
						version = version.Substring(VersionLinePrefix.Length).Trim();

						// See if we support the solution version.
						VSVersionInfo versionInfo = GetSolutionVersion(version, lines);
						if (versionInfo != null)
						{
							solutionVersion = versionInfo.Version;
							configurations = GetConfigurationsFromLines(lines);
						}
					}
				}
			}

			return configurations ?? CollectionUtility.EmptyArray<string>();
		}

		private static VSVersionInfo GetSolutionVersion(string version, IEnumerable<string> lines)
		{
			// Get the solution version.  Use InvariantInfo since we know the
			// version number always has a '.' separator.  We don't want this to
			// blow up for international users that have different decimal separators.
			decimal solutionVersionNumber = decimal.Parse(version, NumberFormatInfo.InvariantInfo);

			VSVersionInfo[] versions = VSVersionInfo.AllVersions.Where(v => v.SolutionVersion == solutionVersionNumber).ToArray();
			VSVersionInfo result = null;
			if (versions.Length > 0)
			{
				// VS 2002 through VS 2012 use distinct solution versions.
				result = versions[0];
				if (versions.Length > 1)
				{
					// VS 2013 uses the same solution version as VS 2012, so we have to
					// distinguish the solutions by looking for a #-prefixed version comment.
					// VS 2012
					// 		Microsoft Visual Studio Solution File, Format Version 12.00
					// 		# Visual Studio 2012
					// VS 2013
					// 		Microsoft Visual Studio Solution File, Format Version 12.00
					// 		# Visual Studio 2013
					// VS 2015
					// 		Microsoft Visual Studio Solution File, Format Version 12.00
					// 		# Visual Studio 14
					// VS 2017
					// 		Microsoft Visual Studio Solution File, Format Version 12.00
					// 		# Visual Studio 15
					// VS 2019
					// 		Microsoft Visual Studio Solution File, Format Version 12.00
					// 		# Visual Studio Version 16
					// VS 2022
					// 		Microsoft Visual Studio Solution File, Format Version 12.00
					// 		# Visual Studio Version 17
					string versionComment = lines.FirstOrDefault(line => line.StartsWith("# Visual Studio "));
					if (!string.IsNullOrEmpty(versionComment))
					{
						result = versions.FirstOrDefault(v => versionComment.EndsWith(v.SolutionCommentVersion)) ?? versions[0];
					}
				}
			}

			return result;
		}

		private static string[] GetConfigurationsFromLines(string[] lines)
		{
			List<string> result = new();
			int numLines = lines.Length;

			// Find the start of the configurations block.
			// In 2002, we're looking for a section like:
			// 	GlobalSection(SolutionConfiguration) = preSolution
			// 		ConfigName.0 = Debug
			// 		ConfigName.1 = Release
			// 	EndGlobalSection
			// In 2003, we're looking for a section like:
			// 	GlobalSection(SolutionConfiguration) = preSolution
			// 		Debug = Debug
			// 		Release = Release
			// 	EndGlobalSection
			// In 2005, 2008, 2010, 2012, 2013, 2015, 2017, 2019, and 2022 we're looking for a section like:
			// 	GlobalSection(SolutionConfigurationPlatforms) = preSolution
			// 		Debug|Any CPU = Debug|Any CPU
			// 		Release|Any CPU = Release|Any CPU
			// 	EndGlobalSection
			for (int searchIndex = 0; searchIndex < numLines; searchIndex++)
			{
				string line = lines[searchIndex];
				if (line.Trim().StartsWith("GlobalSection(SolutionConfiguration"))
				{
					for (int configIndex = searchIndex + 1; configIndex < numLines; configIndex++)
					{
						line = lines[configIndex];
						int equalPos = line.IndexOf('=');
						if (equalPos < 0)
						{
							break;
						}

						string configuration = line.Substring(equalPos + 1).Trim();
						result.Add(configuration);
					}

					break;
				}
			}

			return result.ToArray();
		}

		private void Browse_Click(object sender, EventArgs e)
		{
			this.OpenDlg.FileName = Manager.ExpandVariables(this.edtSolution.Text);
			if (this.OpenDlg.ShowDialog(this) == DialogResult.OK)
			{
				string fileName = Manager.CollapseVariables(this.OpenDlg.FileName);
				if (this.edtSolution.Text != fileName)
				{
					this.edtSolution.Text = fileName;
					this.RefreshSolutionInfo();
				}
			}
		}

		private void Refresh_Click(object sender, EventArgs e)
		{
			this.RefreshSolutionInfo();
			this.ConfigMover.UpdateControlStates();
		}

		private void RefreshSolutionInfo()
		{
			VSVersion currentSolutionVersion = (VSVersion)this.cbVersion.SelectedIndex;
			VSVersion newSolutionVersion = currentSolutionVersion;
			string[] configurations = GetConfigurationsFromSolution(this.edtSolution.Text, ref newSolutionVersion);

			// Update the configurations list
			this.lstConfigurations.Items.Clear();
			foreach (string configuration in configurations)
			{
				// Make all the configurations initially enabled.
				ListViewItem item = new(configuration)
				{
					Checked = true,
				};
				this.lstConfigurations.Items.Add(item);
			}

			// Update the version combo
			if (newSolutionVersion != currentSolutionVersion)
			{
				this.cbVersion.SelectedIndex = (int)newSolutionVersion;
			}
		}

		#endregion
	}
}
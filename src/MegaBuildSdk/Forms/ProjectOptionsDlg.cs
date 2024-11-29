namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Windows.Forms;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	internal sealed partial class ProjectOptionsDlg : ExtendedForm
	{
		#region Constructors

		public ProjectOptionsDlg()
		{
			this.InitializeComponent();

			this.cbVersion.Items.AddRange(VSVersionInfo.AllVersions.Select(v => v.FullDisplayName).ToArray());
		}

		#endregion

		#region Private Properties

		private ListViewItem? SelectedItem
			=> this.lstVariables.SelectedIndices.Count > 0 ? this.lstVariables.SelectedItems[0] : null;

		#endregion

		#region Internal Methods

		internal void LoadVariables(List<Project.VariableDefinition> variableDefinitions)
		{
			this.lstVariables.Items.Clear();
			foreach (var definition in variableDefinitions)
			{
				this.lstVariables.Items.Add(new ListViewItem([definition.Name, definition.Value, definition.ExpandPath.ToString()]));
			}
		}

		internal bool SaveVariables(List<Project.VariableDefinition> variableDefinitions)
		{
			List<Project.VariableDefinition> newDefinitions = new(this.lstVariables.Items.Count);
			foreach (ListViewItem item in this.lstVariables.Items)
			{
				string name = item.SubItems[0].Text;
				string value = item.SubItems[1].Text;
				if (!bool.TryParse(item.SubItems[2].Text, out bool expandPath))
				{
					expandPath = false;
				}

				newDefinitions.Add(new Project.VariableDefinition(name, value, expandPath));
			}

			// See if anything has changed.
			bool changed = variableDefinitions.Count != newDefinitions.Count;
			if (!changed)
			{
				// The counts were the same, so we need to compare each definition after ordering by name.
				var originalList = variableDefinitions.OrderBy(d => d.Name).ToList();
				var newList = newDefinitions.OrderBy(d => d.Name).ToList();
				for (int i = 0; i < newList.Count; i++)
				{
					var originalItem = originalList[i];
					var newItem = newList[i];
					if (originalItem.Name != newItem.Name || originalItem.Value != newItem.Value || originalItem.ExpandPath != newItem.ExpandPath)
					{
						changed = true;
						break;
					}
				}
			}

			// If anything changed, then replace the old definitions with the new ones.
			if (changed)
			{
				variableDefinitions.Clear();
				variableDefinitions.AddRange(newDefinitions);
			}

			return changed;
		}

		#endregion

		#region Private Methods

		private void Add_Click(object sender, EventArgs e)
		{
			ListViewItem item = new(["%%", string.Empty, "False"]);
			this.lstVariables.Items.Add(item);
			item.Selected = true;
			this.edtName.Focus();
		}

		private void Delete_Click(object sender, EventArgs e)
		{
			ListViewItem? item = this.SelectedItem;
			item?.Remove();
		}

		private void LogFile_Click(object sender, EventArgs e)
		{
			this.SaveDlg.FileName = this.edtLogFile.Text;
			if (this.SaveDlg.ShowDialog(this) == DialogResult.OK)
			{
				this.edtLogFile.Text = this.SaveDlg.FileName;
			}
		}

		private void OK_Click(object sender, EventArgs e)
		{
			if (this.chkLogOutput.Checked)
			{
				string logFile = this.edtLogFile.Text.Trim();
				if (logFile.Length == 0)
				{
					WindowsUtility.ShowError(this, "You must enter a log file name.");
					this.DialogResult = DialogResult.None;
				}

				string logPath = Manager.ExpandVariables(Path.GetDirectoryName(logFile));
				if (logPath.Length > 0 && !Directory.Exists(logPath))
				{
					WindowsUtility.ShowError(this, "The log file path does not exist.");
					this.DialogResult = DialogResult.None;
				}
			}

			// Check the variables for uniqueness and non-emptyness. Since the ListView
			// is sorted, we'll just scan and check the current against the previous.
			string previousName = string.Empty;
			foreach (ListViewItem item in this.lstVariables.Items)
			{
				string name = item.Text;
				if (name.Length == 0 || name == "%%")
				{
					WindowsUtility.ShowError(this, "A variable cannot have an empty name (i.e. %%).");
					this.DialogResult = DialogResult.None;
				}

				if (name == previousName)
				{
					WindowsUtility.ShowError(this, string.Format("The variable {0} is defined multiple times, but it can only be defined once.", name));
					this.DialogResult = DialogResult.None;
				}

				previousName = name;
			}
		}

		private void ExpandPath_CheckedChanged(object sender, EventArgs e)
		{
			ListViewItem? item = this.SelectedItem;
			if (item != null)
			{
				item.SubItems[2].Text = this.chkExpandPath.Checked.ToString();
			}
		}

		private void ControlStateChanged(object sender, EventArgs e)
		{
			this.UpdateControlStates();
		}

		private void Name_TextChanged(object sender, EventArgs e)
		{
			this.UpdateSelectedItem(this.edtName, 0, true);
		}

		private void Value_TextChanged(object sender, EventArgs e)
		{
			this.UpdateSelectedItem(this.edtValue, 1, false);
		}

		private void Variables_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool hasSelection = this.UpdateControlStates();

			if (hasSelection)
			{
				ListViewItem item = this.lstVariables.SelectedItems[0];
				this.edtName.Text = TextUtility.StripQuotes(item.SubItems[0].Text, "%");
				this.edtValue.Text = item.SubItems[1].Text;
				if (!bool.TryParse(item.SubItems[2].Text, out bool expandPath))
				{
					expandPath = false;
				}

				this.chkExpandPath.Checked = expandPath;
			}
			else
			{
				this.edtName.Text = string.Empty;
				this.edtValue.Text = string.Empty;
				this.chkExpandPath.Checked = false;
			}
		}

		private void ProjectOptionsDlg_Load(object sender, EventArgs e)
		{
			this.UpdateControlStates();

			if (this.lstConfigurations.Columns.Count > 0)
			{
				this.lstConfigurations.Columns[0].Width = this.lstConfigurations.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 1;
			}
		}

		private bool UpdateControlStates()
		{
			this.cbAction.Enabled = this.chkOverrideActions.Checked;
			this.cbVersion.Enabled = this.chkOverrideVersions.Checked;
			this.lstConfigurations.Enabled = this.chkOverrideConfigurations.Checked;
			this.ConfigMover.Enabled = this.chkOverrideConfigurations.Checked;

			bool hasSelection = this.SelectedItem != null;
			this.btnDelete.Enabled = hasSelection;
			this.edtName.Enabled = hasSelection;
			this.edtValue.Enabled = hasSelection;
			this.chkExpandPath.Enabled = hasSelection;

			bool enabled = this.chkLogOutput.Checked;
			this.edtLogFile.Enabled = enabled;
			this.btnLogFile.Enabled = enabled;
			this.chkOverwriteLog.Enabled = enabled;
			this.chkTimestamp.Enabled = enabled;

			return hasSelection;
		}

		private void UpdateSelectedItem(TextBox data, int subItemIndex, bool addPercents)
		{
			ListViewItem? item = this.SelectedItem;
			if (item != null)
			{
				string dataText = data.Text;
				if (addPercents)
				{
					dataText = string.Format("%{0}%", dataText);
				}

				item.SubItems[subItemIndex].Text = dataText;
			}
		}

		#endregion
	}
}
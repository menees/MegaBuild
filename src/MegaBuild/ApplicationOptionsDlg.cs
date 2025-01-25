namespace MegaBuild;

#region Using Directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Menees;
using Menees.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;

#endregion

internal sealed partial class ApplicationOptionsDlg : ExtendedForm
{
	#region Constructors

	public ApplicationOptionsDlg()
	{
		this.InitializeComponent();
		this.timestampFormat.Items.AddRange([.. Options.SupportedTimestampFormats]);
	}

	#endregion

	#region Private Properties

	private ListViewItem? SelectedItem
	{
		get
		{
			ListViewItem? result = null;

			if (this.lstVariables.SelectedIndices.Count > 0)
			{
				result = this.lstVariables.SelectedItems[0];
			}

			return result;
		}
	}

	#endregion

	#region Public Methods

	public bool Execute(Form owner)
	{
		this.chkAlwaysOnTop.Checked = Options.AlwaysOnTop;
		this.chkSaveChanges.Checked = Options.SaveChangesBeforeBuild;
		this.chkReloadLast.Checked = Options.ReloadLastProjectAtStartup;
		this.chkClearOutput.Checked = Options.ClearOutputBeforeBuild;
		this.chkWordWrap.Checked = Options.WordWrapOutputWindow;
		this.chkNeverShowProjectComments.Checked = Options.NeverShowProjectComments;
		this.chkSwitchToFailure.Checked = Options.SwitchTabsOnFailure;
		this.chkOutputWindowOnRight.Checked = Options.OutputWindowOnRight;
		this.chkParseOutputCommands.Checked = Manager.ParseOutputCommands;
		this.chkShowProgressInTaskbar.Checked = Options.ShowProgressInTaskbar;
		this.chkShowProgressInTaskbar.Enabled = TaskbarManager.IsPlatformSupported;
		this.chkShowProgressInTaskbar.Visible = TaskbarManager.IsPlatformSupported;
		this.timestampFormat.SelectedItem = Options.TimestampFormat;
		if (this.timestampFormat.SelectedIndex < 0)
		{
			this.timestampFormat.SelectedIndex = 0;
		}

		// Load variables
		this.lstVariables.Items.Clear();
		foreach (KeyValuePair<string, string> entry in Manager.Variables)
		{
			this.lstVariables.Items.Add(new ListViewItem([entry.Key, entry.Value]));
		}

		bool result = false;
		if (this.ShowDialog(owner) == DialogResult.OK)
		{
			Options.AlwaysOnTop = this.chkAlwaysOnTop.Checked;
			Options.SaveChangesBeforeBuild = this.chkSaveChanges.Checked;
			Options.ReloadLastProjectAtStartup = this.chkReloadLast.Checked;
			Options.ClearOutputBeforeBuild = this.chkClearOutput.Checked;
			Options.WordWrapOutputWindow = this.chkWordWrap.Checked;
			Options.NeverShowProjectComments = this.chkNeverShowProjectComments.Checked;
			Options.SwitchTabsOnFailure = this.chkSwitchToFailure.Checked;
			Options.OutputWindowOnRight = this.chkOutputWindowOnRight.Checked;
			Manager.ParseOutputCommands = this.chkParseOutputCommands.Checked;
			Options.ShowProgressInTaskbar = this.chkShowProgressInTaskbar.Checked;
			Options.TimestampFormat = Convert.ToString(this.timestampFormat.SelectedItem) ?? Options.DefaultTimestampFormat;

			// Save variables
			Manager.Variables.Clear();
			foreach (ListViewItem item in this.lstVariables.Items)
			{
				string key = item.SubItems[0].Text;
				if (key.Length > 0)
				{
					Manager.Variables.Add(key, item.SubItems[1].Text);
				}
			}

			result = true;
		}

		return result;
	}

	#endregion

	#region Private Methods

	private void ApplicationOptionsDlg_Load(object sender, EventArgs e)
	{
		this.UpdateControlStates();
	}

	private void Add_Click(object sender, EventArgs e)
	{
		ListViewItem item = new(["%%", string.Empty]);
		this.lstVariables.Items.Add(item);
		item.Selected = true;
		this.edtName.Focus();
	}

	private void Delete_Click(object sender, EventArgs e)
	{
		ListViewItem? item = this.SelectedItem;
		item?.Remove();
	}

	private void OK_Click(object sender, EventArgs e)
	{
		// Check the variables for uniqueness and non-emptyness.
		// Since the ListView is sorted, we'll just scan and
		// check the current against the previous.
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
		}
		else
		{
			this.edtName.Text = string.Empty;
			this.edtValue.Text = string.Empty;
		}
	}

	private bool UpdateControlStates()
	{
		bool hasSelection = this.SelectedItem != null;
		this.btnDelete.Enabled = hasSelection;
		this.edtName.Enabled = hasSelection;
		this.edtValue.Enabled = hasSelection;
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
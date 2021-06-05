namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Text;
	using System.Windows.Forms;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	internal partial class PowerShellStepCtrl : StepEditorControl
	{
		#region Private Data Members

		private PowerShellStep step;

		#endregion

		#region Constructors

		public PowerShellStepCtrl()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Public Properties

		public override string DisplayName => nameof(PowerShell);

		public PowerShellStep Step
		{
			set
			{
				if (this.step != value)
				{
					this.step = value;

					this.edtCommand.Text = this.step.Command;
					this.edtWorkingDirectory.Text = this.step.WorkingDirectory;
					this.shell.SelectedIndex = (int)this.step.Shell;
					this.treatErrorAsOutput.Checked = this.step.TreatErrorStreamAsOutput;
				}
			}
		}

		#endregion

		#region Public Methods

		public override bool OnOk()
		{
			bool result = false;

			if (string.IsNullOrEmpty(this.edtCommand.Text.Trim()))
			{
				WindowsUtility.ShowError(this, "You must enter a script or command.");
			}
			else
			{
				this.step.Command = this.edtCommand.Text;
				this.step.WorkingDirectory = this.edtWorkingDirectory.Text;
				this.step.Shell = (PowerShell)this.shell.SelectedIndex;
				this.step.TreatErrorStreamAsOutput = this.treatErrorAsOutput.Checked;
				result = true;
			}

			return result;
		}

		#endregion

		#region Private Methods

		private void BrowseCmd_Click(object sender, EventArgs e)
		{
			this.OpenDlg.FileName = Manager.ExpandVariables(this.edtCommand.Text);
			if (this.OpenDlg.ShowDialog(this) == DialogResult.OK)
			{
				this.edtCommand.Text = Manager.CollapseVariables(this.OpenDlg.FileName);
			}
		}

		private void BrowseDirectory_Click(object sender, EventArgs e)
		{
			string initialFolder = Manager.ExpandVariables(this.edtWorkingDirectory.Text);
			string selectedFolder = WindowsUtility.SelectFolder(this, "Select Working Directory", initialFolder);
			if (!string.IsNullOrEmpty(selectedFolder))
			{
				this.edtWorkingDirectory.Text = Manager.CollapseVariables(selectedFolder);
			}
		}

		#endregion
	}
}
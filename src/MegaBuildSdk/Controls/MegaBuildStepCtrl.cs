namespace MegaBuild;

#region Using Directives

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Menees;
using Menees.Windows.Forms;

#endregion

internal sealed partial class MegaBuildStepCtrl : StepEditorControl
{
	#region Private Data Members

	private MegaBuildStep? step;

	#endregion

	#region Constructors

	public MegaBuildStepCtrl()
	{
		this.InitializeComponent();
	}

	#endregion

	#region Public Properties

	public override string DisplayName => nameof(MegaBuild);

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public MegaBuildStep Step
	{
		set
		{
			if (this.step != value)
			{
				this.step = value;

				this.edtProject.Text = this.step.ProjectFile;
				this.chkExit.Checked = this.step.Exit;
				this.chkMinimize.Checked = this.step.Minimize;
				this.chkOutOfProc.Checked = !this.step.InProc;

				this.UpdateControls();
			}
		}
	}

	#endregion

	#region Public Methods

	public override bool OnOk()
	{
		bool result = false;

		string project = this.edtProject.Text.Trim();
		if (string.IsNullOrEmpty(project))
		{
			WindowsUtility.ShowError(this, "You must enter a MegaBuild project for the step.");
		}
		else if (this.step != null)
		{
			this.step.ProjectFile = this.edtProject.Text.Trim();
			this.step.InProc = !this.chkOutOfProc.Checked;
			this.step.Exit = this.chkExit.Checked;
			this.step.Minimize = this.chkMinimize.Checked;
			result = true;
		}

		return result;
	}

	#endregion

	#region Private Methods

	private void Open_Click(object sender, EventArgs e)
	{
		this.OpenDlg.FileName = Manager.ExpandVariables(this.edtProject.Text);
		if (this.OpenDlg.ShowDialog(this) == DialogResult.OK)
		{
			this.edtProject.Text = Manager.CollapseVariables(this.OpenDlg.FileName);
		}
	}

	private void OutOfProc_CheckedChanged(object sender, EventArgs e)
	{
		this.UpdateControls();
	}

	private void UpdateControls()
	{
		bool enabled = this.chkOutOfProc.Checked;
		this.chkMinimize.Enabled = enabled;
		this.chkExit.Enabled = enabled;
	}

	#endregion
}
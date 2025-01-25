namespace MegaBuild;

#region Using Directives

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Menees;
using Menees.Windows.Forms;

#endregion

internal sealed partial class OutputStepCtrl : StepEditorControl
{
	#region Private Data Members

	private OutputStep? step;

	#endregion

	#region Constructors

	public OutputStepCtrl()
	{
		this.InitializeComponent();
	}

	#endregion

	#region Public Properties

	public override string DisplayName => "Output";

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public OutputStep Step
	{
		set
		{
			if (this.step != value)
			{
				this.step = value;

				this.txtMessage.Text = this.step.Message;
				this.chkIncludeTimestamp.Checked = this.step.IncludeTimestamp;
				this.numIndent.Value = this.step.IndentOutput;
				this.pnlColor.BackColor = this.step.TextColor;
				this.chkIsHighlight.Checked = this.step.IsHighlight;
			}
		}
	}

	#endregion

	#region Public Methods

	public override bool OnOk()
	{
		bool result = false;

		if (string.IsNullOrEmpty(this.txtMessage.Text.Trim()))
		{
			WindowsUtility.ShowError(this, "You must enter a message to be output.");
		}
		else if (this.step != null)
		{
			this.step.Message = this.txtMessage.Text;
			this.step.IncludeTimestamp = this.chkIncludeTimestamp.Checked;
			this.step.IndentOutput = (int)this.numIndent.Value;
			this.step.TextColor = this.pnlColor.BackColor;
			this.step.IsHighlight = this.chkIsHighlight.Checked;
			result = true;
		}

		return result;
	}

	#endregion

	#region Private Methods

	private void Color_Click(object sender, EventArgs e)
	{
		this.ColorDlg.Color = this.pnlColor.BackColor;
		if (this.ColorDlg.ShowDialog(this) == DialogResult.OK)
		{
			this.pnlColor.BackColor = this.ColorDlg.Color;
		}
	}

	#endregion
}
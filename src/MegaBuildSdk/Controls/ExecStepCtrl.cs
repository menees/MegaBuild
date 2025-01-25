namespace MegaBuild;

#region Using Directives

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Menees.Windows.Forms;

#endregion

internal sealed partial class ExecStepCtrl : StepEditorControl
{
	#region Private Data Members

	private ExecutableStep? step;

	#endregion

	#region Constructors

	public ExecStepCtrl()
	{
		this.InitializeComponent();
	}

	#endregion

	#region Public Properties

	public override string DisplayName => "Execution";

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ExecutableStep Step
	{
		set
		{
			if (this.step != value)
			{
				this.step = value;

				this.chkIgnoreFailure.Checked = this.step.IgnoreFailure;
				this.chkWaitForCompletion.Checked = this.step.WaitForCompletion;
				this.chkOnlyIfParentSucceeded.Checked = this.step.OnlyIfParentSucceeded;
				this.chkPromptFirst.Checked = this.step.PromptFirst;
				this.chkTimeout.Checked = this.step.Timeout;
				this.numTimeout.Value = this.step.TimeoutMinutes;
				this.chkRunAsAdministrator.Checked = this.step.IsAdministratorRequired;
				this.chkRunAsAdministrator.Enabled = this.step.MayRequireAdministrator;

				this.UpdateControlStates();
			}
		}
	}

	#endregion

	#region Public Methods

	public override bool OnOk()
	{
		bool result = false;

		if (this.step != null)
		{
			this.step.IgnoreFailure = this.chkIgnoreFailure.Checked;
			this.step.WaitForCompletion = this.chkWaitForCompletion.Checked;
			this.step.OnlyIfParentSucceeded = this.chkOnlyIfParentSucceeded.Checked;
			this.step.PromptFirst = this.chkPromptFirst.Checked;
			this.step.Timeout = this.chkTimeout.Checked;
			this.step.TimeoutMinutes = (int)this.numTimeout.Value;
			this.step.IsAdministratorRequired = this.chkRunAsAdministrator.Checked;
			result = true;
		}

		return result;
	}

	#endregion

	#region Private Methods

	private void Timeout_CheckedChanged(object sender, EventArgs e)
	{
		this.UpdateControlStates();
	}

	private void WaitForCompletion_CheckedChanged(object sender, EventArgs e)
	{
		this.UpdateControlStates();
	}

	private void UpdateControlStates()
	{
		if (this.step != null)
		{
			this.chkWaitForCompletion.Enabled = this.step.SupportsWaitForCompletion;
			this.chkTimeout.Enabled = this.step.SupportsTimeout && this.chkWaitForCompletion.Checked;
			this.numTimeout.Enabled = this.chkTimeout.Enabled && this.chkTimeout.Checked;
		}
	}

	#endregion
}
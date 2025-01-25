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

internal sealed partial class GeneralStepCtrl : StepEditorControl
{
	#region Private Data Members

	private Step? step;

	#endregion

	#region Constructors

	public GeneralStepCtrl()
	{
		this.InitializeComponent();
	}

	#endregion

	#region Public Properties

	public override string DisplayName => "General";

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Step Step
	{
		set
		{
			if (this.step != value)
			{
				this.step = value;

				this.edtName.Text = this.step.Name;
				this.txtDescription.Text = this.step.Description;
			}
		}
	}

	#endregion

	#region Public Methods

	public override bool OnOk()
	{
		bool result = false;

		string name = this.edtName.Text.Trim();
		if (name.Length == 0)
		{
			WindowsUtility.ShowError(this, "You must enter a name for the step.");
		}
		else if (this.step != null)
		{
			this.step.Name = name;
			this.step.Description = this.txtDescription.Text;
			result = true;
		}

		return result;
	}

	#endregion
}
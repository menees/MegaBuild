namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Windows.Forms;
	using Menees.Windows.Forms;

	#endregion

	internal sealed partial class SleepStepCtrl : StepEditorControl
	{
		#region Private Data Members

		private SleepStep? step;

		#endregion

		#region Constructors

		public SleepStepCtrl()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Public Properties

		public override string DisplayName => "Sleep";

		public SleepStep Step
		{
			set
			{
				if (this.step != value)
				{
					this.step = value;

					this.edtSleepTimeMilliseconds.Value = this.step.SleepTimeMilliseconds;
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
				this.step.SleepTimeMilliseconds = (int)this.edtSleepTimeMilliseconds.Value;
				result = true;
			}

			return result;
		}

		#endregion
	}
}
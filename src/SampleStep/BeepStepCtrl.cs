namespace SampleStep
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Windows.Forms;
	using MegaBuild;
	using Menees.Windows.Forms;

	#endregion

	public sealed partial class BeepStepCtrl : StepEditorControl
	{
		#region Private Data Members

		private BeepStep step;

		#endregion

		#region Constructors

		public BeepStepCtrl()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Public Properties

		public override string DisplayName => "Beep";

		public BeepStep Step
		{
			get => this.step;

			set
			{
				if (this.step != value)
				{
					this.step = value;

					this.edtFrequency.Value = this.step.Frequency;
					this.edtDuration.Value = this.step.Duration;
				}
			}
		}

		#endregion

		#region Public Methods

		public override bool OnOk()
		{
			this.step.Frequency = (int)this.edtFrequency.Value;
			this.step.Duration = (int)this.edtDuration.Value;
			return true;
		}

		#endregion
	}
}
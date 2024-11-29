namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Linq;
	using System.Windows.Forms;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	internal sealed partial class SoundStepCtrl : StepEditorControl
	{
		#region Private Data Members

		private SoundStep? step;

		#endregion

		#region Constructors

		public SoundStepCtrl()
		{
			this.InitializeComponent();

			// Get all the SystemSound enum values, but order by name instead of binary value.
			var items = ((SystemSound[])Enum.GetValues(typeof(SystemSound))).OrderBy(s => s.ToString()).Select(s => (object)s).ToArray();
			this.cbSystemSound.Items.AddRange(items);
		}

		#endregion

		#region Public Properties

		public override string DisplayName => "Sound";

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SoundStep Step
		{
			set
			{
				if (this.step != value)
				{
					this.step = value;

					this.rbSystemSound.Checked = false;
					this.rbWavFile.Checked = false;
					this.rbBeep.Checked = false;

					switch (this.step.Style)
					{
						case SoundStyle.SystemSound:
							this.rbSystemSound.Checked = true;
							break;

						case SoundStyle.WavFile:
							this.rbWavFile.Checked = true;
							break;

						case SoundStyle.Beep:
							this.rbBeep.Checked = true;
							break;
					}

					this.cbSystemSound.SelectedItem = this.step.SystemSound;
					this.edtWavFile.Text = this.step.WavFile;
					this.edtFrequency.Value = this.step.Frequency;
					this.edtDuration.Value = this.step.Duration;

					this.UpdateControlStates();
				}
			}
		}

		#endregion

		#region Public Methods

		public override bool OnOk()
		{
			bool result = false;

			if (this.rbWavFile.Checked && string.IsNullOrEmpty(this.edtWavFile.Text.Trim()))
			{
				WindowsUtility.ShowError(this, "You must specify a .wav filename.");
			}
			else if (this.step != null)
			{
				if (this.rbSystemSound.Checked)
				{
					this.step.Style = SoundStyle.SystemSound;
				}
				else if (this.rbWavFile.Checked)
				{
					this.step.Style = SoundStyle.WavFile;
				}
				else
				{
					this.step.Style = SoundStyle.Beep;
				}

				this.step.SystemSound = (SystemSound?)this.cbSystemSound.SelectedItem ?? default;
				this.step.WavFile = this.edtWavFile.Text.Trim();
				this.step.Frequency = (int)this.edtFrequency.Value;
				this.step.Duration = (int)this.edtDuration.Value;

				result = true;
			}

			return result;
		}

		#endregion

		#region Private Methods

		private void WavFile_Click(object sender, EventArgs e)
		{
			this.OpenDlg.FileName = Manager.ExpandVariables(this.edtWavFile.Text);
			if (this.OpenDlg.ShowDialog(this) == DialogResult.OK)
			{
				this.edtWavFile.Text = Manager.CollapseVariables(this.OpenDlg.FileName);
			}
		}

		private void RadioButton_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateControlStates();
		}

		private void UpdateControlStates()
		{
			this.cbSystemSound.Enabled = this.rbSystemSound.Checked;

			this.edtWavFile.Enabled = this.rbWavFile.Checked;
			this.btnWavFile.Enabled = this.rbWavFile.Checked;

			this.edtFrequency.Enabled = this.rbBeep.Checked;
			this.edtDuration.Enabled = this.rbBeep.Checked;
		}

		#endregion
	}
}
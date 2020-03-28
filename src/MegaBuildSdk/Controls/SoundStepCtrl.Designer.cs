using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Menees;
using Menees.Windows.Forms;
namespace MegaBuild
{
	internal sealed partial class SoundStepCtrl
	{
		private System.Windows.Forms.ComboBox cbSystemSound;
		private System.Windows.Forms.OpenFileDialog OpenDlg;
		private System.Windows.Forms.RadioButton rbSystemSound;
		private System.Windows.Forms.RadioButton rbWavFile;
		private System.Windows.Forms.Button btnWavFile;
		private System.Windows.Forms.TextBox edtWavFile;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.NumericUpDown edtFrequency;
		private System.Windows.Forms.Label lblFrequency;
		private System.Windows.Forms.NumericUpDown edtDuration;
		private System.Windows.Forms.Label lblDuration;
		private System.Windows.Forms.RadioButton rbBeep;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cbSystemSound = new System.Windows.Forms.ComboBox();
			this.OpenDlg = new System.Windows.Forms.OpenFileDialog();
			this.rbSystemSound = new System.Windows.Forms.RadioButton();
			this.rbWavFile = new System.Windows.Forms.RadioButton();
			this.btnWavFile = new System.Windows.Forms.Button();
			this.edtWavFile = new System.Windows.Forms.TextBox();
			this.edtFrequency = new System.Windows.Forms.NumericUpDown();
			this.lblFrequency = new System.Windows.Forms.Label();
			this.edtDuration = new System.Windows.Forms.NumericUpDown();
			this.lblDuration = new System.Windows.Forms.Label();
			this.rbBeep = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.edtFrequency)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.edtDuration)).BeginInit();
			this.SuspendLayout();
			// 
			// cbSystemSound
			// 
			this.cbSystemSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSystemSound.Location = new System.Drawing.Point(28, 40);
			this.cbSystemSound.Name = "cbSystemSound";
			this.cbSystemSound.Size = new System.Drawing.Size(156, 23);
			this.cbSystemSound.TabIndex = 1;
			// 
			// OpenDlg
			// 
			this.OpenDlg.DefaultExt = "wav";
			this.OpenDlg.Filter = "Wav Files (*.wav)|*.wav|All Files (*.*)|*.*";
			this.OpenDlg.Title = "Select Sound";
			// 
			// rbSystemSound
			// 
			this.rbSystemSound.AutoSize = true;
			this.rbSystemSound.Location = new System.Drawing.Point(12, 12);
			this.rbSystemSound.Name = "rbSystemSound";
			this.rbSystemSound.Size = new System.Drawing.Size(133, 19);
			this.rbSystemSound.TabIndex = 0;
			this.rbSystemSound.Text = "Use A System Sound";
			this.rbSystemSound.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
			// 
			// rbWavFile
			// 
			this.rbWavFile.AutoSize = true;
			this.rbWavFile.Location = new System.Drawing.Point(12, 72);
			this.rbWavFile.Name = "rbWavFile";
			this.rbWavFile.Size = new System.Drawing.Size(102, 19);
			this.rbWavFile.TabIndex = 2;
			this.rbWavFile.Text = "Use A Wav File";
			// 
			// btnWavFile
			// 
			this.btnWavFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnWavFile.Location = new System.Drawing.Point(340, 99);
			this.btnWavFile.Name = "btnWavFile";
			this.btnWavFile.Size = new System.Drawing.Size(28, 24);
			this.btnWavFile.TabIndex = 4;
			this.btnWavFile.Text = "...";
			this.btnWavFile.Click += new System.EventHandler(this.WavFile_Click);
			// 
			// edtWavFile
			// 
			this.edtWavFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtWavFile.Location = new System.Drawing.Point(28, 100);
			this.edtWavFile.Name = "edtWavFile";
			this.edtWavFile.Size = new System.Drawing.Size(300, 23);
			this.edtWavFile.TabIndex = 3;
			// 
			// edtFrequency
			// 
			this.edtFrequency.Location = new System.Drawing.Point(176, 164);
			this.edtFrequency.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.edtFrequency.Minimum = new decimal(new int[] {
            37,
            0,
            0,
            0});
			this.edtFrequency.Name = "edtFrequency";
			this.edtFrequency.Size = new System.Drawing.Size(68, 23);
			this.edtFrequency.TabIndex = 7;
			this.edtFrequency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.edtFrequency.Value = new decimal(new int[] {
            37,
            0,
            0,
            0});
			// 
			// lblFrequency
			// 
			this.lblFrequency.AutoSize = true;
			this.lblFrequency.Location = new System.Drawing.Point(28, 168);
			this.lblFrequency.Name = "lblFrequency";
			this.lblFrequency.Size = new System.Drawing.Size(109, 15);
			this.lblFrequency.TabIndex = 6;
			this.lblFrequency.Text = "Frequency in Hertz:";
			// 
			// edtDuration
			// 
			this.edtDuration.Location = new System.Drawing.Point(176, 196);
			this.edtDuration.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.edtDuration.Name = "edtDuration";
			this.edtDuration.Size = new System.Drawing.Size(68, 23);
			this.edtDuration.TabIndex = 9;
			this.edtDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblDuration
			// 
			this.lblDuration.AutoSize = true;
			this.lblDuration.Location = new System.Drawing.Point(28, 200);
			this.lblDuration.Name = "lblDuration";
			this.lblDuration.Size = new System.Drawing.Size(138, 15);
			this.lblDuration.TabIndex = 8;
			this.lblDuration.Text = "Duration in Milliseconds:";
			// 
			// rbBeep
			// 
			this.rbBeep.AutoSize = true;
			this.rbBeep.Location = new System.Drawing.Point(12, 136);
			this.rbBeep.Name = "rbBeep";
			this.rbBeep.Size = new System.Drawing.Size(84, 19);
			this.rbBeep.TabIndex = 5;
			this.rbBeep.Text = "Use A Beep";
			this.rbBeep.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
			// 
			// SoundStepCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.Controls.Add(this.rbBeep);
			this.Controls.Add(this.edtFrequency);
			this.Controls.Add(this.lblFrequency);
			this.Controls.Add(this.edtDuration);
			this.Controls.Add(this.lblDuration);
			this.Controls.Add(this.btnWavFile);
			this.Controls.Add(this.edtWavFile);
			this.Controls.Add(this.rbWavFile);
			this.Controls.Add(this.rbSystemSound);
			this.Controls.Add(this.cbSystemSound);
			this.Name = "SoundStepCtrl";
			this.Size = new System.Drawing.Size(380, 314);
			((System.ComponentModel.ISupportInitialize)(this.edtFrequency)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.edtDuration)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}


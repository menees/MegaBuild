using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MegaBuild;
using Menees.Windows.Forms;
namespace SampleStep
{
	public sealed partial class BeepStepCtrl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label lblDuration;
		private System.Windows.Forms.NumericUpDown edtFrequency;
		private System.Windows.Forms.Label lblFrequency;
		private System.Windows.Forms.NumericUpDown edtDuration;

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
			this.edtDuration = new System.Windows.Forms.NumericUpDown();
			this.lblDuration = new System.Windows.Forms.Label();
			this.edtFrequency = new System.Windows.Forms.NumericUpDown();
			this.lblFrequency = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// edtDuration
			// 
			this.edtDuration.Location = new System.Drawing.Point(160, 44);
			this.edtDuration.Maximum = 10000;
			this.edtDuration.Name = "edtDuration";
			this.edtDuration.Size = new System.Drawing.Size(68, 20);
			this.edtDuration.TabIndex = 3;
			this.edtDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblDuration
			// 
			this.lblDuration.AutoSize = true;
			this.lblDuration.Location = new System.Drawing.Point(12, 48);
			this.lblDuration.Name = "lblDuration";
			this.lblDuration.Size = new System.Drawing.Size(127, 13);
			this.lblDuration.TabIndex = 2;
			this.lblDuration.Text = "Duration in Milliseconds:";
			// 
			// edtFrequency
			// 
			this.edtFrequency.Location = new System.Drawing.Point(160, 12);
			this.edtFrequency.Maximum = 32767;
			this.edtFrequency.Minimum = 37;
			this.edtFrequency.Name = "edtFrequency";
			this.edtFrequency.Size = new System.Drawing.Size(68, 20);
			this.edtFrequency.TabIndex = 1;
			this.edtFrequency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.edtFrequency.Value = 37;
			// 
			// lblFrequency
			// 
			this.lblFrequency.AutoSize = true;
			this.lblFrequency.Location = new System.Drawing.Point(12, 16);
			this.lblFrequency.Name = "lblFrequency";
			this.lblFrequency.Size = new System.Drawing.Size(103, 13);
			this.lblFrequency.TabIndex = 0;
			this.lblFrequency.Text = "Frequency in Hertz:";
			// 
			// BeepStepCtrl
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.edtFrequency,
																		  this.lblFrequency,
																		  this.edtDuration,
																		  this.lblDuration});
			this.Name = "BeepStepCtrl";
			this.Size = new System.Drawing.Size(380, 314);
			this.ResumeLayout(false);

		}
		#endregion
	}
}


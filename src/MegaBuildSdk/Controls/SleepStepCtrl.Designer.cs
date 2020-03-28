using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Menees.Windows.Forms;
namespace MegaBuild
{
	internal sealed partial class SleepStepCtrl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label lblSleepTime;
		private System.Windows.Forms.NumericUpDown edtSleepTimeMilliseconds;

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
			this.lblSleepTime = new System.Windows.Forms.Label();
			this.edtSleepTimeMilliseconds = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.edtSleepTimeMilliseconds)).BeginInit();
			this.SuspendLayout();
			// 
			// lblSleepTime
			// 
			this.lblSleepTime.AutoSize = true;
			this.lblSleepTime.Location = new System.Drawing.Point(12, 20);
			this.lblSleepTime.Name = "lblSleepTime";
			this.lblSleepTime.Size = new System.Drawing.Size(150, 15);
			this.lblSleepTime.TabIndex = 0;
			this.lblSleepTime.Text = "Sleep Time In Milliseconds:";
			// 
			// edtSleepTime
			// 
			this.edtSleepTimeMilliseconds.Location = new System.Drawing.Point(168, 16);
			this.edtSleepTimeMilliseconds.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.edtSleepTimeMilliseconds.Name = "edtSleepTime";
			this.edtSleepTimeMilliseconds.Size = new System.Drawing.Size(68, 23);
			this.edtSleepTimeMilliseconds.TabIndex = 1;
			this.edtSleepTimeMilliseconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// SleepStepCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.Controls.Add(this.edtSleepTimeMilliseconds);
			this.Controls.Add(this.lblSleepTime);
			this.Name = "SleepStepCtrl";
			this.Size = new System.Drawing.Size(380, 314);
			((System.ComponentModel.ISupportInitialize)(this.edtSleepTimeMilliseconds)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}


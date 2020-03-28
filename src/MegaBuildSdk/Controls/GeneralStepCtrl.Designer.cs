using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Menees;
using Menees.Windows.Forms;
namespace MegaBuild
{
	internal sealed partial class GeneralStepCtrl
	{
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.TextBox edtName;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.Label lblName;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

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
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.edtName = new System.Windows.Forms.TextBox();
			this.lblDescription = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtDescription
			// 
			this.txtDescription.AcceptsReturn = true;
			this.txtDescription.AcceptsTab = true;
			this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDescription.Location = new System.Drawing.Point(12, 79);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtDescription.Size = new System.Drawing.Size(356, 224);
			this.txtDescription.TabIndex = 3;
			// 
			// edtName
			// 
			this.edtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtName.Location = new System.Drawing.Point(12, 31);
			this.edtName.Name = "edtName";
			this.edtName.Size = new System.Drawing.Size(356, 23);
			this.edtName.TabIndex = 1;
			// 
			// lblDescription
			// 
			this.lblDescription.AutoSize = true;
			this.lblDescription.Location = new System.Drawing.Point(12, 59);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(70, 15);
			this.lblDescription.TabIndex = 2;
			this.lblDescription.Text = "Description:";
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(12, 11);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(42, 15);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name:";
			// 
			// GeneralStepCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.edtName);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.lblName);
			this.Name = "GeneralStepCtrl";
			this.Size = new System.Drawing.Size(380, 314);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}


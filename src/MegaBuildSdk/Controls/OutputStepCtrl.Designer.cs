using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Menees;
using Menees.Windows.Forms;
namespace MegaBuild
{
	internal sealed partial class OutputStepCtrl
	{
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.CheckBox chkIncludeTimestamp;
		private System.Windows.Forms.Label lblIndent;
		private System.Windows.Forms.Label lblColor;
		private System.Windows.Forms.ColorDialog ColorDlg;
		private System.Windows.Forms.NumericUpDown numIndent;
		private System.Windows.Forms.Panel pnlColor;
		private System.Windows.Forms.Button btnColor;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private CheckBox chkIsHighlight;

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
			this.chkIncludeTimestamp = new System.Windows.Forms.CheckBox();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.lblMessage = new System.Windows.Forms.Label();
			this.lblIndent = new System.Windows.Forms.Label();
			this.lblColor = new System.Windows.Forms.Label();
			this.ColorDlg = new System.Windows.Forms.ColorDialog();
			this.numIndent = new System.Windows.Forms.NumericUpDown();
			this.pnlColor = new System.Windows.Forms.Panel();
			this.btnColor = new System.Windows.Forms.Button();
			this.chkIsHighlight = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.numIndent)).BeginInit();
			this.SuspendLayout();
			// 
			// chkIncludeTimestamp
			// 
			this.chkIncludeTimestamp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkIncludeTimestamp.AutoSize = true;
			this.chkIncludeTimestamp.Location = new System.Drawing.Point(12, 273);
			this.chkIncludeTimestamp.Name = "chkIncludeTimestamp";
			this.chkIncludeTimestamp.Size = new System.Drawing.Size(128, 19);
			this.chkIncludeTimestamp.TabIndex = 2;
			this.chkIncludeTimestamp.Text = "Include Timestamp";
			// 
			// txtMessage
			// 
			this.txtMessage.AcceptsReturn = true;
			this.txtMessage.AcceptsTab = true;
			this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMessage.Location = new System.Drawing.Point(12, 32);
			this.txtMessage.Multiline = true;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtMessage.Size = new System.Drawing.Size(356, 236);
			this.txtMessage.TabIndex = 1;
			// 
			// lblMessage
			// 
			this.lblMessage.AutoSize = true;
			this.lblMessage.Location = new System.Drawing.Point(8, 12);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(56, 15);
			this.lblMessage.TabIndex = 0;
			this.lblMessage.Text = "Message:";
			// 
			// lblIndent
			// 
			this.lblIndent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblIndent.AutoSize = true;
			this.lblIndent.Location = new System.Drawing.Point(144, 282);
			this.lblIndent.Name = "lblIndent";
			this.lblIndent.Size = new System.Drawing.Size(44, 15);
			this.lblIndent.TabIndex = 4;
			this.lblIndent.Text = "Indent:";
			// 
			// lblColor
			// 
			this.lblColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblColor.AutoSize = true;
			this.lblColor.Location = new System.Drawing.Point(252, 282);
			this.lblColor.Name = "lblColor";
			this.lblColor.Size = new System.Drawing.Size(39, 15);
			this.lblColor.TabIndex = 6;
			this.lblColor.Text = "Color:";
			// 
			// numIndent
			// 
			this.numIndent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.numIndent.Location = new System.Drawing.Point(192, 280);
			this.numIndent.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numIndent.Name = "numIndent";
			this.numIndent.Size = new System.Drawing.Size(36, 23);
			this.numIndent.TabIndex = 5;
			this.numIndent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// pnlColor
			// 
			this.pnlColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlColor.BackColor = System.Drawing.SystemColors.WindowText;
			this.pnlColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlColor.Location = new System.Drawing.Point(296, 280);
			this.pnlColor.Name = "pnlColor";
			this.pnlColor.Size = new System.Drawing.Size(40, 24);
			this.pnlColor.TabIndex = 7;
			// 
			// btnColor
			// 
			this.btnColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnColor.BackColor = System.Drawing.SystemColors.Control;
			this.btnColor.Location = new System.Drawing.Point(344, 280);
			this.btnColor.Name = "btnColor";
			this.btnColor.Size = new System.Drawing.Size(28, 24);
			this.btnColor.TabIndex = 8;
			this.btnColor.Text = "...";
			this.btnColor.UseVisualStyleBackColor = false;
			this.btnColor.Click += new System.EventHandler(this.Color_Click);
			// 
			// chkIsHighlight
			// 
			this.chkIsHighlight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkIsHighlight.AutoSize = true;
			this.chkIsHighlight.Location = new System.Drawing.Point(12, 293);
			this.chkIsHighlight.Name = "chkIsHighlight";
			this.chkIsHighlight.Size = new System.Drawing.Size(87, 19);
			this.chkIsHighlight.TabIndex = 3;
			this.chkIsHighlight.Text = "Is Highlight";
			// 
			// OutputStepCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.Controls.Add(this.chkIsHighlight);
			this.Controls.Add(this.pnlColor);
			this.Controls.Add(this.numIndent);
			this.Controls.Add(this.lblColor);
			this.Controls.Add(this.lblIndent);
			this.Controls.Add(this.chkIncludeTimestamp);
			this.Controls.Add(this.txtMessage);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.btnColor);
			this.Name = "OutputStepCtrl";
			this.Size = new System.Drawing.Size(380, 314);
			((System.ComponentModel.ISupportInitialize)(this.numIndent)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}


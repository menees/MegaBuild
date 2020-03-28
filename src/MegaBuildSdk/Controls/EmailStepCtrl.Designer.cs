using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Menees;
using System.Net.Mail;
using Menees.Windows.Forms;
namespace MegaBuild
{
	internal sealed partial class EmailStepCtrl
	{
		private System.Windows.Forms.TextBox edtTo;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.TextBox edtCC;
		private System.Windows.Forms.Label lblCC;
		private System.Windows.Forms.TextBox edtSubject;
		private System.Windows.Forms.Label lblSubject;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.CheckBox chkAppendOutput;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox edtFrom;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.ComboBox cbPriority;
		private System.Windows.Forms.Label lblPriority;
		private System.Windows.Forms.Label lblSmtpServer;
		private System.Windows.Forms.TextBox edtSmtpServer;

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
			this.edtTo = new System.Windows.Forms.TextBox();
			this.lblTo = new System.Windows.Forms.Label();
			this.edtCC = new System.Windows.Forms.TextBox();
			this.lblCC = new System.Windows.Forms.Label();
			this.edtSubject = new System.Windows.Forms.TextBox();
			this.lblSubject = new System.Windows.Forms.Label();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.chkAppendOutput = new System.Windows.Forms.CheckBox();
			this.edtFrom = new System.Windows.Forms.TextBox();
			this.lblFrom = new System.Windows.Forms.Label();
			this.cbPriority = new System.Windows.Forms.ComboBox();
			this.lblPriority = new System.Windows.Forms.Label();
			this.edtSmtpServer = new System.Windows.Forms.TextBox();
			this.lblSmtpServer = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// edtTo
			// 
			this.edtTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtTo.Location = new System.Drawing.Point(60, 40);
			this.edtTo.Name = "edtTo";
			this.edtTo.Size = new System.Drawing.Size(308, 23);
			this.edtTo.TabIndex = 3;
			// 
			// lblTo
			// 
			this.lblTo.AutoSize = true;
			this.lblTo.Location = new System.Drawing.Point(8, 44);
			this.lblTo.Name = "lblTo";
			this.lblTo.Size = new System.Drawing.Size(24, 15);
			this.lblTo.TabIndex = 2;
			this.lblTo.Text = "To:";
			// 
			// edtCC
			// 
			this.edtCC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtCC.Location = new System.Drawing.Point(60, 68);
			this.edtCC.Name = "edtCC";
			this.edtCC.Size = new System.Drawing.Size(308, 23);
			this.edtCC.TabIndex = 5;
			// 
			// lblCC
			// 
			this.lblCC.AutoSize = true;
			this.lblCC.Location = new System.Drawing.Point(8, 72);
			this.lblCC.Name = "lblCC";
			this.lblCC.Size = new System.Drawing.Size(26, 15);
			this.lblCC.TabIndex = 4;
			this.lblCC.Text = "CC:";
			// 
			// edtSubject
			// 
			this.edtSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtSubject.Location = new System.Drawing.Point(60, 96);
			this.edtSubject.Name = "edtSubject";
			this.edtSubject.Size = new System.Drawing.Size(308, 23);
			this.edtSubject.TabIndex = 7;
			// 
			// lblSubject
			// 
			this.lblSubject.AutoSize = true;
			this.lblSubject.Location = new System.Drawing.Point(8, 100);
			this.lblSubject.Name = "lblSubject";
			this.lblSubject.Size = new System.Drawing.Size(49, 15);
			this.lblSubject.TabIndex = 6;
			this.lblSubject.Text = "Subject:";
			// 
			// txtMessage
			// 
			this.txtMessage.AcceptsReturn = true;
			this.txtMessage.AcceptsTab = true;
			this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMessage.Location = new System.Drawing.Point(12, 156);
			this.txtMessage.Multiline = true;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtMessage.Size = new System.Drawing.Size(356, 116);
			this.txtMessage.TabIndex = 11;
			// 
			// chkAppendOutput
			// 
			this.chkAppendOutput.AutoSize = true;
			this.chkAppendOutput.Location = new System.Drawing.Point(188, 128);
			this.chkAppendOutput.Name = "chkAppendOutput";
			this.chkAppendOutput.Size = new System.Drawing.Size(181, 19);
			this.chkAppendOutput.TabIndex = 10;
			this.chkAppendOutput.Text = "Append Output Window Text";
			// 
			// edtFrom
			// 
			this.edtFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtFrom.Location = new System.Drawing.Point(60, 12);
			this.edtFrom.Name = "edtFrom";
			this.edtFrom.Size = new System.Drawing.Size(308, 23);
			this.edtFrom.TabIndex = 1;
			// 
			// lblFrom
			// 
			this.lblFrom.AutoSize = true;
			this.lblFrom.Location = new System.Drawing.Point(8, 16);
			this.lblFrom.Name = "lblFrom";
			this.lblFrom.Size = new System.Drawing.Size(38, 15);
			this.lblFrom.TabIndex = 0;
			this.lblFrom.Text = "From:";
			// 
			// cbPriority
			// 
			this.cbPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPriority.Items.AddRange(new object[] {
            "Normal",
            "Low",
            "High"});
			this.cbPriority.Location = new System.Drawing.Point(60, 124);
			this.cbPriority.Name = "cbPriority";
			this.cbPriority.Size = new System.Drawing.Size(88, 23);
			this.cbPriority.TabIndex = 9;
			// 
			// lblPriority
			// 
			this.lblPriority.AutoSize = true;
			this.lblPriority.Location = new System.Drawing.Point(8, 128);
			this.lblPriority.Name = "lblPriority";
			this.lblPriority.Size = new System.Drawing.Size(48, 15);
			this.lblPriority.TabIndex = 8;
			this.lblPriority.Text = "Priority:";
			// 
			// edtSmtpServer
			// 
			this.edtSmtpServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtSmtpServer.Location = new System.Drawing.Point(92, 284);
			this.edtSmtpServer.Name = "edtSmtpServer";
			this.edtSmtpServer.Size = new System.Drawing.Size(276, 23);
			this.edtSmtpServer.TabIndex = 13;
			// 
			// lblSmtpServer
			// 
			this.lblSmtpServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblSmtpServer.AutoSize = true;
			this.lblSmtpServer.Location = new System.Drawing.Point(8, 288);
			this.lblSmtpServer.Name = "lblSmtpServer";
			this.lblSmtpServer.Size = new System.Drawing.Size(76, 15);
			this.lblSmtpServer.TabIndex = 12;
			this.lblSmtpServer.Text = "SMTP Server:";
			// 
			// EmailStepCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.Controls.Add(this.edtSmtpServer);
			this.Controls.Add(this.lblSmtpServer);
			this.Controls.Add(this.lblPriority);
			this.Controls.Add(this.cbPriority);
			this.Controls.Add(this.edtFrom);
			this.Controls.Add(this.lblFrom);
			this.Controls.Add(this.chkAppendOutput);
			this.Controls.Add(this.txtMessage);
			this.Controls.Add(this.edtSubject);
			this.Controls.Add(this.lblSubject);
			this.Controls.Add(this.edtCC);
			this.Controls.Add(this.lblCC);
			this.Controls.Add(this.edtTo);
			this.Controls.Add(this.lblTo);
			this.Name = "EmailStepCtrl";
			this.Size = new System.Drawing.Size(380, 314);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}


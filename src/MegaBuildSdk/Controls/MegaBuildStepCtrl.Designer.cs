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
	internal sealed partial class MegaBuildStepCtrl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.OpenFileDialog OpenDlg;
		private System.Windows.Forms.Button btnOpen;
		private System.Windows.Forms.TextBox edtProject;
		private System.Windows.Forms.Label lblProject;
		private System.Windows.Forms.CheckBox chkExit;
		private System.Windows.Forms.CheckBox chkMinimize;
		private CheckBox chkOutOfProc;

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
			this.OpenDlg = new System.Windows.Forms.OpenFileDialog();
			this.btnOpen = new System.Windows.Forms.Button();
			this.edtProject = new System.Windows.Forms.TextBox();
			this.lblProject = new System.Windows.Forms.Label();
			this.chkExit = new System.Windows.Forms.CheckBox();
			this.chkMinimize = new System.Windows.Forms.CheckBox();
			this.chkOutOfProc = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// OpenDlg
			// 
			this.OpenDlg.DefaultExt = "mgb";
			this.OpenDlg.Filter = "MegaBuild Files (*.mgb)|*.mgb|All Files (*.*)|*.*";
			this.OpenDlg.Title = "Open Project";
			// 
			// btnOpen
			// 
			this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpen.Location = new System.Drawing.Point(340, 28);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(28, 24);
			this.btnOpen.TabIndex = 2;
			this.btnOpen.Text = "...";
			this.btnOpen.Click += new System.EventHandler(this.Open_Click);
			// 
			// edtProject
			// 
			this.edtProject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtProject.Location = new System.Drawing.Point(12, 29);
			this.edtProject.Name = "edtProject";
			this.edtProject.Size = new System.Drawing.Size(316, 23);
			this.edtProject.TabIndex = 1;
			// 
			// lblProject
			// 
			this.lblProject.AutoSize = true;
			this.lblProject.Location = new System.Drawing.Point(12, 9);
			this.lblProject.Name = "lblProject";
			this.lblProject.Size = new System.Drawing.Size(47, 15);
			this.lblProject.TabIndex = 0;
			this.lblProject.Text = "Project:";
			// 
			// chkExit
			// 
			this.chkExit.AutoSize = true;
			this.chkExit.Location = new System.Drawing.Point(32, 108);
			this.chkExit.Name = "chkExit";
			this.chkExit.Size = new System.Drawing.Size(270, 19);
			this.chkExit.TabIndex = 5;
			this.chkExit.Text = "Exit the new MegaBuild instance after building";
			// 
			// chkMinimize
			// 
			this.chkMinimize.AutoSize = true;
			this.chkMinimize.Location = new System.Drawing.Point(32, 84);
			this.chkMinimize.Name = "chkMinimize";
			this.chkMinimize.Size = new System.Drawing.Size(276, 19);
			this.chkMinimize.TabIndex = 4;
			this.chkMinimize.Text = "Launch the new MegaBuild instance minimized";
			// 
			// chkOutOfProc
			// 
			this.chkOutOfProc.AutoSize = true;
			this.chkOutOfProc.Location = new System.Drawing.Point(12, 60);
			this.chkOutOfProc.Name = "chkOutOfProc";
			this.chkOutOfProc.Size = new System.Drawing.Size(238, 19);
			this.chkOutOfProc.TabIndex = 3;
			this.chkOutOfProc.Text = "Execute in a separate MegaBuild process";
			this.chkOutOfProc.UseVisualStyleBackColor = true;
			this.chkOutOfProc.CheckedChanged += new System.EventHandler(this.OutOfProc_CheckedChanged);
			// 
			// MegaBuildStepCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.Controls.Add(this.chkOutOfProc);
			this.Controls.Add(this.chkMinimize);
			this.Controls.Add(this.chkExit);
			this.Controls.Add(this.btnOpen);
			this.Controls.Add(this.edtProject);
			this.Controls.Add(this.lblProject);
			this.Name = "MegaBuildStepCtrl";
			this.Size = new System.Drawing.Size(380, 314);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}


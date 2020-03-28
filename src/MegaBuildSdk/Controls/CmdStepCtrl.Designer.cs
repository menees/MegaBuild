using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Menees;
using System.Diagnostics;
using Menees.Windows.Forms;
namespace MegaBuild
{
	internal sealed partial class CmdStepCtrl
	{
		private System.Windows.Forms.Button btnBrowseCmd;
		private System.Windows.Forms.TextBox edtCommand;
		private System.Windows.Forms.Label lblCommand;
		private System.Windows.Forms.Button btnBrowseArguments;
		private System.Windows.Forms.TextBox edtArguments;
		private System.Windows.Forms.Label lblArguments;
		private System.Windows.Forms.Button btnBrowseDirectory;
		private System.Windows.Forms.TextBox edtWorkingDirectory;
		private System.Windows.Forms.Label lblWorkingDirectory;
		private System.Windows.Forms.Label lblWindowState;
		private System.Windows.Forms.ComboBox cbWindowState;
		private System.Windows.Forms.Label lblSuccessCodes;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.CheckBox chkShellExecute;
		private System.Windows.Forms.Label lblVerb;
		private System.Windows.Forms.TextBox edtVerb;
		private System.Windows.Forms.NumericUpDown numFirstSuccess;
		private System.Windows.Forms.NumericUpDown numLastSuccess;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.OpenFileDialog OpenDlg;
		private Label lblRedirectConsole;
		private CheckBox chkRedirectInput;
		private CheckBox chkRedirectOutput;
		private CheckBox chkRedirectError;

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
			this.btnBrowseCmd = new System.Windows.Forms.Button();
			this.edtCommand = new System.Windows.Forms.TextBox();
			this.lblCommand = new System.Windows.Forms.Label();
			this.btnBrowseArguments = new System.Windows.Forms.Button();
			this.edtArguments = new System.Windows.Forms.TextBox();
			this.lblArguments = new System.Windows.Forms.Label();
			this.btnBrowseDirectory = new System.Windows.Forms.Button();
			this.edtWorkingDirectory = new System.Windows.Forms.TextBox();
			this.lblWorkingDirectory = new System.Windows.Forms.Label();
			this.lblWindowState = new System.Windows.Forms.Label();
			this.cbWindowState = new System.Windows.Forms.ComboBox();
			this.lblSuccessCodes = new System.Windows.Forms.Label();
			this.numFirstSuccess = new System.Windows.Forms.NumericUpDown();
			this.lblTo = new System.Windows.Forms.Label();
			this.numLastSuccess = new System.Windows.Forms.NumericUpDown();
			this.chkShellExecute = new System.Windows.Forms.CheckBox();
			this.lblVerb = new System.Windows.Forms.Label();
			this.edtVerb = new System.Windows.Forms.TextBox();
			this.OpenDlg = new System.Windows.Forms.OpenFileDialog();
			this.lblRedirectConsole = new System.Windows.Forms.Label();
			this.chkRedirectInput = new System.Windows.Forms.CheckBox();
			this.chkRedirectOutput = new System.Windows.Forms.CheckBox();
			this.chkRedirectError = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.numFirstSuccess)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numLastSuccess)).BeginInit();
			this.SuspendLayout();
			// 
			// btnBrowseCmd
			// 
			this.btnBrowseCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseCmd.Location = new System.Drawing.Point(340, 27);
			this.btnBrowseCmd.Name = "btnBrowseCmd";
			this.btnBrowseCmd.Size = new System.Drawing.Size(28, 24);
			this.btnBrowseCmd.TabIndex = 2;
			this.btnBrowseCmd.Text = "...";
			this.btnBrowseCmd.Click += new System.EventHandler(this.BrowseCmd_Click);
			// 
			// edtCommand
			// 
			this.edtCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtCommand.Location = new System.Drawing.Point(12, 28);
			this.edtCommand.Name = "edtCommand";
			this.edtCommand.Size = new System.Drawing.Size(316, 23);
			this.edtCommand.TabIndex = 1;
			// 
			// lblCommand
			// 
			this.lblCommand.AutoSize = true;
			this.lblCommand.Location = new System.Drawing.Point(12, 12);
			this.lblCommand.Name = "lblCommand";
			this.lblCommand.Size = new System.Drawing.Size(67, 15);
			this.lblCommand.TabIndex = 0;
			this.lblCommand.Text = "Command:";
			// 
			// btnBrowseArguments
			// 
			this.btnBrowseArguments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseArguments.Location = new System.Drawing.Point(340, 71);
			this.btnBrowseArguments.Name = "btnBrowseArguments";
			this.btnBrowseArguments.Size = new System.Drawing.Size(28, 24);
			this.btnBrowseArguments.TabIndex = 5;
			this.btnBrowseArguments.Text = "...";
			this.btnBrowseArguments.Click += new System.EventHandler(this.BrowseArguments_Click);
			// 
			// edtArguments
			// 
			this.edtArguments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtArguments.Location = new System.Drawing.Point(12, 72);
			this.edtArguments.Name = "edtArguments";
			this.edtArguments.Size = new System.Drawing.Size(316, 23);
			this.edtArguments.TabIndex = 4;
			// 
			// lblArguments
			// 
			this.lblArguments.AutoSize = true;
			this.lblArguments.Location = new System.Drawing.Point(12, 56);
			this.lblArguments.Name = "lblArguments";
			this.lblArguments.Size = new System.Drawing.Size(69, 15);
			this.lblArguments.TabIndex = 3;
			this.lblArguments.Text = "Arguments:";
			// 
			// btnBrowseDirectory
			// 
			this.btnBrowseDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseDirectory.Location = new System.Drawing.Point(340, 115);
			this.btnBrowseDirectory.Name = "btnBrowseDirectory";
			this.btnBrowseDirectory.Size = new System.Drawing.Size(28, 24);
			this.btnBrowseDirectory.TabIndex = 8;
			this.btnBrowseDirectory.Text = "...";
			this.btnBrowseDirectory.Click += new System.EventHandler(this.BrowseDirectory_Click);
			// 
			// edtWorkingDirectory
			// 
			this.edtWorkingDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtWorkingDirectory.Location = new System.Drawing.Point(12, 116);
			this.edtWorkingDirectory.Name = "edtWorkingDirectory";
			this.edtWorkingDirectory.Size = new System.Drawing.Size(316, 23);
			this.edtWorkingDirectory.TabIndex = 7;
			// 
			// lblWorkingDirectory
			// 
			this.lblWorkingDirectory.AutoSize = true;
			this.lblWorkingDirectory.Location = new System.Drawing.Point(12, 100);
			this.lblWorkingDirectory.Name = "lblWorkingDirectory";
			this.lblWorkingDirectory.Size = new System.Drawing.Size(106, 15);
			this.lblWorkingDirectory.TabIndex = 6;
			this.lblWorkingDirectory.Text = "Working Directory:";
			// 
			// lblWindowState
			// 
			this.lblWindowState.AutoSize = true;
			this.lblWindowState.Location = new System.Drawing.Point(12, 156);
			this.lblWindowState.Name = "lblWindowState";
			this.lblWindowState.Size = new System.Drawing.Size(83, 15);
			this.lblWindowState.TabIndex = 9;
			this.lblWindowState.Text = "Window State:";
			// 
			// cbWindowState
			// 
			this.cbWindowState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbWindowState.Items.AddRange(new object[] {
            "Normal",
            "Hidden",
            "Minimized",
            "Maximized"});
			this.cbWindowState.Location = new System.Drawing.Point(140, 152);
			this.cbWindowState.Name = "cbWindowState";
			this.cbWindowState.Size = new System.Drawing.Size(121, 23);
			this.cbWindowState.TabIndex = 10;
			// 
			// lblSuccessCodes
			// 
			this.lblSuccessCodes.AutoSize = true;
			this.lblSuccessCodes.Location = new System.Drawing.Point(12, 192);
			this.lblSuccessCodes.Name = "lblSuccessCodes";
			this.lblSuccessCodes.Size = new System.Drawing.Size(108, 15);
			this.lblSuccessCodes.TabIndex = 11;
			this.lblSuccessCodes.Text = "Success Exit Codes:";
			// 
			// numFirstSuccess
			// 
			this.numFirstSuccess.Location = new System.Drawing.Point(140, 188);
			this.numFirstSuccess.Name = "numFirstSuccess";
			this.numFirstSuccess.Size = new System.Drawing.Size(48, 23);
			this.numFirstSuccess.TabIndex = 12;
			this.numFirstSuccess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblTo
			// 
			this.lblTo.AutoSize = true;
			this.lblTo.Location = new System.Drawing.Point(192, 192);
			this.lblTo.Name = "lblTo";
			this.lblTo.Size = new System.Drawing.Size(18, 15);
			this.lblTo.TabIndex = 13;
			this.lblTo.Text = "to";
			// 
			// numLastSuccess
			// 
			this.numLastSuccess.Location = new System.Drawing.Point(212, 188);
			this.numLastSuccess.Name = "numLastSuccess";
			this.numLastSuccess.Size = new System.Drawing.Size(48, 23);
			this.numLastSuccess.TabIndex = 14;
			this.numLastSuccess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// chkShellExecute
			// 
			this.chkShellExecute.AutoSize = true;
			this.chkShellExecute.Location = new System.Drawing.Point(12, 256);
			this.chkShellExecute.Name = "chkShellExecute";
			this.chkShellExecute.Size = new System.Drawing.Size(185, 19);
			this.chkShellExecute.TabIndex = 19;
			this.chkShellExecute.Text = "Use Windows Shell To Execute";
			this.chkShellExecute.CheckedChanged += new System.EventHandler(this.ShellExecute_CheckedChanged);
			// 
			// lblVerb
			// 
			this.lblVerb.AutoSize = true;
			this.lblVerb.Location = new System.Drawing.Point(40, 284);
			this.lblVerb.Name = "lblVerb";
			this.lblVerb.Size = new System.Drawing.Size(62, 15);
			this.lblVerb.TabIndex = 20;
			this.lblVerb.Text = "Shell Verb:";
			// 
			// edtVerb
			// 
			this.edtVerb.Location = new System.Drawing.Point(140, 280);
			this.edtVerb.Name = "edtVerb";
			this.edtVerb.Size = new System.Drawing.Size(122, 23);
			this.edtVerb.TabIndex = 21;
			// 
			// OpenDlg
			// 
			this.OpenDlg.Filter = "All Files (*.*)|*.*";
			// 
			// lblRedirectConsole
			// 
			this.lblRedirectConsole.AutoSize = true;
			this.lblRedirectConsole.Location = new System.Drawing.Point(12, 224);
			this.lblRedirectConsole.Name = "lblRedirectConsole";
			this.lblRedirectConsole.Size = new System.Drawing.Size(99, 15);
			this.lblRedirectConsole.TabIndex = 15;
			this.lblRedirectConsole.Text = "Redirect Console:";
			// 
			// chkRedirectInput
			// 
			this.chkRedirectInput.AutoSize = true;
			this.chkRedirectInput.Location = new System.Drawing.Point(144, 224);
			this.chkRedirectInput.Name = "chkRedirectInput";
			this.chkRedirectInput.Size = new System.Drawing.Size(54, 19);
			this.chkRedirectInput.TabIndex = 16;
			this.chkRedirectInput.Text = "Input";
			this.chkRedirectInput.UseVisualStyleBackColor = true;
			this.chkRedirectInput.CheckedChanged += new System.EventHandler(this.RedirectStreams_CheckedChanged);
			// 
			// chkRedirectOutput
			// 
			this.chkRedirectOutput.AutoSize = true;
			this.chkRedirectOutput.Location = new System.Drawing.Point(220, 224);
			this.chkRedirectOutput.Name = "chkRedirectOutput";
			this.chkRedirectOutput.Size = new System.Drawing.Size(64, 19);
			this.chkRedirectOutput.TabIndex = 17;
			this.chkRedirectOutput.Text = "Output";
			this.chkRedirectOutput.UseVisualStyleBackColor = true;
			this.chkRedirectOutput.CheckedChanged += new System.EventHandler(this.RedirectStreams_CheckedChanged);
			// 
			// chkRedirectError
			// 
			this.chkRedirectError.AutoSize = true;
			this.chkRedirectError.Location = new System.Drawing.Point(304, 224);
			this.chkRedirectError.Name = "chkRedirectError";
			this.chkRedirectError.Size = new System.Drawing.Size(51, 19);
			this.chkRedirectError.TabIndex = 18;
			this.chkRedirectError.Text = "Error";
			this.chkRedirectError.UseVisualStyleBackColor = true;
			this.chkRedirectError.CheckedChanged += new System.EventHandler(this.RedirectStreams_CheckedChanged);
			// 
			// CmdStepCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.Controls.Add(this.chkRedirectError);
			this.Controls.Add(this.chkRedirectOutput);
			this.Controls.Add(this.chkRedirectInput);
			this.Controls.Add(this.lblRedirectConsole);
			this.Controls.Add(this.edtVerb);
			this.Controls.Add(this.lblVerb);
			this.Controls.Add(this.chkShellExecute);
			this.Controls.Add(this.numLastSuccess);
			this.Controls.Add(this.lblTo);
			this.Controls.Add(this.numFirstSuccess);
			this.Controls.Add(this.lblSuccessCodes);
			this.Controls.Add(this.cbWindowState);
			this.Controls.Add(this.lblWindowState);
			this.Controls.Add(this.btnBrowseDirectory);
			this.Controls.Add(this.edtWorkingDirectory);
			this.Controls.Add(this.lblWorkingDirectory);
			this.Controls.Add(this.btnBrowseArguments);
			this.Controls.Add(this.edtArguments);
			this.Controls.Add(this.lblArguments);
			this.Controls.Add(this.btnBrowseCmd);
			this.Controls.Add(this.edtCommand);
			this.Controls.Add(this.lblCommand);
			this.Name = "CmdStepCtrl";
			this.Size = new System.Drawing.Size(380, 314);
			((System.ComponentModel.ISupportInitialize)(this.numFirstSuccess)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numLastSuccess)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}


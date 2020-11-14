namespace MegaBuild
{
	partial class PowerShellStepCtrl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnBrowseDirectory = new System.Windows.Forms.Button();
			this.edtWorkingDirectory = new System.Windows.Forms.TextBox();
			this.lblWorkingDirectory = new System.Windows.Forms.Label();
			this.btnBrowseCmd = new System.Windows.Forms.Button();
			this.edtCommand = new System.Windows.Forms.TextBox();
			this.lblCommand = new System.Windows.Forms.Label();
			this.OpenDlg = new System.Windows.Forms.OpenFileDialog();
			this.lblShell = new System.Windows.Forms.Label();
			this.shell = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// btnBrowseDirectory
			// 
			this.btnBrowseDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseDirectory.Location = new System.Drawing.Point(405, 74);
			this.btnBrowseDirectory.Margin = new System.Windows.Forms.Padding(2);
			this.btnBrowseDirectory.Name = "btnBrowseDirectory";
			this.btnBrowseDirectory.Size = new System.Drawing.Size(28, 24);
			this.btnBrowseDirectory.TabIndex = 5;
			this.btnBrowseDirectory.Text = "...";
			this.btnBrowseDirectory.Click += new System.EventHandler(this.BrowseDirectory_Click);
			// 
			// edtWorkingDirectory
			// 
			this.edtWorkingDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtWorkingDirectory.Location = new System.Drawing.Point(10, 75);
			this.edtWorkingDirectory.Margin = new System.Windows.Forms.Padding(2);
			this.edtWorkingDirectory.Name = "edtWorkingDirectory";
			this.edtWorkingDirectory.Size = new System.Drawing.Size(384, 23);
			this.edtWorkingDirectory.TabIndex = 4;
			// 
			// lblWorkingDirectory
			// 
			this.lblWorkingDirectory.AutoSize = true;
			this.lblWorkingDirectory.Location = new System.Drawing.Point(10, 58);
			this.lblWorkingDirectory.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblWorkingDirectory.Name = "lblWorkingDirectory";
			this.lblWorkingDirectory.Size = new System.Drawing.Size(106, 15);
			this.lblWorkingDirectory.TabIndex = 3;
			this.lblWorkingDirectory.Text = "Working Directory:";
			// 
			// btnBrowseCmd
			// 
			this.btnBrowseCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseCmd.Location = new System.Drawing.Point(405, 28);
			this.btnBrowseCmd.Margin = new System.Windows.Forms.Padding(2);
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
			this.edtCommand.Location = new System.Drawing.Point(10, 29);
			this.edtCommand.Margin = new System.Windows.Forms.Padding(2);
			this.edtCommand.Name = "edtCommand";
			this.edtCommand.Size = new System.Drawing.Size(384, 23);
			this.edtCommand.TabIndex = 1;
			// 
			// lblCommand
			// 
			this.lblCommand.AutoSize = true;
			this.lblCommand.Location = new System.Drawing.Point(10, 12);
			this.lblCommand.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblCommand.Name = "lblCommand";
			this.lblCommand.Size = new System.Drawing.Size(114, 15);
			this.lblCommand.TabIndex = 0;
			this.lblCommand.Text = "Script or Command:";
			// 
			// OpenDlg
			// 
			this.OpenDlg.Filter = "PowerShell Scripts (*.ps1)|*.ps1|All Files (*.*)|*.*";
			this.OpenDlg.Title = "Select PowerShell Script";
			// 
			// lblShell
			// 
			this.lblShell.AutoSize = true;
			this.lblShell.Location = new System.Drawing.Point(10, 104);
			this.lblShell.Name = "lblShell";
			this.lblShell.Size = new System.Drawing.Size(35, 15);
			this.lblShell.TabIndex = 6;
			this.lblShell.Text = "Shell:";
			// 
			// shell
			// 
			this.shell.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.shell.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.shell.FormattingEnabled = true;
			this.shell.Items.AddRange(new object[] {
            "Windows PowerShell",
            "PowerShell Core",
            "PowerShell Core or Windows PowerShell"});
			this.shell.Location = new System.Drawing.Point(10, 120);
			this.shell.Name = "shell";
			this.shell.Size = new System.Drawing.Size(384, 23);
			this.shell.TabIndex = 7;
			// 
			// PowerShellStepCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.shell);
			this.Controls.Add(this.lblShell);
			this.Controls.Add(this.btnBrowseDirectory);
			this.Controls.Add(this.edtWorkingDirectory);
			this.Controls.Add(this.lblWorkingDirectory);
			this.Controls.Add(this.btnBrowseCmd);
			this.Controls.Add(this.edtCommand);
			this.Controls.Add(this.lblCommand);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "PowerShellStepCtrl";
			this.Size = new System.Drawing.Size(443, 362);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnBrowseDirectory;
		private System.Windows.Forms.TextBox edtWorkingDirectory;
		private System.Windows.Forms.Label lblWorkingDirectory;
		private System.Windows.Forms.Button btnBrowseCmd;
		private System.Windows.Forms.TextBox edtCommand;
		private System.Windows.Forms.Label lblCommand;
		private System.Windows.Forms.OpenFileDialog OpenDlg;
		private System.Windows.Forms.Label lblShell;
		private System.Windows.Forms.ComboBox shell;
	}
}

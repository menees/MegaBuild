namespace MegaBuild
{
    partial class MSBuildStepCtrl
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
			this.btnSelectProject = new System.Windows.Forms.Button();
			this.edtProject = new System.Windows.Forms.TextBox();
			this.lblProject = new System.Windows.Forms.Label();
			this.OpenDlg = new System.Windows.Forms.OpenFileDialog();
			this.btnSelectWorkingDirectory = new System.Windows.Forms.Button();
			this.edtWorkingDirectory = new System.Windows.Forms.TextBox();
			this.lblWorkingDirectory = new System.Windows.Forms.Label();
			this.lblOtherOptions = new System.Windows.Forms.Label();
			this.edtOtherOptions = new System.Windows.Forms.TextBox();
			this.lblToolsVersion = new System.Windows.Forms.Label();
			this.lblVerbosity = new System.Windows.Forms.Label();
			this.cbVerbosity = new System.Windows.Forms.ComboBox();
			this.cbToolsVersion = new System.Windows.Forms.ComboBox();
			this.lblTargets = new System.Windows.Forms.Label();
			this.lblProperties = new System.Windows.Forms.Label();
			this.txtTargets = new System.Windows.Forms.TextBox();
			this.txtProperties = new System.Windows.Forms.TextBox();
			this.btnShowTargets = new System.Windows.Forms.Button();
			this.btnShowProperties = new System.Windows.Forms.Button();
			this.chk32Bit = new System.Windows.Forms.CheckBox();
			this.tblLayout = new System.Windows.Forms.TableLayoutPanel();
			this.pnlTop = new System.Windows.Forms.Panel();
			this.pnlMidLeft = new System.Windows.Forms.Panel();
			this.pnlMidRight = new System.Windows.Forms.Panel();
			this.pnlBottom = new System.Windows.Forms.Panel();
			this.tblLayout.SuspendLayout();
			this.pnlTop.SuspendLayout();
			this.pnlMidLeft.SuspendLayout();
			this.pnlMidRight.SuspendLayout();
			this.pnlBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnSelectProject
			// 
			this.btnSelectProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelectProject.Location = new System.Drawing.Point(504, 24);
			this.btnSelectProject.Name = "btnSelectProject";
			this.btnSelectProject.Size = new System.Drawing.Size(28, 24);
			this.btnSelectProject.TabIndex = 2;
			this.btnSelectProject.Text = "...";
			this.btnSelectProject.Click += new System.EventHandler(this.SelectProject_Click);
			// 
			// edtProject
			// 
			this.edtProject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtProject.Location = new System.Drawing.Point(4, 25);
			this.edtProject.Name = "edtProject";
			this.edtProject.Size = new System.Drawing.Size(492, 23);
			this.edtProject.TabIndex = 1;
			// 
			// lblProject
			// 
			this.lblProject.AutoSize = true;
			this.lblProject.Location = new System.Drawing.Point(4, 6);
			this.lblProject.Name = "lblProject";
			this.lblProject.Size = new System.Drawing.Size(47, 15);
			this.lblProject.TabIndex = 0;
			this.lblProject.Text = "Project:";
			// 
			// OpenDlg
			// 
			this.OpenDlg.DefaultExt = "proj";
			this.OpenDlg.Filter = "Projects and Solutions (*.*proj;*.sln)|*.*proj;*.sln|Project Files (*.*proj)|*.*p" +
    "roj|Solution Files (*.sln)|*.sln|All Files (*.*)|*.*";
			this.OpenDlg.Title = "Select Project";
			// 
			// btnSelectWorkingDirectory
			// 
			this.btnSelectWorkingDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelectWorkingDirectory.Location = new System.Drawing.Point(504, 74);
			this.btnSelectWorkingDirectory.Name = "btnSelectWorkingDirectory";
			this.btnSelectWorkingDirectory.Size = new System.Drawing.Size(28, 24);
			this.btnSelectWorkingDirectory.TabIndex = 5;
			this.btnSelectWorkingDirectory.Text = "...";
			this.btnSelectWorkingDirectory.Click += new System.EventHandler(this.SelectWorkingDirectory_Click);
			// 
			// edtWorkingDirectory
			// 
			this.edtWorkingDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtWorkingDirectory.Location = new System.Drawing.Point(4, 75);
			this.edtWorkingDirectory.Name = "edtWorkingDirectory";
			this.edtWorkingDirectory.Size = new System.Drawing.Size(492, 23);
			this.edtWorkingDirectory.TabIndex = 4;
			// 
			// lblWorkingDirectory
			// 
			this.lblWorkingDirectory.AutoSize = true;
			this.lblWorkingDirectory.Location = new System.Drawing.Point(4, 57);
			this.lblWorkingDirectory.Name = "lblWorkingDirectory";
			this.lblWorkingDirectory.Size = new System.Drawing.Size(106, 15);
			this.lblWorkingDirectory.TabIndex = 3;
			this.lblWorkingDirectory.Text = "Working Directory:";
			// 
			// lblOtherOptions
			// 
			this.lblOtherOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblOtherOptions.AutoSize = true;
			this.lblOtherOptions.Location = new System.Drawing.Point(4, 38);
			this.lblOtherOptions.Name = "lblOtherOptions";
			this.lblOtherOptions.Size = new System.Drawing.Size(170, 15);
			this.lblOtherOptions.TabIndex = 17;
			this.lblOtherOptions.Text = "Other Command Line Options:";
			// 
			// edtOtherOptions
			// 
			this.edtOtherOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtOtherOptions.Location = new System.Drawing.Point(4, 58);
			this.edtOtherOptions.Name = "edtOtherOptions";
			this.edtOtherOptions.Size = new System.Drawing.Size(528, 23);
			this.edtOtherOptions.TabIndex = 18;
			// 
			// lblToolsVersion
			// 
			this.lblToolsVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblToolsVersion.AutoSize = true;
			this.lblToolsVersion.Location = new System.Drawing.Point(4, 366);
			this.lblToolsVersion.Name = "lblToolsVersion";
			this.lblToolsVersion.Size = new System.Drawing.Size(79, 15);
			this.lblToolsVersion.TabIndex = 12;
			this.lblToolsVersion.Text = "Tools Version:";
			// 
			// lblVerbosity
			// 
			this.lblVerbosity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblVerbosity.AutoSize = true;
			this.lblVerbosity.Location = new System.Drawing.Point(4, 366);
			this.lblVerbosity.Name = "lblVerbosity";
			this.lblVerbosity.Size = new System.Drawing.Size(99, 15);
			this.lblVerbosity.TabIndex = 14;
			this.lblVerbosity.Text = "Output Verbosity:";
			// 
			// cbVerbosity
			// 
			this.cbVerbosity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbVerbosity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVerbosity.FormattingEnabled = true;
			this.cbVerbosity.Items.AddRange(new object[] {
            "Quiet",
            "Minimal",
            "Normal",
            "Detailed",
            "Diagnostic"});
			this.cbVerbosity.Location = new System.Drawing.Point(112, 362);
			this.cbVerbosity.Name = "cbVerbosity";
			this.cbVerbosity.Size = new System.Drawing.Size(148, 23);
			this.cbVerbosity.TabIndex = 15;
			// 
			// cbToolsVersion
			// 
			this.cbToolsVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbToolsVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbToolsVersion.FormattingEnabled = true;
			this.cbToolsVersion.Location = new System.Drawing.Point(112, 362);
			this.cbToolsVersion.Name = "cbToolsVersion";
			this.cbToolsVersion.Size = new System.Drawing.Size(148, 23);
			this.cbToolsVersion.TabIndex = 13;
			// 
			// lblTargets
			// 
			this.lblTargets.AutoSize = true;
			this.lblTargets.Location = new System.Drawing.Point(4, 6);
			this.lblTargets.Name = "lblTargets";
			this.lblTargets.Size = new System.Drawing.Size(91, 30);
			this.lblTargets.TabIndex = 6;
			this.lblTargets.Text = "Targets To Build\r\n(One Per Line):";
			// 
			// lblProperties
			// 
			this.lblProperties.AutoSize = true;
			this.lblProperties.Location = new System.Drawing.Point(4, 6);
			this.lblProperties.Name = "lblProperties";
			this.lblProperties.Size = new System.Drawing.Size(156, 30);
			this.lblProperties.TabIndex = 9;
			this.lblProperties.Text = "Property Overrides\r\n(One Name=Value Per Line):";
			// 
			// txtTargets
			// 
			this.txtTargets.AcceptsReturn = true;
			this.txtTargets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTargets.Location = new System.Drawing.Point(4, 43);
			this.txtTargets.Multiline = true;
			this.txtTargets.Name = "txtTargets";
			this.txtTargets.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtTargets.Size = new System.Drawing.Size(256, 306);
			this.txtTargets.TabIndex = 8;
			// 
			// txtProperties
			// 
			this.txtProperties.AcceptsReturn = true;
			this.txtProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtProperties.Location = new System.Drawing.Point(4, 43);
			this.txtProperties.Multiline = true;
			this.txtProperties.Name = "txtProperties";
			this.txtProperties.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtProperties.Size = new System.Drawing.Size(256, 306);
			this.txtProperties.TabIndex = 11;
			// 
			// btnShowTargets
			// 
			this.btnShowTargets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnShowTargets.Location = new System.Drawing.Point(232, 12);
			this.btnShowTargets.Name = "btnShowTargets";
			this.btnShowTargets.Size = new System.Drawing.Size(28, 24);
			this.btnShowTargets.TabIndex = 7;
			this.btnShowTargets.Text = "...";
			this.btnShowTargets.Click += new System.EventHandler(this.ShowTargets_Click);
			// 
			// btnShowProperties
			// 
			this.btnShowProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnShowProperties.Location = new System.Drawing.Point(232, 12);
			this.btnShowProperties.Name = "btnShowProperties";
			this.btnShowProperties.Size = new System.Drawing.Size(28, 24);
			this.btnShowProperties.TabIndex = 10;
			this.btnShowProperties.Text = "...";
			this.btnShowProperties.Click += new System.EventHandler(this.ShowProperties_Click);
			// 
			// chk32Bit
			// 
			this.chk32Bit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chk32Bit.AutoSize = true;
			this.chk32Bit.Location = new System.Drawing.Point(4, 10);
			this.chk32Bit.Name = "chk32Bit";
			this.chk32Bit.Size = new System.Drawing.Size(258, 19);
			this.chk32Bit.TabIndex = 16;
			this.chk32Bit.Text = "Force use of 32-bit tools on 64-bit machines";
			this.chk32Bit.UseVisualStyleBackColor = true;
			// 
			// tblLayout
			// 
			this.tblLayout.ColumnCount = 2;
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tblLayout.Controls.Add(this.pnlTop, 0, 0);
			this.tblLayout.Controls.Add(this.pnlMidLeft, 0, 1);
			this.tblLayout.Controls.Add(this.pnlMidRight, 1, 1);
			this.tblLayout.Controls.Add(this.pnlBottom, 0, 2);
			this.tblLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tblLayout.Location = new System.Drawing.Point(0, 0);
			this.tblLayout.Name = "tblLayout";
			this.tblLayout.RowCount = 3;
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayout.Size = new System.Drawing.Size(545, 600);
			this.tblLayout.TabIndex = 19;
			// 
			// pnlTop
			// 
			this.tblLayout.SetColumnSpan(this.pnlTop, 2);
			this.pnlTop.Controls.Add(this.edtProject);
			this.pnlTop.Controls.Add(this.lblProject);
			this.pnlTop.Controls.Add(this.btnSelectProject);
			this.pnlTop.Controls.Add(this.lblWorkingDirectory);
			this.pnlTop.Controls.Add(this.edtWorkingDirectory);
			this.pnlTop.Controls.Add(this.btnSelectWorkingDirectory);
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlTop.Location = new System.Drawing.Point(3, 3);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(539, 100);
			this.pnlTop.TabIndex = 0;
			// 
			// pnlMidLeft
			// 
			this.pnlMidLeft.Controls.Add(this.txtTargets);
			this.pnlMidLeft.Controls.Add(this.lblToolsVersion);
			this.pnlMidLeft.Controls.Add(this.cbToolsVersion);
			this.pnlMidLeft.Controls.Add(this.btnShowTargets);
			this.pnlMidLeft.Controls.Add(this.lblTargets);
			this.pnlMidLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMidLeft.Location = new System.Drawing.Point(3, 109);
			this.pnlMidLeft.Name = "pnlMidLeft";
			this.pnlMidLeft.Size = new System.Drawing.Size(266, 392);
			this.pnlMidLeft.TabIndex = 1;
			// 
			// pnlMidRight
			// 
			this.pnlMidRight.Controls.Add(this.lblProperties);
			this.pnlMidRight.Controls.Add(this.lblVerbosity);
			this.pnlMidRight.Controls.Add(this.btnShowProperties);
			this.pnlMidRight.Controls.Add(this.cbVerbosity);
			this.pnlMidRight.Controls.Add(this.txtProperties);
			this.pnlMidRight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMidRight.Location = new System.Drawing.Point(275, 109);
			this.pnlMidRight.Name = "pnlMidRight";
			this.pnlMidRight.Size = new System.Drawing.Size(267, 392);
			this.pnlMidRight.TabIndex = 2;
			// 
			// pnlBottom
			// 
			this.tblLayout.SetColumnSpan(this.pnlBottom, 2);
			this.pnlBottom.Controls.Add(this.chk32Bit);
			this.pnlBottom.Controls.Add(this.lblOtherOptions);
			this.pnlBottom.Controls.Add(this.edtOtherOptions);
			this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlBottom.Location = new System.Drawing.Point(3, 507);
			this.pnlBottom.Name = "pnlBottom";
			this.pnlBottom.Size = new System.Drawing.Size(539, 90);
			this.pnlBottom.TabIndex = 3;
			// 
			// MSBuildStepCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tblLayout);
			this.Name = "MSBuildStepCtrl";
			this.Size = new System.Drawing.Size(545, 600);
			this.tblLayout.ResumeLayout(false);
			this.pnlTop.ResumeLayout(false);
			this.pnlTop.PerformLayout();
			this.pnlMidLeft.ResumeLayout(false);
			this.pnlMidLeft.PerformLayout();
			this.pnlMidRight.ResumeLayout(false);
			this.pnlMidRight.PerformLayout();
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSelectProject;
        private System.Windows.Forms.TextBox edtProject;
        private System.Windows.Forms.Label lblProject;
        private System.Windows.Forms.OpenFileDialog OpenDlg;
        private System.Windows.Forms.Button btnSelectWorkingDirectory;
        private System.Windows.Forms.TextBox edtWorkingDirectory;
        private System.Windows.Forms.Label lblWorkingDirectory;
        private System.Windows.Forms.Label lblOtherOptions;
        private System.Windows.Forms.TextBox edtOtherOptions;
        private System.Windows.Forms.Label lblToolsVersion;
        private System.Windows.Forms.Label lblVerbosity;
        private System.Windows.Forms.ComboBox cbVerbosity;
        private System.Windows.Forms.ComboBox cbToolsVersion;
        private System.Windows.Forms.Label lblTargets;
        private System.Windows.Forms.Label lblProperties;
        private System.Windows.Forms.TextBox txtTargets;
        private System.Windows.Forms.TextBox txtProperties;
        private System.Windows.Forms.Button btnShowTargets;
		private System.Windows.Forms.Button btnShowProperties;
        private System.Windows.Forms.CheckBox chk32Bit;
		private System.Windows.Forms.TableLayoutPanel tblLayout;
		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.Panel pnlMidLeft;
		private System.Windows.Forms.Panel pnlMidRight;
		private System.Windows.Forms.Panel pnlBottom;
    }
}

namespace MegaBuild
{
	partial class CsiStepCtrl
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
			this.btnBrowseScript = new System.Windows.Forms.Button();
			this.edtScript = new System.Windows.Forms.TextBox();
			this.lblScript = new System.Windows.Forms.Label();
			this.OpenCsxDlg = new System.Windows.Forms.OpenFileDialog();
			this.treatErrorAsOutput = new System.Windows.Forms.CheckBox();
			this.lblToolsVersion = new System.Windows.Forms.Label();
			this.cbToolsVersion = new System.Windows.Forms.ComboBox();
			this.btnAddScriptArg = new System.Windows.Forms.Button();
			this.edtScriptArgs = new Menees.Windows.Forms.ExtendedRichTextBox();
			this.lblScriptArgs = new System.Windows.Forms.Label();
			this.btnReference = new System.Windows.Forms.Button();
			this.edtCsiOptions = new Menees.Windows.Forms.ExtendedRichTextBox();
			this.lblCsiOptions = new System.Windows.Forms.Label();
			this.OpenAllDlg = new System.Windows.Forms.OpenFileDialog();
			this.OpenAsmDlg = new System.Windows.Forms.OpenFileDialog();
			this.btnLibPath = new System.Windows.Forms.Button();
			this.btnUsing = new System.Windows.Forms.Button();
			this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this.pnlScriptArgs = new System.Windows.Forms.Panel();
			this.pnlCsiOptions = new System.Windows.Forms.Panel();
			this.tableLayout.SuspendLayout();
			this.pnlScriptArgs.SuspendLayout();
			this.pnlCsiOptions.SuspendLayout();
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
			// btnBrowseScript
			// 
			this.btnBrowseScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseScript.Location = new System.Drawing.Point(405, 28);
			this.btnBrowseScript.Margin = new System.Windows.Forms.Padding(2);
			this.btnBrowseScript.Name = "btnBrowseScript";
			this.btnBrowseScript.Size = new System.Drawing.Size(28, 24);
			this.btnBrowseScript.TabIndex = 2;
			this.btnBrowseScript.Text = "...";
			this.btnBrowseScript.Click += new System.EventHandler(this.BrowseScript_Click);
			// 
			// edtScript
			// 
			this.edtScript.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtScript.Location = new System.Drawing.Point(10, 29);
			this.edtScript.Margin = new System.Windows.Forms.Padding(2);
			this.edtScript.Name = "edtScript";
			this.edtScript.Size = new System.Drawing.Size(384, 23);
			this.edtScript.TabIndex = 1;
			// 
			// lblScript
			// 
			this.lblScript.AutoSize = true;
			this.lblScript.Location = new System.Drawing.Point(10, 12);
			this.lblScript.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblScript.Name = "lblScript";
			this.lblScript.Size = new System.Drawing.Size(40, 15);
			this.lblScript.TabIndex = 0;
			this.lblScript.Text = "Script:";
			// 
			// OpenCsxDlg
			// 
			this.OpenCsxDlg.Filter = "C# Interactive Scripts (*.csx)|*.csx|All Files (*.*)|*.*";
			this.OpenCsxDlg.Title = "Select C# Interactive Script";
			// 
			// treatErrorAsOutput
			// 
			this.treatErrorAsOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.treatErrorAsOutput.AutoSize = true;
			this.treatErrorAsOutput.Location = new System.Drawing.Point(260, 112);
			this.treatErrorAsOutput.Name = "treatErrorAsOutput";
			this.treatErrorAsOutput.Size = new System.Drawing.Size(176, 19);
			this.treatErrorAsOutput.TabIndex = 8;
			this.treatErrorAsOutput.Text = "Treat Error Stream As Output";
			this.treatErrorAsOutput.UseVisualStyleBackColor = true;
			// 
			// lblToolsVersion
			// 
			this.lblToolsVersion.AutoSize = true;
			this.lblToolsVersion.Location = new System.Drawing.Point(12, 112);
			this.lblToolsVersion.Name = "lblToolsVersion";
			this.lblToolsVersion.Size = new System.Drawing.Size(78, 15);
			this.lblToolsVersion.TabIndex = 6;
			this.lblToolsVersion.Text = "Tools Version:";
			// 
			// cbToolsVersion
			// 
			this.cbToolsVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbToolsVersion.FormattingEnabled = true;
			this.cbToolsVersion.Location = new System.Drawing.Point(96, 108);
			this.cbToolsVersion.Name = "cbToolsVersion";
			this.cbToolsVersion.Size = new System.Drawing.Size(80, 23);
			this.cbToolsVersion.TabIndex = 7;
			// 
			// btnAddScriptArg
			// 
			this.btnAddScriptArg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddScriptArg.Location = new System.Drawing.Point(376, 0);
			this.btnAddScriptArg.Margin = new System.Windows.Forms.Padding(2);
			this.btnAddScriptArg.Name = "btnAddScriptArg";
			this.btnAddScriptArg.Size = new System.Drawing.Size(44, 24);
			this.btnAddScriptArg.TabIndex = 1;
			this.btnAddScriptArg.Text = "+File";
			this.btnAddScriptArg.Click += new System.EventHandler(this.AddScriptArg_Click);
			// 
			// edtScriptArgs
			// 
			this.edtScriptArgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtScriptArgs.Location = new System.Drawing.Point(0, 24);
			this.edtScriptArgs.Margin = new System.Windows.Forms.Padding(2);
			this.edtScriptArgs.Name = "edtScriptArgs";
			this.edtScriptArgs.RichTextShortcutsEnabled = false;
			this.edtScriptArgs.Size = new System.Drawing.Size(420, 144);
			this.edtScriptArgs.TabIndex = 2;
			this.edtScriptArgs.WordWrap = false;
			// 
			// lblScriptArgs
			// 
			this.lblScriptArgs.AutoSize = true;
			this.lblScriptArgs.Location = new System.Drawing.Point(0, 8);
			this.lblScriptArgs.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblScriptArgs.Name = "lblScriptArgs";
			this.lblScriptArgs.Size = new System.Drawing.Size(102, 15);
			this.lblScriptArgs.TabIndex = 0;
			this.lblScriptArgs.Text = "Script Arguments:";
			// 
			// btnReference
			// 
			this.btnReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnReference.Location = new System.Drawing.Point(268, 0);
			this.btnReference.Margin = new System.Windows.Forms.Padding(2);
			this.btnReference.Name = "btnReference";
			this.btnReference.Size = new System.Drawing.Size(40, 24);
			this.btnReference.TabIndex = 1;
			this.btnReference.Text = "+Ref";
			this.btnReference.Click += new System.EventHandler(this.AddReference_Click);
			// 
			// edtCsiOptions
			// 
			this.edtCsiOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtCsiOptions.Location = new System.Drawing.Point(0, 24);
			this.edtCsiOptions.Margin = new System.Windows.Forms.Padding(2);
			this.edtCsiOptions.Name = "edtCsiOptions";
			this.edtCsiOptions.RichTextShortcutsEnabled = false;
			this.edtCsiOptions.Size = new System.Drawing.Size(420, 144);
			this.edtCsiOptions.TabIndex = 4;
			this.edtCsiOptions.WordWrap = false;
			// 
			// lblCsiOptions
			// 
			this.lblCsiOptions.AutoSize = true;
			this.lblCsiOptions.Location = new System.Drawing.Point(0, 8);
			this.lblCsiOptions.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblCsiOptions.Name = "lblCsiOptions";
			this.lblCsiOptions.Size = new System.Drawing.Size(92, 15);
			this.lblCsiOptions.TabIndex = 0;
			this.lblCsiOptions.Text = "Csi.exe Options:";
			// 
			// OpenAllDlg
			// 
			this.OpenAllDlg.Filter = "All Files (*.*)|*.*";
			this.OpenAllDlg.Multiselect = true;
			this.OpenAllDlg.Title = "Select Files";
			// 
			// OpenAsmDlg
			// 
			this.OpenAsmDlg.Filter = ".NET Assemblies (*.dll;*.exe)|*.dll;*.exe|All Files (*.*)|*.*";
			this.OpenAsmDlg.Title = "Select .NET Assembly";
			// 
			// btnLibPath
			// 
			this.btnLibPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLibPath.Location = new System.Drawing.Point(312, 0);
			this.btnLibPath.Margin = new System.Windows.Forms.Padding(2);
			this.btnLibPath.Name = "btnLibPath";
			this.btnLibPath.Size = new System.Drawing.Size(48, 24);
			this.btnLibPath.TabIndex = 2;
			this.btnLibPath.Text = "+Path";
			this.btnLibPath.Click += new System.EventHandler(this.AddLibPath_Click);
			// 
			// btnUsing
			// 
			this.btnUsing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUsing.Location = new System.Drawing.Point(364, 0);
			this.btnUsing.Margin = new System.Windows.Forms.Padding(2);
			this.btnUsing.Name = "btnUsing";
			this.btnUsing.Size = new System.Drawing.Size(56, 24);
			this.btnUsing.TabIndex = 3;
			this.btnUsing.Text = "+Using";
			this.btnUsing.Click += new System.EventHandler(this.AddUsing_Click);
			// 
			// tableLayout
			// 
			this.tableLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayout.ColumnCount = 1;
			this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayout.Controls.Add(this.pnlScriptArgs, 0, 0);
			this.tableLayout.Controls.Add(this.pnlCsiOptions, 0, 1);
			this.tableLayout.Location = new System.Drawing.Point(8, 132);
			this.tableLayout.Name = "tableLayout";
			this.tableLayout.RowCount = 2;
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayout.Size = new System.Drawing.Size(428, 352);
			this.tableLayout.TabIndex = 9;
			// 
			// pnlScriptArgs
			// 
			this.pnlScriptArgs.Controls.Add(this.lblScriptArgs);
			this.pnlScriptArgs.Controls.Add(this.edtScriptArgs);
			this.pnlScriptArgs.Controls.Add(this.btnAddScriptArg);
			this.pnlScriptArgs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlScriptArgs.Location = new System.Drawing.Point(3, 3);
			this.pnlScriptArgs.Name = "pnlScriptArgs";
			this.pnlScriptArgs.Size = new System.Drawing.Size(422, 170);
			this.pnlScriptArgs.TabIndex = 0;
			// 
			// pnlCsiOptions
			// 
			this.pnlCsiOptions.Controls.Add(this.edtCsiOptions);
			this.pnlCsiOptions.Controls.Add(this.btnUsing);
			this.pnlCsiOptions.Controls.Add(this.lblCsiOptions);
			this.pnlCsiOptions.Controls.Add(this.btnLibPath);
			this.pnlCsiOptions.Controls.Add(this.btnReference);
			this.pnlCsiOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlCsiOptions.Location = new System.Drawing.Point(3, 179);
			this.pnlCsiOptions.Name = "pnlCsiOptions";
			this.pnlCsiOptions.Size = new System.Drawing.Size(422, 170);
			this.pnlCsiOptions.TabIndex = 1;
			// 
			// CsiStepCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayout);
			this.Controls.Add(this.lblToolsVersion);
			this.Controls.Add(this.cbToolsVersion);
			this.Controls.Add(this.treatErrorAsOutput);
			this.Controls.Add(this.btnBrowseDirectory);
			this.Controls.Add(this.edtWorkingDirectory);
			this.Controls.Add(this.lblWorkingDirectory);
			this.Controls.Add(this.btnBrowseScript);
			this.Controls.Add(this.edtScript);
			this.Controls.Add(this.lblScript);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "CsiStepCtrl";
			this.Size = new System.Drawing.Size(443, 485);
			this.tableLayout.ResumeLayout(false);
			this.pnlScriptArgs.ResumeLayout(false);
			this.pnlScriptArgs.PerformLayout();
			this.pnlCsiOptions.ResumeLayout(false);
			this.pnlCsiOptions.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnBrowseDirectory;
		private System.Windows.Forms.TextBox edtWorkingDirectory;
		private System.Windows.Forms.Label lblWorkingDirectory;
		private System.Windows.Forms.Button btnBrowseScript;
		private System.Windows.Forms.TextBox edtScript;
		private System.Windows.Forms.Label lblScript;
		private System.Windows.Forms.OpenFileDialog OpenCsxDlg;
		private System.Windows.Forms.CheckBox treatErrorAsOutput;
		private System.Windows.Forms.Label lblToolsVersion;
		private System.Windows.Forms.ComboBox cbToolsVersion;
		private System.Windows.Forms.Button btnAddScriptArg;
		private Menees.Windows.Forms.ExtendedRichTextBox edtScriptArgs;
		private System.Windows.Forms.Label lblScriptArgs;
		private System.Windows.Forms.Button btnReference;
		private Menees.Windows.Forms.ExtendedRichTextBox edtCsiOptions;
		private System.Windows.Forms.Label lblCsiOptions;
		private System.Windows.Forms.OpenFileDialog OpenAllDlg;
		private System.Windows.Forms.OpenFileDialog OpenAsmDlg;
		private System.Windows.Forms.Button btnLibPath;
		private System.Windows.Forms.Button btnUsing;
		private System.Windows.Forms.TableLayoutPanel tableLayout;
		private System.Windows.Forms.Panel pnlScriptArgs;
		private System.Windows.Forms.Panel pnlCsiOptions;
	}
}

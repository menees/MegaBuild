using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Menees;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Menees.Windows.Forms;
namespace MegaBuild
{
	internal sealed partial class ProjectOptionsDlg : ExtendedForm
	{
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TabControl TabCtrl;
		private System.Windows.Forms.TabPage tabVS;
		private System.Windows.Forms.TabPage tabComments;
		private System.Windows.Forms.TabPage tabLogging;
		private System.Windows.Forms.SaveFileDialog SaveDlg;
		private System.Windows.Forms.Button btnLogFile;
		internal System.Windows.Forms.TextBox edtLogFile;
		internal System.Windows.Forms.CheckBox chkLogOutput;
		internal System.Windows.Forms.CheckBox chkOverwriteLog;
		internal System.Windows.Forms.CheckBox chkTimestamp;
		internal System.Windows.Forms.CheckBox chkShowComments;
		internal System.Windows.Forms.ListView lstConfigurations;
		internal System.Windows.Forms.ComboBox cbVersion;
		internal System.Windows.Forms.ComboBox cbAction;
		internal System.Windows.Forms.CheckBox chkOverrideConfigurations;
		internal System.Windows.Forms.CheckBox chkOverrideActions;
		internal System.Windows.Forms.CheckBox chkOverrideVersions;
		private System.Windows.Forms.ColumnHeader colMain;
		private TabPage tabVariables;
		private CheckBox chkExpandPath;
		private Label lblPercent2;
		private Label lblPercent1;
		private TextBox edtValue;
		private Label lblValue;
		private TextBox edtName;
		private Label lblName;
		private Button btnDelete;
		private Button btnAdd;
		private ListView lstVariables;
		private ColumnHeader colName;
		private ColumnHeader colValue;
		private ColumnHeader colExpand;
		private System.ComponentModel.Container components = null;
		internal Menees.Windows.Forms.ExtendedRichTextBox txtComments;
		private MegaBuild.ListViewItemMover ConfigMover;

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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.txtComments = new Menees.Windows.Forms.ExtendedRichTextBox();
			this.TabCtrl = new System.Windows.Forms.TabControl();
			this.tabVS = new System.Windows.Forms.TabPage();
			this.ConfigMover = new MegaBuild.ListViewItemMover();
			this.lstConfigurations = new System.Windows.Forms.ListView();
			this.colMain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.cbAction = new System.Windows.Forms.ComboBox();
			this.cbVersion = new System.Windows.Forms.ComboBox();
			this.chkOverrideVersions = new System.Windows.Forms.CheckBox();
			this.chkOverrideActions = new System.Windows.Forms.CheckBox();
			this.chkOverrideConfigurations = new System.Windows.Forms.CheckBox();
			this.tabLogging = new System.Windows.Forms.TabPage();
			this.chkTimestamp = new System.Windows.Forms.CheckBox();
			this.chkOverwriteLog = new System.Windows.Forms.CheckBox();
			this.edtLogFile = new System.Windows.Forms.TextBox();
			this.btnLogFile = new System.Windows.Forms.Button();
			this.chkLogOutput = new System.Windows.Forms.CheckBox();
			this.tabComments = new System.Windows.Forms.TabPage();
			this.chkShowComments = new System.Windows.Forms.CheckBox();
			this.tabVariables = new System.Windows.Forms.TabPage();
			this.chkExpandPath = new System.Windows.Forms.CheckBox();
			this.lblPercent2 = new System.Windows.Forms.Label();
			this.edtValue = new System.Windows.Forms.TextBox();
			this.lblValue = new System.Windows.Forms.Label();
			this.edtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.lstVariables = new System.Windows.Forms.ListView();
			this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colExpand = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblPercent1 = new System.Windows.Forms.Label();
			this.SaveDlg = new System.Windows.Forms.SaveFileDialog();
			this.chkShowDebugOutput = new System.Windows.Forms.CheckBox();
			this.TabCtrl.SuspendLayout();
			this.tabVS.SuspendLayout();
			this.tabLogging.SuspendLayout();
			this.tabComments.SuspendLayout();
			this.tabVariables.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(387, 399);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(90, 29);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "&Cancel";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(287, 399);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(90, 29);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.OK_Click);
			// 
			// txtComments
			// 
			this.txtComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtComments.Location = new System.Drawing.Point(10, 10);
			this.txtComments.Name = "txtComments";
			this.txtComments.RichTextShortcutsEnabled = false;
			this.txtComments.Size = new System.Drawing.Size(437, 286);
			this.txtComments.TabIndex = 0;
			// 
			// TabCtrl
			// 
			this.TabCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TabCtrl.Controls.Add(this.tabVS);
			this.TabCtrl.Controls.Add(this.tabLogging);
			this.TabCtrl.Controls.Add(this.tabComments);
			this.TabCtrl.Controls.Add(this.tabVariables);
			this.TabCtrl.Location = new System.Drawing.Point(14, 15);
			this.TabCtrl.Name = "TabCtrl";
			this.TabCtrl.SelectedIndex = 0;
			this.TabCtrl.Size = new System.Drawing.Size(465, 370);
			this.TabCtrl.TabIndex = 0;
			// 
			// tabVS
			// 
			this.tabVS.Controls.Add(this.ConfigMover);
			this.tabVS.Controls.Add(this.cbAction);
			this.tabVS.Controls.Add(this.cbVersion);
			this.tabVS.Controls.Add(this.chkOverrideVersions);
			this.tabVS.Controls.Add(this.chkOverrideActions);
			this.tabVS.Controls.Add(this.chkOverrideConfigurations);
			this.tabVS.Controls.Add(this.lstConfigurations);
			this.tabVS.Location = new System.Drawing.Point(4, 24);
			this.tabVS.Name = "tabVS";
			this.tabVS.Size = new System.Drawing.Size(457, 342);
			this.tabVS.TabIndex = 0;
			this.tabVS.Text = "Visual Studio";
			this.tabVS.UseVisualStyleBackColor = true;
			// 
			// ConfigMover
			// 
			this.ConfigMover.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ConfigMover.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.ConfigMover.ListView = this.lstConfigurations;
			this.ConfigMover.Location = new System.Drawing.Point(404, 118);
			this.ConfigMover.Name = "ConfigMover";
			this.ConfigMover.Size = new System.Drawing.Size(39, 203);
			this.ConfigMover.TabIndex = 6;
			// 
			// lstConfigurations
			// 
			this.lstConfigurations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstConfigurations.CheckBoxes = true;
			this.lstConfigurations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colMain});
			this.lstConfigurations.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lstConfigurations.HideSelection = false;
			this.lstConfigurations.Location = new System.Drawing.Point(38, 118);
			this.lstConfigurations.Name = "lstConfigurations";
			this.lstConfigurations.Size = new System.Drawing.Size(352, 203);
			this.lstConfigurations.TabIndex = 5;
			this.lstConfigurations.UseCompatibleStateImageBehavior = false;
			this.lstConfigurations.View = System.Windows.Forms.View.Details;
			// 
			// colMain
			// 
			this.colMain.Text = "";
			// 
			// cbAction
			// 
			this.cbAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbAction.Items.AddRange(new object[] {
            "Build",
            "Rebuild",
            "Clean",
            "Deploy"});
			this.cbAction.Location = new System.Drawing.Point(187, 15);
			this.cbAction.Name = "cbAction";
			this.cbAction.Size = new System.Drawing.Size(256, 23);
			this.cbAction.TabIndex = 1;
			// 
			// cbVersion
			// 
			this.cbVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVersion.Location = new System.Drawing.Point(187, 54);
			this.cbVersion.Name = "cbVersion";
			this.cbVersion.Size = new System.Drawing.Size(256, 23);
			this.cbVersion.TabIndex = 3;
			// 
			// chkOverrideVersions
			// 
			this.chkOverrideVersions.AutoSize = true;
			this.chkOverrideVersions.Location = new System.Drawing.Point(14, 54);
			this.chkOverrideVersions.Name = "chkOverrideVersions";
			this.chkOverrideVersions.Size = new System.Drawing.Size(146, 19);
			this.chkOverrideVersions.TabIndex = 2;
			this.chkOverrideVersions.Text = "Override Step Versions:";
			this.chkOverrideVersions.CheckedChanged += new System.EventHandler(this.ControlStateChanged);
			// 
			// chkOverrideActions
			// 
			this.chkOverrideActions.AutoSize = true;
			this.chkOverrideActions.Location = new System.Drawing.Point(14, 15);
			this.chkOverrideActions.Name = "chkOverrideActions";
			this.chkOverrideActions.Size = new System.Drawing.Size(143, 19);
			this.chkOverrideActions.TabIndex = 0;
			this.chkOverrideActions.Text = "Override Step Actions:";
			this.chkOverrideActions.CheckedChanged += new System.EventHandler(this.ControlStateChanged);
			// 
			// chkOverrideConfigurations
			// 
			this.chkOverrideConfigurations.AutoSize = true;
			this.chkOverrideConfigurations.Location = new System.Drawing.Point(14, 94);
			this.chkOverrideConfigurations.Name = "chkOverrideConfigurations";
			this.chkOverrideConfigurations.Size = new System.Drawing.Size(182, 19);
			this.chkOverrideConfigurations.TabIndex = 4;
			this.chkOverrideConfigurations.Text = "Override Step Configurations:";
			this.chkOverrideConfigurations.CheckedChanged += new System.EventHandler(this.ControlStateChanged);
			// 
			// tabLogging
			// 
			this.tabLogging.Controls.Add(this.chkShowDebugOutput);
			this.tabLogging.Controls.Add(this.chkTimestamp);
			this.tabLogging.Controls.Add(this.chkOverwriteLog);
			this.tabLogging.Controls.Add(this.edtLogFile);
			this.tabLogging.Controls.Add(this.btnLogFile);
			this.tabLogging.Controls.Add(this.chkLogOutput);
			this.tabLogging.Location = new System.Drawing.Point(4, 24);
			this.tabLogging.Name = "tabLogging";
			this.tabLogging.Size = new System.Drawing.Size(457, 342);
			this.tabLogging.TabIndex = 2;
			this.tabLogging.Text = "Logging";
			this.tabLogging.UseVisualStyleBackColor = true;
			// 
			// chkTimestamp
			// 
			this.chkTimestamp.AutoSize = true;
			this.chkTimestamp.Location = new System.Drawing.Point(38, 113);
			this.chkTimestamp.Name = "chkTimestamp";
			this.chkTimestamp.Size = new System.Drawing.Size(207, 19);
			this.chkTimestamp.TabIndex = 4;
			this.chkTimestamp.Text = "Put Timestamp On Each Log Entry";
			// 
			// chkOverwriteLog
			// 
			this.chkOverwriteLog.AutoSize = true;
			this.chkOverwriteLog.Location = new System.Drawing.Point(38, 79);
			this.chkOverwriteLog.Name = "chkOverwriteLog";
			this.chkOverwriteLog.Size = new System.Drawing.Size(154, 19);
			this.chkOverwriteLog.TabIndex = 3;
			this.chkOverwriteLog.Text = "Overwrite On Each Build";
			// 
			// edtLogFile
			// 
			this.edtLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtLogFile.Location = new System.Drawing.Point(38, 39);
			this.edtLogFile.Name = "edtLogFile";
			this.edtLogFile.Size = new System.Drawing.Size(361, 23);
			this.edtLogFile.TabIndex = 1;
			// 
			// btnLogFile
			// 
			this.btnLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLogFile.Location = new System.Drawing.Point(412, 39);
			this.btnLogFile.Name = "btnLogFile";
			this.btnLogFile.Size = new System.Drawing.Size(28, 24);
			this.btnLogFile.TabIndex = 2;
			this.btnLogFile.Text = "...";
			this.btnLogFile.Click += new System.EventHandler(this.LogFile_Click);
			// 
			// chkLogOutput
			// 
			this.chkLogOutput.AutoSize = true;
			this.chkLogOutput.Location = new System.Drawing.Point(14, 10);
			this.chkLogOutput.Name = "chkLogOutput";
			this.chkLogOutput.Size = new System.Drawing.Size(138, 19);
			this.chkLogOutput.TabIndex = 0;
			this.chkLogOutput.Text = "Log Output To A File:";
			this.chkLogOutput.CheckedChanged += new System.EventHandler(this.ControlStateChanged);
			// 
			// tabComments
			// 
			this.tabComments.Controls.Add(this.chkShowComments);
			this.tabComments.Controls.Add(this.txtComments);
			this.tabComments.Location = new System.Drawing.Point(4, 24);
			this.tabComments.Name = "tabComments";
			this.tabComments.Size = new System.Drawing.Size(457, 342);
			this.tabComments.TabIndex = 1;
			this.tabComments.Text = "Project Comments";
			this.tabComments.UseVisualStyleBackColor = true;
			// 
			// chkShowComments
			// 
			this.chkShowComments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkShowComments.AutoSize = true;
			this.chkShowComments.Location = new System.Drawing.Point(10, 312);
			this.chkShowComments.Name = "chkShowComments";
			this.chkShowComments.Size = new System.Drawing.Size(247, 19);
			this.chkShowComments.TabIndex = 1;
			this.chkShowComments.Text = "Show Comments When Project Is Opened";
			// 
			// tabVariables
			// 
			this.tabVariables.Controls.Add(this.chkExpandPath);
			this.tabVariables.Controls.Add(this.lblPercent2);
			this.tabVariables.Controls.Add(this.edtValue);
			this.tabVariables.Controls.Add(this.lblValue);
			this.tabVariables.Controls.Add(this.edtName);
			this.tabVariables.Controls.Add(this.lblName);
			this.tabVariables.Controls.Add(this.btnDelete);
			this.tabVariables.Controls.Add(this.btnAdd);
			this.tabVariables.Controls.Add(this.lstVariables);
			this.tabVariables.Controls.Add(this.lblPercent1);
			this.tabVariables.Location = new System.Drawing.Point(4, 24);
			this.tabVariables.Name = "tabVariables";
			this.tabVariables.Padding = new System.Windows.Forms.Padding(3);
			this.tabVariables.Size = new System.Drawing.Size(457, 342);
			this.tabVariables.TabIndex = 3;
			this.tabVariables.Text = "Variables";
			this.tabVariables.UseVisualStyleBackColor = true;
			// 
			// chkExpandPath
			// 
			this.chkExpandPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkExpandPath.AutoSize = true;
			this.chkExpandPath.Location = new System.Drawing.Point(19, 302);
			this.chkExpandPath.Name = "chkExpandPath";
			this.chkExpandPath.Size = new System.Drawing.Size(301, 19);
			this.chkExpandPath.TabIndex = 7;
			this.chkExpandPath.Text = "Expand Project-Relative Path (e.g., $(ProjectDir)\\..\\..)";
			this.chkExpandPath.UseVisualStyleBackColor = true;
			this.chkExpandPath.CheckedChanged += new System.EventHandler(this.ExpandPath_CheckedChanged);
			// 
			// lblPercent2
			// 
			this.lblPercent2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPercent2.AutoSize = true;
			this.lblPercent2.Location = new System.Drawing.Point(341, 232);
			this.lblPercent2.Name = "lblPercent2";
			this.lblPercent2.Size = new System.Drawing.Size(17, 15);
			this.lblPercent2.TabIndex = 4;
			this.lblPercent2.Text = "%";
			// 
			// edtValue
			// 
			this.edtValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.edtValue.Location = new System.Drawing.Point(86, 262);
			this.edtValue.Name = "edtValue";
			this.edtValue.Size = new System.Drawing.Size(250, 23);
			this.edtValue.TabIndex = 6;
			this.edtValue.TextChanged += new System.EventHandler(this.Value_TextChanged);
			// 
			// lblValue
			// 
			this.lblValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblValue.AutoSize = true;
			this.lblValue.Location = new System.Drawing.Point(14, 267);
			this.lblValue.Name = "lblValue";
			this.lblValue.Size = new System.Drawing.Size(38, 15);
			this.lblValue.TabIndex = 5;
			this.lblValue.Text = "Value:";
			// 
			// edtName
			// 
			this.edtName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.edtName.Location = new System.Drawing.Point(86, 227);
			this.edtName.Name = "edtName";
			this.edtName.Size = new System.Drawing.Size(250, 23);
			this.edtName.TabIndex = 3;
			this.edtName.TextChanged += new System.EventHandler(this.Name_TextChanged);
			// 
			// lblName
			// 
			this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(14, 232);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(42, 15);
			this.lblName.TabIndex = 1;
			this.lblName.Text = "Name:";
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Location = new System.Drawing.Point(379, 267);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(63, 28);
			this.btnDelete.TabIndex = 9;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.Click += new System.EventHandler(this.Delete_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.Location = new System.Drawing.Point(379, 227);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(63, 28);
			this.btnAdd.TabIndex = 8;
			this.btnAdd.Text = "&Add";
			this.btnAdd.Click += new System.EventHandler(this.Add_Click);
			// 
			// lstVariables
			// 
			this.lstVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstVariables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colValue,
            this.colExpand});
			this.lstVariables.FullRowSelect = true;
			this.lstVariables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstVariables.HideSelection = false;
			this.lstVariables.Location = new System.Drawing.Point(14, 15);
			this.lstVariables.MultiSelect = false;
			this.lstVariables.Name = "lstVariables";
			this.lstVariables.Size = new System.Drawing.Size(429, 197);
			this.lstVariables.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lstVariables.TabIndex = 0;
			this.lstVariables.UseCompatibleStateImageBehavior = false;
			this.lstVariables.View = System.Windows.Forms.View.Details;
			this.lstVariables.SelectedIndexChanged += new System.EventHandler(this.Variables_SelectedIndexChanged);
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.Width = 165;
			// 
			// colValue
			// 
			this.colValue.Text = "Value";
			this.colValue.Width = 176;
			// 
			// colExpand
			// 
			this.colExpand.Text = "Expand";
			// 
			// lblPercent1
			// 
			this.lblPercent1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPercent1.AutoSize = true;
			this.lblPercent1.Location = new System.Drawing.Point(67, 231);
			this.lblPercent1.Name = "lblPercent1";
			this.lblPercent1.Size = new System.Drawing.Size(17, 15);
			this.lblPercent1.TabIndex = 2;
			this.lblPercent1.Text = "%";
			// 
			// SaveDlg
			// 
			this.SaveDlg.DefaultExt = "txt";
			this.SaveDlg.FileName = "BuildLog";
			this.SaveDlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			this.SaveDlg.Title = "Choose Log File Name";
			// 
			// chkShowDebugOutput
			// 
			this.chkShowDebugOutput.AutoSize = true;
			this.chkShowDebugOutput.Location = new System.Drawing.Point(14, 147);
			this.chkShowDebugOutput.Name = "chkShowDebugOutput";
			this.chkShowDebugOutput.Size = new System.Drawing.Size(225, 19);
			this.chkShowDebugOutput.TabIndex = 5;
			this.chkShowDebugOutput.Text = "Include MegaBuild Diagnostic Output";
			// 
			// ProjectOptionsDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(493, 441);
			this.Controls.Add(this.TabCtrl);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProjectOptionsDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Project Options";
			this.Load += new System.EventHandler(this.ProjectOptionsDlg_Load);
			this.TabCtrl.ResumeLayout(false);
			this.tabVS.ResumeLayout(false);
			this.tabVS.PerformLayout();
			this.tabLogging.ResumeLayout(false);
			this.tabLogging.PerformLayout();
			this.tabComments.ResumeLayout(false);
			this.tabComments.PerformLayout();
			this.tabVariables.ResumeLayout(false);
			this.tabVariables.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		internal CheckBox chkShowDebugOutput;
	}
}


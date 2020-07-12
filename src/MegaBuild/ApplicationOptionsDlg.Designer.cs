using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Specialized;
using Menees;
using System.Collections.Generic;
using Menees.Windows.Forms;
namespace MegaBuild
{
	internal sealed partial class ApplicationOptionsDlg : ExtendedForm
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TabControl TabCtrl;
		private System.Windows.Forms.CheckBox chkAlwaysOnTop;
		private System.Windows.Forms.CheckBox chkSaveChanges;
		private System.Windows.Forms.CheckBox chkReloadLast;
		private System.Windows.Forms.CheckBox chkNeverShowProjectComments;
		private System.Windows.Forms.TabPage tabApplication;
		private System.Windows.Forms.TabPage tabVariables;
		private System.Windows.Forms.GroupBox grpOutput;
		private System.Windows.Forms.CheckBox chkWordWrap;
		private System.Windows.Forms.CheckBox chkClearOutput;
		private System.Windows.Forms.GroupBox grpGeneral;
		private System.Windows.Forms.ListView lstVariables;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colValue;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox edtName;
		private System.Windows.Forms.TextBox edtValue;
		private System.Windows.Forms.Label lblValue;
		private System.Windows.Forms.Label lblPercent1;
		private System.Windows.Forms.Label lblPercent2;
		private System.Windows.Forms.CheckBox chkSwitchToFailure;
		private System.Windows.Forms.CheckBox chkParseOutputCommands;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.TabCtrl = new System.Windows.Forms.TabControl();
			this.tabApplication = new System.Windows.Forms.TabPage();
			this.grpGeneral = new System.Windows.Forms.GroupBox();
			this.chkShowProgressInTaskbar = new System.Windows.Forms.CheckBox();
			this.chkSwitchToFailure = new System.Windows.Forms.CheckBox();
			this.chkAlwaysOnTop = new System.Windows.Forms.CheckBox();
			this.chkNeverShowProjectComments = new System.Windows.Forms.CheckBox();
			this.chkSaveChanges = new System.Windows.Forms.CheckBox();
			this.chkReloadLast = new System.Windows.Forms.CheckBox();
			this.grpOutput = new System.Windows.Forms.GroupBox();
			this.timestampLabel = new System.Windows.Forms.Label();
			this.timestampFormat = new System.Windows.Forms.ComboBox();
			this.chkParseOutputCommands = new System.Windows.Forms.CheckBox();
			this.chkWordWrap = new System.Windows.Forms.CheckBox();
			this.chkClearOutput = new System.Windows.Forms.CheckBox();
			this.tabVariables = new System.Windows.Forms.TabPage();
			this.lblPercent2 = new System.Windows.Forms.Label();
			this.lblPercent1 = new System.Windows.Forms.Label();
			this.edtValue = new System.Windows.Forms.TextBox();
			this.lblValue = new System.Windows.Forms.Label();
			this.edtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.lstVariables = new System.Windows.Forms.ListView();
			this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chkOutputWindowOnRight = new System.Windows.Forms.CheckBox();
			this.TabCtrl.SuspendLayout();
			this.tabApplication.SuspendLayout();
			this.grpGeneral.SuspendLayout();
			this.grpOutput.SuspendLayout();
			this.tabVariables.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(251, 428);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(90, 28);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.OK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(352, 428);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(90, 28);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "&Cancel";
			// 
			// TabCtrl
			// 
			this.TabCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TabCtrl.Controls.Add(this.tabApplication);
			this.TabCtrl.Controls.Add(this.tabVariables);
			this.TabCtrl.Location = new System.Drawing.Point(14, 15);
			this.TabCtrl.Name = "TabCtrl";
			this.TabCtrl.SelectedIndex = 0;
			this.TabCtrl.Size = new System.Drawing.Size(427, 400);
			this.TabCtrl.TabIndex = 0;
			// 
			// tabApplication
			// 
			this.tabApplication.Controls.Add(this.grpGeneral);
			this.tabApplication.Controls.Add(this.grpOutput);
			this.tabApplication.Location = new System.Drawing.Point(4, 24);
			this.tabApplication.Name = "tabApplication";
			this.tabApplication.Size = new System.Drawing.Size(419, 372);
			this.tabApplication.TabIndex = 0;
			this.tabApplication.Text = "Application";
			this.tabApplication.UseVisualStyleBackColor = true;
			// 
			// grpGeneral
			// 
			this.grpGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpGeneral.Controls.Add(this.chkShowProgressInTaskbar);
			this.grpGeneral.Controls.Add(this.chkSwitchToFailure);
			this.grpGeneral.Controls.Add(this.chkAlwaysOnTop);
			this.grpGeneral.Controls.Add(this.chkNeverShowProjectComments);
			this.grpGeneral.Controls.Add(this.chkSaveChanges);
			this.grpGeneral.Controls.Add(this.chkReloadLast);
			this.grpGeneral.Location = new System.Drawing.Point(14, 10);
			this.grpGeneral.Name = "grpGeneral";
			this.grpGeneral.Size = new System.Drawing.Size(388, 174);
			this.grpGeneral.TabIndex = 0;
			this.grpGeneral.TabStop = false;
			this.grpGeneral.Text = "General";
			// 
			// chkShowProgressInTaskbar
			// 
			this.chkShowProgressInTaskbar.AutoSize = true;
			this.chkShowProgressInTaskbar.Location = new System.Drawing.Point(16, 144);
			this.chkShowProgressInTaskbar.Name = "chkShowProgressInTaskbar";
			this.chkShowProgressInTaskbar.Size = new System.Drawing.Size(158, 19);
			this.chkShowProgressInTaskbar.TabIndex = 5;
			this.chkShowProgressInTaskbar.Text = "Show Progress In Taskbar";
			// 
			// chkSwitchToFailure
			// 
			this.chkSwitchToFailure.AutoSize = true;
			this.chkSwitchToFailure.Location = new System.Drawing.Point(16, 120);
			this.chkSwitchToFailure.Name = "chkSwitchToFailure";
			this.chkSwitchToFailure.Size = new System.Drawing.Size(289, 19);
			this.chkSwitchToFailure.TabIndex = 4;
			this.chkSwitchToFailure.Text = "Switch To Failure Steps Tab If Failure Steps Execute";
			// 
			// chkAlwaysOnTop
			// 
			this.chkAlwaysOnTop.AutoSize = true;
			this.chkAlwaysOnTop.Location = new System.Drawing.Point(16, 96);
			this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
			this.chkAlwaysOnTop.Size = new System.Drawing.Size(104, 19);
			this.chkAlwaysOnTop.TabIndex = 3;
			this.chkAlwaysOnTop.Text = "Always On Top";
			// 
			// chkNeverShowProjectComments
			// 
			this.chkNeverShowProjectComments.AutoSize = true;
			this.chkNeverShowProjectComments.Location = new System.Drawing.Point(16, 72);
			this.chkNeverShowProjectComments.Name = "chkNeverShowProjectComments";
			this.chkNeverShowProjectComments.Size = new System.Drawing.Size(191, 19);
			this.chkNeverShowProjectComments.TabIndex = 2;
			this.chkNeverShowProjectComments.Text = "Never Show Project Comments";
			// 
			// chkSaveChanges
			// 
			this.chkSaveChanges.AutoSize = true;
			this.chkSaveChanges.Location = new System.Drawing.Point(16, 24);
			this.chkSaveChanges.Name = "chkSaveChanges";
			this.chkSaveChanges.Size = new System.Drawing.Size(166, 19);
			this.chkSaveChanges.TabIndex = 0;
			this.chkSaveChanges.Text = "Save Changes Before Build";
			// 
			// chkReloadLast
			// 
			this.chkReloadLast.AutoSize = true;
			this.chkReloadLast.Location = new System.Drawing.Point(16, 48);
			this.chkReloadLast.Name = "chkReloadLast";
			this.chkReloadLast.Size = new System.Drawing.Size(182, 19);
			this.chkReloadLast.TabIndex = 1;
			this.chkReloadLast.Text = "Reload Last Project At Startup";
			// 
			// grpOutput
			// 
			this.grpOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpOutput.Controls.Add(this.chkOutputWindowOnRight);
			this.grpOutput.Controls.Add(this.timestampLabel);
			this.grpOutput.Controls.Add(this.timestampFormat);
			this.grpOutput.Controls.Add(this.chkParseOutputCommands);
			this.grpOutput.Controls.Add(this.chkWordWrap);
			this.grpOutput.Controls.Add(this.chkClearOutput);
			this.grpOutput.Location = new System.Drawing.Point(14, 196);
			this.grpOutput.Name = "grpOutput";
			this.grpOutput.Size = new System.Drawing.Size(388, 160);
			this.grpOutput.TabIndex = 1;
			this.grpOutput.TabStop = false;
			this.grpOutput.Text = "Output";
			// 
			// timestampLabel
			// 
			this.timestampLabel.AutoSize = true;
			this.timestampLabel.Location = new System.Drawing.Point(12, 128);
			this.timestampLabel.Name = "timestampLabel";
			this.timestampLabel.Size = new System.Drawing.Size(140, 15);
			this.timestampLabel.TabIndex = 4;
			this.timestampLabel.Text = "Entry Timestamp Format:";
			// 
			// timestampFormat
			// 
			this.timestampFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.timestampFormat.FormattingEnabled = true;
			this.timestampFormat.Location = new System.Drawing.Point(164, 124);
			this.timestampFormat.Name = "timestampFormat";
			this.timestampFormat.Size = new System.Drawing.Size(121, 23);
			this.timestampFormat.TabIndex = 5;
			// 
			// chkParseOutputCommands
			// 
			this.chkParseOutputCommands.AutoSize = true;
			this.chkParseOutputCommands.Location = new System.Drawing.Point(16, 96);
			this.chkParseOutputCommands.Name = "chkParseOutputCommands";
			this.chkParseOutputCommands.Size = new System.Drawing.Size(269, 19);
			this.chkParseOutputCommands.TabIndex = 3;
			this.chkParseOutputCommands.Text = "Parse Output Commands (e.g. Megabuild.Set)";
			// 
			// chkWordWrap
			// 
			this.chkWordWrap.AutoSize = true;
			this.chkWordWrap.Location = new System.Drawing.Point(16, 72);
			this.chkWordWrap.Name = "chkWordWrap";
			this.chkWordWrap.Size = new System.Drawing.Size(174, 19);
			this.chkWordWrap.TabIndex = 2;
			this.chkWordWrap.Text = "Word Wrap Output Window";
			// 
			// chkClearOutput
			// 
			this.chkClearOutput.AutoSize = true;
			this.chkClearOutput.Location = new System.Drawing.Point(16, 48);
			this.chkClearOutput.Name = "chkClearOutput";
			this.chkClearOutput.Size = new System.Drawing.Size(161, 19);
			this.chkClearOutput.TabIndex = 1;
			this.chkClearOutput.Text = "Clear Output Before Build";
			// 
			// tabVariables
			// 
			this.tabVariables.Controls.Add(this.lblPercent2);
			this.tabVariables.Controls.Add(this.lblPercent1);
			this.tabVariables.Controls.Add(this.edtValue);
			this.tabVariables.Controls.Add(this.lblValue);
			this.tabVariables.Controls.Add(this.edtName);
			this.tabVariables.Controls.Add(this.lblName);
			this.tabVariables.Controls.Add(this.btnDelete);
			this.tabVariables.Controls.Add(this.btnAdd);
			this.tabVariables.Controls.Add(this.lstVariables);
			this.tabVariables.Location = new System.Drawing.Point(4, 24);
			this.tabVariables.Name = "tabVariables";
			this.tabVariables.Size = new System.Drawing.Size(419, 372);
			this.tabVariables.TabIndex = 2;
			this.tabVariables.Text = "Variables";
			this.tabVariables.UseVisualStyleBackColor = true;
			// 
			// lblPercent2
			// 
			this.lblPercent2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPercent2.AutoSize = true;
			this.lblPercent2.Location = new System.Drawing.Point(298, 286);
			this.lblPercent2.Name = "lblPercent2";
			this.lblPercent2.Size = new System.Drawing.Size(17, 15);
			this.lblPercent2.TabIndex = 8;
			this.lblPercent2.Text = "%";
			// 
			// lblPercent1
			// 
			this.lblPercent1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPercent1.AutoSize = true;
			this.lblPercent1.Location = new System.Drawing.Point(64, 286);
			this.lblPercent1.Name = "lblPercent1";
			this.lblPercent1.Size = new System.Drawing.Size(17, 15);
			this.lblPercent1.TabIndex = 7;
			this.lblPercent1.Text = "%";
			// 
			// edtValue
			// 
			this.edtValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.edtValue.Location = new System.Drawing.Point(86, 316);
			this.edtValue.Name = "edtValue";
			this.edtValue.Size = new System.Drawing.Size(207, 23);
			this.edtValue.TabIndex = 4;
			this.edtValue.TextChanged += new System.EventHandler(this.Value_TextChanged);
			// 
			// lblValue
			// 
			this.lblValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblValue.AutoSize = true;
			this.lblValue.Location = new System.Drawing.Point(14, 321);
			this.lblValue.Name = "lblValue";
			this.lblValue.Size = new System.Drawing.Size(38, 15);
			this.lblValue.TabIndex = 3;
			this.lblValue.Text = "Value:";
			// 
			// edtName
			// 
			this.edtName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.edtName.Location = new System.Drawing.Point(86, 282);
			this.edtName.Name = "edtName";
			this.edtName.Size = new System.Drawing.Size(207, 23);
			this.edtName.TabIndex = 2;
			this.edtName.TextChanged += new System.EventHandler(this.Name_TextChanged);
			// 
			// lblName
			// 
			this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(14, 287);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(42, 15);
			this.lblName.TabIndex = 1;
			this.lblName.Text = "Name:";
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Location = new System.Drawing.Point(341, 316);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(62, 29);
			this.btnDelete.TabIndex = 6;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.Click += new System.EventHandler(this.Delete_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.Location = new System.Drawing.Point(341, 277);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(62, 28);
			this.btnAdd.TabIndex = 5;
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
            this.colValue});
			this.lstVariables.FullRowSelect = true;
			this.lstVariables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstVariables.HideSelection = false;
			this.lstVariables.Location = new System.Drawing.Point(14, 15);
			this.lstVariables.MultiSelect = false;
			this.lstVariables.Name = "lstVariables";
			this.lstVariables.Size = new System.Drawing.Size(388, 249);
			this.lstVariables.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lstVariables.TabIndex = 0;
			this.lstVariables.UseCompatibleStateImageBehavior = false;
			this.lstVariables.View = System.Windows.Forms.View.Details;
			this.lstVariables.SelectedIndexChanged += new System.EventHandler(this.Variables_SelectedIndexChanged);
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.Width = 160;
			// 
			// colValue
			// 
			this.colValue.Text = "Value";
			this.colValue.Width = 160;
			// 
			// chkOutputWindowOnRight
			// 
			this.chkOutputWindowOnRight.AutoSize = true;
			this.chkOutputWindowOnRight.Location = new System.Drawing.Point(16, 24);
			this.chkOutputWindowOnRight.Name = "chkOutputWindowOnRight";
			this.chkOutputWindowOnRight.Size = new System.Drawing.Size(215, 19);
			this.chkOutputWindowOnRight.TabIndex = 0;
			this.chkOutputWindowOnRight.Text = "Show Output Window On The Right";
			// 
			// ApplicationOptionsDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(457, 470);
			this.Controls.Add(this.TabCtrl);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ApplicationOptionsDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "MegaBuild Options";
			this.Load += new System.EventHandler(this.ApplicationOptionsDlg_Load);
			this.TabCtrl.ResumeLayout(false);
			this.tabApplication.ResumeLayout(false);
			this.grpGeneral.ResumeLayout(false);
			this.grpGeneral.PerformLayout();
			this.grpOutput.ResumeLayout(false);
			this.grpOutput.PerformLayout();
			this.tabVariables.ResumeLayout(false);
			this.tabVariables.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private CheckBox chkShowProgressInTaskbar;
		private Label timestampLabel;
		private ComboBox timestampFormat;
		private CheckBox chkOutputWindowOnRight;
	}
}


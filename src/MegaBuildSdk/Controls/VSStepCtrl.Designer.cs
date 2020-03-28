using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Menees;
using Menees.Windows.Forms;
namespace MegaBuild
{
	internal sealed partial class VSStepCtrl
	{
		private System.Windows.Forms.Label lblSolution;
		private System.Windows.Forms.Label lblConfigurations;
		private System.Windows.Forms.ComboBox cbAction;
		private System.Windows.Forms.Label lblAction;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox edtSolution;
		private System.Windows.Forms.OpenFileDialog OpenDlg;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ComboBox cbVersion;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.ComboBox cbWindowState;
		private System.Windows.Forms.Label lblWindowState;
		private System.Windows.Forms.Label lblArguments;
		private System.Windows.Forms.CheckBox chkRedirectStreams;
		private System.Windows.Forms.TextBox edtDevEnvArgs;
		private System.Windows.Forms.ListView lstConfigurations;
		private System.Windows.Forms.ColumnHeader colMain;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblConfigurations = new System.Windows.Forms.Label();
			this.cbAction = new System.Windows.Forms.ComboBox();
			this.lblAction = new System.Windows.Forms.Label();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.edtSolution = new System.Windows.Forms.TextBox();
			this.lblSolution = new System.Windows.Forms.Label();
			this.OpenDlg = new System.Windows.Forms.OpenFileDialog();
			this.cbVersion = new System.Windows.Forms.ComboBox();
			this.lblVersion = new System.Windows.Forms.Label();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.lblArguments = new System.Windows.Forms.Label();
			this.edtDevEnvArgs = new System.Windows.Forms.TextBox();
			this.cbWindowState = new System.Windows.Forms.ComboBox();
			this.lblWindowState = new System.Windows.Forms.Label();
			this.chkRedirectStreams = new System.Windows.Forms.CheckBox();
			this.lstConfigurations = new System.Windows.Forms.ListView();
			this.colMain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ConfigMover = new MegaBuild.ListViewItemMover();
			this.SuspendLayout();
			// 
			// lblConfigurations
			// 
			this.lblConfigurations.AutoSize = true;
			this.lblConfigurations.Location = new System.Drawing.Point(12, 128);
			this.lblConfigurations.Name = "lblConfigurations";
			this.lblConfigurations.Size = new System.Drawing.Size(89, 15);
			this.lblConfigurations.TabIndex = 7;
			this.lblConfigurations.Text = "Configurations:";
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
			this.cbAction.Location = new System.Drawing.Point(104, 60);
			this.cbAction.Name = "cbAction";
			this.cbAction.Size = new System.Drawing.Size(224, 23);
			this.cbAction.TabIndex = 4;
			// 
			// lblAction
			// 
			this.lblAction.AutoSize = true;
			this.lblAction.Location = new System.Drawing.Point(12, 64);
			this.lblAction.Name = "lblAction";
			this.lblAction.Size = new System.Drawing.Size(45, 15);
			this.lblAction.TabIndex = 3;
			this.lblAction.Text = "Action:";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(340, 27);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(28, 24);
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "...";
			this.btnBrowse.Click += new System.EventHandler(this.Browse_Click);
			// 
			// edtSolution
			// 
			this.edtSolution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.edtSolution.Location = new System.Drawing.Point(12, 28);
			this.edtSolution.Name = "edtSolution";
			this.edtSolution.Size = new System.Drawing.Size(316, 23);
			this.edtSolution.TabIndex = 1;
			// 
			// lblSolution
			// 
			this.lblSolution.AutoSize = true;
			this.lblSolution.Location = new System.Drawing.Point(12, 8);
			this.lblSolution.Name = "lblSolution";
			this.lblSolution.Size = new System.Drawing.Size(54, 15);
			this.lblSolution.TabIndex = 0;
			this.lblSolution.Text = "Solution:";
			// 
			// OpenDlg
			// 
			this.OpenDlg.DefaultExt = "sln";
			this.OpenDlg.Filter = "Solution Files (*.sln)|*.sln|All Files (*.*)|*.*";
			this.OpenDlg.Title = "Select Solution";
			// 
			// cbVersion
			// 
			this.cbVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.cbVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVersion.Location = new System.Drawing.Point(104, 92);
			this.cbVersion.Name = "cbVersion";
			this.cbVersion.Size = new System.Drawing.Size(224, 23);
			this.cbVersion.TabIndex = 6;
			// 
			// lblVersion
			// 
			this.lblVersion.AutoSize = true;
			this.lblVersion.Location = new System.Drawing.Point(12, 96);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(49, 15);
			this.lblVersion.TabIndex = 5;
			this.lblVersion.Text = "Version:";
			// 
			// btnRefresh
			// 
			this.btnRefresh.Location = new System.Drawing.Point(12, 148);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(72, 24);
			this.btnRefresh.TabIndex = 8;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.Click += new System.EventHandler(this.Refresh_Click);
			// 
			// lblArguments
			// 
			this.lblArguments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblArguments.AutoSize = true;
			this.lblArguments.Location = new System.Drawing.Point(12, 252);
			this.lblArguments.Name = "lblArguments";
			this.lblArguments.Size = new System.Drawing.Size(76, 15);
			this.lblArguments.TabIndex = 13;
			this.lblArguments.Text = "DevEnv Args:";
			// 
			// edtDevEnvArgs
			// 
			this.edtDevEnvArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.edtDevEnvArgs.Location = new System.Drawing.Point(104, 248);
			this.edtDevEnvArgs.Name = "edtDevEnvArgs";
			this.edtDevEnvArgs.Size = new System.Drawing.Size(224, 23);
			this.edtDevEnvArgs.TabIndex = 14;
			// 
			// cbWindowState
			// 
			this.cbWindowState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.cbWindowState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbWindowState.Items.AddRange(new object[] {
			"Normal",
			"Hidden",
			"Minimized",
			"Maximized"});
			this.cbWindowState.Location = new System.Drawing.Point(104, 216);
			this.cbWindowState.Name = "cbWindowState";
			this.cbWindowState.Size = new System.Drawing.Size(224, 23);
			this.cbWindowState.TabIndex = 12;
			// 
			// lblWindowState
			// 
			this.lblWindowState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblWindowState.AutoSize = true;
			this.lblWindowState.Location = new System.Drawing.Point(12, 220);
			this.lblWindowState.Name = "lblWindowState";
			this.lblWindowState.Size = new System.Drawing.Size(83, 15);
			this.lblWindowState.TabIndex = 11;
			this.lblWindowState.Text = "Window State:";
			// 
			// chkRedirectStreams
			// 
			this.chkRedirectStreams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkRedirectStreams.AutoSize = true;
			this.chkRedirectStreams.Location = new System.Drawing.Point(12, 280);
			this.chkRedirectStreams.Name = "chkRedirectStreams";
			this.chkRedirectStreams.Size = new System.Drawing.Size(265, 19);
			this.chkRedirectStreams.TabIndex = 15;
			this.chkRedirectStreams.Text = "Redirect Console Streams To Output Window";
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
			this.lstConfigurations.Location = new System.Drawing.Point(104, 124);
			this.lstConfigurations.Name = "lstConfigurations";
			this.lstConfigurations.Size = new System.Drawing.Size(224, 80);
			this.lstConfigurations.TabIndex = 9;
			this.lstConfigurations.UseCompatibleStateImageBehavior = false;
			this.lstConfigurations.View = System.Windows.Forms.View.Details;
			// 
			// colMain
			// 
			this.colMain.Text = "";
			// 
			// ConfigMover
			// 
			this.ConfigMover.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.ConfigMover.ListView = this.lstConfigurations;
			this.ConfigMover.Location = new System.Drawing.Point(340, 124);
			this.ConfigMover.Name = "ConfigMover";
			this.ConfigMover.Size = new System.Drawing.Size(28, 80);
			this.ConfigMover.TabIndex = 10;
			// 
			// VSStepCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.Controls.Add(this.ConfigMover);
			this.Controls.Add(this.lstConfigurations);
			this.Controls.Add(this.chkRedirectStreams);
			this.Controls.Add(this.cbWindowState);
			this.Controls.Add(this.lblWindowState);
			this.Controls.Add(this.edtDevEnvArgs);
			this.Controls.Add(this.lblArguments);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.lblVersion);
			this.Controls.Add(this.cbVersion);
			this.Controls.Add(this.lblConfigurations);
			this.Controls.Add(this.cbAction);
			this.Controls.Add(this.lblAction);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.edtSolution);
			this.Controls.Add(this.lblSolution);
			this.Name = "VSStepCtrl";
			this.Size = new System.Drawing.Size(380, 314);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}


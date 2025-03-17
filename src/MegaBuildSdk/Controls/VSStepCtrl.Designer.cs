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
			this.lblConfigurations = new Label();
			this.cbAction = new ComboBox();
			this.lblAction = new Label();
			this.btnBrowse = new Button();
			this.edtSolution = new TextBox();
			this.lblSolution = new Label();
			this.OpenDlg = new OpenFileDialog();
			this.cbVersion = new ComboBox();
			this.lblVersion = new Label();
			this.btnRefresh = new Button();
			this.lblArguments = new Label();
			this.edtDevEnvArgs = new TextBox();
			this.cbWindowState = new ComboBox();
			this.lblWindowState = new Label();
			this.chkRedirectStreams = new CheckBox();
			this.lstConfigurations = new ListView();
			this.colMain = new ColumnHeader();
			this.ConfigMover = new ListViewItemMover();
			this.SuspendLayout();
			// 
			// lblConfigurations
			// 
			this.lblConfigurations.AutoSize = true;
			this.lblConfigurations.Location = new Point(12, 128);
			this.lblConfigurations.Name = "lblConfigurations";
			this.lblConfigurations.Size = new Size(89, 15);
			this.lblConfigurations.TabIndex = 7;
			this.lblConfigurations.Text = "Configurations:";
			// 
			// cbAction
			// 
			this.cbAction.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.cbAction.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbAction.Items.AddRange(new object[] { "Build", "Rebuild", "Clean", "Deploy" });
			this.cbAction.Location = new Point(104, 60);
			this.cbAction.Name = "cbAction";
			this.cbAction.Size = new Size(224, 23);
			this.cbAction.TabIndex = 4;
			// 
			// lblAction
			// 
			this.lblAction.AutoSize = true;
			this.lblAction.Location = new Point(12, 64);
			this.lblAction.Name = "lblAction";
			this.lblAction.Size = new Size(45, 15);
			this.lblAction.TabIndex = 3;
			this.lblAction.Text = "Action:";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.btnBrowse.Location = new Point(340, 27);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new Size(28, 24);
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "...";
			this.btnBrowse.Click += this.Browse_Click;
			// 
			// edtSolution
			// 
			this.edtSolution.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.edtSolution.Location = new Point(12, 28);
			this.edtSolution.Name = "edtSolution";
			this.edtSolution.Size = new Size(316, 23);
			this.edtSolution.TabIndex = 1;
			// 
			// lblSolution
			// 
			this.lblSolution.AutoSize = true;
			this.lblSolution.Location = new Point(12, 8);
			this.lblSolution.Name = "lblSolution";
			this.lblSolution.Size = new Size(54, 15);
			this.lblSolution.TabIndex = 0;
			this.lblSolution.Text = "Solution:";
			// 
			// OpenDlg
			// 
			this.OpenDlg.DefaultExt = "sln";
			this.OpenDlg.Filter = "Solution Files (*.sln;*.slnx)|*.sln;*.slnx|All Files (*.*)|*.*";
			this.OpenDlg.Title = "Select Solution";
			// 
			// cbVersion
			// 
			this.cbVersion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.cbVersion.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbVersion.Location = new Point(104, 92);
			this.cbVersion.Name = "cbVersion";
			this.cbVersion.Size = new Size(224, 23);
			this.cbVersion.TabIndex = 6;
			// 
			// lblVersion
			// 
			this.lblVersion.AutoSize = true;
			this.lblVersion.Location = new Point(12, 96);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new Size(48, 15);
			this.lblVersion.TabIndex = 5;
			this.lblVersion.Text = "Version:";
			// 
			// btnRefresh
			// 
			this.btnRefresh.Location = new Point(12, 148);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new Size(72, 24);
			this.btnRefresh.TabIndex = 8;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.Click += this.Refresh_Click;
			// 
			// lblArguments
			// 
			this.lblArguments.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.lblArguments.AutoSize = true;
			this.lblArguments.Location = new Point(12, 252);
			this.lblArguments.Name = "lblArguments";
			this.lblArguments.Size = new Size(76, 15);
			this.lblArguments.TabIndex = 13;
			this.lblArguments.Text = "DevEnv Args:";
			// 
			// edtDevEnvArgs
			// 
			this.edtDevEnvArgs.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.edtDevEnvArgs.Location = new Point(104, 248);
			this.edtDevEnvArgs.Name = "edtDevEnvArgs";
			this.edtDevEnvArgs.Size = new Size(224, 23);
			this.edtDevEnvArgs.TabIndex = 14;
			// 
			// cbWindowState
			// 
			this.cbWindowState.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.cbWindowState.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbWindowState.Items.AddRange(new object[] { "Normal", "Hidden", "Minimized", "Maximized" });
			this.cbWindowState.Location = new Point(104, 216);
			this.cbWindowState.Name = "cbWindowState";
			this.cbWindowState.Size = new Size(224, 23);
			this.cbWindowState.TabIndex = 12;
			// 
			// lblWindowState
			// 
			this.lblWindowState.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.lblWindowState.AutoSize = true;
			this.lblWindowState.Location = new Point(12, 220);
			this.lblWindowState.Name = "lblWindowState";
			this.lblWindowState.Size = new Size(83, 15);
			this.lblWindowState.TabIndex = 11;
			this.lblWindowState.Text = "Window State:";
			// 
			// chkRedirectStreams
			// 
			this.chkRedirectStreams.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.chkRedirectStreams.AutoSize = true;
			this.chkRedirectStreams.Location = new Point(12, 280);
			this.chkRedirectStreams.Name = "chkRedirectStreams";
			this.chkRedirectStreams.Size = new Size(264, 19);
			this.chkRedirectStreams.TabIndex = 15;
			this.chkRedirectStreams.Text = "Redirect Console Streams To Output Window";
			// 
			// lstConfigurations
			// 
			this.lstConfigurations.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.lstConfigurations.CheckBoxes = true;
			this.lstConfigurations.Columns.AddRange(new ColumnHeader[] { this.colMain });
			this.lstConfigurations.HeaderStyle = ColumnHeaderStyle.None;
			this.lstConfigurations.Location = new Point(104, 124);
			this.lstConfigurations.Name = "lstConfigurations";
			this.lstConfigurations.Size = new Size(224, 80);
			this.lstConfigurations.TabIndex = 9;
			this.lstConfigurations.UseCompatibleStateImageBehavior = false;
			this.lstConfigurations.View = View.Details;
			// 
			// colMain
			// 
			this.colMain.Text = "";
			// 
			// ConfigMover
			// 
			this.ConfigMover.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			this.ConfigMover.Font = new Font("Segoe UI", 9F);
			this.ConfigMover.ListView = this.lstConfigurations;
			this.ConfigMover.Location = new Point(340, 124);
			this.ConfigMover.Name = "ConfigMover";
			this.ConfigMover.Size = new Size(28, 80);
			this.ConfigMover.TabIndex = 10;
			// 
			// VSStepCtrl
			// 
			this.AutoScaleDimensions = new SizeF(7F, 15F);
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
			this.Size = new Size(380, 314);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}


using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Menees.Windows.Forms;
namespace MegaBuild
{
	internal sealed partial class ExecStepCtrl
	{
		private System.Windows.Forms.GroupBox grpExecSettings;
		private System.Windows.Forms.CheckBox chkIgnoreFailure;
		private System.Windows.Forms.CheckBox chkWaitForCompletion;
		private System.Windows.Forms.GroupBox grpConditions;
		private System.Windows.Forms.CheckBox chkOnlyIfParentSucceeded;
		private System.Windows.Forms.CheckBox chkPromptFirst;
		private System.Windows.Forms.CheckBox chkTimeout;
		private System.Windows.Forms.NumericUpDown numTimeout;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private CheckBox chkRunAsAdministrator;

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
			this.grpExecSettings = new System.Windows.Forms.GroupBox();
			this.chkRunAsAdministrator = new System.Windows.Forms.CheckBox();
			this.numTimeout = new System.Windows.Forms.NumericUpDown();
			this.chkTimeout = new System.Windows.Forms.CheckBox();
			this.chkIgnoreFailure = new System.Windows.Forms.CheckBox();
			this.chkWaitForCompletion = new System.Windows.Forms.CheckBox();
			this.grpConditions = new System.Windows.Forms.GroupBox();
			this.chkOnlyIfParentSucceeded = new System.Windows.Forms.CheckBox();
			this.chkPromptFirst = new System.Windows.Forms.CheckBox();
			this.grpExecSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
			this.grpConditions.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpExecSettings
			// 
			this.grpExecSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpExecSettings.Controls.Add(this.chkRunAsAdministrator);
			this.grpExecSettings.Controls.Add(this.numTimeout);
			this.grpExecSettings.Controls.Add(this.chkTimeout);
			this.grpExecSettings.Controls.Add(this.chkIgnoreFailure);
			this.grpExecSettings.Controls.Add(this.chkWaitForCompletion);
			this.grpExecSettings.Location = new System.Drawing.Point(12, 8);
			this.grpExecSettings.Name = "grpExecSettings";
			this.grpExecSettings.Size = new System.Drawing.Size(356, 136);
			this.grpExecSettings.TabIndex = 0;
			this.grpExecSettings.TabStop = false;
			this.grpExecSettings.Text = "Settings";
			// 
			// chkRunAsAdministrator
			// 
			this.chkRunAsAdministrator.AutoSize = true;
			this.chkRunAsAdministrator.Location = new System.Drawing.Point(16, 104);
			this.chkRunAsAdministrator.Name = "chkRunAsAdministrator";
			this.chkRunAsAdministrator.Size = new System.Drawing.Size(168, 19);
			this.chkRunAsAdministrator.TabIndex = 5;
			this.chkRunAsAdministrator.Text = "Require Administrator Role";
			// 
			// numTimeout
			// 
			this.numTimeout.Location = new System.Drawing.Point(256, 48);
			this.numTimeout.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
			this.numTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numTimeout.Name = "numTimeout";
			this.numTimeout.Size = new System.Drawing.Size(48, 23);
			this.numTimeout.TabIndex = 2;
			this.numTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numTimeout.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// chkTimeout
			// 
			this.chkTimeout.AutoSize = true;
			this.chkTimeout.Location = new System.Drawing.Point(36, 48);
			this.chkTimeout.Name = "chkTimeout";
			this.chkTimeout.Size = new System.Drawing.Size(221, 19);
			this.chkTimeout.TabIndex = 1;
			this.chkTimeout.Text = "Timeout After N Minutes, where N = ";
			this.chkTimeout.CheckedChanged += new System.EventHandler(this.Timeout_CheckedChanged);
			// 
			// chkIgnoreFailure
			// 
			this.chkIgnoreFailure.AutoSize = true;
			this.chkIgnoreFailure.Location = new System.Drawing.Point(16, 76);
			this.chkIgnoreFailure.Name = "chkIgnoreFailure";
			this.chkIgnoreFailure.Size = new System.Drawing.Size(98, 19);
			this.chkIgnoreFailure.TabIndex = 3;
			this.chkIgnoreFailure.Text = "Ignore Failure";
			// 
			// chkWaitForCompletion
			// 
			this.chkWaitForCompletion.AutoSize = true;
			this.chkWaitForCompletion.Location = new System.Drawing.Point(16, 20);
			this.chkWaitForCompletion.Name = "chkWaitForCompletion";
			this.chkWaitForCompletion.Size = new System.Drawing.Size(136, 19);
			this.chkWaitForCompletion.TabIndex = 0;
			this.chkWaitForCompletion.Text = "Wait For Completion";
			this.chkWaitForCompletion.CheckedChanged += new System.EventHandler(this.WaitForCompletion_CheckedChanged);
			// 
			// grpConditions
			// 
			this.grpConditions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpConditions.Controls.Add(this.chkOnlyIfParentSucceeded);
			this.grpConditions.Controls.Add(this.chkPromptFirst);
			this.grpConditions.Location = new System.Drawing.Point(12, 152);
			this.grpConditions.Name = "grpConditions";
			this.grpConditions.Size = new System.Drawing.Size(356, 80);
			this.grpConditions.TabIndex = 1;
			this.grpConditions.TabStop = false;
			this.grpConditions.Text = "Conditions";
			// 
			// chkOnlyIfParentSucceeded
			// 
			this.chkOnlyIfParentSucceeded.AutoSize = true;
			this.chkOnlyIfParentSucceeded.Location = new System.Drawing.Point(16, 48);
			this.chkOnlyIfParentSucceeded.Name = "chkOnlyIfParentSucceeded";
			this.chkOnlyIfParentSucceeded.Size = new System.Drawing.Size(214, 19);
			this.chkOnlyIfParentSucceeded.TabIndex = 1;
			this.chkOnlyIfParentSucceeded.Text = "Only Build If Parent Step Succeeded";
			// 
			// chkPromptFirst
			// 
			this.chkPromptFirst.AutoSize = true;
			this.chkPromptFirst.Location = new System.Drawing.Point(16, 20);
			this.chkPromptFirst.Name = "chkPromptFirst";
			this.chkPromptFirst.Size = new System.Drawing.Size(216, 19);
			this.chkPromptFirst.TabIndex = 0;
			this.chkPromptFirst.Text = "Confirm This Step When Build Starts";
			// 
			// ExecStepCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.Controls.Add(this.grpExecSettings);
			this.Controls.Add(this.grpConditions);
			this.Name = "ExecStepCtrl";
			this.Size = new System.Drawing.Size(380, 314);
			this.grpExecSettings.ResumeLayout(false);
			this.grpExecSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
			this.grpConditions.ResumeLayout(false);
			this.grpConditions.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion
	}
}


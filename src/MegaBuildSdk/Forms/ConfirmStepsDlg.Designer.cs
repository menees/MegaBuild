using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Menees;
using Menees.Windows.Forms;
namespace MegaBuild
{
	internal sealed partial class ConfirmStepsDlg : ExtendedForm
	{
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colDescription;
		private System.Windows.Forms.ColumnHeader colCategory;
		private System.Windows.Forms.Label instructions;
		private Menees.Windows.Forms.ExtendedListView List;
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
			this.List = new Menees.Windows.Forms.ExtendedListView();
			this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.instructions = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// List
			// 
			this.List.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.List.CheckBoxes = true;
			this.List.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colCategory,
            this.colDescription});
			this.List.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.List.Location = new System.Drawing.Point(14, 44);
			this.List.Name = "List";
			this.List.Size = new System.Drawing.Size(551, 184);
			this.List.TabIndex = 0;
			this.List.UseCompatibleStateImageBehavior = false;
			// 
			// colName
			// 
			this.colName.Text = "Name";
			// 
			// colCategory
			// 
			this.colCategory.Text = "Category";
			// 
			// colDescription
			// 
			this.colDescription.Text = "Description";
			this.colDescription.Width = 292;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(474, 247);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(90, 29);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "&Cancel";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(373, 247);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(90, 29);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "&OK";
			// 
			// instructions
			// 
			this.instructions.AutoSize = true;
			this.instructions.Location = new System.Drawing.Point(14, 15);
			this.instructions.Name = "instructions";
			this.instructions.Size = new System.Drawing.Size(328, 15);
			this.instructions.TabIndex = 3;
			this.instructions.Text = "Check each step that should be included in the current build:";
			// 
			// ConfirmStepsDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(582, 293);
			this.Controls.Add(this.instructions);
			this.Controls.Add(this.List);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConfirmStepsDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Confirm Steps";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}


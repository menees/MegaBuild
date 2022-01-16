using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Menees;
using Menees.Windows.Forms;
namespace MegaBuild
{
	internal sealed partial class StepTypeDlg : ExtendedForm
	{
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ColumnHeader colType;
		private System.Windows.Forms.ColumnHeader colDescription;
		private System.ComponentModel.IContainer components = null;
		private Menees.Windows.Forms.ExtendedListView List;

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
			this.List = new Menees.Windows.Forms.ExtendedListView();
			this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(484, 299);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(90, 28);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "&Cancel";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(384, 299);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(90, 28);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "&OK";
			// 
			// List
			// 
			this.List.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.List.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType,
            this.colDescription});
			this.List.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.List.Location = new System.Drawing.Point(14, 20);
			this.List.Name = "List";
			this.List.Size = new System.Drawing.Size(559, 262);
			this.List.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.List.TabIndex = 0;
			this.List.UseCompatibleStateImageBehavior = false;
			this.List.SelectedIndexChanged += new System.EventHandler(this.List_SelectedIndexChanged);
			this.List.DoubleClick += new System.EventHandler(this.List_DoubleClick);
			// 
			// colType
			// 
			this.colType.Text = "Type";
			this.colType.Width = 154;
			// 
			// colDescription
			// 
			this.colDescription.Text = "Description";
			this.colDescription.Width = 403;
			// 
			// StepTypeDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(590, 341);
			this.Controls.Add(this.List);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StepTypeDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Insert Step";
			this.Load += new System.EventHandler(this.StepTypeDlg_Load);
			this.ResumeLayout(false);

		}
		#endregion
	}
}


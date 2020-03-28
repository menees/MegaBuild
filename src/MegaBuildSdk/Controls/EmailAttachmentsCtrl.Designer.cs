using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections.Specialized;
using Menees.Windows.Forms;
namespace MegaBuild
{
	internal sealed partial class EmailAttachmentsCtrl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.OpenFileDialog OpenDlg;
		private System.Windows.Forms.ColumnHeader colFileName;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnAddBlank;
		private Menees.Windows.Forms.ExtendedListView lstAttachments;

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
			this.OpenDlg = new System.Windows.Forms.OpenFileDialog();
			this.lstAttachments = new Menees.Windows.Forms.ExtendedListView();
			this.colFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnAddBlank = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// OpenDlg
			// 
			this.OpenDlg.Filter = "All Files (*.*)|*.*";
			this.OpenDlg.Multiselect = true;
			this.OpenDlg.Title = "Select Attachment(s)";
			// 
			// lstAttachments
			// 
			this.lstAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstAttachments.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFileName});
			this.lstAttachments.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstAttachments.LabelEdit = true;
			this.lstAttachments.Location = new System.Drawing.Point(12, 12);
			this.lstAttachments.Name = "lstAttachments";
			this.lstAttachments.Size = new System.Drawing.Size(356, 256);
			this.lstAttachments.TabIndex = 0;
			this.lstAttachments.UseCompatibleStateImageBehavior = false;
			this.lstAttachments.SelectedIndexChanged += new System.EventHandler(this.Attachments_SelectedIndexChanged);
			// 
			// colFileName
			// 
			this.colFileName.Text = "File Name";
			this.colFileName.Width = 75;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.Location = new System.Drawing.Point(200, 280);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(80, 23);
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "Add &File(s)...";
			this.btnAdd.Click += new System.EventHandler(this.Add_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDelete.Location = new System.Drawing.Point(288, 280);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(80, 23);
			this.btnDelete.TabIndex = 3;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.Click += new System.EventHandler(this.Delete_Click);
			// 
			// btnAddBlank
			// 
			this.btnAddBlank.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddBlank.Location = new System.Drawing.Point(112, 280);
			this.btnAddBlank.Name = "btnAddBlank";
			this.btnAddBlank.Size = new System.Drawing.Size(80, 23);
			this.btnAddBlank.TabIndex = 1;
			this.btnAddBlank.Text = "Add &Blank";
			this.btnAddBlank.Click += new System.EventHandler(this.AddBlank_Click);
			// 
			// EmailAttachmentsCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.Controls.Add(this.btnAddBlank);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.lstAttachments);
			this.Name = "EmailAttachmentsCtrl";
			this.Size = new System.Drawing.Size(380, 314);
			this.ResumeLayout(false);

		}
		#endregion
	}
}


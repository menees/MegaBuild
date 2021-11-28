namespace MegaBuild
{
	partial class ExecOutputCtrl
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
			this.chkAutoColorErrorsAndWarnings = new System.Windows.Forms.CheckBox();
			this.edtRegex = new System.Windows.Forms.TextBox();
			this.lblRegex = new System.Windows.Forms.Label();
			this.lblStyle = new System.Windows.Forms.Label();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.lstPatterns = new Menees.Windows.Forms.ExtendedListView();
			this.colStyle = new System.Windows.Forms.ColumnHeader();
			this.colRegex = new System.Windows.Forms.ColumnHeader();
			this.cbStyle = new System.Windows.Forms.ComboBox();
			this.lblCustom = new System.Windows.Forms.Label();
			this.listItemMover = new MegaBuild.ListViewItemMover();
			this.SuspendLayout();
			// 
			// chkAutoColorErrorsAndWarnings
			// 
			this.chkAutoColorErrorsAndWarnings.AutoSize = true;
			this.chkAutoColorErrorsAndWarnings.Location = new System.Drawing.Point(12, 12);
			this.chkAutoColorErrorsAndWarnings.Name = "chkAutoColorErrorsAndWarnings";
			this.chkAutoColorErrorsAndWarnings.Size = new System.Drawing.Size(237, 19);
			this.chkAutoColorErrorsAndWarnings.TabIndex = 0;
			this.chkAutoColorErrorsAndWarnings.Text = "&Highlight Standard Errors And Warnings";
			// 
			// edtRegex
			// 
			this.edtRegex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.edtRegex.Location = new System.Drawing.Point(56, 275);
			this.edtRegex.Name = "edtRegex";
			this.edtRegex.Size = new System.Drawing.Size(310, 23);
			this.edtRegex.TabIndex = 9;
			this.edtRegex.TextChanged += new System.EventHandler(this.Regex_TextChanged);
			// 
			// lblRegex
			// 
			this.lblRegex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblRegex.AutoSize = true;
			this.lblRegex.Location = new System.Drawing.Point(8, 280);
			this.lblRegex.Name = "lblRegex";
			this.lblRegex.Size = new System.Drawing.Size(42, 15);
			this.lblRegex.TabIndex = 8;
			this.lblRegex.Text = "&Regex:";
			// 
			// lblStyle
			// 
			this.lblStyle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblStyle.AutoSize = true;
			this.lblStyle.Location = new System.Drawing.Point(8, 246);
			this.lblStyle.Name = "lblStyle";
			this.lblStyle.Size = new System.Drawing.Size(35, 15);
			this.lblStyle.TabIndex = 6;
			this.lblStyle.Text = "&Style:";
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDelete.Location = new System.Drawing.Point(324, 84);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(48, 23);
			this.btnDelete.TabIndex = 4;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.Click += new System.EventHandler(this.Delete_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.Location = new System.Drawing.Point(324, 60);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(48, 23);
			this.btnAdd.TabIndex = 3;
			this.btnAdd.Text = "&Add";
			this.btnAdd.Click += new System.EventHandler(this.Add_Click);
			// 
			// lstPatterns
			// 
			this.lstPatterns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstPatterns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colStyle,
            this.colRegex});
			this.lstPatterns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstPatterns.Location = new System.Drawing.Point(12, 60);
			this.lstPatterns.Name = "lstPatterns";
			this.lstPatterns.Size = new System.Drawing.Size(304, 167);
			this.lstPatterns.TabIndex = 2;
			this.lstPatterns.UseCompatibleStateImageBehavior = false;
			this.lstPatterns.SelectedIndexChanged += new System.EventHandler(this.Patterns_SelectedIndexChanged);
			// 
			// colStyle
			// 
			this.colStyle.Text = "Style";
			this.colStyle.Width = 100;
			// 
			// colRegex
			// 
			this.colRegex.Text = "Regex";
			this.colRegex.Width = 160;
			// 
			// cbStyle
			// 
			this.cbStyle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbStyle.FormattingEnabled = true;
			this.cbStyle.Location = new System.Drawing.Point(56, 242);
			this.cbStyle.Name = "cbStyle";
			this.cbStyle.Size = new System.Drawing.Size(129, 23);
			this.cbStyle.TabIndex = 7;
			this.cbStyle.SelectedIndexChanged += new System.EventHandler(this.Style_SelectedIndexChanged);
			// 
			// lblCustom
			// 
			this.lblCustom.AutoSize = true;
			this.lblCustom.Location = new System.Drawing.Point(12, 40);
			this.lblCustom.Name = "lblCustom";
			this.lblCustom.Size = new System.Drawing.Size(85, 15);
			this.lblCustom.TabIndex = 1;
			this.lblCustom.Text = "&Custom Styles:";
			// 
			// listItemMover
			// 
			this.listItemMover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.listItemMover.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.listItemMover.ListView = this.lstPatterns;
			this.listItemMover.Location = new System.Drawing.Point(324, 108);
			this.listItemMover.Name = "listItemMover";
			this.listItemMover.Size = new System.Drawing.Size(48, 48);
			this.listItemMover.TabIndex = 5;
			// 
			// ExecOutputCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.listItemMover);
			this.Controls.Add(this.lblCustom);
			this.Controls.Add(this.cbStyle);
			this.Controls.Add(this.edtRegex);
			this.Controls.Add(this.lblRegex);
			this.Controls.Add(this.lblStyle);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.lstPatterns);
			this.Controls.Add(this.chkAutoColorErrorsAndWarnings);
			this.Name = "ExecOutputCtrl";
			this.Size = new System.Drawing.Size(380, 314);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chkAutoColorErrorsAndWarnings;
		private System.Windows.Forms.TextBox edtRegex;
		private System.Windows.Forms.Label lblRegex;
		private System.Windows.Forms.Label lblStyle;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ColumnHeader colStyle;
		private System.Windows.Forms.ColumnHeader colRegex;
		private System.Windows.Forms.ComboBox cbStyle;
		private System.Windows.Forms.Label lblCustom;
		private ListViewItemMover listItemMover;
		private Menees.Windows.Forms.ExtendedListView lstPatterns;
	}
}

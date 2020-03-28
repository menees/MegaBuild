using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Menees.Windows.Forms;
namespace MegaBuild
{
	public partial class ListViewItemMover : ExtendedUserControl
	{
		private System.ComponentModel.IContainer components = null;
		private ImageList Images;
		private MegaBuild.NonSelectButton btnDown;
		private MegaBuild.NonSelectButton btnUp;

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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListViewItemMover));
			this.btnDown = new MegaBuild.NonSelectButton();
			this.btnUp = new MegaBuild.NonSelectButton();
			this.Images = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// btnDown
			// 
			this.btnDown.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnDown.ImageIndex = 1;
			this.btnDown.ImageList = this.Images;
			this.btnDown.Location = new System.Drawing.Point(0, 45);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(24, 23);
			this.btnDown.TabIndex = 1;
			this.btnDown.Click += new System.EventHandler(this.Down_Click);
			// 
			// btnUp
			// 
			this.btnUp.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnUp.ImageIndex = 0;
			this.btnUp.ImageList = this.Images;
			this.btnUp.Location = new System.Drawing.Point(0, 0);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(24, 23);
			this.btnUp.TabIndex = 0;
			this.btnUp.Click += new System.EventHandler(this.Up_Click);
			// 
			// Images
			// 
			this.Images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Images.ImageStream")));
			this.Images.TransparentColor = System.Drawing.Color.Magenta;
			this.Images.Images.SetKeyName(0, "MoveUp.bmp");
			this.Images.Images.SetKeyName(1, "MoveDown.bmp");
			// 
			// ListViewItemMover
			// 
			this.Controls.Add(this.btnDown);
			this.Controls.Add(this.btnUp);
			this.Name = "ListViewItemMover";
			this.Size = new System.Drawing.Size(24, 68);
			this.Load += new System.EventHandler(this.ListViewItemMover_Load);
			this.ResumeLayout(false);

		}
		#endregion
	}
}


namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Windows.Forms;
	using Menees.Windows.Forms;

	#endregion

	internal sealed partial class EmailAttachmentsCtrl : StepEditorControl
	{
		#region Private Data Members

		private bool isModified;
		private EmailStep? step;

		#endregion

		#region Constructors

		public EmailAttachmentsCtrl()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Public Properties

		public override string DisplayName => "Attachments";

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public EmailStep Step
		{
			set
			{
				if (this.step != value)
				{
					this.step = value;

					this.lstAttachments.Items.Clear();
					IList<string> attachments = this.step.Attachments;
					int numAttachments = attachments.Count;
					for (int i = 0; i < numAttachments; i++)
					{
						this.lstAttachments.Items.Add(attachments[i]);
					}

					this.isModified = false;
					this.UpdateControls();
				}
			}
		}

		#endregion

		#region Public Methods

		public override bool OnOk()
		{
			if (this.isModified && this.step != null)
			{
				// Don't call Manager.UnexpandVariables.  We called it when files
				// were added with the Add File button, but if the user manually
				// edited any entries, then they're responsible for them now.
				List<string> attachments = [];
				foreach (ListViewItem item in this.lstAttachments.Items)
				{
					string text = item.Text.Trim();
					if (text.Length > 0)
					{
						attachments.Add(text);
					}
				}

				this.step.Attachments = attachments;
			}

			return true;
		}

		public override void OnReadyToDisplay()
		{
			this.lstAttachments.AutoSizeColumns();
		}

		#endregion

		#region Private Methods

		private void Add_Click(object sender, EventArgs e)
		{
			if (this.OpenDlg.ShowDialog(this) == DialogResult.OK)
			{
				foreach (string fileName in this.OpenDlg.FileNames)
				{
					this.lstAttachments.Items.Add(Manager.CollapseVariables(fileName));
				}

				this.isModified = true;
				this.lstAttachments.AutoSizeColumns();
			}
		}

		private void AddBlank_Click(object sender, EventArgs e)
		{
			ListViewItem item = new();
			this.lstAttachments.Items.Add(item);
			this.isModified = true;
			item.BeginEdit();
		}

		private void Attachments_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateControls();
		}

		private void Delete_Click(object sender, EventArgs e)
		{
			int numSelected = this.lstAttachments.SelectedItems.Count;
			if (numSelected > 0)
			{
				for (int i = numSelected - 1; i >= 0; i--)
				{
					this.lstAttachments.SelectedItems[i].Remove();
				}

				this.isModified = true;
			}
		}

		private void UpdateControls()
		{
			this.btnDelete.Enabled = this.lstAttachments.SelectedItems.Count > 0;
		}

		#endregion
	}
}
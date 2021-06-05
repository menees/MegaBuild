namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	internal sealed partial class StepTypeDlg : ExtendedForm
	{
		#region Constructors

		public StepTypeDlg()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Public Methods

		public bool Execute(IWin32Window owner, out StepTypeInfo resultInfo)
		{
			// Add all the available types to the list
			StepTypeInfo[] typeInfos = Manager.GetStepTypeInfos();

			this.List.BeginUpdate();
			try
			{
				this.List.Items.Clear();
				this.List.SmallImageList = Manager.StepImages;

				foreach (StepTypeInfo info in typeInfos)
				{
					string[] items = { info.Name, info.Description };
					ListViewItem item = new(items, info.ImageIndex)
					{
						Tag = info,
					};
					this.List.Items.Add(item);
				}

				this.List.AutoSizeColumns();
			}
			finally
			{
				this.List.EndUpdate();
			}

			resultInfo = null;
			bool result = false;
			if (this.ShowDialog(owner) == DialogResult.OK)
			{
				resultInfo = (StepTypeInfo)this.List.SelectedItems[0].Tag;
				result = true;
			}

			return result;
		}

		#endregion

		#region Private Methods

		private void List_DoubleClick(object sender, EventArgs e)
		{
			if (this.btnOK.Enabled)
			{
				this.DialogResult = DialogResult.OK;
			}
		}

		private void List_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.btnOK.Enabled = this.List.SelectedIndices.Count > 0;
		}

		private void StepTypeDlg_Load(object sender, EventArgs e)
		{
			// I'm not checking Count here because I know this assembly has several types in it.
			this.List.Items[0].Selected = true;
		}

		#endregion
	}
}
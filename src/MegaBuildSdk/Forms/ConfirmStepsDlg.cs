namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	internal sealed partial class ConfirmStepsDlg : ExtendedForm
	{
		#region Constructors

		public ConfirmStepsDlg()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Public Methods

		public bool Execute(IWin32Window owner, ref ExecutableStep[] buildSteps, ref ExecutableStep[] failureSteps)
		{
			this.List.Items.Clear();
			this.List.SmallImageList = Manager.StepImages;

			this.AddSteps(buildSteps, "Build Step");
			this.AddSteps(failureSteps, "Failure Step");

			this.List.AutoSizeColumns();

			bool result = false;
			if (this.ShowDialog(owner) == DialogResult.OK)
			{
				buildSteps = this.GetCheckedSteps(StepCategory.Build);
				failureSteps = this.GetCheckedSteps(StepCategory.Failure);
				result = true;
			}

			return result;
		}

		#endregion

		#region Private Methods

		private void AddSteps(ExecutableStep[] steps, string type)
		{
			foreach (ExecutableStep step in steps)
			{
				ListViewItem item = new([step.Name, type, step.Description], step.StepTypeInfo.ImageIndex)
				{
					Tag = step,
					Checked = step.IncludeInBuild,
				};
				this.List.Items.Add(item);
			}
		}

		private ExecutableStep[] GetCheckedSteps(StepCategory category)
		{
			List<ExecutableStep> steps = [];
			foreach (ListViewItem item in this.List.CheckedItems)
			{
				ExecutableStep? step = (ExecutableStep?)item.Tag;
				if (step?.Category == category)
				{
					steps.Add(step);
				}
			}

			ExecutableStep[] result = [.. steps];
			return result;
		}

		#endregion
	}
}
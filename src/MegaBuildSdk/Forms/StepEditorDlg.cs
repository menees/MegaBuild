namespace MegaBuild;

#region Using Directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Menees;
using Menees.Windows.Forms;

#endregion

internal sealed partial class StepEditorDlg : ExtendedForm
{
	#region Private Data Members

	private static readonly Type[] TabsToSkipOnEdit = [typeof(GeneralStepCtrl), typeof(ExecStepCtrl), typeof(ExecOutputCtrl)];

	private ICollection<StepEditorControl> editorControls = Array.Empty<StepEditorControl>();

	#endregion

	#region Constructors

	public StepEditorDlg()
	{
		this.InitializeComponent();
	}

	#endregion

	#region Public Methods

	public bool Execute(IWin32Window owner, Step step, bool insertingStep)
	{
		bool result = false;

		// Begin an update batch on the step so that only
		// one StepEdited notification will be sent at the
		// end no matter how many properties get changed.
		step.BeginUpdate();
		try
		{
			bool defaultTabSelected = false;

			// Add a tab for each editor control returned
			this.editorControls = [];
			step.GetStepEditorControls(this.editorControls);
			foreach (StepEditorControl editCtrl in this.editorControls)
			{
				TabPage page = new(editCtrl.DisplayName)
				{
					UseVisualStyleBackColor = true,
				};

				// Set the control to fill the entire page client area
				editCtrl.Dock = DockStyle.Fill;
				page.Controls.Add(editCtrl);

				this.TabCtrl.TabPages.Add(page);

				// When we're editing a step, we typically want to
				// ignore the first couple of tabs.
				if (!defaultTabSelected && !insertingStep && Array.IndexOf(TabsToSkipOnEdit, editCtrl.GetType()) < 0)
				{
					this.TabCtrl.SelectedTab = page;
					defaultTabSelected = true;
				}
			}

			// The OnReadyToDisplay methods will be called in the Load event.
			result = this.ShowDialog(owner) == DialogResult.OK;
		}
		finally
		{
			step.EndUpdate();
		}

		return result;
	}

	#endregion

	#region Private Methods

	private void Cancel_Click(object sender, EventArgs e)
	{
		foreach (StepEditorControl editCtrl in this.editorControls)
		{
			editCtrl.OnCancel();
		}
	}

	private void OK_Click(object sender, EventArgs e)
	{
		foreach (StepEditorControl editCtrl in this.editorControls)
		{
			// If any of the controls return false, then we stop processing immediately.
			if (!editCtrl.OnOk())
			{
				this.DialogResult = DialogResult.None;
				break;
			}
		}
	}

	private void StepEditorDlg_Load(object sender, EventArgs e)
	{
		if (this.TabCtrl.SelectedTab != null && this.TabCtrl.SelectedTab.Controls.Count > 0)
		{
			this.ActiveControl = this.TabCtrl.SelectedTab.Controls[0];
		}

		foreach (StepEditorControl editCtrl in this.editorControls)
		{
			editCtrl.OnReadyToDisplay();
		}
	}

	#endregion
}
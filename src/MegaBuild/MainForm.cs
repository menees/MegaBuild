namespace MegaBuild;

#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Menees;
using Menees.Windows;
using Menees.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Taskbar;

#endregion

internal sealed partial class MainForm : ExtendedForm
{
	#region Private Data Members

	private static readonly Color FailedOrTimedOutColor = Color.FromArgb(255, 75, 75);

	private readonly CommandLineArgs commandLineArgs = new();
	private readonly FindData findData = new();
#pragma warning disable CA1859 // Use concrete types when possible for improved performance. The interface makes the intent clearer.
	private readonly IFindTarget findTarget;
#pragma warning restore CA1859 // Use concrete types when possible for improved performance
	private readonly ToolStripMenuItem firstListContextMenuItem;
	private readonly Stopwatch currentStepTimer = new();
	private readonly OutputQueue outputQueue;
	private readonly AnsiCodeHandler ansiCodeHandler = new();
	private bool loading;

	#endregion

	#region Constructors

	public MainForm()
	{
		this.InitializeComponent();

		this.lstBuildSteps.SmallImageList = Manager.StepImages;
		this.lstFailureSteps.SmallImageList = Manager.StepImages;

		this.firstListContextMenuItem = (ToolStripMenuItem)this.listContextMenu.Items[0];

		this.outputWindow.RemoveLinePrefix = OutputQueue.RemoveTimestampPrefix;
		this.outputWindow.OwnerWindow = this;
		this.findTarget = this.outputWindow;
		this.outputQueue = new(this.outputWindow, this.ansiCodeHandler);
		ExtendedRichTextBox richText = this.outputWindow.Controls.OfType<ExtendedRichTextBox>().First();
		richText.KeyDown += this.RichText_KeyDown;

		this.OnIdle(this, EventArgs.Empty);
	}

	#endregion

	#region Private Properties

	private ExtendedListView ActiveListView => this.IsBuildStep ? this.lstBuildSteps : this.lstFailureSteps;

	private bool IsBuildStep => this.TabCtrl.SelectedIndex == 0;

	private double ProgressPercentage
	{
		set
		{
			int maxValue = this.spProgress.Maximum;
			int currentValue = (int)Math.Round(maxValue * value);
			this.spProgress.Value = currentValue;
			this.UpdateTaskbarProgress();
		}
	}

	private StepCategory SelectedCategory => this.IsBuildStep ? StepCategory.Build : StepCategory.Failure;

	private Step? SelectedStep => GetSelectedStep(this.ActiveListView);

	#endregion

	#region Internal Methods

	internal void OnIdle(object? sender, EventArgs e)
	{
		try
		{
			bool isBuildStep = this.IsBuildStep;
			Step[] buildSteps = GetSelectedSteps(this.lstBuildSteps);
			Step[] steps = isBuildStep ? buildSteps : GetSelectedSteps(this.lstFailureSteps);
			bool building = this.project.Building;
			int numSteps = steps.Length;
			int numBuildSteps = buildSteps.Length;
			int projectBuildSteps = this.project.BuildSteps.Count;
			bool hasOutput = this.outputWindow.HasText;

			EnableComponents(!building, this.mnuNew, this.mnuOpen, this.mnuSave, this.mnuSaveAs, this.tbNew, this.tbOpen, this.tbSave);

			foreach (ToolStripMenuItem mi in this.mnuRecentFiles.DropDownItems)
			{
				mi.Enabled = !building;
			}

			// We can't safely call Project.CanPasteSteps while minimized! It makes an OLE call to the Clipboard that behaves erratically
			// when there's no visible window. It seems to be responsible for the long-standing "stuck minimized" bug. I've even seen it
			// throw an AccessViolationException once in the debugger (while testing for the minimized bug), and that exited the process.
			// Microsoft's Remarks for AccessViolationException indicate that it bypassed my catch(Exception) block below because it
			// occurred outside of CLR's managed memory.
			bool isMinimized = this.WindowState == FormWindowState.Minimized;
			EnableComponents(!building && numSteps > 0, this.mnuCutStep, this.mnuCutStep2, this.mnuCopyStep, this.mnuCopyStep2);
			EnableComponents(!building && numSteps <= 1 && !isMinimized && Project.CanPasteSteps, this.mnuPasteStep, this.mnuPasteStep2);
			EnableComponents(!building && hasOutput, this.mnuFindInOutput, this.mnuFindNextInOutput, this.mnuFindPreviousInOutput);

			EnableComponents(
				this.outputWindow.FindNextHighlightPosition(false, false),
				this.mnuGoToPreviousHighlight,
				this.mnuGoToPreviousHighlight2,
				this.btnGoToPreviousHighlight);
			EnableComponents(
				this.outputWindow.FindNextHighlightPosition(true, false),
				this.mnuGoToNextHighlight,
				this.mnuGoToNextHighlight2,
				this.btnGoToNextHighlight);

			bool canGoToStepOutput = false;
			if (numSteps == 1 && steps[0] is ExecutableStep step)
			{
				canGoToStepOutput = this.outputWindow.FindOutput(step.OutputStartId, false);
			}

			EnableComponents(canGoToStepOutput, this.mnuGoToStepOutput, this.mnuGoToStepOutput2);
			EnableComponents(!building, this.mnuAddStep1, this.mnuAddStep2, this.tbAddStep);
			EnableComponents(!building && numSteps <= 1, this.mnuInsertStep1, this.mnuInsertStep2, this.tbInsertStep);
			bool canEdit = !building && numSteps == 1;
			EnableComponents(canEdit, this.mnuEditStep1, this.mnuEditStep2, this.tbEditStep);
			if (canEdit)
			{
				// Make the text bold to indicate that "Edit Step" is the default item.
				this.mnuEditStep2.Font = new Font(this.mnuAddStep2.Font, FontStyle.Bold);
			}
			else
			{
				this.mnuEditStep2.Font = this.mnuAddStep2.Font;
			}

			EnableComponents(!building && numSteps > 0, this.mnuDeleteStep1, this.mnuDeleteStep2, this.tbDeleteStep);
			EnableComponents(!building && numSteps > 0, this.mnuIncludeInBuild1, this.mnuIncludeInBuild2, this.tbIncludeInBuild);
			EnableComponents(!building && numSteps == 1, this.mnuMoveUp1, this.mnuMoveUp2, this.tbMoveUp);
			EnableComponents(!building && numSteps == 1, this.mnuMoveDown1, this.mnuMoveDown2, this.tbMoveDown);
			EnableComponents(!building && numSteps > 0, this.mnuIndent1, this.mnuIndent2, this.tbIndent);
			EnableComponents(!building && numSteps > 0, this.mnuUnindent1, this.mnuUnindent2, this.tbUnindent);

			EnableComponents(!building && projectBuildSteps > 0, this.mnuBuildProject, this.tbBuildProject);
			EnableComponents(!building && numSteps > 0, this.mnuBuildSelectedStepsOnly1, this.mnuBuildSelectedStepsOnly2, this.tbBuildSelected);
			EnableComponents(
				!building && isBuildStep && numBuildSteps == 1,
				[this.mnuBuildFromSelectedStep1, this.mnuBuildFromSelectedStep2, this.tbBuildFrom]);
			EnableComponents(!building && isBuildStep && numBuildSteps == 1, this.mnuBuildToSelectedStep1, this.mnuBuildToSelectedStep2, this.tbBuildTo);
			EnableComponents(building, this.mnuStopBuild, this.tbStopBuild);

			bool canResetStatuses = !building && (projectBuildSteps > 0 || this.project.FailureSteps.Count > 0);
			EnableComponents(canResetStatuses && hasOutput, this.mnuResetAndClear);
			EnableComponents(canResetStatuses, this.mnuResetStatuses1);
			EnableComponents(!building, this.mnuProjectOptions);

			EnableComponents(this.outputWindow.HasSelection, this.mnuCopyOutput);
			EnableComponents(hasOutput, this.mnuSelectAllOutput);
			EnableComponents(hasOutput, this.mnuClearAll, this.mnuClearOutputWindow);
			EnableComponents(hasOutput, this.mnuSaveOutputAs1, this.mnuSaveOutputAs2);

			this.spProgress.Visible = building;
		}
		catch (Exception ex)
		{
			// We must explicitly call this because Application.Idle
			// doesn't run inside the normal ThreadException protection
			// that the Application provides for the main message pump.
			Application.OnThreadException(ex);
		}
	}

	#endregion

	#region Protected Methods

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			this.components?.Dispose();

			this.outputQueue.Dispose();
		}

		base.Dispose(disposing);
	}

	#endregion

	#region Private Methods

	[Conditional("DEBUG")]
	private static void DebugOutputLine(string line) => Debug.WriteLine("*** " + line + " ***");

	private static void ClearOutputWindow()
	{
		Manager.ClearOutput();
	}

	private static void ClearSubItemText(ListViewItem.ListViewSubItemCollection subItems, int index)
		=> UpdateSubItemText(subItems, index, string.Empty);

	private static void EnableComponents(bool enabled, params Component[] components)
	{
		foreach (Component c in components)
		{
			if (c is ToolStripItem item)
			{
				item.Enabled = enabled;
			}
			else if (c is Control ctrl)
			{
				ctrl.Enabled = enabled;
			}
			else
			{
				throw Exceptions.NewInvalidOperationException(
					$"Internal Error: A component that can't be disabled was passed to {nameof(EnableComponents)}");
			}
		}
	}

	private static string FormatTimeSpan(TimeSpan ts, string prefix)
	{
		const int HoursPerDay = 24;
		return string.Format("{0}{1}:{2:D2}:{3:D2}", prefix, (ts.Days * HoursPerDay) + ts.Hours, ts.Minutes, ts.Seconds);
	}

	private static Step? GetSelectedStep(ListView list)
	{
		Step? result = null;

		// If there are 0 or >1 selections, return null.
		// Then we don't need a separate method like IsSingleStepSelected.
		// Single-select-only methods like Edit, Up, Down, Insert, BuildFrom,
		// and BuildTo all depend on this behavior.
		if (list.SelectedIndices.Count == 1)
		{
			result = GetStepForItem(list.SelectedItems[0]);
		}

		return result;
	}

	private static Step[] GetSelectedSteps(ListView list)
	{
		int numSteps = list.SelectedIndices.Count;
		Step[] steps = new Step[numSteps];
		for (int i = 0; i < numSteps; i++)
		{
			steps[i] = GetStepForItem(list.Items[list.SelectedIndices[i]]);
		}

		return steps;
	}

	private static Step GetStepForItem(ListViewItem item) => (Step?)item.Tag
		?? throw new InvalidOperationException("List view item's Tag should have a Step.");

	private static void OutputMessage(string message, string caption)
	{
		Color defaultColor = SystemColors.WindowText;

		if (!string.IsNullOrEmpty(caption))
		{
			Manager.Output(caption, 0, OutputColors.Heading);
			Manager.Output(Environment.NewLine, 0, defaultColor);
		}

		Manager.Output(message, 0, defaultColor);
		Manager.Output(Environment.NewLine, 0, defaultColor);
	}

	private static void UpdateSubItemText(ListViewItem.ListViewSubItemCollection subItems, int index, string text)
	{
		if (index >= 0)
		{
			ListViewItem.ListViewSubItem subItem = subItems[index];
			if (subItem.Text != text)
			{
				subItem.Text = text;
			}
		}
	}

	private void ApplyOptions()
	{
		this.TopMost = Options.AlwaysOnTop;
		this.outputWindow.WordWrap = Options.WordWrapOutputWindow;
		this.UpdateTaskbarProgress();

		Orientation requiredOrientation = Options.OutputWindowOnRight ? Orientation.Vertical : Orientation.Horizontal;
		if (this.Splitter.Orientation != requiredOrientation)
		{
			this.Splitter.Orientation = requiredOrientation;
			this.ContentsReset();
		}
	}

	private void AutoSizeColumns(ExtendedListView list)
	{
		list.BeginUpdate();
		try
		{
			// Auto-size all of the columns first.
			list.AutoSizeColumns();

			// Leave some padding on Run-Time and maybe Status, so we don't have to auto-size them each time a step changes.
			const int RunTimeColumnPadding = 10;
			const int StatusColumnPadding = 40;
			list.Columns[this.colRunTime.Index].Width += RunTimeColumnPadding;
			ColumnHeader statusColumn = list.Columns[this.colStatus.Index];
			if (!Options.OutputWindowOnRight)
			{
				statusColumn.Width += StatusColumnPadding;
			}
			else
			{
				// When Status is last, AutoSize sizes it to fill. However, padding the RunTime column scoots Status over,
				// which forces a horizontal scroll bar. Try to fix that by shrinking Status or just auto-sizing it again.
				int autoStatusWidth = statusColumn.Width;
				int availableStatusWidth = autoStatusWidth - RunTimeColumnPadding;
				const int MinStatusColumnWidth = 100;
				if (availableStatusWidth >= MinStatusColumnWidth)
				{
					statusColumn.Width = availableStatusWidth;
				}
				else
				{
					list.AutoSizeColumn(statusColumn);
					if (statusColumn.Width < MinStatusColumnWidth)
					{
						statusColumn.Width = MinStatusColumnWidth;
					}
				}
			}
		}
		finally
		{
			list.EndUpdate();
		}
	}

	private void BuildProject(BuildOptions options)
	{
		Step[] steps = new Step[this.project.BuildSteps.Count];
		this.project.BuildSteps.CopyTo(steps);
		this.BuildSteps(steps, options);
	}

	private void BuildSteps(Step[] steps, BuildOptions options)
	{
		// Build the steps.
		// We can't put the active list in a begin/end update
		// because the Confirm dialog can popup and leave the
		// active list unpainted.
		this.project.Build(steps, options, StepExecuteArgs.Empty);
	}

	private void BuildTimer_Tick(object? sender, EventArgs e)
	{
		// Update overall time.
		if (this.project.Building)
		{
			this.UpdateBuildTime();
		}
	}

	private void ContentsReset()
	{
		if (Options.OutputWindowOnRight)
		{
			this.PopulateList(this.lstBuildSteps, this.project.BuildSteps, this.colStep, this.colRunTime, this.colStatus);
			this.PopulateList(this.lstFailureSteps, this.project.FailureSteps, this.colFStep, this.colFRunTime, this.colFStatus);
		}
		else
		{
#pragma warning disable SA1117 // Parameters should be on same line or separate lines
			this.PopulateList(this.lstBuildSteps, this.project.BuildSteps, this.colStep, this.colType, this.colIgnoreFailure, this.colConfirm,
				this.colInfo, this.colRunTime, this.colStatus, this.colDescription);
			this.PopulateList(this.lstFailureSteps, this.project.FailureSteps, this.colFStep, this.colFType, this.colFIgnoreFailure, this.colFConfirm,
				this.colFInfo, this.colFRunTime, this.colFStatus, this.colFDescription);
#pragma warning restore SA1117 // Parameters should be on same line or separate lines
		}
	}

	private void FocusOutputWindow()
	{
		this.outputWindow.Focus();
	}

	private void FormSave_LoadSettings(object? sender, SettingsEventArgs e)
	{
		this.loading = true;

		const int IndentSpaces = 2;
		this.outputWindow.IndentWidth = TextRenderer.MeasureText(new string(' ', IndentSpaces), this.Font).Width;

		// Attach to the manager's Output events so our output stays in sync.
		Manager.OutputAdded += this.OnOutputAdded;
		Manager.OutputCleared += this.OnOutputCleared;

		ISettingsNode baseNode = e.SettingsNode;

		// Load all the type information now.
		try
		{
			Manager.Load(this, baseNode.GetSubNode(nameof(Manager)));
		}
		catch (Exception ex)
		{
			// Don't let a step type loading exception stop everything else.
			// Otherwise, the app doesn't get to load any of its settings,
			// and then when it closes it saves the settings with everything
			// turned off.
			Manager.Output("Exception during Manager.Load: " + ex.Message, 0, OutputColors.Error, true);
		}

		// Load app settings.
		Options.Load(baseNode.GetSubNode(nameof(Options)));
		this.ApplyOptions();

		if (this.WindowState != FormWindowState.Minimized)
		{
			int distance = baseNode.GetValue("SplitterPos", this.Splitter.SplitterDistance);
			if (distance >= this.Splitter.Panel1MinSize && distance <= (this.Splitter.ClientSize.Height - this.Splitter.Panel2MinSize))
			{
				this.Splitter.SplitterDistance = distance;
			}
		}

		// Try to open some type of project here (even a new one) to force
		// callbacks which will set things up correctly (e.g. Caption text).
		bool newProject = true;

		// Check the command line before reloading the last project.
		if (this.commandLineArgs.Project.Length > 0)
		{
			newProject = !this.project.Open(this.commandLineArgs.Project);
		}

		// If the command-line open failed or was skipped, then try the last project.
		if (newProject && Options.ReloadLastProjectAtStartup && this.recentFiles.Count > 0)
		{
			newProject = !this.project.Open(this.recentFiles[0]);
		}

		// Just create a new project if the cmd-line and last projects weren't used.
		if (newProject)
		{
			this.project.New();
		}

		this.ActiveControl = this.lstBuildSteps;

		// Use BeginInvoke to do a "PostMessage" so that the FinishedLoading
		// "event" will fire after any project event messages are processed
		// (e.g. Project_ContentsResetting).
		this.BeginInvoke(new EventHandler(this.OnFinishedLoading), [this, EventArgs.Empty]);
	}

	private void FormSave_SaveSettings(object? sender, SettingsEventArgs e)
	{
		// Only save out the options if we finished loading successfully.
		// If an exception occurred during loading, then the settings probably
		// weren't loaded correctly.  So we don't want to overwrite the good
		// settings that are still stored.
		if (!this.loading)
		{
			ISettingsNode baseNode = e.SettingsNode;
			Manager.Save(baseNode.GetSubNode(nameof(Manager)));
			Options.Save(baseNode.GetSubNode(nameof(Options)));
			baseNode.SetValue("SplitterPos", this.Splitter.SplitterDistance);
		}
	}

	private ExtendedListView GetListForStep(Step step) => step.Category == StepCategory.Build ? this.lstBuildSteps : this.lstFailureSteps;

	private Step[] GetSelectedSteps() => GetSelectedSteps(this.ActiveListView);

	[SuppressMessage("Usage", "CC0022:Should dispose object", Justification = "Caller disposes new menu items.")]
	private void ListContextMenu_Opening(object? sender, CancelEventArgs e)
	{
		// We must call this first to make sure all of the item states are set correctly.  If you right-click off of an item the
		// selection changes and the Popup occurs before OnIdle gets a normal chance to fire.  So we have to force it.
		this.OnIdle(sender, e);

		// Clear out any old Verbs
		while (this.listContextMenu.Items[0] != this.firstListContextMenuItem)
		{
			this.listContextMenu.Items.RemoveAt(0);
		}

		// Add any new verbs, but only if a single step is selected.
		Step? step = this.SelectedStep;
		if (step != null)
		{
			// These verbs will all be enabled, even if we're building.
			// If a verb isn't appropriate to use while building, then
			// it shouldn't be returned.
			string[]? verbs = step.GetCustomVerbs();
			int numVerbs = verbs != null ? verbs.Length : 0;
			if (numVerbs > 0)
			{
				EventHandler eh = new(this.OnCustomVerbClicked);
				for (int i = 0; i < numVerbs; i++)
				{
					ToolStripMenuItem mi = new(verbs![i], null, eh);
					this.listContextMenu.Items.Insert(i, mi);
				}

				// Add a separator
				this.listContextMenu.Items.Insert(numVerbs, new ToolStripSeparator());
			}
		}
	}

	private void Steps_DoubleClick(object? sender, EventArgs e)
	{
		if (!this.project.Building)
		{
			if (this.SelectedStep == null)
			{
				// If multiple items are selected, you can double-click
				// the checks or icons and this event will fire without
				// first clearing the selection.  Then SelectedStep will
				// return null (because of the multi-selection), so we
				// have to make sure we don't add then.
				if (this.ActiveListView.SelectedIndices.Count == 0)
				{
					this.AddStep1_Click(sender, e);
				}
			}
			else
			{
				this.EditStep1_Click(sender, e);
			}
		}
	}

	private void Steps_ItemCheck(object? sender, ItemCheckEventArgs e)
	{
		if (!this.project.Building && sender is ExtendedListView list)
		{
			Step step = GetStepForItem(list.Items[e.Index]);

			// Check for state change because updating IncludeInBuild will cause
			// a StepChanged event which will re-set the item's Checked property.
			bool newCheckState = e.NewValue == CheckState.Checked;
			if (step != null && step.IncludeInBuild != newCheckState)
			{
				step.IncludeInBuild = newCheckState;
			}
		}
	}

	private void MainForm_Closed(object? sender, EventArgs e)
	{
		// Make sure the build is stopped since the app is closing.
		this.StopBuild();
	}

	private void MainForm_Closing(object? sender, CancelEventArgs e)
	{
		if (this.project.CanClose() != DialogResult.Yes)
		{
			e.Cancel = true;
		}
	}

	private void MainForm_DragDrop(object? sender, DragEventArgs e)
	{
		if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
		{
			string[]? files = (string[]?)e.Data.GetData(DataFormats.FileDrop);
			if (files?.Length == 1)
			{
				// Post a message back to ourselves to process these files.
				// It isn't safe to pop up a modal during an OLE/shell drop,
				// which could happen if a "close a modified file" dialog pops up.
				this.BeginInvoke(new Action(() => this.project.Open(files[0])));
			}
		}
	}

#pragma warning disable CC0091 // Use static method. Designer likes instance event handlers.
	private void MainForm_DragEnter(object? sender, DragEventArgs e)
#pragma warning restore CC0091 // Use static method
	{
		if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
		{
			e.Effect = DragDropEffects.Copy;
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}
	}

	private void About_Click(object? sender, EventArgs e)
	{
		WindowsUtility.ShowAboutBox(this, Assembly.GetExecutingAssembly());
	}

	private void AddStep1_Click(object? sender, EventArgs e)
	{
		int index = this.IsBuildStep ? this.project.BuildSteps.Count : this.project.FailureSteps.Count;
		this.project.InsertStep(this, "Add Step", this.SelectedCategory, index);
	}

	private void ApplicationOptions_Click(object? sender, EventArgs e)
	{
		using (ApplicationOptionsDlg dlg = new())
		{
			if (dlg.Execute(this))
			{
				this.ApplyOptions();
			}
		}
	}

	private void BuildFromSelectedStep1_Click(object? sender, EventArgs e)
	{
		Step? step = GetSelectedStep(this.lstBuildSteps);
		if (step != null)
		{
			StepCollection steps = step.CategorySteps;
			int index = step.GetIndex();
			int count = steps.Count - index;
			Step[] stepsToBuild = new Step[count];
			step.CategorySteps.CopyTo(index, stepsToBuild, 0, count);
			this.BuildSteps(stepsToBuild, BuildOptions.None);
		}
	}

	private void BuildProject_Click(object? sender, EventArgs e)
	{
		this.BuildProject(BuildOptions.None);
	}

	private void BuildSelectedStepOnly1_Click(object? sender, EventArgs e)
	{
		this.BuildSteps(GetSelectedSteps(this.ActiveListView), BuildOptions.ForceStepsToBeIncludedInBuild);
	}

	private void BuildToSelectedStep1_Click(object? sender, EventArgs e)
	{
		Step? step = GetSelectedStep(this.lstBuildSteps);
		if (step != null)
		{
			int count = step.GetIndex() + 1;
			Step[] stepsToBuild = new Step[count];
			step.CategorySteps.CopyTo(0, stepsToBuild, 0, count);
			this.BuildSteps(stepsToBuild, BuildOptions.None);
		}
	}

#pragma warning disable CC0091 // Use static method. Designer likes instance event handlers.
	private void ClearOutputWindow_Click(object? sender, EventArgs e)
#pragma warning restore CC0091 // Use static method
	{
		ClearOutputWindow();
	}

	private void CopyOutput_Click(object? sender, EventArgs e)
	{
		if (this.outputWindow.HasSelection)
		{
			IDataObject? beforeCopy = Clipboard.GetDataObject();
			this.outputWindow.Copy();
			IDataObject? afterCopy = Clipboard.GetDataObject();
			if (beforeCopy != afterCopy && afterCopy != null)
			{
				// Replace the special "figure space" and "punctuation space" characters
				// with regular spaces in the plain text on the clipboard.
				DataObject? final = null;
				if (afterCopy.GetDataPresent(DataFormats.UnicodeText))
				{
					final ??= new();
					string input = afterCopy.GetData(DataFormats.UnicodeText)?.ToString() ?? string.Empty;
					string output = OutputQueue.UseStandardSpaces(input);
					final.SetData(DataFormats.UnicodeText, true, output);
				}

				// Preserve the special spaces in the RTF since it will only be pasted into
				// a rich text editor, which can render special spaces correctly.
				if (afterCopy.GetDataPresent(DataFormats.Rtf))
				{
					final ??= new();
					object? rtf = afterCopy.GetData(DataFormats.Rtf);
					final.SetData(DataFormats.Rtf, false, rtf);
				}

				if (final != null)
				{
					Clipboard.SetDataObject(final);
				}
			}
		}
	}

	private void CopyStep_Click(object? sender, EventArgs e)
	{
		// The Ctrl+C shortcut maps to here.
		if (this.outputWindow.IsFocused)
		{
			this.CopyOutput_Click(sender, e);
		}
		else
		{
			Step[] steps = this.GetSelectedSteps();
			if (steps.Length > 0)
			{
				Project.CopySteps(steps);
			}
		}
	}

	private void CutStep_Click(object? sender, EventArgs e)
	{
		Step[] steps = this.GetSelectedSteps();
		if (steps.Length > 0)
		{
			this.project.CutSteps(steps);
		}
	}

	private void DeleteStep1_Click(object? sender, EventArgs e)
	{
		Step[] steps = this.GetSelectedSteps();
		if (steps.Length > 0)
		{
			string message = string.Format(
				"Are you sure you want to delete the selected step{0}?",
				steps.Length == 1 ? (": " + steps[0].Name) : "s");

			if (MessageBox.Show(
				this,
				message,
				"Delete Step",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				foreach (Step step in steps)
				{
					this.project.DeleteStep(step);
				}
			}
		}
	}

	private void EditStep1_Click(object? sender, EventArgs e)
	{
		Step? step = this.SelectedStep;
		if (step != null)
		{
			this.project.EditStep(this, step);
		}
	}

	private void Exit_Click(object? sender, EventArgs e)
	{
		this.Close();
	}

	private void FindInOutput(FindMode findMode)
	{
		this.findData.Caption = this.findTarget.FindCaption;
		if (this.findTarget.Find(this.findData, findMode))
		{
			this.FocusOutputWindow();
		}
	}

	private void FindInOutput_Click(object? sender, EventArgs e)
	{
		this.FindInOutput(FindMode.ShowDialog);
	}

	private void FindNextInOutput_Click(object? sender, EventArgs e)
	{
		this.FindInOutput(FindMode.FindNext);
	}

	private void FindPreviousInOutput_Click(object? sender, EventArgs e)
	{
		this.FindInOutput(FindMode.FindPrevious);
	}

	private void GoToNextHighlight_Click(object? sender, EventArgs e)
	{
		this.outputWindow.FindNextHighlightPosition(true, true);
	}

	private void GoToPreviousHighlight_Click(object? sender, EventArgs e)
	{
		this.outputWindow.FindNextHighlightPosition(false, true);
	}

	private void GoToStepOutput_Click(object? sender, EventArgs e)
	{
		if (this.SelectedStep is ExecutableStep step)
		{
			this.outputWindow.FindOutput(step.OutputStartId, true);
		}
	}

	private void IncludeInBuild1_Click(object? sender, EventArgs e)
	{
		Step[] steps = this.GetSelectedSteps();
		if (steps.Length > 0)
		{
			// Just check the state once.  This is necessary to avoid
			// weirdness with child steps being set both here and implicitly
			// by Step.IncludeInBuild.
			bool includeInBuild = !steps[0].IncludeInBuild;
			foreach (Step step in steps)
			{
				step.IncludeInBuild = includeInBuild;
			}
		}
	}

	private void Indent1_Click(object? sender, EventArgs e)
	{
		Step[] steps = this.GetSelectedSteps();
		foreach (Step step in steps)
		{
			step.Indent();
		}
	}

	private void InsertStep1_Click(object? sender, EventArgs e)
	{
		int index = -1;

		// If there's a single selected step OR if there are no steps
		// then we can insert.  If multiple steps are selected, we can't.
		Step? step = this.SelectedStep;
		if (step != null)
		{
			index = step.GetIndex();
		}
		else if (this.ActiveListView.Items.Count > 0)
		{
			index = 0;
		}

		if (index >= 0)
		{
			this.project.InsertStep(this, "Insert Step", this.SelectedCategory, index);
		}
	}

	private void MoveDown1_Click(object? sender, EventArgs e)
	{
		Step? step = this.SelectedStep;
		if (step != null)
		{
			this.project.MoveDown(step);
		}
	}

	private void MoveUp1_Click(object? sender, EventArgs e)
	{
		Step? step = this.SelectedStep;
		if (step != null)
		{
			this.project.MoveUp(step);
		}
	}

	private void New_Click(object? sender, EventArgs e)
	{
		this.project.New();
	}

	private void Open_Click(object? sender, EventArgs e)
	{
		this.project.Open();
	}

	private void PasteStep_Click(object? sender, EventArgs e)
	{
		if (Project.CanPasteSteps)
		{
			int index = -1;

			// If there's a single selected step paste at its index.
			// Otherwise, paste at the end.
			Step? step = this.SelectedStep;
			if (step != null)
			{
				index = step.GetIndex();
			}

			if (index < 0)
			{
				index = this.ActiveListView.Items.Count;
			}

			this.project.PasteSteps(this.SelectedCategory, index);
		}
	}

	private void ProjectOptions_Click(object? sender, EventArgs e)
	{
		this.project.DisplayOptions(this);
	}

	private void ResetAndClear_Click(object? sender, EventArgs e)
	{
		this.ResetStatus_Click(sender, e);
		this.ClearOutputWindow_Click(sender, e);
	}

	private void ResetStatus_Click(object? sender, EventArgs e)
	{
		this.project.ResetStatuses();
		this.ResetStatusBar();
	}

	private void Save_Click(object? sender, EventArgs e)
	{
		this.project.Save(false);
	}

	private void SaveAs_Click(object? sender, EventArgs e)
	{
		this.project.Save(true);
	}

	private void SaveOutputAs_Click(object? sender, EventArgs e)
	{
		if (this.saveOutputDlg.ShowDialog(this) == DialogResult.OK)
		{
			string fileName = this.saveOutputDlg.FileName;
			bool asRichText = string.Equals(Path.GetExtension(fileName), ".rtf", StringComparison.CurrentCultureIgnoreCase);
			this.outputWindow.SaveContent(fileName, asRichText);
		}
	}

	private void SelectAll_Click(object? sender, EventArgs e)
	{
		this.outputWindow.SelectAll();
	}

	private void SelectAllSteps_Click(object? sender, EventArgs e)
	{
		// The Ctrl+A shortcut maps to here.
		if (this.outputWindow.IsFocused)
		{
			this.outputWindow.SelectAll();
		}
		else
		{
			foreach (ListViewItem item in this.ActiveListView.Items)
			{
				item.Selected = true;
			}
		}
	}

	private void StopBuild_Click(object? sender, EventArgs e)
	{
		this.StopBuild();
	}

	private void Unindent1_Click(object? sender, EventArgs e)
	{
		Step[] steps = this.GetSelectedSteps();
		foreach (Step step in steps)
		{
			step.Unindent();
		}
	}

	private void OnCustomVerbClicked(object? sender, EventArgs e)
	{
		if (sender is ToolStripMenuItem mi && mi.Text.IsNotEmpty())
		{
			Step? step = this.SelectedStep;
			step?.ExecuteCustomVerb(mi.Text);
		}
	}

	private void OnFinishedLoading(object? sender, EventArgs e)
	{
		this.loading = false;

		// Process command-line arguments
		if (this.commandLineArgs.ShowHelp)
		{
			OutputMessage(CommandLineArgs.HelpString, "Command Line Usage");
		}
		else if (this.commandLineArgs.Build)
		{
			// We don't want to pop up a modal confirmation dialog
			// if they're trying to build from the command line.  So
			// we'll auto-confirm the same way that MegaBuildStep
			// does for in-proc builds.
			this.BuildProject(BuildOptions.AutoConfirmSteps);
		}
	}

	private void OnOutputAdded(object? sender, OutputAddedEventArgs e)
	{
		// Command-line builds that use the /exit switch will have output
		// come through after the Close() method has been called.
		if (!this.Splitter.IsDisposed)
		{
			this.outputQueue.Add(e);
		}
	}

	private void OnOutputCleared(object? sender, EventArgs e)
	{
		this.outputQueue.Clear();
		this.outputWindow.Clear();
	}

	private void PopulateList(ExtendedListView list, StepCollection steps, params ColumnHeader[] requiredColumns)
	{
		list.BeginUpdate();
		try
		{
			ListView.ListViewItemCollection items = list.Items;
			items.Clear();

			if (list.Columns.Count != requiredColumns.Length)
			{
				list.Columns.Clear();
				list.Columns.AddRange(requiredColumns);
			}

			int numSteps = steps.Count;
			for (int i = 0; i < numSteps; i++)
			{
				ListViewItem item = new();
				items.Add(item);
				this.UpdateListItem(item, steps[i]);
			}

			this.AutoSizeColumns(list);
		}
		finally
		{
			list.EndUpdate();
		}
	}

	private void Project_BuildFailed(object? sender, EventArgs e)
	{
		// Make the taskbar progress turn red even if there are no failure steps.
		this.UpdateTaskbarProgress();

		// Switch to the Failed Steps view if there are any
		// because they'll be executing in this case.
		if (Options.SwitchTabsOnFailure && this.project.FailureSteps.Count > 0)
		{
			this.TabCtrl.SelectedIndex = 1;
		}
	}

	private void Project_BuildProgress(object? sender, MegaBuild.BuildProgressEventArgs e)
	{
		this.spName.Text = e.Message;
		this.ansiCodeHandler.Reset();

		if (e.UseStepNumbers)
		{
			this.currentStepTimer.Restart();
			this.UpdateBuildTime();
			this.ProgressPercentage = (double)e.CurrentStep / e.TotalSteps;
		}
	}

	private void Project_BuildStarted(object? sender, EventArgs e)
	{
		// Reset status bar
		this.ResetStatusBar();

		// Force an initial update.
		this.UpdateBuildTime();

		// Start the clock.
		this.buildTimer.Enabled = true;

		// Don't allow any list edits during the build.
		this.lstBuildSteps.AllowItemCheck = false;
		this.lstFailureSteps.AllowItemCheck = false;
	}

	private void Project_BuildStarting(object? sender, CancelEventArgs e)
	{
		// Save if necessary.
		if (Options.SaveChangesBeforeBuild && this.project.Save(false) != DialogResult.Yes)
		{
			e.Cancel = true;
		}

		// Clear the output if necessary.
		if (!e.Cancel && Options.ClearOutputBeforeBuild)
		{
			ClearOutputWindow();
		}
	}

	private void Project_BuildStopped(object? sender, EventArgs e)
	{
		// Stop the clock.
		this.buildTimer.Enabled = false;
		this.currentStepTimer.Reset();

		// Force one last status update so the total time is correct.
		this.UpdateBuildTime();

		// If this is the active app, then we can clear the progress now.
		// Otherwise, we'll leave it for MainForm_Activated to clear.
		if (ApplicationInfo.IsActivated)
		{
			this.ProgressPercentage = 0;
		}

		// Reenable item checking.
		this.lstBuildSteps.AllowItemCheck = true;
		this.lstFailureSteps.AllowItemCheck = true;

		// I'm intentionally leaving all other status information.
		// The user can clear it with ResetStatuses when they want to.

		// Exit the app if the command-line arguments say to.
		if (this.commandLineArgs.Build && this.commandLineArgs.Exit)
		{
			this.Close();

			// Set the exit code so a caller can determine if it succeeded or failed.
			Environment.ExitCode = this.project.BuildStatus == BuildStatus.Succeeded ? 0 : 1;
		}
	}

	private void Project_ContentsReset(object? sender, EventArgs e)
	{
		this.ContentsReset();
	}

	private void Project_ContentsResetting(object? sender, EventArgs e)
	{
		// We want any loading errors to still be displayed.
		if (!this.loading)
		{
			ClearOutputWindow();
			this.ResetStatusBar();
		}
	}

	private void Project_DisplayComments(object? sender, EventArgs e)
	{
		if (!Options.NeverShowProjectComments && !this.commandLineArgs.Build)
		{
			OutputMessage(this.project.Comments, "Project Comments");
		}
	}

	private void Project_FileNameSet(object? sender, EventArgs e)
	{
		this.UpdateWindowTitle();
	}

	private void Project_ModifiedChanged(object? sender, EventArgs e)
	{
		this.UpdateWindowTitle();
	}

	private void Project_ProjectStepsChanged(object? sender, MegaBuild.ProjectStepsChangedEventArgs e)
	{
		switch (e.ChangeType)
		{
			case ProjectStepsChangedType.StepInserted:
				this.StepInserted(e.Step, e.NewIndex);
				break;

			case ProjectStepsChangedType.StepEdited:
				this.StepEdited(e.Step, e.OldIndex);
				break;

			case ProjectStepsChangedType.StepDeleted:
				this.StepDeleted(e.Step, e.OldIndex);
				break;

			case ProjectStepsChangedType.StepMoved:
				this.StepMoved(e.Step, e.OldIndex, e.NewIndex);
				break;
		}
	}

	private void Project_RecentFileAdded(object? sender, EventArgs e)
	{
		if (this.IsHandleCreated && TaskbarManager.IsPlatformSupported)
		{
			List<IDisposable> disposables = [];
			try
			{
				// http://visualstudiomagazine.com/articles/2010/07/29/getting-the-jump-on-jump-lists.aspx
				// http://www.developer.com/net/article.php/3850661/Creating-Windows-7-Jump-Lists-With-The-API-Code-Pack-and-Visual-Studio-2008.htm
				JumpList jumpList = JumpList.CreateJumpList();

				// We can't use the known "Recent" category because it requires that the file extension
				// be associated with the current application.  So we'll simulate it using our own category
				// with JumpListLinks instead.
				jumpList.KnownCategoryToDisplay = JumpListKnownCategoryType.Neither;
				JumpListCustomCategory category = new("Recent");
				string exePath = ApplicationInfo.ExecutableFile;
				IconReference iconRef = new(exePath, 0);
				foreach (string fileName in this.recentFiles.Items)
				{
					JumpListLink link = new(TextUtility.EnsureQuotes(exePath), Path.GetFileName(fileName))
					{
						Arguments = TextUtility.EnsureQuotes(fileName),
						IconReference = iconRef,
					};
					category.AddJumpListItems(link);
					disposables.Add(link);
				}

				jumpList.AddCustomCategories(category);
				jumpList.Refresh();
			}
#pragma warning disable CC0004 // Catch block cannot be empty
			catch (UnauthorizedAccessException)
			{
				// The user has turned off Jump List support (Taskbar -> Properties -> Jump Lists).
			}
#pragma warning restore CC0004 // Catch block cannot be empty
#pragma warning disable CC0004 // Catch block cannot be empty
			catch (InvalidOperationException)
			{
				// Sometimes TaskbarManager.get_OwnerHandle will throw with:
				// "A valid active Window is needed to update the Taskbar"
			}
#pragma warning restore CC0004 // Catch block cannot be empty
			finally
			{
				foreach (IDisposable disposable in disposables)
				{
					disposable.Dispose();
				}
			}
		}
	}

	private void RecentFiles_FileClick(object? sender, RecentItemClickEventArgs e)
	{
		if (File.Exists(e.Item))
		{
			this.project.Open(e.Item);
		}
		else
		{
			string message = string.Format("The selected file ({0}) does not exist.  Would you like to remove it from the list?", e.Item);
			if (MessageBox.Show(this, message, "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				this.recentFiles.Remove(e.Item);
			}
		}
	}

	private void ResetStatusBar()
	{
		this.spName.Text = string.Empty;
		this.spTime.Text = string.Empty;
		this.ProgressPercentage = 0;
	}

	private void StepDeleted(Step step, int index)
	{
		ExtendedListView list = this.GetListForStep(step);
		list.Items.RemoveAt(index);
	}

	private void StepEdited(Step step, int index)
	{
		// If a step constructor modifies a property, then we can get an
		// edited notification before the list item is added.  Ignore it.
		if (index >= 0)
		{
			ExtendedListView list = this.GetListForStep(step);
			ListViewItem item = list.Items[index];
			this.UpdateListItem(item, step);
		}
	}

	private void StepInserted(Step step, int index)
	{
		ListViewItem item = new();
		ExtendedListView list = this.GetListForStep(step);
		list.Items.Insert(index, item);
		this.UpdateListItem(item, step);
		this.AutoSizeColumns(list);
	}

	private void StepMoved(Step step, int oldIndex, int newIndex)
	{
		ExtendedListView list = this.GetListForStep(step);

		int lowerIndex = Math.Min(oldIndex, newIndex);
		int upperIndex = Math.Max(oldIndex, newIndex);

		StepCollection steps = step.CategorySteps;
		for (int i = lowerIndex; i <= upperIndex; i++)
		{
			ListViewItem item = list.Items[i];
			this.UpdateListItem(item, steps[i]);
			item.Selected = false;
			item.Focused = false;
		}

		// Select the new item
		list.Items[newIndex].Selected = true;
		list.Items[newIndex].Focused = true;
	}

	private void StopBuild()
	{
		this.project.StopBuild();
	}

	private void TabCtrl_SelectedIndexChanged(object? sender, EventArgs e)
	{
		this.ActiveListView.Focus();
	}

	private void UpdateBuildTime()
	{
		StringBuilder sb = new(FormatTimeSpan(DateTime.UtcNow - this.project.BuildStart, "Build: "));
		if (this.currentStepTimer.IsRunning)
		{
			sb.Append(' ').Append(FormatTimeSpan(this.currentStepTimer.Elapsed, "Step: "));
		}

		this.spTime.Text = sb.ToString();
	}

	private void UpdateListItem(ListViewItem item, Step step)
	{
		Conditions.RequireReference(item.ListView, nameof(item.ListView));

		item.Tag = step;

		ListViewItem.ListViewSubItemCollection subItems = item.SubItems;

		ExtendedListView list = (ExtendedListView)item.ListView;

		// We need to do everything we can to minimize updates to the item.  That will help minimize repaint flicker.
		// The extra comparison code takes no time compared to an unnecessary repaint.
		if (item.ImageIndex != step.StepTypeInfo.ImageIndex)
		{
			item.ImageIndex = step.StepTypeInfo.ImageIndex;
		}

		if (item.Checked != step.IncludeInBuild)
		{
			item.Checked = step.IncludeInBuild;
		}

		bool resizeFirstColumn = false;
		if (item.IndentCount != step.Level)
		{
			item.IndentCount = step.Level;
			resizeFirstColumn = true;
		}

		// Make sure we have a subitem for each column.
		int numColumns = this.lstBuildSteps.Columns.Count;
		while (subItems.Count < numColumns)
		{
			subItems.Add(string.Empty);
		}

		UpdateSubItemText(subItems, this.colStep.Index, step.Name);
		UpdateSubItemText(subItems, this.colType.Index, step.StepTypeInfo.Name);
		UpdateSubItemText(subItems, this.colInfo.Index, step.StepInformation);

		if (step is ExecutableStep executableStep)
		{
			UpdateSubItemText(subItems, this.colIgnoreFailure.Index, executableStep.IgnoreFailure.ToString());
			UpdateSubItemText(subItems, this.colConfirm.Index, executableStep.PromptFirst ? "Yes" : "No");

			if (executableStep.Status != StepStatus.None)
			{
				string time = FormatTimeSpan(executableStep.TotalExecutionTime, string.Empty);
				UpdateSubItemText(subItems, this.colRunTime.Index, time);
				UpdateSubItemText(subItems, this.colStatus.Index, executableStep.Status.ToString());
			}
			else
			{
				ClearSubItemText(subItems, this.colRunTime.Index);
				ClearSubItemText(subItems, this.colStatus.Index);
			}

			// Set BackColor based on Status, etc.
			Color clrNewBackColor;
			switch (executableStep.Status)
			{
				case StepStatus.Canceled:
					clrNewBackColor = Color.MistyRose;
					break;

				case StepStatus.Skipped:
					clrNewBackColor = Color.Gainsboro; // Sort of a light gray
					break;

				case StepStatus.Failed:
				case StepStatus.TimedOut:
					clrNewBackColor = executableStep.IgnoreFailure ? Color.Orange : FailedOrTimedOutColor;
					break;

				case StepStatus.Executing:
					clrNewBackColor = Color.Yellow;
					break;

				default:
					clrNewBackColor = executableStep.InCurrentBuild ? Color.PaleTurquoise : list.BackColor;
					break;
			}

			if (item.BackColor != clrNewBackColor)
			{
				item.BackColor = clrNewBackColor;
			}
		}
		else
		{
			// We have to clear these for non-executable steps because StepMoved may move a Group
			// into an item that used to have an executable step.
			ClearSubItemText(subItems, this.colIgnoreFailure.Index);
			ClearSubItemText(subItems, this.colConfirm.Index);
			ClearSubItemText(subItems, this.colRunTime.Index);
			ClearSubItemText(subItems, this.colStatus.Index);
			if (item.BackColor != list.BackColor)
			{
				item.BackColor = list.BackColor;
			}
		}

		string description = TextUtility.ReplaceControlCharacters(step.Description);
		UpdateSubItemText(subItems, this.colDescription.Index, description);

		// Note: Resizing columns causes tremendous flicker because it repaints the entire control at least twice, sometimes
		// three times (depending on whether the first or second auto-size is bigger.
		if (resizeFirstColumn)
		{
			list.AutoSizeColumn(list.Columns[this.colStep.Index]);
		}
	}

	private void UpdateTaskbarProgress()
	{
		if (this.IsHandleCreated && TaskbarManager.IsPlatformSupported)
		{
			try
			{
				var taskbar = TaskbarManager.Instance;
				TaskbarProgressBarState state = TaskbarProgressBarState.NoProgress;
				if (Options.ShowProgressInTaskbar)
				{
					int currentValue = this.spProgress.Value;
					int maxValue = this.spProgress.Maximum;
					taskbar.SetProgressValue(currentValue, maxValue, this.Handle);
					if (currentValue != 0)
					{
						switch (this.project.BuildStatus)
						{
							case BuildStatus.Failing:
							case BuildStatus.Failed:
								state = TaskbarProgressBarState.Error;
								break;

							default:
								state = TaskbarProgressBarState.Normal;
								break;
						}
					}
				}

				taskbar.SetProgressState(state, this.Handle);
			}
			catch (InvalidOperationException ex)
			{
				// Sometimes TaskbarManager.get_OwnerHandle will throw with:
				// "A valid active Window is needed to update the Taskbar"
				DebugOutputLine(nameof(this.UpdateTaskbarProgress) + " Error: " + ex.Message);
			}
		}
	}

	private void UpdateWindowTitle()
	{
		StringBuilder sb = new();
		sb.Append("MegaBuild - ");
		sb.Append(this.project.Title);

		if (this.project.Modified)
		{
			sb.Append('*');
		}

		if (ApplicationInfo.IsUserRunningAsAdministrator)
		{
			sb.Append(" (Administrator)");
		}

		this.Text = sb.ToString();
	}

	private void MainForm_Activated(object? sender, EventArgs e)
	{
		DebugOutputLine(nameof(this.MainForm_Activated));

		if (!this.project.Building)
		{
			// If this app is inactive when Project_BuildStopped fires, it will leave the taskbar in its last state
			// so users can glance at the taskbar and see whether the build succeeded or failed.  Now that
			// we know the app is active again, we need to clear the build's state from the taskbar.
			this.ProgressPercentage = 0;
		}
	}

	private void RichText_KeyDown(object? sender, KeyEventArgs e)
	{
		if (e.Modifiers == Keys.Control && (e.KeyCode == Keys.C || e.KeyCode == Keys.Insert))
		{
			e.Handled = true;
			this.CopyOutput_Click(sender, e);
		}
	}

	#endregion
}
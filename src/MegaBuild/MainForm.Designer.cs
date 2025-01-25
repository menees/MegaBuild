using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Menees;
using System.Diagnostics;
using System.IO;
using System.Text;
using Menees.Windows.Forms;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
namespace MegaBuild
{
	internal sealed partial class MainForm : ExtendedForm
	{
		private System.Windows.Forms.MenuStrip mainMenuInst;
		private Menees.Windows.Forms.ExtendedToolStrip toolbar;
		private System.Windows.Forms.Panel pnlList;
		private System.Windows.Forms.ImageList images;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.ContextMenuStrip listContextMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuRecentFiles;
		private System.Windows.Forms.ToolStripMenuItem mnuNew;
		private System.Windows.Forms.ToolStripMenuItem mnuOpen;
		private System.Windows.Forms.ToolStripMenuItem mnuSave;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
		private System.Windows.Forms.ToolStripMenuItem mnuExit;
		private System.Windows.Forms.ToolStripMenuItem mnuSteps;
		private System.Windows.Forms.ToolStripMenuItem mnuInsertStep1;
		private System.Windows.Forms.ToolStripMenuItem mnuEditStep1;
		private System.Windows.Forms.ToolStripMenuItem mnuDeleteStep1;
		private System.Windows.Forms.ToolStripMenuItem mnuIncludeInBuild1;
		private System.Windows.Forms.ToolStripMenuItem mnuMoveUp1;
		private System.Windows.Forms.ToolStripMenuItem mnuMoveDown1;
		private System.Windows.Forms.ToolStripMenuItem mnuIndent1;
		private System.Windows.Forms.ToolStripMenuItem mnuUnindent1;
		private System.Windows.Forms.ToolStripMenuItem mnuBuild;
		private System.Windows.Forms.ToolStripMenuItem mnuBuildProject;
		private System.Windows.Forms.ToolStripMenuItem mnuBuildToSelectedStep1;
		private System.Windows.Forms.ToolStripMenuItem mnuBuildFromSelectedStep1;
		private System.Windows.Forms.ToolStripMenuItem mnuStopBuild;
		private System.Windows.Forms.ToolStripMenuItem mnuHelp;
		private System.Windows.Forms.ToolStripMenuItem mnuTools;
		private System.Windows.Forms.ToolStripMenuItem mnuProjectOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuApplicationOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuAbout;
		private System.Windows.Forms.ToolStripMenuItem mnuClearOutputWindow;
		private System.Windows.Forms.ToolStripMenuItem mnuInsertStep2;
		private System.Windows.Forms.ToolStripMenuItem mnuIncludeInBuild2;
		private System.Windows.Forms.ToolStripMenuItem mnuMoveUp2;
		private System.Windows.Forms.ToolStripMenuItem mnuMoveDown2;
		private System.Windows.Forms.ToolStripMenuItem mnuIndent2;
		private System.Windows.Forms.ToolStripMenuItem mnuUnindent2;
		private System.Windows.Forms.ToolStripMenuItem mnuBuildToSelectedStep2;
		private System.Windows.Forms.ToolStripMenuItem mnuBuildFromSelectedStep2;
		private System.Windows.Forms.ToolStripMenuItem mnuEditStep2;
		private System.Windows.Forms.ToolStripMenuItem mnuDeleteStep2;
		private System.Windows.Forms.ToolStripButton tbNew;
		private System.Windows.Forms.ToolStripButton tbSave;
		private System.Windows.Forms.ToolStripSeparator tbSep1;
		private System.Windows.Forms.ToolStripButton tbInsertStep;
		private System.Windows.Forms.ToolStripButton tbEditStep;
		private System.Windows.Forms.ToolStripButton tbDeleteStep;
		private System.Windows.Forms.ToolStripSeparator tbSep2;
		private System.Windows.Forms.ToolStripButton tbIncludeInBuild;
		private System.Windows.Forms.ToolStripButton tbMoveUp;
		private System.Windows.Forms.ToolStripButton tbMoveDown;
		private System.Windows.Forms.ToolStripButton tbIndent;
		private System.Windows.Forms.ToolStripButton tbUnindent;
		private System.Windows.Forms.ToolStripSeparator tbSep3;
		private System.Windows.Forms.ToolStripButton tbBuildProject;
		private System.Windows.Forms.ToolStripButton tbStopBuild;
		private System.Windows.Forms.OpenFileDialog openProjectDlg;
		private System.Windows.Forms.SaveFileDialog saveProjectDlg;
		private System.Windows.Forms.SaveFileDialog saveOutputDlg;
		private System.Windows.Forms.ToolStripMenuItem mnuResetStatuses1;
		private System.Windows.Forms.ToolStripMenuItem mnuAddStep1;
		private System.Windows.Forms.ToolStripMenuItem mnuAddStep2;
		private System.Windows.Forms.ToolStripButton tbAddStep;
		private System.Windows.Forms.TabControl TabCtrl;
		private System.Windows.Forms.TabPage tabBuildSteps;
		private System.Windows.Forms.TabPage tabFailureSteps;
		private System.Windows.Forms.ColumnHeader colStep;
		private System.Windows.Forms.ColumnHeader colType;
		private System.Windows.Forms.ColumnHeader colIgnoreFailure;
		private System.Windows.Forms.ColumnHeader colRunTime;
		private System.Windows.Forms.ColumnHeader colStatus;
		private System.Windows.Forms.ColumnHeader colDescription;
		private System.Windows.Forms.ColumnHeader colFStep;
		private System.Windows.Forms.ColumnHeader colFType;
		private System.Windows.Forms.ColumnHeader colFIgnoreFailure;
		private System.Windows.Forms.ColumnHeader colFRunTime;
		private System.Windows.Forms.ColumnHeader colFStatus;
		private System.Windows.Forms.ColumnHeader colFDescription;
		private System.ComponentModel.IContainer components;

		private System.Windows.Forms.Timer buildTimer;
		private System.Windows.Forms.ToolStripButton tbBuildSelected;
		private System.Windows.Forms.ToolStripButton tbBuildFrom;
		private System.Windows.Forms.ToolStripButton tbBuildTo;
		private System.Windows.Forms.ToolStripSeparator tbSep4;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveOutputAs1;
		private System.Windows.Forms.ToolStripMenuItem mnuBuildSelectedStepsOnly1;
		private System.Windows.Forms.ToolStripMenuItem mnuBuildSelectedStepsOnly2;
		private System.Windows.Forms.ToolStripMenuItem mnuResetAndClear;
		private System.Windows.Forms.ColumnHeader colConfirm;
		private System.Windows.Forms.ColumnHeader colFConfirm;
		private System.Windows.Forms.ColumnHeader colInfo;
		private System.Windows.Forms.ColumnHeader colFInfo;
		private System.Windows.Forms.ToolStripMenuItem mnuEdit;
		private System.Windows.Forms.ToolStripMenuItem mnuCutStep;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyStep;
		private System.Windows.Forms.ToolStripMenuItem mnuPasteStep;
		private System.Windows.Forms.ToolStripMenuItem mnuSelectAllSteps;
		private System.Windows.Forms.ToolStripMenuItem mnuCutStep2;
		private System.Windows.Forms.ToolStripMenuItem mnuCopyStep2;
		private System.Windows.Forms.ToolStripMenuItem mnuPasteStep2;
		private System.Windows.Forms.ToolStripMenuItem mnuFindInOutput;
		private System.Windows.Forms.ToolStripMenuItem mnuFindNextInOutput;
		private System.Windows.Forms.ToolStripMenuItem mnuFindPreviousInOutput;
		private ToolStripSeparator menuItem7;
		private ToolStripSeparator mnuOpenProjectSeparator;
		private ToolStripSeparator menuItem5;
		private ToolStripSeparator menuItem8;
		private ToolStripSeparator menuItem12;
		private ToolStripSeparator menuItem26;
		private ToolStripSeparator menuItem20;
		private ToolStripSeparator menuItem13;
		private ToolStripSeparator menuItem34;
		private ToolStripSeparator menuItem43;
		private ToolStripSeparator menuItem1;
		private StatusStrip status;
		private ToolStripStatusLabel spName;
		private ToolStripStatusLabel spTime;
		private ToolStripProgressBar spProgress;
		private SplitContainer Splitter;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem mnuGoToPreviousHighlight;
		private ToolStripMenuItem mnuGoToNextHighlight;
		private ToolStripButton btnGoToPreviousHighlight;
		private ToolStripButton btnGoToNextHighlight;
		private ToolStripMenuItem mnuCopyOutput;
		private ToolStripMenuItem mnuSelectAllOutput;
		private ToolStripSeparator menuItem6;
		private ToolStripMenuItem mnuSaveOutputAs2;
		private ToolStripMenuItem mnuClearAll;
		private ToolStripSeparator menuItem3;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripMenuItem mnuGoToPreviousHighlight2;
		private ToolStripMenuItem mnuGoToNextHighlight2;
		private ContextMenuStrip outputContextMenu;
		private ToolStripSplitButton tbOpen;
		private ContextMenuStrip recentFilesContextMenu;
		private ToolStripMenuItem mnuGoToStepOutput2;
		private ToolStripSeparator toolStripMenuItem3;
		private ToolStripMenuItem mnuGoToStepOutput;
		private MegaBuild.Project project;
		private Menees.Windows.Forms.RecentItemList recentFiles;
		private Menees.Windows.Forms.FormSaver formSaver;
		private Menees.Windows.Forms.ExtendedListView lstBuildSteps;
		private Menees.Windows.Forms.ExtendedListView lstFailureSteps;
		private OutputWindow outputWindow;

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new Container();
			ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
			this.mainMenuInst = new MenuStrip();
			this.mnuFile = new ToolStripMenuItem();
			this.mnuNew = new ToolStripMenuItem();
			this.mnuOpen = new ToolStripMenuItem();
			this.mnuSave = new ToolStripMenuItem();
			this.mnuSaveAs = new ToolStripMenuItem();
			this.menuItem7 = new ToolStripSeparator();
			this.mnuRecentFiles = new ToolStripMenuItem();
			this.mnuOpenProjectSeparator = new ToolStripSeparator();
			this.mnuOpenProjectFolderInExplorer = new ToolStripMenuItem();
			this.mnuOpenProjectFolderInTerminal = new ToolStripMenuItem();
			this.toolStripMenuItem4 = new ToolStripSeparator();
			this.mnuExit = new ToolStripMenuItem();
			this.mnuEdit = new ToolStripMenuItem();
			this.mnuCutStep = new ToolStripMenuItem();
			this.mnuCopyStep = new ToolStripMenuItem();
			this.mnuPasteStep = new ToolStripMenuItem();
			this.menuItem5 = new ToolStripSeparator();
			this.mnuSelectAllSteps = new ToolStripMenuItem();
			this.menuItem8 = new ToolStripSeparator();
			this.mnuFindInOutput = new ToolStripMenuItem();
			this.mnuFindNextInOutput = new ToolStripMenuItem();
			this.mnuFindPreviousInOutput = new ToolStripMenuItem();
			this.toolStripMenuItem1 = new ToolStripSeparator();
			this.mnuGoToPreviousHighlight = new ToolStripMenuItem();
			this.mnuGoToNextHighlight = new ToolStripMenuItem();
			this.toolStripMenuItem3 = new ToolStripSeparator();
			this.mnuGoToStepOutput = new ToolStripMenuItem();
			this.mnuSteps = new ToolStripMenuItem();
			this.mnuAddStep1 = new ToolStripMenuItem();
			this.mnuInsertStep1 = new ToolStripMenuItem();
			this.mnuEditStep1 = new ToolStripMenuItem();
			this.mnuDeleteStep1 = new ToolStripMenuItem();
			this.menuItem12 = new ToolStripSeparator();
			this.mnuIncludeInBuild1 = new ToolStripMenuItem();
			this.mnuMoveUp1 = new ToolStripMenuItem();
			this.mnuMoveDown1 = new ToolStripMenuItem();
			this.mnuUnindent1 = new ToolStripMenuItem();
			this.mnuIndent1 = new ToolStripMenuItem();
			this.mnuBuild = new ToolStripMenuItem();
			this.mnuBuildProject = new ToolStripMenuItem();
			this.mnuBuildSelectedStepsOnly1 = new ToolStripMenuItem();
			this.mnuBuildFromSelectedStep1 = new ToolStripMenuItem();
			this.mnuBuildToSelectedStep1 = new ToolStripMenuItem();
			this.menuItem26 = new ToolStripSeparator();
			this.mnuStopBuild = new ToolStripMenuItem();
			this.mnuTools = new ToolStripMenuItem();
			this.mnuResetAndClear = new ToolStripMenuItem();
			this.mnuResetStatuses1 = new ToolStripMenuItem();
			this.mnuClearOutputWindow = new ToolStripMenuItem();
			this.mnuSaveOutputAs1 = new ToolStripMenuItem();
			this.menuItem20 = new ToolStripSeparator();
			this.mnuProjectOptions = new ToolStripMenuItem();
			this.mnuApplicationOptions = new ToolStripMenuItem();
			this.mnuHelp = new ToolStripMenuItem();
			this.mnuAbout = new ToolStripMenuItem();
			this.toolbar = new ExtendedToolStrip();
			this.tbNew = new ToolStripButton();
			this.tbOpen = new ToolStripSplitButton();
			this.recentFilesContextMenu = new ContextMenuStrip(this.components);
			this.tbSave = new ToolStripButton();
			this.tbSep1 = new ToolStripSeparator();
			this.tbAddStep = new ToolStripButton();
			this.tbInsertStep = new ToolStripButton();
			this.tbEditStep = new ToolStripButton();
			this.tbDeleteStep = new ToolStripButton();
			this.tbSep2 = new ToolStripSeparator();
			this.tbIncludeInBuild = new ToolStripButton();
			this.tbMoveUp = new ToolStripButton();
			this.tbMoveDown = new ToolStripButton();
			this.tbUnindent = new ToolStripButton();
			this.tbIndent = new ToolStripButton();
			this.tbSep3 = new ToolStripSeparator();
			this.tbBuildProject = new ToolStripButton();
			this.tbBuildSelected = new ToolStripButton();
			this.tbBuildFrom = new ToolStripButton();
			this.tbBuildTo = new ToolStripButton();
			this.tbStopBuild = new ToolStripButton();
			this.tbSep4 = new ToolStripSeparator();
			this.btnGoToPreviousHighlight = new ToolStripButton();
			this.btnGoToNextHighlight = new ToolStripButton();
			this.images = new ImageList(this.components);
			this.pnlList = new Panel();
			this.TabCtrl = new TabControl();
			this.tabBuildSteps = new TabPage();
			this.lstBuildSteps = new ExtendedListView();
			this.colStep = new ColumnHeader();
			this.colType = new ColumnHeader();
			this.colIgnoreFailure = new ColumnHeader();
			this.colConfirm = new ColumnHeader();
			this.colInfo = new ColumnHeader();
			this.colRunTime = new ColumnHeader();
			this.colStatus = new ColumnHeader();
			this.colDescription = new ColumnHeader();
			this.listContextMenu = new ContextMenuStrip(this.components);
			this.mnuBuildSelectedStepsOnly2 = new ToolStripMenuItem();
			this.mnuBuildFromSelectedStep2 = new ToolStripMenuItem();
			this.mnuBuildToSelectedStep2 = new ToolStripMenuItem();
			this.menuItem1 = new ToolStripSeparator();
			this.mnuGoToStepOutput2 = new ToolStripMenuItem();
			this.menuItem43 = new ToolStripSeparator();
			this.mnuCutStep2 = new ToolStripMenuItem();
			this.mnuCopyStep2 = new ToolStripMenuItem();
			this.mnuPasteStep2 = new ToolStripMenuItem();
			this.menuItem13 = new ToolStripSeparator();
			this.mnuAddStep2 = new ToolStripMenuItem();
			this.mnuInsertStep2 = new ToolStripMenuItem();
			this.mnuEditStep2 = new ToolStripMenuItem();
			this.mnuDeleteStep2 = new ToolStripMenuItem();
			this.menuItem34 = new ToolStripSeparator();
			this.mnuIncludeInBuild2 = new ToolStripMenuItem();
			this.mnuMoveUp2 = new ToolStripMenuItem();
			this.mnuMoveDown2 = new ToolStripMenuItem();
			this.mnuUnindent2 = new ToolStripMenuItem();
			this.mnuIndent2 = new ToolStripMenuItem();
			this.tabFailureSteps = new TabPage();
			this.lstFailureSteps = new ExtendedListView();
			this.colFStep = new ColumnHeader();
			this.colFType = new ColumnHeader();
			this.colFIgnoreFailure = new ColumnHeader();
			this.colFConfirm = new ColumnHeader();
			this.colFInfo = new ColumnHeader();
			this.colFRunTime = new ColumnHeader();
			this.colFStatus = new ColumnHeader();
			this.colFDescription = new ColumnHeader();
			this.outputContextMenu = new ContextMenuStrip(this.components);
			this.mnuCopyOutput = new ToolStripMenuItem();
			this.mnuSelectAllOutput = new ToolStripMenuItem();
			this.menuItem6 = new ToolStripSeparator();
			this.mnuClearAll = new ToolStripMenuItem();
			this.menuItem3 = new ToolStripSeparator();
			this.mnuSaveOutputAs2 = new ToolStripMenuItem();
			this.toolStripMenuItem2 = new ToolStripSeparator();
			this.mnuGoToPreviousHighlight2 = new ToolStripMenuItem();
			this.mnuGoToNextHighlight2 = new ToolStripMenuItem();
			this.formSaver = new FormSaver(this.components);
			this.recentFiles = new RecentItemList(this.components);
			this.openProjectDlg = new OpenFileDialog();
			this.saveProjectDlg = new SaveFileDialog();
			this.saveOutputDlg = new SaveFileDialog();
			this.buildTimer = new Timer(this.components);
			this.Splitter = new SplitContainer();
			this.outputWindow = new OutputWindow();
			this.status = new StatusStrip();
			this.spName = new ToolStripStatusLabel();
			this.spProgress = new ToolStripProgressBar();
			this.spTime = new ToolStripStatusLabel();
			this.project = new Project(this.components);
			this.mainMenuInst.SuspendLayout();
			this.toolbar.SuspendLayout();
			this.pnlList.SuspendLayout();
			this.TabCtrl.SuspendLayout();
			this.tabBuildSteps.SuspendLayout();
			this.listContextMenu.SuspendLayout();
			this.tabFailureSteps.SuspendLayout();
			this.outputContextMenu.SuspendLayout();
			((ISupportInitialize)this.Splitter).BeginInit();
			this.Splitter.Panel1.SuspendLayout();
			this.Splitter.Panel2.SuspendLayout();
			this.Splitter.SuspendLayout();
			this.status.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenuInst
			// 
			this.mainMenuInst.Items.AddRange(new ToolStripItem[] { this.mnuFile, this.mnuEdit, this.mnuSteps, this.mnuBuild, this.mnuTools, this.mnuHelp });
			this.mainMenuInst.Location = new Point(0, 0);
			this.mainMenuInst.Name = "mainMenuInst";
			this.mainMenuInst.Size = new Size(616, 24);
			this.mainMenuInst.TabIndex = 2;
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new ToolStripItem[] { this.mnuNew, this.mnuOpen, this.mnuSave, this.mnuSaveAs, this.menuItem7, this.mnuRecentFiles, this.mnuOpenProjectSeparator, this.mnuOpenProjectFolderInExplorer, this.mnuOpenProjectFolderInTerminal, this.toolStripMenuItem4, this.mnuExit });
			this.mnuFile.MergeIndex = 0;
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new Size(37, 20);
			this.mnuFile.Text = "&File";
			// 
			// mnuNew
			// 
			this.mnuNew.Image = Properties.Resources.New;
			this.mnuNew.ImageTransparentColor = Color.Fuchsia;
			this.mnuNew.MergeIndex = 0;
			this.mnuNew.Name = "mnuNew";
			this.mnuNew.ShortcutKeys = Keys.Control | Keys.N;
			this.mnuNew.Size = new Size(241, 22);
			this.mnuNew.Text = "&New";
			this.mnuNew.Click += this.New_Click;
			// 
			// mnuOpen
			// 
			this.mnuOpen.Image = Properties.Resources.Open;
			this.mnuOpen.ImageTransparentColor = Color.Fuchsia;
			this.mnuOpen.MergeIndex = 1;
			this.mnuOpen.Name = "mnuOpen";
			this.mnuOpen.ShortcutKeys = Keys.Control | Keys.O;
			this.mnuOpen.Size = new Size(241, 22);
			this.mnuOpen.Text = "&Open...";
			this.mnuOpen.Click += this.Open_Click;
			// 
			// mnuSave
			// 
			this.mnuSave.Image = Properties.Resources.Save;
			this.mnuSave.ImageTransparentColor = Color.Fuchsia;
			this.mnuSave.MergeIndex = 2;
			this.mnuSave.Name = "mnuSave";
			this.mnuSave.ShortcutKeys = Keys.Control | Keys.S;
			this.mnuSave.Size = new Size(241, 22);
			this.mnuSave.Text = "&Save";
			this.mnuSave.Click += this.Save_Click;
			// 
			// mnuSaveAs
			// 
			this.mnuSaveAs.MergeIndex = 3;
			this.mnuSaveAs.Name = "mnuSaveAs";
			this.mnuSaveAs.Size = new Size(241, 22);
			this.mnuSaveAs.Text = "Save &As...";
			this.mnuSaveAs.Click += this.SaveAs_Click;
			// 
			// menuItem7
			// 
			this.menuItem7.MergeIndex = 4;
			this.menuItem7.Name = "menuItem7";
			this.menuItem7.Size = new Size(238, 6);
			// 
			// mnuRecentFiles
			// 
			this.mnuRecentFiles.MergeIndex = 5;
			this.mnuRecentFiles.Name = "mnuRecentFiles";
			this.mnuRecentFiles.Size = new Size(241, 22);
			this.mnuRecentFiles.Text = "Recent Files";
			// 
			// mnuOpenProjectSeparator
			// 
			this.mnuOpenProjectSeparator.MergeIndex = 6;
			this.mnuOpenProjectSeparator.Name = "mnuOpenProjectSeparator";
			this.mnuOpenProjectSeparator.Size = new Size(238, 6);
			// 
			// mnuOpenProjectFolderInExplorer
			// 
			this.mnuOpenProjectFolderInExplorer.Name = "mnuOpenProjectFolderInExplorer";
			this.mnuOpenProjectFolderInExplorer.Size = new Size(241, 22);
			this.mnuOpenProjectFolderInExplorer.Text = "Open Project Folder In &Explorer";
			this.mnuOpenProjectFolderInExplorer.Click += this.OpenProjectFolderInExplorer_Click;
			// 
			// mnuOpenProjectFolderInTerminal
			// 
			this.mnuOpenProjectFolderInTerminal.Name = "mnuOpenProjectFolderInTerminal";
			this.mnuOpenProjectFolderInTerminal.Size = new Size(241, 22);
			this.mnuOpenProjectFolderInTerminal.Text = "Open Project Folder In &Terminal";
			this.mnuOpenProjectFolderInTerminal.Click += this.OpenProjectFolderInTerminal_Click;
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new Size(238, 6);
			// 
			// mnuExit
			// 
			this.mnuExit.MergeIndex = 7;
			this.mnuExit.Name = "mnuExit";
			this.mnuExit.Size = new Size(241, 22);
			this.mnuExit.Text = "E&xit";
			this.mnuExit.Click += this.Exit_Click;
			// 
			// mnuEdit
			// 
			this.mnuEdit.DropDownItems.AddRange(new ToolStripItem[] { this.mnuCutStep, this.mnuCopyStep, this.mnuPasteStep, this.menuItem5, this.mnuSelectAllSteps, this.menuItem8, this.mnuFindInOutput, this.mnuFindNextInOutput, this.mnuFindPreviousInOutput, this.toolStripMenuItem1, this.mnuGoToPreviousHighlight, this.mnuGoToNextHighlight, this.toolStripMenuItem3, this.mnuGoToStepOutput });
			this.mnuEdit.MergeIndex = 1;
			this.mnuEdit.Name = "mnuEdit";
			this.mnuEdit.Size = new Size(39, 20);
			this.mnuEdit.Text = "&Edit";
			// 
			// mnuCutStep
			// 
			this.mnuCutStep.Image = Properties.Resources.Cut;
			this.mnuCutStep.ImageTransparentColor = Color.Fuchsia;
			this.mnuCutStep.MergeIndex = 0;
			this.mnuCutStep.Name = "mnuCutStep";
			this.mnuCutStep.ShortcutKeys = Keys.Control | Keys.X;
			this.mnuCutStep.Size = new Size(257, 22);
			this.mnuCutStep.Text = "Cu&t Step";
			this.mnuCutStep.Click += this.CutStep_Click;
			// 
			// mnuCopyStep
			// 
			this.mnuCopyStep.Image = Properties.Resources.Copy;
			this.mnuCopyStep.ImageTransparentColor = Color.Fuchsia;
			this.mnuCopyStep.MergeIndex = 1;
			this.mnuCopyStep.Name = "mnuCopyStep";
			this.mnuCopyStep.ShortcutKeys = Keys.Control | Keys.C;
			this.mnuCopyStep.Size = new Size(257, 22);
			this.mnuCopyStep.Text = "&Copy Step";
			this.mnuCopyStep.Click += this.CopyStep_Click;
			// 
			// mnuPasteStep
			// 
			this.mnuPasteStep.Image = Properties.Resources.Paste;
			this.mnuPasteStep.ImageTransparentColor = Color.Fuchsia;
			this.mnuPasteStep.MergeIndex = 2;
			this.mnuPasteStep.Name = "mnuPasteStep";
			this.mnuPasteStep.ShortcutKeys = Keys.Control | Keys.V;
			this.mnuPasteStep.Size = new Size(257, 22);
			this.mnuPasteStep.Text = "&Paste Step";
			this.mnuPasteStep.Click += this.PasteStep_Click;
			// 
			// menuItem5
			// 
			this.menuItem5.MergeIndex = 3;
			this.menuItem5.Name = "menuItem5";
			this.menuItem5.Size = new Size(254, 6);
			// 
			// mnuSelectAllSteps
			// 
			this.mnuSelectAllSteps.MergeIndex = 4;
			this.mnuSelectAllSteps.Name = "mnuSelectAllSteps";
			this.mnuSelectAllSteps.ShortcutKeys = Keys.Control | Keys.A;
			this.mnuSelectAllSteps.Size = new Size(257, 22);
			this.mnuSelectAllSteps.Text = "Select &All Steps";
			this.mnuSelectAllSteps.Click += this.SelectAllSteps_Click;
			// 
			// menuItem8
			// 
			this.menuItem8.MergeIndex = 5;
			this.menuItem8.Name = "menuItem8";
			this.menuItem8.Size = new Size(254, 6);
			// 
			// mnuFindInOutput
			// 
			this.mnuFindInOutput.Image = Properties.Resources.Find;
			this.mnuFindInOutput.ImageTransparentColor = Color.Fuchsia;
			this.mnuFindInOutput.MergeIndex = 6;
			this.mnuFindInOutput.Name = "mnuFindInOutput";
			this.mnuFindInOutput.ShortcutKeys = Keys.Control | Keys.F;
			this.mnuFindInOutput.Size = new Size(257, 22);
			this.mnuFindInOutput.Text = "&Find In Output...";
			this.mnuFindInOutput.Click += this.FindInOutput_Click;
			// 
			// mnuFindNextInOutput
			// 
			this.mnuFindNextInOutput.Image = Properties.Resources.FindNext;
			this.mnuFindNextInOutput.ImageTransparentColor = Color.Fuchsia;
			this.mnuFindNextInOutput.MergeIndex = 7;
			this.mnuFindNextInOutput.Name = "mnuFindNextInOutput";
			this.mnuFindNextInOutput.ShortcutKeys = Keys.F3;
			this.mnuFindNextInOutput.Size = new Size(257, 22);
			this.mnuFindNextInOutput.Text = "Find &Next In Output";
			this.mnuFindNextInOutput.Click += this.FindNextInOutput_Click;
			// 
			// mnuFindPreviousInOutput
			// 
			this.mnuFindPreviousInOutput.Image = Properties.Resources.FindPrev;
			this.mnuFindPreviousInOutput.ImageTransparentColor = Color.Fuchsia;
			this.mnuFindPreviousInOutput.MergeIndex = 8;
			this.mnuFindPreviousInOutput.Name = "mnuFindPreviousInOutput";
			this.mnuFindPreviousInOutput.ShortcutKeys = Keys.Shift | Keys.F3;
			this.mnuFindPreviousInOutput.Size = new Size(257, 22);
			this.mnuFindPreviousInOutput.Text = "Find P&revious In Output";
			this.mnuFindPreviousInOutput.Click += this.FindPreviousInOutput_Click;
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new Size(254, 6);
			// 
			// mnuGoToPreviousHighlight
			// 
			this.mnuGoToPreviousHighlight.Image = Properties.Resources.GoToPreviousHighlight;
			this.mnuGoToPreviousHighlight.ImageTransparentColor = Color.Fuchsia;
			this.mnuGoToPreviousHighlight.Name = "mnuGoToPreviousHighlight";
			this.mnuGoToPreviousHighlight.ShortcutKeys = Keys.Shift | Keys.F4;
			this.mnuGoToPreviousHighlight.Size = new Size(257, 22);
			this.mnuGoToPreviousHighlight.Text = "Go To Pre&vious Highlight";
			this.mnuGoToPreviousHighlight.Click += this.GoToPreviousHighlight_Click;
			// 
			// mnuGoToNextHighlight
			// 
			this.mnuGoToNextHighlight.Image = Properties.Resources.GoToNextHighlight;
			this.mnuGoToNextHighlight.ImageTransparentColor = Color.Fuchsia;
			this.mnuGoToNextHighlight.Name = "mnuGoToNextHighlight";
			this.mnuGoToNextHighlight.ShortcutKeys = Keys.F4;
			this.mnuGoToNextHighlight.Size = new Size(257, 22);
			this.mnuGoToNextHighlight.Text = "Go To Ne&xt Highlight";
			this.mnuGoToNextHighlight.Click += this.GoToNextHighlight_Click;
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new Size(254, 6);
			// 
			// mnuGoToStepOutput
			// 
			this.mnuGoToStepOutput.Name = "mnuGoToStepOutput";
			this.mnuGoToStepOutput.ShortcutKeys = Keys.Control | Keys.G;
			this.mnuGoToStepOutput.Size = new Size(257, 22);
			this.mnuGoToStepOutput.Text = "&Go To Step Output";
			this.mnuGoToStepOutput.Click += this.GoToStepOutput_Click;
			// 
			// mnuSteps
			// 
			this.mnuSteps.DropDownItems.AddRange(new ToolStripItem[] { this.mnuAddStep1, this.mnuInsertStep1, this.mnuEditStep1, this.mnuDeleteStep1, this.menuItem12, this.mnuIncludeInBuild1, this.mnuMoveUp1, this.mnuMoveDown1, this.mnuUnindent1, this.mnuIndent1 });
			this.mnuSteps.MergeIndex = 2;
			this.mnuSteps.Name = "mnuSteps";
			this.mnuSteps.Size = new Size(47, 20);
			this.mnuSteps.Text = "&Steps";
			// 
			// mnuAddStep1
			// 
			this.mnuAddStep1.Image = Properties.Resources.AddStep;
			this.mnuAddStep1.ImageTransparentColor = Color.Lime;
			this.mnuAddStep1.MergeIndex = 0;
			this.mnuAddStep1.Name = "mnuAddStep1";
			this.mnuAddStep1.ShortcutKeys = Keys.Control | Keys.Insert;
			this.mnuAddStep1.Size = new Size(194, 22);
			this.mnuAddStep1.Text = "&Add Step...";
			this.mnuAddStep1.Click += this.AddStep1_Click;
			// 
			// mnuInsertStep1
			// 
			this.mnuInsertStep1.Image = Properties.Resources.InsertStep;
			this.mnuInsertStep1.ImageTransparentColor = Color.Lime;
			this.mnuInsertStep1.MergeIndex = 1;
			this.mnuInsertStep1.Name = "mnuInsertStep1";
			this.mnuInsertStep1.ShortcutKeys = Keys.Insert;
			this.mnuInsertStep1.Size = new Size(194, 22);
			this.mnuInsertStep1.Text = "&Insert Step...";
			this.mnuInsertStep1.Click += this.InsertStep1_Click;
			// 
			// mnuEditStep1
			// 
			this.mnuEditStep1.Image = Properties.Resources.EditStep;
			this.mnuEditStep1.ImageTransparentColor = Color.Lime;
			this.mnuEditStep1.MergeIndex = 2;
			this.mnuEditStep1.Name = "mnuEditStep1";
			this.mnuEditStep1.ShortcutKeys = Keys.Control | Keys.E;
			this.mnuEditStep1.Size = new Size(194, 22);
			this.mnuEditStep1.Text = "&Edit Step...";
			this.mnuEditStep1.Click += this.EditStep1_Click;
			// 
			// mnuDeleteStep1
			// 
			this.mnuDeleteStep1.Image = Properties.Resources.DeleteStep;
			this.mnuDeleteStep1.ImageTransparentColor = Color.Lime;
			this.mnuDeleteStep1.MergeIndex = 3;
			this.mnuDeleteStep1.Name = "mnuDeleteStep1";
			this.mnuDeleteStep1.ShortcutKeys = Keys.Delete;
			this.mnuDeleteStep1.Size = new Size(194, 22);
			this.mnuDeleteStep1.Text = "&Delete Step...";
			this.mnuDeleteStep1.Click += this.DeleteStep1_Click;
			// 
			// menuItem12
			// 
			this.menuItem12.MergeIndex = 4;
			this.menuItem12.Name = "menuItem12";
			this.menuItem12.Size = new Size(191, 6);
			// 
			// mnuIncludeInBuild1
			// 
			this.mnuIncludeInBuild1.Image = Properties.Resources.IncludeInBuild;
			this.mnuIncludeInBuild1.ImageTransparentColor = Color.Fuchsia;
			this.mnuIncludeInBuild1.MergeIndex = 5;
			this.mnuIncludeInBuild1.Name = "mnuIncludeInBuild1";
			this.mnuIncludeInBuild1.ShortcutKeys = Keys.Control | Keys.I;
			this.mnuIncludeInBuild1.Size = new Size(194, 22);
			this.mnuIncludeInBuild1.Text = "Include In &Build";
			this.mnuIncludeInBuild1.Click += this.IncludeInBuild1_Click;
			// 
			// mnuMoveUp1
			// 
			this.mnuMoveUp1.Image = Properties.Resources.MoveUp;
			this.mnuMoveUp1.ImageTransparentColor = Color.Fuchsia;
			this.mnuMoveUp1.MergeIndex = 6;
			this.mnuMoveUp1.Name = "mnuMoveUp1";
			this.mnuMoveUp1.ShortcutKeys = Keys.Control | Keys.U;
			this.mnuMoveUp1.Size = new Size(194, 22);
			this.mnuMoveUp1.Text = "Move &Up";
			this.mnuMoveUp1.Click += this.MoveUp1_Click;
			// 
			// mnuMoveDown1
			// 
			this.mnuMoveDown1.Image = Properties.Resources.MoveDown;
			this.mnuMoveDown1.ImageTransparentColor = Color.Fuchsia;
			this.mnuMoveDown1.MergeIndex = 7;
			this.mnuMoveDown1.Name = "mnuMoveDown1";
			this.mnuMoveDown1.ShortcutKeys = Keys.Control | Keys.D;
			this.mnuMoveDown1.Size = new Size(194, 22);
			this.mnuMoveDown1.Text = "Move Do&wn";
			this.mnuMoveDown1.Click += this.MoveDown1_Click;
			// 
			// mnuUnindent1
			// 
			this.mnuUnindent1.Image = Properties.Resources.MoveLeft;
			this.mnuUnindent1.ImageTransparentColor = Color.Fuchsia;
			this.mnuUnindent1.MergeIndex = 8;
			this.mnuUnindent1.Name = "mnuUnindent1";
			this.mnuUnindent1.ShortcutKeys = Keys.Control | Keys.L;
			this.mnuUnindent1.Size = new Size(194, 22);
			this.mnuUnindent1.Text = "Uninden&t";
			this.mnuUnindent1.Click += this.Unindent1_Click;
			// 
			// mnuIndent1
			// 
			this.mnuIndent1.Image = Properties.Resources.MoveRight;
			this.mnuIndent1.ImageTransparentColor = Color.Fuchsia;
			this.mnuIndent1.MergeIndex = 9;
			this.mnuIndent1.Name = "mnuIndent1";
			this.mnuIndent1.ShortcutKeys = Keys.Control | Keys.R;
			this.mnuIndent1.Size = new Size(194, 22);
			this.mnuIndent1.Text = "I&ndent";
			this.mnuIndent1.Click += this.Indent1_Click;
			// 
			// mnuBuild
			// 
			this.mnuBuild.DropDownItems.AddRange(new ToolStripItem[] { this.mnuBuildProject, this.mnuBuildSelectedStepsOnly1, this.mnuBuildFromSelectedStep1, this.mnuBuildToSelectedStep1, this.menuItem26, this.mnuStopBuild });
			this.mnuBuild.MergeIndex = 3;
			this.mnuBuild.Name = "mnuBuild";
			this.mnuBuild.Size = new Size(46, 20);
			this.mnuBuild.Text = "&Build";
			// 
			// mnuBuildProject
			// 
			this.mnuBuildProject.Image = Properties.Resources.Build;
			this.mnuBuildProject.ImageTransparentColor = Color.Lime;
			this.mnuBuildProject.MergeIndex = 0;
			this.mnuBuildProject.Name = "mnuBuildProject";
			this.mnuBuildProject.ShortcutKeys = Keys.F7;
			this.mnuBuildProject.Size = new Size(268, 22);
			this.mnuBuildProject.Text = "&Build Project";
			this.mnuBuildProject.Click += this.BuildProject_Click;
			// 
			// mnuBuildSelectedStepsOnly1
			// 
			this.mnuBuildSelectedStepsOnly1.Image = Properties.Resources.BuildSelectedSteps;
			this.mnuBuildSelectedStepsOnly1.ImageTransparentColor = Color.Lime;
			this.mnuBuildSelectedStepsOnly1.MergeIndex = 1;
			this.mnuBuildSelectedStepsOnly1.Name = "mnuBuildSelectedStepsOnly1";
			this.mnuBuildSelectedStepsOnly1.ShortcutKeys = Keys.Shift | Keys.F7;
			this.mnuBuildSelectedStepsOnly1.Size = new Size(268, 22);
			this.mnuBuildSelectedStepsOnly1.Text = "Build &Selected Step(s) Only";
			this.mnuBuildSelectedStepsOnly1.Click += this.BuildSelectedStepOnly1_Click;
			// 
			// mnuBuildFromSelectedStep1
			// 
			this.mnuBuildFromSelectedStep1.Image = Properties.Resources.BuildFromSelectedStep;
			this.mnuBuildFromSelectedStep1.ImageTransparentColor = Color.Lime;
			this.mnuBuildFromSelectedStep1.MergeIndex = 2;
			this.mnuBuildFromSelectedStep1.Name = "mnuBuildFromSelectedStep1";
			this.mnuBuildFromSelectedStep1.ShortcutKeys = Keys.Control | Keys.F7;
			this.mnuBuildFromSelectedStep1.Size = new Size(268, 22);
			this.mnuBuildFromSelectedStep1.Text = "Build &From Selected Step";
			this.mnuBuildFromSelectedStep1.Click += this.BuildFromSelectedStep1_Click;
			// 
			// mnuBuildToSelectedStep1
			// 
			this.mnuBuildToSelectedStep1.Image = Properties.Resources.BuildToSelectedStep;
			this.mnuBuildToSelectedStep1.ImageTransparentColor = Color.Lime;
			this.mnuBuildToSelectedStep1.MergeIndex = 3;
			this.mnuBuildToSelectedStep1.Name = "mnuBuildToSelectedStep1";
			this.mnuBuildToSelectedStep1.ShortcutKeys = Keys.Control | Keys.Shift | Keys.F7;
			this.mnuBuildToSelectedStep1.Size = new Size(268, 22);
			this.mnuBuildToSelectedStep1.Text = "Build &To Selected Step";
			this.mnuBuildToSelectedStep1.Click += this.BuildToSelectedStep1_Click;
			// 
			// menuItem26
			// 
			this.menuItem26.MergeIndex = 4;
			this.menuItem26.Name = "menuItem26";
			this.menuItem26.Size = new Size(265, 6);
			// 
			// mnuStopBuild
			// 
			this.mnuStopBuild.Image = Properties.Resources.StopBuild;
			this.mnuStopBuild.ImageTransparentColor = Color.Lime;
			this.mnuStopBuild.MergeIndex = 5;
			this.mnuStopBuild.Name = "mnuStopBuild";
			this.mnuStopBuild.ShortcutKeys = Keys.Control | Keys.Q;
			this.mnuStopBuild.Size = new Size(268, 22);
			this.mnuStopBuild.Text = "Sto&p Build";
			this.mnuStopBuild.Click += this.StopBuild_Click;
			// 
			// mnuTools
			// 
			this.mnuTools.DropDownItems.AddRange(new ToolStripItem[] { this.mnuResetAndClear, this.mnuResetStatuses1, this.mnuClearOutputWindow, this.mnuSaveOutputAs1, this.menuItem20, this.mnuProjectOptions, this.mnuApplicationOptions });
			this.mnuTools.MergeIndex = 4;
			this.mnuTools.Name = "mnuTools";
			this.mnuTools.Size = new Size(47, 20);
			this.mnuTools.Text = "&Tools";
			// 
			// mnuResetAndClear
			// 
			this.mnuResetAndClear.MergeIndex = 0;
			this.mnuResetAndClear.Name = "mnuResetAndClear";
			this.mnuResetAndClear.Size = new Size(242, 22);
			this.mnuResetAndClear.Text = "Reset &Statuses and Clear Output";
			this.mnuResetAndClear.Click += this.ResetAndClear_Click;
			// 
			// mnuResetStatuses1
			// 
			this.mnuResetStatuses1.MergeIndex = 1;
			this.mnuResetStatuses1.Name = "mnuResetStatuses1";
			this.mnuResetStatuses1.Size = new Size(242, 22);
			this.mnuResetStatuses1.Text = "&Reset Statuses";
			this.mnuResetStatuses1.Click += this.ResetStatus_Click;
			// 
			// mnuClearOutputWindow
			// 
			this.mnuClearOutputWindow.MergeIndex = 2;
			this.mnuClearOutputWindow.Name = "mnuClearOutputWindow";
			this.mnuClearOutputWindow.Size = new Size(242, 22);
			this.mnuClearOutputWindow.Text = "&Clear Output Window";
			this.mnuClearOutputWindow.Click += this.ClearOutputWindow_Click;
			// 
			// mnuSaveOutputAs1
			// 
			this.mnuSaveOutputAs1.MergeIndex = 3;
			this.mnuSaveOutputAs1.Name = "mnuSaveOutputAs1";
			this.mnuSaveOutputAs1.Size = new Size(242, 22);
			this.mnuSaveOutputAs1.Text = "Sa&ve Output As...";
			this.mnuSaveOutputAs1.Click += this.SaveOutputAs_Click;
			// 
			// menuItem20
			// 
			this.menuItem20.MergeIndex = 4;
			this.menuItem20.Name = "menuItem20";
			this.menuItem20.Size = new Size(239, 6);
			// 
			// mnuProjectOptions
			// 
			this.mnuProjectOptions.MergeIndex = 5;
			this.mnuProjectOptions.Name = "mnuProjectOptions";
			this.mnuProjectOptions.Size = new Size(242, 22);
			this.mnuProjectOptions.Text = "&Project Options...";
			this.mnuProjectOptions.Click += this.ProjectOptions_Click;
			// 
			// mnuApplicationOptions
			// 
			this.mnuApplicationOptions.MergeIndex = 6;
			this.mnuApplicationOptions.Name = "mnuApplicationOptions";
			this.mnuApplicationOptions.Size = new Size(242, 22);
			this.mnuApplicationOptions.Text = "&Application Options..";
			this.mnuApplicationOptions.Click += this.ApplicationOptions_Click;
			// 
			// mnuHelp
			// 
			this.mnuHelp.DropDownItems.AddRange(new ToolStripItem[] { this.mnuAbout });
			this.mnuHelp.MergeIndex = 5;
			this.mnuHelp.Name = "mnuHelp";
			this.mnuHelp.Size = new Size(44, 20);
			this.mnuHelp.Text = "&Help";
			// 
			// mnuAbout
			// 
			this.mnuAbout.MergeIndex = 2;
			this.mnuAbout.Name = "mnuAbout";
			this.mnuAbout.Size = new Size(116, 22);
			this.mnuAbout.Text = "&About...";
			this.mnuAbout.Click += this.About_Click;
			// 
			// toolbar
			// 
			this.toolbar.AutoSize = false;
			this.toolbar.GripStyle = ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new ToolStripItem[] { this.tbNew, this.tbOpen, this.tbSave, this.tbSep1, this.tbAddStep, this.tbInsertStep, this.tbEditStep, this.tbDeleteStep, this.tbSep2, this.tbIncludeInBuild, this.tbMoveUp, this.tbMoveDown, this.tbUnindent, this.tbIndent, this.tbSep3, this.tbBuildProject, this.tbBuildSelected, this.tbBuildFrom, this.tbBuildTo, this.tbStopBuild, this.tbSep4, this.btnGoToPreviousHighlight, this.btnGoToNextHighlight });
			this.toolbar.Location = new Point(0, 24);
			this.toolbar.Name = "toolbar";
			this.toolbar.Size = new Size(616, 29);
			this.toolbar.TabIndex = 0;
			// 
			// tbNew
			// 
			this.tbNew.Image = Properties.Resources.New;
			this.tbNew.ImageTransparentColor = Color.Fuchsia;
			this.tbNew.Name = "tbNew";
			this.tbNew.Size = new Size(23, 26);
			this.tbNew.ToolTipText = "New";
			this.tbNew.Click += this.New_Click;
			// 
			// tbOpen
			// 
			this.tbOpen.DropDown = this.recentFilesContextMenu;
			this.tbOpen.Image = Properties.Resources.Open;
			this.tbOpen.ImageTransparentColor = Color.Fuchsia;
			this.tbOpen.Name = "tbOpen";
			this.tbOpen.Size = new Size(32, 26);
			this.tbOpen.ToolTipText = "Open";
			this.tbOpen.ButtonClick += this.Open_Click;
			// 
			// recentFilesContextMenu
			// 
			this.recentFilesContextMenu.Name = "RecentFilesContextMenu";
			this.recentFilesContextMenu.OwnerItem = this.tbOpen;
			this.recentFilesContextMenu.Size = new Size(61, 4);
			// 
			// tbSave
			// 
			this.tbSave.Image = Properties.Resources.Save;
			this.tbSave.ImageTransparentColor = Color.Fuchsia;
			this.tbSave.Name = "tbSave";
			this.tbSave.Size = new Size(23, 26);
			this.tbSave.ToolTipText = "Save";
			this.tbSave.Click += this.Save_Click;
			// 
			// tbSep1
			// 
			this.tbSep1.Name = "tbSep1";
			this.tbSep1.Size = new Size(6, 29);
			// 
			// tbAddStep
			// 
			this.tbAddStep.Image = Properties.Resources.AddStep;
			this.tbAddStep.ImageTransparentColor = Color.Lime;
			this.tbAddStep.Name = "tbAddStep";
			this.tbAddStep.Size = new Size(23, 26);
			this.tbAddStep.ToolTipText = "Add Step";
			this.tbAddStep.Click += this.AddStep1_Click;
			// 
			// tbInsertStep
			// 
			this.tbInsertStep.Image = Properties.Resources.InsertStep;
			this.tbInsertStep.ImageTransparentColor = Color.Lime;
			this.tbInsertStep.Name = "tbInsertStep";
			this.tbInsertStep.Size = new Size(23, 26);
			this.tbInsertStep.ToolTipText = "Insert Step";
			this.tbInsertStep.Click += this.InsertStep1_Click;
			// 
			// tbEditStep
			// 
			this.tbEditStep.Image = Properties.Resources.EditStep;
			this.tbEditStep.ImageTransparentColor = Color.Lime;
			this.tbEditStep.Name = "tbEditStep";
			this.tbEditStep.Size = new Size(23, 26);
			this.tbEditStep.ToolTipText = "Edit Step";
			this.tbEditStep.Click += this.EditStep1_Click;
			// 
			// tbDeleteStep
			// 
			this.tbDeleteStep.Image = Properties.Resources.DeleteStep;
			this.tbDeleteStep.ImageTransparentColor = Color.Lime;
			this.tbDeleteStep.Name = "tbDeleteStep";
			this.tbDeleteStep.Size = new Size(23, 26);
			this.tbDeleteStep.ToolTipText = "Delete Step";
			this.tbDeleteStep.Click += this.DeleteStep1_Click;
			// 
			// tbSep2
			// 
			this.tbSep2.Name = "tbSep2";
			this.tbSep2.Size = new Size(6, 29);
			// 
			// tbIncludeInBuild
			// 
			this.tbIncludeInBuild.Image = Properties.Resources.IncludeInBuild;
			this.tbIncludeInBuild.ImageTransparentColor = Color.Fuchsia;
			this.tbIncludeInBuild.Name = "tbIncludeInBuild";
			this.tbIncludeInBuild.Size = new Size(23, 26);
			this.tbIncludeInBuild.ToolTipText = "Include In Build";
			this.tbIncludeInBuild.Click += this.IncludeInBuild1_Click;
			// 
			// tbMoveUp
			// 
			this.tbMoveUp.Image = Properties.Resources.MoveUp;
			this.tbMoveUp.ImageTransparentColor = Color.Fuchsia;
			this.tbMoveUp.Name = "tbMoveUp";
			this.tbMoveUp.Size = new Size(23, 26);
			this.tbMoveUp.ToolTipText = "Move Up";
			this.tbMoveUp.Click += this.MoveUp1_Click;
			// 
			// tbMoveDown
			// 
			this.tbMoveDown.Image = Properties.Resources.MoveDown;
			this.tbMoveDown.ImageTransparentColor = Color.Fuchsia;
			this.tbMoveDown.Name = "tbMoveDown";
			this.tbMoveDown.Size = new Size(23, 26);
			this.tbMoveDown.ToolTipText = "Move Down";
			this.tbMoveDown.Click += this.MoveDown1_Click;
			// 
			// tbUnindent
			// 
			this.tbUnindent.Image = Properties.Resources.MoveLeft;
			this.tbUnindent.ImageTransparentColor = Color.Fuchsia;
			this.tbUnindent.Name = "tbUnindent";
			this.tbUnindent.Size = new Size(23, 26);
			this.tbUnindent.ToolTipText = "Unindent";
			this.tbUnindent.Click += this.Unindent1_Click;
			// 
			// tbIndent
			// 
			this.tbIndent.Image = Properties.Resources.MoveRight;
			this.tbIndent.ImageTransparentColor = Color.Fuchsia;
			this.tbIndent.Name = "tbIndent";
			this.tbIndent.Size = new Size(23, 26);
			this.tbIndent.ToolTipText = "Indent";
			this.tbIndent.Click += this.Indent1_Click;
			// 
			// tbSep3
			// 
			this.tbSep3.Name = "tbSep3";
			this.tbSep3.Size = new Size(6, 29);
			// 
			// tbBuildProject
			// 
			this.tbBuildProject.Image = Properties.Resources.Build;
			this.tbBuildProject.ImageTransparentColor = Color.Lime;
			this.tbBuildProject.Name = "tbBuildProject";
			this.tbBuildProject.Size = new Size(23, 26);
			this.tbBuildProject.ToolTipText = "Build Project";
			this.tbBuildProject.Click += this.BuildProject_Click;
			// 
			// tbBuildSelected
			// 
			this.tbBuildSelected.Image = Properties.Resources.BuildSelectedSteps;
			this.tbBuildSelected.ImageTransparentColor = Color.Lime;
			this.tbBuildSelected.Name = "tbBuildSelected";
			this.tbBuildSelected.Size = new Size(23, 26);
			this.tbBuildSelected.ToolTipText = "Build Selected Step(s) Only";
			this.tbBuildSelected.Click += this.BuildSelectedStepOnly1_Click;
			// 
			// tbBuildFrom
			// 
			this.tbBuildFrom.Image = Properties.Resources.BuildFromSelectedStep;
			this.tbBuildFrom.ImageTransparentColor = Color.Lime;
			this.tbBuildFrom.Name = "tbBuildFrom";
			this.tbBuildFrom.Size = new Size(23, 26);
			this.tbBuildFrom.ToolTipText = "Build From Selected Step";
			this.tbBuildFrom.Click += this.BuildFromSelectedStep1_Click;
			// 
			// tbBuildTo
			// 
			this.tbBuildTo.Image = Properties.Resources.BuildToSelectedStep;
			this.tbBuildTo.ImageTransparentColor = Color.Lime;
			this.tbBuildTo.Name = "tbBuildTo";
			this.tbBuildTo.Size = new Size(23, 26);
			this.tbBuildTo.ToolTipText = "Build To Selected Step";
			this.tbBuildTo.Click += this.BuildToSelectedStep1_Click;
			// 
			// tbStopBuild
			// 
			this.tbStopBuild.Image = Properties.Resources.StopBuild;
			this.tbStopBuild.ImageTransparentColor = Color.Lime;
			this.tbStopBuild.Name = "tbStopBuild";
			this.tbStopBuild.Size = new Size(23, 26);
			this.tbStopBuild.ToolTipText = "Stop Build";
			this.tbStopBuild.Click += this.StopBuild_Click;
			// 
			// tbSep4
			// 
			this.tbSep4.Name = "tbSep4";
			this.tbSep4.Size = new Size(6, 29);
			// 
			// btnGoToPreviousHighlight
			// 
			this.btnGoToPreviousHighlight.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.btnGoToPreviousHighlight.Image = Properties.Resources.GoToPreviousHighlight;
			this.btnGoToPreviousHighlight.ImageTransparentColor = Color.Magenta;
			this.btnGoToPreviousHighlight.Name = "btnGoToPreviousHighlight";
			this.btnGoToPreviousHighlight.Size = new Size(23, 26);
			this.btnGoToPreviousHighlight.ToolTipText = "Go To Previous Highlight";
			this.btnGoToPreviousHighlight.Click += this.GoToPreviousHighlight_Click;
			// 
			// btnGoToNextHighlight
			// 
			this.btnGoToNextHighlight.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.btnGoToNextHighlight.Image = Properties.Resources.GoToNextHighlight;
			this.btnGoToNextHighlight.ImageTransparentColor = Color.Magenta;
			this.btnGoToNextHighlight.Name = "btnGoToNextHighlight";
			this.btnGoToNextHighlight.Size = new Size(23, 26);
			this.btnGoToNextHighlight.ToolTipText = "Go To Next Highlight";
			this.btnGoToNextHighlight.Click += this.GoToNextHighlight_Click;
			// 
			// images
			// 
			this.images.ColorDepth = ColorDepth.Depth24Bit;
			this.images.ImageStream = (ImageListStreamer)resources.GetObject("images.ImageStream");
			this.images.TransparentColor = Color.Lime;
			this.images.Images.SetKeyName(0, "BuildSteps.bmp");
			this.images.Images.SetKeyName(1, "FailureSteps.bmp");
			// 
			// pnlList
			// 
			this.pnlList.BorderStyle = BorderStyle.Fixed3D;
			this.pnlList.Controls.Add(this.TabCtrl);
			this.pnlList.Dock = DockStyle.Fill;
			this.pnlList.Location = new Point(0, 0);
			this.pnlList.Name = "pnlList";
			this.pnlList.Size = new Size(178, 357);
			this.pnlList.TabIndex = 1;
			// 
			// TabCtrl
			// 
			this.TabCtrl.Controls.Add(this.tabBuildSteps);
			this.TabCtrl.Controls.Add(this.tabFailureSteps);
			this.TabCtrl.Dock = DockStyle.Fill;
			this.TabCtrl.ImageList = this.images;
			this.TabCtrl.Location = new Point(0, 0);
			this.TabCtrl.Name = "TabCtrl";
			this.TabCtrl.SelectedIndex = 0;
			this.TabCtrl.Size = new Size(174, 353);
			this.TabCtrl.TabIndex = 2;
			this.TabCtrl.SelectedIndexChanged += this.TabCtrl_SelectedIndexChanged;
			// 
			// tabBuildSteps
			// 
			this.tabBuildSteps.Controls.Add(this.lstBuildSteps);
			this.tabBuildSteps.ImageIndex = 0;
			this.tabBuildSteps.Location = new Point(4, 24);
			this.tabBuildSteps.Name = "tabBuildSteps";
			this.tabBuildSteps.Size = new Size(166, 325);
			this.tabBuildSteps.TabIndex = 0;
			this.tabBuildSteps.Text = "Build Steps";
			// 
			// lstBuildSteps
			// 
			this.lstBuildSteps.CheckBoxes = true;
			this.lstBuildSteps.Columns.AddRange(new ColumnHeader[] { this.colStep, this.colType, this.colIgnoreFailure, this.colConfirm, this.colInfo, this.colRunTime, this.colStatus, this.colDescription });
			this.lstBuildSteps.ContextMenuStrip = this.listContextMenu;
			this.lstBuildSteps.Dock = DockStyle.Fill;
			this.lstBuildSteps.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			this.lstBuildSteps.Location = new Point(0, 0);
			this.lstBuildSteps.MultiSelect = true;
			this.lstBuildSteps.Name = "lstBuildSteps";
			this.lstBuildSteps.Size = new Size(166, 325);
			this.lstBuildSteps.SmallImageList = this.images;
			this.lstBuildSteps.TabIndex = 0;
			this.lstBuildSteps.UseCompatibleStateImageBehavior = false;
			this.lstBuildSteps.ItemCheck += this.Steps_ItemCheck;
			this.lstBuildSteps.DoubleClick += this.Steps_DoubleClick;
			// 
			// colStep
			// 
			this.colStep.Text = "Step";
			// 
			// colType
			// 
			this.colType.Text = "Type";
			// 
			// colIgnoreFailure
			// 
			this.colIgnoreFailure.Text = "Ignore Failure";
			this.colIgnoreFailure.Width = 100;
			// 
			// colConfirm
			// 
			this.colConfirm.Text = "Confirm";
			// 
			// colInfo
			// 
			this.colInfo.Text = "Step Info";
			// 
			// colRunTime
			// 
			this.colRunTime.Text = "Step Time";
			// 
			// colStatus
			// 
			this.colStatus.Text = "Status";
			// 
			// colDescription
			// 
			this.colDescription.Text = "Description";
			this.colDescription.Width = 260;
			// 
			// listContextMenu
			// 
			this.listContextMenu.Items.AddRange(new ToolStripItem[] { this.mnuBuildSelectedStepsOnly2, this.mnuBuildFromSelectedStep2, this.mnuBuildToSelectedStep2, this.menuItem1, this.mnuGoToStepOutput2, this.menuItem43, this.mnuCutStep2, this.mnuCopyStep2, this.mnuPasteStep2, this.menuItem13, this.mnuAddStep2, this.mnuInsertStep2, this.mnuEditStep2, this.mnuDeleteStep2, this.menuItem34, this.mnuIncludeInBuild2, this.mnuMoveUp2, this.mnuMoveDown2, this.mnuUnindent2, this.mnuIndent2 });
			this.listContextMenu.Name = "ListContextMenu";
			this.listContextMenu.Size = new Size(216, 380);
			this.listContextMenu.Opening += this.ListContextMenu_Opening;
			// 
			// mnuBuildSelectedStepsOnly2
			// 
			this.mnuBuildSelectedStepsOnly2.Image = Properties.Resources.BuildSelectedSteps;
			this.mnuBuildSelectedStepsOnly2.ImageTransparentColor = Color.Lime;
			this.mnuBuildSelectedStepsOnly2.MergeIndex = 15;
			this.mnuBuildSelectedStepsOnly2.Name = "mnuBuildSelectedStepsOnly2";
			this.mnuBuildSelectedStepsOnly2.Size = new Size(215, 22);
			this.mnuBuildSelectedStepsOnly2.Text = "Build &Selected Step(s) Only";
			this.mnuBuildSelectedStepsOnly2.Click += this.BuildSelectedStepOnly1_Click;
			// 
			// mnuBuildFromSelectedStep2
			// 
			this.mnuBuildFromSelectedStep2.Image = Properties.Resources.BuildFromSelectedStep;
			this.mnuBuildFromSelectedStep2.ImageTransparentColor = Color.Lime;
			this.mnuBuildFromSelectedStep2.MergeIndex = 16;
			this.mnuBuildFromSelectedStep2.Name = "mnuBuildFromSelectedStep2";
			this.mnuBuildFromSelectedStep2.Size = new Size(215, 22);
			this.mnuBuildFromSelectedStep2.Text = "Build &From Selected Step";
			this.mnuBuildFromSelectedStep2.Click += this.BuildFromSelectedStep1_Click;
			// 
			// mnuBuildToSelectedStep2
			// 
			this.mnuBuildToSelectedStep2.Image = Properties.Resources.BuildToSelectedStep;
			this.mnuBuildToSelectedStep2.ImageTransparentColor = Color.Lime;
			this.mnuBuildToSelectedStep2.MergeIndex = 17;
			this.mnuBuildToSelectedStep2.Name = "mnuBuildToSelectedStep2";
			this.mnuBuildToSelectedStep2.Size = new Size(215, 22);
			this.mnuBuildToSelectedStep2.Text = "Build T&o Selected Step";
			this.mnuBuildToSelectedStep2.Click += this.BuildToSelectedStep1_Click;
			// 
			// menuItem1
			// 
			this.menuItem1.MergeIndex = 18;
			this.menuItem1.Name = "menuItem1";
			this.menuItem1.Size = new Size(212, 6);
			// 
			// mnuGoToStepOutput2
			// 
			this.mnuGoToStepOutput2.Name = "mnuGoToStepOutput2";
			this.mnuGoToStepOutput2.Size = new Size(215, 22);
			this.mnuGoToStepOutput2.Text = "&Go To Step Output";
			this.mnuGoToStepOutput2.Click += this.GoToStepOutput_Click;
			// 
			// menuItem43
			// 
			this.menuItem43.MergeIndex = 14;
			this.menuItem43.Name = "menuItem43";
			this.menuItem43.Size = new Size(212, 6);
			// 
			// mnuCutStep2
			// 
			this.mnuCutStep2.Image = Properties.Resources.Cut;
			this.mnuCutStep2.ImageTransparentColor = Color.Fuchsia;
			this.mnuCutStep2.MergeIndex = 0;
			this.mnuCutStep2.Name = "mnuCutStep2";
			this.mnuCutStep2.Size = new Size(215, 22);
			this.mnuCutStep2.Text = "Cu&t Step";
			this.mnuCutStep2.Click += this.CutStep_Click;
			// 
			// mnuCopyStep2
			// 
			this.mnuCopyStep2.Image = Properties.Resources.Copy;
			this.mnuCopyStep2.ImageTransparentColor = Color.Fuchsia;
			this.mnuCopyStep2.MergeIndex = 1;
			this.mnuCopyStep2.Name = "mnuCopyStep2";
			this.mnuCopyStep2.Size = new Size(215, 22);
			this.mnuCopyStep2.Text = "&Copy Step";
			this.mnuCopyStep2.Click += this.CopyStep_Click;
			// 
			// mnuPasteStep2
			// 
			this.mnuPasteStep2.Image = Properties.Resources.Paste;
			this.mnuPasteStep2.ImageTransparentColor = Color.Fuchsia;
			this.mnuPasteStep2.MergeIndex = 2;
			this.mnuPasteStep2.Name = "mnuPasteStep2";
			this.mnuPasteStep2.Size = new Size(215, 22);
			this.mnuPasteStep2.Text = "&Paste Step";
			this.mnuPasteStep2.Click += this.PasteStep_Click;
			// 
			// menuItem13
			// 
			this.menuItem13.MergeIndex = 3;
			this.menuItem13.Name = "menuItem13";
			this.menuItem13.Size = new Size(212, 6);
			// 
			// mnuAddStep2
			// 
			this.mnuAddStep2.Image = Properties.Resources.AddStep;
			this.mnuAddStep2.ImageTransparentColor = Color.Lime;
			this.mnuAddStep2.MergeIndex = 4;
			this.mnuAddStep2.Name = "mnuAddStep2";
			this.mnuAddStep2.Size = new Size(215, 22);
			this.mnuAddStep2.Text = "&Add Step...";
			this.mnuAddStep2.Click += this.AddStep1_Click;
			// 
			// mnuInsertStep2
			// 
			this.mnuInsertStep2.Image = Properties.Resources.InsertStep;
			this.mnuInsertStep2.ImageTransparentColor = Color.Lime;
			this.mnuInsertStep2.MergeIndex = 5;
			this.mnuInsertStep2.Name = "mnuInsertStep2";
			this.mnuInsertStep2.Size = new Size(215, 22);
			this.mnuInsertStep2.Text = "&Insert Step...";
			this.mnuInsertStep2.Click += this.InsertStep1_Click;
			// 
			// mnuEditStep2
			// 
			this.mnuEditStep2.Image = Properties.Resources.EditStep;
			this.mnuEditStep2.ImageTransparentColor = Color.Lime;
			this.mnuEditStep2.MergeIndex = 6;
			this.mnuEditStep2.Name = "mnuEditStep2";
			this.mnuEditStep2.Size = new Size(215, 22);
			this.mnuEditStep2.Text = "&Edit Step...";
			this.mnuEditStep2.Click += this.EditStep1_Click;
			// 
			// mnuDeleteStep2
			// 
			this.mnuDeleteStep2.Image = Properties.Resources.DeleteStep;
			this.mnuDeleteStep2.ImageTransparentColor = Color.Lime;
			this.mnuDeleteStep2.MergeIndex = 7;
			this.mnuDeleteStep2.Name = "mnuDeleteStep2";
			this.mnuDeleteStep2.Size = new Size(215, 22);
			this.mnuDeleteStep2.Text = "&Delete Step...";
			this.mnuDeleteStep2.Click += this.DeleteStep1_Click;
			// 
			// menuItem34
			// 
			this.menuItem34.MergeIndex = 8;
			this.menuItem34.Name = "menuItem34";
			this.menuItem34.Size = new Size(212, 6);
			// 
			// mnuIncludeInBuild2
			// 
			this.mnuIncludeInBuild2.Image = Properties.Resources.IncludeInBuild;
			this.mnuIncludeInBuild2.ImageTransparentColor = Color.Fuchsia;
			this.mnuIncludeInBuild2.MergeIndex = 9;
			this.mnuIncludeInBuild2.Name = "mnuIncludeInBuild2";
			this.mnuIncludeInBuild2.Size = new Size(215, 22);
			this.mnuIncludeInBuild2.Text = "Include In &Build";
			this.mnuIncludeInBuild2.Click += this.IncludeInBuild1_Click;
			// 
			// mnuMoveUp2
			// 
			this.mnuMoveUp2.Image = Properties.Resources.MoveUp;
			this.mnuMoveUp2.ImageTransparentColor = Color.Fuchsia;
			this.mnuMoveUp2.MergeIndex = 10;
			this.mnuMoveUp2.Name = "mnuMoveUp2";
			this.mnuMoveUp2.Size = new Size(215, 22);
			this.mnuMoveUp2.Text = "Move &Up";
			this.mnuMoveUp2.Click += this.MoveUp1_Click;
			// 
			// mnuMoveDown2
			// 
			this.mnuMoveDown2.Image = Properties.Resources.MoveDown;
			this.mnuMoveDown2.ImageTransparentColor = Color.Fuchsia;
			this.mnuMoveDown2.MergeIndex = 11;
			this.mnuMoveDown2.Name = "mnuMoveDown2";
			this.mnuMoveDown2.Size = new Size(215, 22);
			this.mnuMoveDown2.Text = "Move Do&wn";
			this.mnuMoveDown2.Click += this.MoveDown1_Click;
			// 
			// mnuUnindent2
			// 
			this.mnuUnindent2.Image = Properties.Resources.MoveLeft;
			this.mnuUnindent2.ImageTransparentColor = Color.Fuchsia;
			this.mnuUnindent2.MergeIndex = 12;
			this.mnuUnindent2.Name = "mnuUnindent2";
			this.mnuUnindent2.Size = new Size(215, 22);
			this.mnuUnindent2.Text = "Uninden&t";
			this.mnuUnindent2.Click += this.Unindent1_Click;
			// 
			// mnuIndent2
			// 
			this.mnuIndent2.Image = Properties.Resources.MoveRight;
			this.mnuIndent2.ImageTransparentColor = Color.Fuchsia;
			this.mnuIndent2.MergeIndex = 13;
			this.mnuIndent2.Name = "mnuIndent2";
			this.mnuIndent2.Size = new Size(215, 22);
			this.mnuIndent2.Text = "Inde&nt";
			this.mnuIndent2.Click += this.Indent1_Click;
			// 
			// tabFailureSteps
			// 
			this.tabFailureSteps.Controls.Add(this.lstFailureSteps);
			this.tabFailureSteps.ImageIndex = 1;
			this.tabFailureSteps.Location = new Point(4, 24);
			this.tabFailureSteps.Name = "tabFailureSteps";
			this.tabFailureSteps.Size = new Size(166, 325);
			this.tabFailureSteps.TabIndex = 1;
			this.tabFailureSteps.Text = "Failure Steps";
			this.tabFailureSteps.Visible = false;
			// 
			// lstFailureSteps
			// 
			this.lstFailureSteps.CheckBoxes = true;
			this.lstFailureSteps.Columns.AddRange(new ColumnHeader[] { this.colFStep, this.colFType, this.colFIgnoreFailure, this.colFConfirm, this.colFInfo, this.colFRunTime, this.colFStatus, this.colFDescription });
			this.lstFailureSteps.ContextMenuStrip = this.listContextMenu;
			this.lstFailureSteps.Dock = DockStyle.Fill;
			this.lstFailureSteps.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			this.lstFailureSteps.Location = new Point(0, 0);
			this.lstFailureSteps.MultiSelect = true;
			this.lstFailureSteps.Name = "lstFailureSteps";
			this.lstFailureSteps.Size = new Size(166, 325);
			this.lstFailureSteps.SmallImageList = this.images;
			this.lstFailureSteps.TabIndex = 1;
			this.lstFailureSteps.UseCompatibleStateImageBehavior = false;
			this.lstFailureSteps.ItemCheck += this.Steps_ItemCheck;
			this.lstFailureSteps.DoubleClick += this.Steps_DoubleClick;
			// 
			// colFStep
			// 
			this.colFStep.Text = "Step";
			// 
			// colFType
			// 
			this.colFType.Text = "Type";
			// 
			// colFIgnoreFailure
			// 
			this.colFIgnoreFailure.Text = "Ignore Failure";
			this.colFIgnoreFailure.Width = 100;
			// 
			// colFConfirm
			// 
			this.colFConfirm.Text = "Confirm";
			// 
			// colFInfo
			// 
			this.colFInfo.Text = "Step Info";
			// 
			// colFRunTime
			// 
			this.colFRunTime.Text = "Step Time";
			// 
			// colFStatus
			// 
			this.colFStatus.Text = "Status";
			// 
			// colFDescription
			// 
			this.colFDescription.Text = "Description";
			this.colFDescription.Width = 260;
			// 
			// outputContextMenu
			// 
			this.outputContextMenu.Items.AddRange(new ToolStripItem[] { this.mnuCopyOutput, this.mnuSelectAllOutput, this.menuItem6, this.mnuClearAll, this.menuItem3, this.mnuSaveOutputAs2, this.toolStripMenuItem2, this.mnuGoToPreviousHighlight2, this.mnuGoToNextHighlight2 });
			this.outputContextMenu.Name = "OutputContextMenu";
			this.outputContextMenu.Size = new Size(207, 154);
			this.outputContextMenu.Opened += this.OnIdle;
			// 
			// mnuCopyOutput
			// 
			this.mnuCopyOutput.MergeIndex = 0;
			this.mnuCopyOutput.Name = "mnuCopyOutput";
			this.mnuCopyOutput.Size = new Size(206, 22);
			this.mnuCopyOutput.Text = "&Copy";
			this.mnuCopyOutput.Click += this.CopyOutput_Click;
			// 
			// mnuSelectAllOutput
			// 
			this.mnuSelectAllOutput.MergeIndex = 1;
			this.mnuSelectAllOutput.Name = "mnuSelectAllOutput";
			this.mnuSelectAllOutput.Size = new Size(206, 22);
			this.mnuSelectAllOutput.Text = "&Select All";
			this.mnuSelectAllOutput.Click += this.SelectAll_Click;
			// 
			// menuItem6
			// 
			this.menuItem6.MergeIndex = 2;
			this.menuItem6.Name = "menuItem6";
			this.menuItem6.Size = new Size(203, 6);
			// 
			// mnuClearAll
			// 
			this.mnuClearAll.MergeIndex = 3;
			this.mnuClearAll.Name = "mnuClearAll";
			this.mnuClearAll.Size = new Size(206, 22);
			this.mnuClearAll.Text = "C&lear All";
			this.mnuClearAll.Click += this.ClearOutputWindow_Click;
			// 
			// menuItem3
			// 
			this.menuItem3.MergeIndex = 4;
			this.menuItem3.Name = "menuItem3";
			this.menuItem3.Size = new Size(203, 6);
			// 
			// mnuSaveOutputAs2
			// 
			this.mnuSaveOutputAs2.MergeIndex = 5;
			this.mnuSaveOutputAs2.Name = "mnuSaveOutputAs2";
			this.mnuSaveOutputAs2.Size = new Size(206, 22);
			this.mnuSaveOutputAs2.Text = "Sa&ve Output As...";
			this.mnuSaveOutputAs2.Click += this.SaveOutputAs_Click;
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new Size(203, 6);
			// 
			// mnuGoToPreviousHighlight2
			// 
			this.mnuGoToPreviousHighlight2.Image = Properties.Resources.GoToPreviousHighlight;
			this.mnuGoToPreviousHighlight2.ImageTransparentColor = Color.Fuchsia;
			this.mnuGoToPreviousHighlight2.Name = "mnuGoToPreviousHighlight2";
			this.mnuGoToPreviousHighlight2.Size = new Size(206, 22);
			this.mnuGoToPreviousHighlight2.Text = "Go To Pre&vious Highlight";
			this.mnuGoToPreviousHighlight2.Click += this.GoToPreviousHighlight_Click;
			// 
			// mnuGoToNextHighlight2
			// 
			this.mnuGoToNextHighlight2.Image = Properties.Resources.GoToNextHighlight;
			this.mnuGoToNextHighlight2.ImageTransparentColor = Color.Fuchsia;
			this.mnuGoToNextHighlight2.Name = "mnuGoToNextHighlight2";
			this.mnuGoToNextHighlight2.Size = new Size(206, 22);
			this.mnuGoToNextHighlight2.Text = "Go To Ne&xt Highlight";
			this.mnuGoToNextHighlight2.Click += this.GoToNextHighlight_Click;
			// 
			// formSaver
			// 
			this.formSaver.ContainerControl = this;
			this.formSaver.LoadSettings += this.FormSave_LoadSettings;
			this.formSaver.SaveSettings += this.FormSave_SaveSettings;
			// 
			// recentFiles
			// 
			this.recentFiles.ContextMenuStrip = this.recentFilesContextMenu;
			this.recentFiles.FormSaver = this.formSaver;
			this.recentFiles.MenuItem = this.mnuRecentFiles;
			this.recentFiles.SettingsNodeName = "Recent Files";
			this.recentFiles.ItemClick += this.RecentFiles_FileClick;
			// 
			// openProjectDlg
			// 
			this.openProjectDlg.DefaultExt = "mgb";
			this.openProjectDlg.Filter = "MegaBuild Files (*.mgb)|*.mgb|All Files (*.*)|*.*";
			this.openProjectDlg.Title = "Open Project";
			// 
			// saveProjectDlg
			// 
			this.saveProjectDlg.DefaultExt = "mgb";
			this.saveProjectDlg.FileName = "Project";
			this.saveProjectDlg.Filter = "MegaBuild Files (*.mgb)|*.mgb|All Files (*.*)|*.*";
			this.saveProjectDlg.Title = "Save Project As";
			// 
			// saveOutputDlg
			// 
			this.saveOutputDlg.DefaultExt = "rtf";
			this.saveOutputDlg.FileName = "Output";
			this.saveOutputDlg.Filter = "RTF Files (*.rtf)|*.rtf|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			this.saveOutputDlg.Title = "Save Output As";
			// 
			// buildTimer
			// 
			this.buildTimer.Interval = 1000;
			this.buildTimer.Tick += this.BuildTimer_Tick;
			// 
			// Splitter
			// 
			this.Splitter.Dock = DockStyle.Fill;
			this.Splitter.Location = new Point(0, 53);
			this.Splitter.Name = "Splitter";
			// 
			// Splitter.Panel1
			// 
			this.Splitter.Panel1.Controls.Add(this.pnlList);
			// 
			// Splitter.Panel2
			// 
			this.Splitter.Panel2.Controls.Add(this.outputWindow);
			this.Splitter.Size = new Size(616, 357);
			this.Splitter.SplitterDistance = 178;
			this.Splitter.TabIndex = 6;
			// 
			// outputWindow
			// 
			this.outputWindow.ContextMenuStrip = this.outputContextMenu;
			this.outputWindow.Dock = DockStyle.Fill;
			this.outputWindow.Font = new Font("Segoe UI", 9F);
			this.outputWindow.Location = new Point(0, 0);
			this.outputWindow.Margin = new Padding(4, 3, 4, 3);
			this.outputWindow.Name = "outputWindow";
			this.outputWindow.OwnerWindow = this;
			this.outputWindow.RemoveLinePrefix = null;
			this.outputWindow.Size = new Size(434, 357);
			this.outputWindow.TabIndex = 0;
			// 
			// status
			// 
			this.status.Items.AddRange(new ToolStripItem[] { this.spName, this.spProgress, this.spTime });
			this.status.Location = new Point(0, 410);
			this.status.Name = "status";
			this.status.Size = new Size(616, 27);
			this.status.TabIndex = 7;
			this.status.Text = "statusStrip1";
			// 
			// spName
			// 
			this.spName.Name = "spName";
			this.spName.Size = new Size(237, 22);
			this.spName.Spring = true;
			this.spName.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// spProgress
			// 
			this.spProgress.AutoSize = false;
			this.spProgress.Name = "spProgress";
			this.spProgress.Size = new Size(192, 21);
			// 
			// spTime
			// 
			this.spTime.AutoSize = false;
			this.spTime.Name = "spTime";
			this.spTime.Size = new Size(170, 22);
			this.spTime.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// project
			// 
			this.project.Form = this;
			this.project.OpenFileDialog = this.openProjectDlg;
			this.project.RecentFiles = this.recentFiles;
			this.project.SaveFileDialog = this.saveProjectDlg;
			this.project.BuildFailed += this.Project_BuildFailed;
			this.project.BuildProgress += this.Project_BuildProgress;
			this.project.BuildStarted += this.Project_BuildStarted;
			this.project.BuildStarting += this.Project_BuildStarting;
			this.project.BuildStopped += this.Project_BuildStopped;
			this.project.ContentsReset += this.Project_ContentsReset;
			this.project.ContentsResetting += this.Project_ContentsResetting;
			this.project.DisplayComments += this.Project_DisplayComments;
			this.project.FileNameSet += this.Project_FileNameSet;
			this.project.ModifiedChanged += this.Project_ModifiedChanged;
			this.project.ProjectStepsChanged += this.Project_ProjectStepsChanged;
			this.project.RecentFileAdded += this.Project_RecentFileAdded;
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new SizeF(7F, 15F);
			this.ClientSize = new Size(616, 437);
			this.Controls.Add(this.Splitter);
			this.Controls.Add(this.toolbar);
			this.Controls.Add(this.mainMenuInst);
			this.Controls.Add(this.status);
			this.Icon = (Icon)resources.GetObject("$this.Icon");
			this.MainMenuStrip = this.mainMenuInst;
			this.MinimumSize = new Size(538, 409);
			this.Name = "MainForm";
			this.Text = "MegaBuild";
			this.Activated += this.MainForm_Activated;
			this.Closing += this.MainForm_Closing;
			this.Closed += this.MainForm_Closed;
			this.DragDrop += this.MainForm_DragDrop;
			this.DragEnter += this.MainForm_DragEnter;
			this.mainMenuInst.ResumeLayout(false);
			this.mainMenuInst.PerformLayout();
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.pnlList.ResumeLayout(false);
			this.TabCtrl.ResumeLayout(false);
			this.tabBuildSteps.ResumeLayout(false);
			this.listContextMenu.ResumeLayout(false);
			this.tabFailureSteps.ResumeLayout(false);
			this.outputContextMenu.ResumeLayout(false);
			this.Splitter.Panel1.ResumeLayout(false);
			this.Splitter.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.Splitter).EndInit();
			this.Splitter.ResumeLayout(false);
			this.status.ResumeLayout(false);
			this.status.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		#endregion

		private ToolStripSeparator toolStripMenuItem4;
		private ToolStripMenuItem mnuOpenProjectFolderInExplorer;
		private ToolStripMenuItem mnuOpenProjectFolderInTerminal;
	}
}


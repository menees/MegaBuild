namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Threading;
	using System.Windows.Forms;
	using System.Xml;
	using System.Xml.Linq;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	public sealed class Project : Component
	{
		#region Private Data Members

		private const string CopiedStepFormat = "Menees.MegaBuild.CopiedSteps";
		private const decimal ProjectVersion = 5M;
		private readonly int baseIndentLevel;

		private readonly StepCollection buildSteps = new();
		private readonly Dictionary<string, string> cachedVariables = new(StringComparer.CurrentCultureIgnoreCase);
		private readonly object[] eventHandlerParams;
		private readonly StepCollection failureSteps = new();

		// Build Data
		private DateTime buildStart;
		private Thread buildThread;
		private string comments = string.Empty;

		// Runtime-only data
		private System.ComponentModel.Container components;
		private ExecutableStep[] executableBuildSteps;
		private ExecutableStep[] executableFailureSteps;
		private StepExecuteArgs executeArgs;

		// Persisted data and options
		// Note: If you add a data member here, then make sure you
		// reset it in the Close() method so New() will work correctly.
		private string fileName = string.Empty;
		private int indentOutput;
		private bool insertingStep;
		private bool loading;
		private string logFile = string.Empty;
		private bool logOutput;
		private bool logTimestamp = true;
		private bool modified;
		private VSConfigurationList overrideConfigurations = new();
		private VSAction overrideVSAction = VSAction.Build;
		private bool overrideVSActions;
		private bool overrideVSStepConfigurations;
		private VSVersion overrideVSVersion = VSVersionInfo.LatestVersion.Version;
		private bool overrideVSVersions;
		private bool overwriteLog = true;
		private bool showComments;
		private bool showDebugOutput;
		private BuildStatus status = BuildStatus.None;
		private List<VariableDefinition> variableDefinitions = new();

		#endregion

		#region Constructors

		public Project(System.ComponentModel.IContainer container)
		{
			container.Add(this);
			this.InitializeComponent();
			this.eventHandlerParams = new object[] { this, EventArgs.Empty };
			Manager.AddProject(this);
		}

		public Project()
			: this(0)
		{
		}

		internal Project(int baseIndentLevel)
		{
			this.InitializeComponent();
			this.eventHandlerParams = new object[] { this, EventArgs.Empty };
			this.baseIndentLevel = baseIndentLevel;
			Manager.AddProject(this);
		}

		#endregion

		#region Public Events

		public event EventHandler BuildFailed;

		public event EventHandler<BuildProgressEventArgs> BuildProgress;

		public event EventHandler BuildStarted;

		public event CancelEventHandler BuildStarting;

		public event EventHandler BuildStopped;

		public event EventHandler ContentsReset;

		public event EventHandler ContentsResetting;

		public event EventHandler DisplayComments;

		public event EventHandler FileNameSet;

		public event EventHandler ModifiedChanged;

		public event EventHandler<ProjectStepsChangedEventArgs> ProjectStepsChanged;

		public event EventHandler RecentFileAdded;

		#endregion

		#region Public Run-Time Properties

		[Browsable(false)]
		public static bool CanPasteSteps
		{
			get
			{
				bool result = false;

				try
				{
					// In rare cases, the clipboard won't be accessible, so we have to
					// protect against that.  CanPasteSteps is called from the OnIdle
					// handler, so if another application has locked the clipboard and
					// the user passes the mouse over MegaBuild, it will call into here.
					// The clipboard is usually only locked briefly, but from idle
					// handlers you may hit it occasionally while it is locked.  So
					// CanPasteSteps should just eat the exception and return false.
					// If the clipboard is still locked when Paste is called, then that
					// should throw the exception on up to the caller.
					IDataObject data = Clipboard.GetDataObject();
					if (data != null)
					{
						result = data.GetDataPresent(CopiedStepFormat);
					}
				}
#pragma warning disable CC0004 // Catch block cannot be empty
				catch (ExternalException)
				{
				}
#pragma warning restore CC0004 // Catch block cannot be empty

				return result;
			}
		}

		[Browsable(false)]
		public bool Building => this.buildThread != null;

		[Browsable(false)]
		public DateTime BuildStart => this.buildStart;

		[Browsable(false)]
		public BuildStatus BuildStatus => this.status;

		[Browsable(false)]
		public StepCollection BuildSteps => this.buildSteps;

		[Browsable(false)]
		public string Comments => this.comments;

		[Browsable(false)]
		public StepCollection FailureSteps => this.failureSteps;

		[Browsable(false)]
		public string FileName
		{
			get => this.fileName;

			set
			{
				// Don't check for changes.  We always want to
				// refire the event.  This ensures notifications
				// go out even if someone does New(), New(), New()...
				this.fileName = value;

				// Clear the cached variables since the values can depend on $(ProjectDir).
				this.cachedVariables.Clear();
				this.FireFileNameSet();
			}
		}

		[Browsable(false)]
		public bool Modified
		{
			get => this.modified;

			set
			{
				if (this.modified != value)
				{
					this.modified = value;
					this.FireModifiedChanged();
				}
			}
		}

		[Browsable(false)]
		public bool StopBuilding
		{
			get
			{
				bool result = this.status != BuildStatus.Started && this.status != MegaBuild.BuildStatus.Failing;
				return result;
			}
		}

		[Browsable(false)]
		[Description("The current file title (e.g. name with no path).")]
		public string Title
			=> this.fileName.Length == 0 ? "<Untitled>" : Path.GetFileName(this.fileName);

		[Browsable(false)]
		public IDictionary<string, string> Variables
		{
			get
			{
				if (this.cachedVariables.Count != this.variableDefinitions.Count)
				{
					this.cachedVariables.Clear();

					// Currently, we only support the $(ProjectDir) token.
					string projectDir = Path.GetDirectoryName(this.FileName);
					if (!string.IsNullOrEmpty(projectDir) && projectDir.EndsWith(@"\"))
					{
						projectDir = projectDir.Substring(0, projectDir.Length - 1);
					}

					foreach (VariableDefinition definition in this.variableDefinitions)
					{
						string value = definition.Value;
						if (!string.IsNullOrEmpty(value))
						{
							if (!string.IsNullOrEmpty(projectDir))
							{
								value = TextUtility.Replace(value, "$(ProjectDir)", projectDir, StringComparison.CurrentCultureIgnoreCase);
								if (definition.ExpandPath)
								{
									value = Path.GetFullPath(value);
								}
							}
						}

						this.cachedVariables[definition.Name] = value;
					}
				}

				return this.cachedVariables;
			}
		}

		[Browsable(false)]
		public bool ShowDebugOutput => this.showDebugOutput;

		#endregion

		#region Public Design-Time Properties

		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Helper Objects")]
		[Description("The form that asynchronous events should be invoked on.")]
		public Form Form { get; set; }

		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Helper Objects")]
		[Description("The dialog to use when opening a file.")]
		public OpenFileDialog OpenFileDialog { get; set; }

		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Helper Objects")]
		[Description("The recent file list manager.")]
		public RecentItemList RecentFiles { get; set; }

		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Helper Objects")]
		[Description("The dialog to use when saving a file.")]
		public SaveFileDialog SaveFileDialog { get; set; }

		#endregion

		#region Internal Properties

		internal VSConfigurationList OverrideConfigurations => this.overrideConfigurations;

		internal VSAction OverrideVSAction => this.overrideVSAction;

		internal bool OverrideVSActions => this.overrideVSActions;

		internal bool OverrideVSStepConfigurations => this.overrideVSStepConfigurations;

		internal VSVersion OverrideVSVersion => this.overrideVSVersion;

		internal bool OverrideVSVersions => this.overrideVSVersions;

		#endregion

		#region Public Methods

		public static void CopySteps(Step[] steps)
		{
			// Put all the steps into a new collection.
			StepCollection stepColl = new();
			foreach (Step step in steps)
			{
				stepColl.Add(step);
			}

			// Save the new collection into an XML document.
			XElement doc = new("MegaBuildCopiedSteps", new XAttribute("CopyVersion", ProjectVersion));
			XmlKey docKey = new(doc);
			stepColl.Save(docKey.GetSubkey("Steps", "CopiedSteps"));

			// Get the XML text.
			string stepXML = doc.ToString();

			// Now register and get a custom data format.
			DataFormats.Format format = DataFormats.GetFormat(CopiedStepFormat);

			// Create a new data object in our format to hold the XML.
			DataObject data = new(format.Name, stepXML);

			// Just to be nice, also store the data in text format.
			// This is useful during testing, and it may be useful to others.
			data.SetData(DataFormats.UnicodeText, true, stepXML);

			// Then copy the data to the clipboard in this format.
			Clipboard.SetDataObject(data, true);
		}

		public void Build(Step[] steps, bool forceIncludeInBuild, StepExecuteArgs args)
		{
			var options = forceIncludeInBuild ? BuildOptions.ForceStepsToBeIncludedInBuild : BuildOptions.None;
			this.Build(steps, options, args);
		}

		public void Build(Step[] steps, BuildOptions options, StepExecuteArgs args)
		{
			// Don't try to do two builds at once.  Then fire the Starting event before anything else.
			// Even if we end up not building anything, we need to fire this event.
			if (!this.Building && this.FireBuildStarting())
			{
				// Store the execute args and make sure they're non-null.
				this.executeArgs = args;
				if (this.executeArgs == null)
				{
					this.executeArgs = StepExecuteArgs.Empty;
				}

				// If the user selectively built different steps last time,
				// make sure we reset statuses for all steps.
				this.ResetStatuses();

				if (steps.Length > 0)
				{
					bool forceIncludeInBuild = (options & BuildOptions.ForceStepsToBeIncludedInBuild) != 0;
					GetExecutableSteps(steps, forceIncludeInBuild, out ExecutableStep[] stepsToBuild, out ExecutableStep[] stepsToConfirm);
					Step[] failureSteps = new Step[this.failureSteps.Count];
					this.failureSteps.CopyTo(failureSteps);
					GetExecutableSteps(failureSteps, false, out ExecutableStep[] failureStepsToBuild, out ExecutableStep[] failureStepsToConfirm);

					// Confirm Steps before build starts (i.e., while we're still in the foreground thread).
					bool build = true;
					if (stepsToConfirm.Length > 0 || failureStepsToConfirm.Length > 0)
					{
						using (ConfirmStepsDlg dialog = new())
						{
							bool autoConfirm = (options & BuildOptions.AutoConfirmSteps) != 0;
							if (autoConfirm || dialog.Execute(this.Form, ref stepsToConfirm, ref failureStepsToConfirm))
							{
								// Combine confirmed steps and build steps.
								stepsToBuild = this.CombineConfirmedSteps(StepCategory.Build, stepsToBuild, stepsToConfirm);
								failureStepsToBuild = this.CombineConfirmedSteps(StepCategory.Failure, failureStepsToBuild, failureStepsToConfirm);
							}
							else
							{
								// They canceled the Confirm dialog, so cancel the build.
								build = false;
							}
						}
					}

					if (build)
					{
						// Set member variables to hold the steps to build.
						// These will be accessed by the background thread.
						this.executableBuildSteps = stepsToBuild;
						this.executableFailureSteps = failureStepsToBuild;

						if (this.executableBuildSteps.Length > 0)
						{
							this.buildThread = new Thread(new ThreadStart(this.ExecuteBackgroundBuild));
							this.buildThread.IsBackground = true;
							this.buildThread.Name = "BuildThread-" + this.Title;
							this.buildThread.Start();
						}
						else
						{
							this.executableBuildSteps = null;
							this.executableFailureSteps = null;
						}
					}
				}
			}
		}

		public DialogResult CanClose()
		{
			DialogResult result;

			if (this.Modified)
			{
				string prompt = string.Format("Do you want to save the changes to \"{0}\"?", this.Title);
				result = MessageBox.Show(prompt, "Save Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

				if (result == DialogResult.Yes)
				{
					// They said "Yes" to saving changes.
					result = this.Save(false);

					// Save() will return Yes, No, or Cancel. If Save returns "No" we want to treat it like Cancel
					// since the user previously said they wanted to save changes.
					if (result != DialogResult.Yes)
					{
						result = DialogResult.Cancel;
					}
				}
				else if (result == DialogResult.No)
				{
					// If they said "No" to saving changes, then that means "Yes" to CanClose.
					result = DialogResult.Yes;
				}
			}
			else
			{
				result = DialogResult.Yes;
			}

			return result;
		}

		public void Close()
		{
			this.FireContentsResetting();

			// Clear contents
			this.buildSteps.Clear();
			this.failureSteps.Clear();
			this.FileName = string.Empty;

			// Reset the Project Options
			this.overrideVSActions = false;
			this.overrideVSAction = VSAction.Build;
			this.overrideVSVersions = false;
			this.overrideVSVersion = VSVersionInfo.LatestVersion.Version;
			this.overrideVSStepConfigurations = false;
			this.overrideConfigurations = new VSConfigurationList();
			this.logOutput = false;
			this.logFile = string.Empty;
			this.overwriteLog = true;
			this.logTimestamp = true;
			this.comments = string.Empty;
			this.showComments = false;
			this.variableDefinitions.Clear();
			this.cachedVariables.Clear();

			this.Modified = false;

			this.FireContentsReset();
		}

		public void CutSteps(Step[] steps)
		{
			CopySteps(steps);

			foreach (Step step in steps)
			{
				this.DeleteStep(step);
			}
		}

		public void DeleteStep(Step step)
		{
			int oldIndex = step.GetIndex();
			StepCollection steps = step.CategorySteps;
			steps.Remove(step);

			// Make sure this wasn't any step's parent.
			foreach (Step stepEntry in steps)
			{
				if (stepEntry.Parent == step)
				{
					stepEntry.SetParent();
				}
			}

			this.Modified = true;
			this.FireChanged(ProjectStepsChangedType.StepDeleted, step, oldIndex, -1);
		}

		public void DisplayOptions(IWin32Window owner)
		{
			using (ProjectOptionsDlg dialog = new())
			{
				dialog.chkOverrideActions.Checked = this.overrideVSActions;
				dialog.chkOverrideVersions.Checked = this.overrideVSVersions;
				dialog.chkOverrideConfigurations.Checked = this.overrideVSStepConfigurations;
				dialog.cbAction.SelectedIndex = (int)this.overrideVSAction;
				dialog.cbVersion.SelectedIndex = (int)this.overrideVSVersion;

				// Get all of the configurations for all of the VS solutions in the project.
				VSConfigurationList allConfigurations = this.GetAllConfigurations();
				allConfigurations.PutInListView(dialog.lstConfigurations);

				dialog.chkLogOutput.Checked = this.logOutput;
				dialog.edtLogFile.Text = this.logFile;
				dialog.chkOverwriteLog.Checked = this.overwriteLog;
				dialog.chkTimestamp.Checked = this.logTimestamp;
				dialog.txtComments.Text = this.comments;
				dialog.chkShowComments.Checked = this.showComments;
				dialog.chkShowDebugOutput.Checked = this.showDebugOutput;
				dialog.LoadVariables(this.variableDefinitions);

				if (dialog.ShowDialog(owner) == DialogResult.OK)
				{
					int optionsChanged = 0;

					optionsChanged += ChangeOption(ref this.overrideVSActions, dialog.chkOverrideActions.Checked);
					if (this.overrideVSActions)
					{
						optionsChanged += ChangeOption(ref this.overrideVSAction, (VSAction)dialog.cbAction.SelectedIndex);
					}

					optionsChanged += ChangeOption(ref this.overrideVSVersions, dialog.chkOverrideVersions.Checked);
					if (this.overrideVSVersions)
					{
						optionsChanged += ChangeOption(ref this.overrideVSVersion, (VSVersion)dialog.cbVersion.SelectedIndex);
					}

					optionsChanged += ChangeOption(ref this.overrideVSStepConfigurations, dialog.chkOverrideConfigurations.Checked);
					if (this.overrideVSStepConfigurations && this.overrideConfigurations.GetFromListView(dialog.lstConfigurations))
					{
						optionsChanged++;
					}

					optionsChanged += ChangeOption(ref this.logOutput, dialog.chkLogOutput.Checked);
					if (this.logOutput)
					{
						optionsChanged += ChangeOption(ref this.logFile, dialog.edtLogFile.Text.Trim());
						optionsChanged += ChangeOption(ref this.overwriteLog, dialog.chkOverwriteLog.Checked);
						optionsChanged += ChangeOption(ref this.logTimestamp, dialog.chkTimestamp.Checked);
					}

					optionsChanged += ChangeOption(ref this.comments, dialog.txtComments.Text);
					optionsChanged += ChangeOption(ref this.showComments, dialog.chkShowComments.Checked);
					optionsChanged += ChangeOption(ref this.showDebugOutput, dialog.chkShowDebugOutput.Checked);

					if (dialog.SaveVariables(this.variableDefinitions))
					{
						this.cachedVariables.Clear();
						optionsChanged++;
					}

					if (optionsChanged > 0)
					{
						this.Modified = true;

						foreach (Step step in this.buildSteps)
						{
							step.ProjectOptionsChanged();
						}

						foreach (Step step in this.failureSteps)
						{
							step.ProjectOptionsChanged();
						}
					}
				}
			}
		}

		public bool EditStep(IWin32Window owner, Step step)
		{
			// When any of the step's properties are changed, it will cause a change notification
			// to be sent once all of the properties are done being changed.
			using (StepEditorDlg dialog = new())
			{
				return dialog.Execute(owner, step, this.insertingStep);
			}
		}

		public Step InsertStep(IWin32Window owner, string caption, StepCategory category, int index)
		{
			Step result = null;

			// Choose a step type
			using (StepTypeDlg typeDlg = new())
			{
				typeDlg.Text = caption;
				if (typeDlg.Execute(owner, out StepTypeInfo info))
				{
					// Create an instance of the step type
					Step step = this.CreateStep(category, info);

					// Set a flag to indicate that StepEdited shouldn't fire
					// since we're adding the step.
					this.insertingStep = true;
					try
					{
						// Edit the new step
						if (this.EditStep(owner, step))
						{
							StepCollection steps = step.CategorySteps;

							// Set the level to the same as the level of the current
							// step at the given index.  Or if the insertion is at the
							// end, then use the previous step's level.
							int count = steps.Count;
							int level = 0;
							if (index >= 0 && index < count)
							{
								level = steps[index].Level;
							}
							else if (count > 0 && index == count)
							{
								level = steps[index - 1].Level;
							}

							// Add the step to the collection.
							steps.Insert(index, step);

							// Now that the step is in the collection, we can set the level.
							// It has to be in the collection so it can find its parent correctly.
							step.SetLevel(level);

							// Editing the new step should have set this to true,
							// but we'll call it again to make sure.
							this.Modified = true;

							this.FireChanged(ProjectStepsChangedType.StepInserted, step, -1, index);

							result = step;
						}
					}
					finally
					{
						this.insertingStep = false;
					}
				}
			}

			return result;
		}

		public bool MoveDown(Step step)
		{
			bool result = false;

			StepCollection steps = step.CategorySteps;
			int oldIndex = step.GetIndex();
			if (oldIndex < (steps.Count - 1))
			{
				steps.RemoveAt(oldIndex);
				int newIndex = oldIndex + 1;
				steps.Insert(newIndex, step);
				step.SetParent();
				this.Modified = true;
				this.FireChanged(ProjectStepsChangedType.StepMoved, step, oldIndex, newIndex);
				result = true;
			}

			return result;
		}

		public bool MoveUp(Step step)
		{
			bool result = false;

			int oldIndex = step.GetIndex();
			if (oldIndex > 0)
			{
				StepCollection steps = step.CategorySteps;
				steps.RemoveAt(oldIndex);
				int newIndex = oldIndex - 1;
				steps.Insert(newIndex, step);
				step.SetParent();
				this.Modified = true;
				this.FireChanged(ProjectStepsChangedType.StepMoved, step, oldIndex, newIndex);
				result = true;
			}

			return result;
		}

		public bool New()
		{
			bool result = false;

			if (this.CanClose() == DialogResult.Yes)
			{
				this.Close();
				result = true;
			}

			return result;
		}

		public bool Open()
		{
			bool result = false;

			if (this.OpenFileDialog != null && this.OpenFileDialog.ShowDialog() == DialogResult.OK)
			{
				result = this.Open(this.OpenFileDialog.FileName);
			}

			return result;
		}

		public bool Open(string fileName)
		{
			bool result = false;

			if (File.Exists(fileName) && this.New())
			{
				// Do this first so "Title" will return the correct name
				// in the catch block.
				this.FileName = fileName;

				try
				{
					// We don't need to call FireContentsResetting() here
					// because New() has already been called above.
					//
					// Get the expanded name to store in the recent files list.
					this.FileName = FileUtility.ExpandFileName(fileName);

					// Load the project from a file
					XElement doc = XElement.Load(this.FileName);
					XmlKey docKey = new(doc);

					// Check the project file version.
					string version = docKey.GetValue("ProjectVersion", string.Empty);

					// We have to use decimal because MegaBuild 1.0 used "1.0".
					if (!decimal.TryParse(version, out decimal versionNumber))
					{
						throw new ArgumentException("The selected file is not a valid MegaBuild project file.");
					}

					if (versionNumber > ProjectVersion)
					{
						throw new ArgumentException(
							"The selected MegaBuild project uses a file format from a newer version of MegaBuild.  " +
							"Please upgrade to the latest version of MegaBuild.");
					}

					// Set flags so edited and inserted events don't fire.
					this.loading = true;
					try
					{
						// Load options first so load errors can be logged correctly (if necessary).
						XmlKey optionsKey = docKey.GetSubkey("Options");
						this.overrideVSStepConfigurations = optionsKey.GetValue(nameof(this.OverrideVSStepConfigurations), this.overrideVSStepConfigurations);
						this.logOutput = optionsKey.GetValue("LogOutput", this.logOutput);
						this.overwriteLog = optionsKey.GetValue("OverwriteLog", this.overwriteLog);
						this.logTimestamp = optionsKey.GetValue("LogTimestamp", this.logTimestamp);
						this.showComments = optionsKey.GetValue("ShowComments", this.showComments);
						this.showDebugOutput = optionsKey.GetValue(nameof(this.ShowDebugOutput), this.showDebugOutput);
						this.logFile = optionsKey.GetValue("LogFile", this.logFile);
						this.comments = optionsKey.GetValue(nameof(this.Comments), this.comments);
						this.overrideConfigurations.Load(optionsKey.GetSubkey(nameof(this.OverrideConfigurations)));
						this.overrideVSActions = optionsKey.GetValue(nameof(this.OverrideVSActions), this.overrideVSActions);
						this.overrideVSVersions = optionsKey.GetValue(nameof(this.OverrideVSVersions), this.overrideVSVersions);
						this.overrideVSAction = optionsKey.GetValue("OverrideVSActionValue", this.overrideVSAction);
						this.overrideVSVersion = optionsKey.GetValue("OverrideVSVersionValue", this.overrideVSVersion);
						this.variableDefinitions = VariableDefinition.Load(optionsKey);

						this.buildSteps.Load(docKey.GetSubkey("Steps", nameof(this.BuildSteps)), this, StepCategory.Build);
						this.failureSteps.Load(docKey.GetSubkey("Steps", nameof(this.FailureSteps)), this, StepCategory.Failure);
					}
					finally
					{
						this.loading = false;
					}

					this.AddToRecentFiles();

					this.Modified = false;
					result = true;

					this.FireContentsReset();

					if (this.showComments && this.comments.Length > 0)
					{
						this.FireDisplayComments();
					}
				}
				catch (Exception e)
				{
					WindowsUtility.ShowError(this.Form, string.Format("Unable to open \"{0}\":\r\n\r\n{1}", this.Title, GetExceptionText(e)));
				}
			}

			return result;
		}

		public void Output(string message)
		{
			this.Output(message, 0);
		}

		public void Output(string message, Color color)
		{
			this.Output(message, color, 0, false, Guid.Empty);
		}

		public void Output(string message, int extraIndent)
		{
			this.Output(message, SystemColors.WindowText, extraIndent, false, Guid.Empty);
		}

		public void Output(string message, Color color, int extraIndent)
		{
			this.Output(message, color, extraIndent, false, Guid.Empty);
		}

		public void Output(string message, Color color, int extraIndent, bool highlight)
		{
			this.Output(message, color, extraIndent, highlight, Guid.Empty);
		}

		public void Output(string message, Color color, int extraIndent, bool highlight, Guid outputId)
		{
			int totalIndent = this.indentOutput + extraIndent;
			Manager.Output(message, totalIndent, color, highlight, outputId);

			// Log the output to a file if necessary.
			if (this.logOutput && this.logFile.Length > 0)
			{
				// Loop a few times in case the log file is locked.
				const int MaxFileChecks = 10;
				for (int fileCheckIndex = 1; fileCheckIndex <= MaxFileChecks; fileCheckIndex++)
				{
					try
					{
						using (StreamWriter log = File.AppendText(this.logFile))
						{
							// Timestamp
							if (this.logTimestamp)
							{
								log.Write(DateTime.UtcNow.ToLocalTime().ToString());
								log.Write(Manager.IndentString);
							}

							// Indent
							for (int i = 0; i < totalIndent; i++)
							{
								log.Write(Manager.IndentString);
							}

							// Message
							log.Write(message);

							// Unfortunately, color, highlight, and id information is lost because we're logging to a text file.
						}

						// If the write was successful, then quit looping.
						break;
					}
					catch (IOException ex)
					{
						// An IO exception occurred, so some other thread or process may be
						// accessing the file.  This can happen on rare occasions when a custom
						// Step's code is sending output at the same time an asynchronous console
						// read finds some output.  Since .NET doesn't provide any QueuedStreamWriter,
						// I'll just sleep a few times in a loop to see if I can get write access.
						if (fileCheckIndex < MaxFileChecks)
						{
							const int ShortSleepMilliseconds = 50;
							Thread.Sleep(ShortSleepMilliseconds);
						}
						else
						{
							// Let the interactive user know what happened.  We don't have to re-log
							// the original message because it was already sent to Manager.Output.
							Manager.Output("IOException sending output to log file: " + GetExceptionText(ex), totalIndent, OutputColors.Error, true);
						}
					}
				}
			}
		}

		public void OutputLine(string message)
		{
			this.OutputLine(message, 0);
		}

		public void OutputLine(string message, Color color)
		{
			this.OutputLine(message, color, 0, false, Guid.Empty);
		}

		public void OutputLine(string message, int extraIndent)
		{
			this.OutputLine(message, SystemColors.WindowText, extraIndent, false, Guid.Empty);
		}

		public void OutputLine(string message, Color color, int extraIndent)
		{
			this.OutputLine(message, color, extraIndent, false, Guid.Empty);
		}

		public void OutputLine(string message, Color color, int extraIndent, bool highlight)
		{
			this.OutputLine(message, color, extraIndent, highlight, Guid.Empty);
		}

		public void OutputLine(string message, Color color, int extraIndent, bool highlight, Guid outputId)
		{
			this.Output(message + Environment.NewLine, color, extraIndent, highlight, outputId);
		}

		public void PasteSteps(StepCategory category, int index)
		{
			if (CanPasteSteps)
			{
				// Get the steps from the clipboard in XML format.
				IDataObject data = Clipboard.GetDataObject();
				string stepXml = (string)data.GetData(CopiedStepFormat);
				XElement doc = XElement.Parse(stepXml);
				XmlKey docKey = new(doc);

				// We can't paste steps from a newer version of MegaBuild, but we can paste from older versions.
				if (decimal.TryParse(docKey.GetValue("CopyVersion", string.Empty), out decimal copyVersion) && copyVersion <= ProjectVersion)
				{
					// Convert the XML back into a step collection.
					StepCollection stepColl = new();

					// Set flags so edited and inserted events don't fire.
					this.loading = true;
					try
					{
						stepColl.Load(docKey.GetSubkey("Steps", "CopiedSteps"), this, category);
					}
					finally
					{
						this.loading = false;
					}

					// Now insert each step
					foreach (Step step in stepColl)
					{
						// Add the step to the correct collection.
						step.CategorySteps.Insert(index, step);

						// Loading the new step should have set this to true,
						// but we'll call it again to make sure.
						this.Modified = true;

						this.FireChanged(ProjectStepsChangedType.StepInserted, step, -1, index);

						// Increment the index so the next one will be inserted correctly.
						checked
						{
							index++;
						}
					}
				}
			}
		}

		public void ResetStatuses()
		{
			this.buildSteps.ResetStatuses();
			this.failureSteps.ResetStatuses();

			this.status = BuildStatus.None;
			this.buildStart = DateTime.MinValue;
		}

		public DialogResult Save(bool saveAs)
		{
			DialogResult result = DialogResult.No;
			bool updateRecentFiles = false;

			if (this.SaveFileDialog != null && (saveAs || this.fileName.Length == 0))
			{
				this.SaveFileDialog.FileName = this.fileName;
				if (this.SaveFileDialog.ShowDialog() == DialogResult.OK)
				{
					this.FileName = this.SaveFileDialog.FileName;

					if (string.IsNullOrEmpty(Path.GetExtension(this.fileName)) && this.SaveFileDialog.DefaultExt.Length > 0)
					{
						this.FileName = this.fileName + "." + this.SaveFileDialog.DefaultExt;
					}

					updateRecentFiles = true;
				}
				else
				{
					result = DialogResult.Cancel;
				}
			}

			if (this.fileName.Length != 0 && result != DialogResult.Cancel)
			{
				try
				{
					// Save the project to a file
					XElement doc = new("MegaBuildProject", new XAttribute("ProjectVersion", ProjectVersion));
					XmlKey docKey = new(doc);

					// Save options
					XmlKey optionsKey = docKey.GetSubkey("Options");
					optionsKey.SetValue(nameof(this.OverrideVSStepConfigurations), this.overrideVSStepConfigurations);
					optionsKey.SetValue("LogOutput", this.logOutput);
					optionsKey.SetValue("OverwriteLog", this.overwriteLog);
					optionsKey.SetValue("LogTimestamp", this.logTimestamp);
					optionsKey.SetValue("ShowComments", this.showComments);
					optionsKey.SetValue(nameof(this.ShowDebugOutput), this.showDebugOutput);
					optionsKey.SetValue("LogFile", this.logFile);
					optionsKey.SetValue(nameof(this.Comments), this.comments);
					this.overrideConfigurations.Save(optionsKey.GetSubkey(nameof(this.OverrideConfigurations)));
					optionsKey.SetValue(nameof(this.OverrideVSActions), this.overrideVSActions);
					optionsKey.SetValue(nameof(this.OverrideVSVersions), this.overrideVSVersions);
					optionsKey.SetValue("OverrideVSActionValue", this.overrideVSAction);
					optionsKey.SetValue("OverrideVSVersionValue", this.overrideVSVersion);
					VariableDefinition.Save(optionsKey, this.variableDefinitions);

					// Save steps
					this.buildSteps.Save(docKey.GetSubkey("Steps", nameof(this.BuildSteps)));
					this.failureSteps.Save(docKey.GetSubkey("Steps", nameof(this.FailureSteps)));

					// Get rid of empty nodes.
					docKey.Prune();

					// Save the XML with each attribute on a separate line to make visually comparing changes easier in source control.
					XmlWriterSettings settings = new()
					{
						Indent = true,
						IndentChars = "\t",
						NewLineOnAttributes = true,
					};
					using (XmlWriter writer = XmlWriter.Create(this.fileName, settings))
					{
						doc.Save(writer);
					}

					if (updateRecentFiles)
					{
						this.AddToRecentFiles();
					}

					this.Modified = false;
					result = DialogResult.Yes;
				}
				catch (Exception e)
				{
					WindowsUtility.ShowError(this.Form, string.Format("Unable to save \"{0}\":\r\n\r\n{1}", this.Title, GetExceptionText(e)));
				}
			}

			return result;
		}

		public void SendBuildProgressMessage(string message)
		{
			if (this.Building)
			{
				this.FireBuildProgress(message);
			}
		}

		public void StopBuild()
		{
			this.SendBuildProgressMessage("Build was canceled.");
			this.StopBuild(BuildStatus.Stopped);
		}

		#endregion

		#region Internal Methods

		internal static decimal GetVersion(XElement element)
		{
			XElement root = element;
			XElement parent = root.Parent;
			while (parent != null)
			{
				root = parent;
				parent = root.Parent;
			}

			string version = root.GetAttributeValue("ProjectVersion", null) ?? root.GetAttributeValue("CopyVersion", null);
			if (!decimal.TryParse(version, out decimal result))
			{
				result = 0;
			}

			return result;
		}

		internal Step CreateStep(StepCategory category, StepTypeInfo info)
		{
			object[] constructorParams = { this, category, info };
			return (Step)Activator.CreateInstance(info.StepType, constructorParams);
		}

		internal void RuntimeStepValueChanged(Step step)
		{
			this.FireStepEdited(step);
		}

		internal void StepEdited(Step step)
		{
			this.Modified = true;
			this.FireStepEdited(step);
		}

		#endregion

		#region Protected Methods

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this.components.Dispose();
			Manager.RemoveProject(this);
		}

		#endregion

		#region Private Methods

		private static void GetConfigurationsForSteps(VSConfigurationList configurations, StepCollection steps)
		{
			foreach (VSStep step in steps.OfType<VSStep>())
			{
				configurations.Add(step.Configurations);
			}
		}

		private static string GetExceptionText(Exception ex)
		{
			// Don't show the call stack in release builds.
			string result = ApplicationInfo.IsDebugBuild ? ex.ToString() : ex.Message;
			return result;
		}

		private static void GetExecutableSteps(Step[] steps, bool forceIncludeInBuild, out ExecutableStep[] stepsToBuild, out ExecutableStep[] stepsToConfirm)
		{
			List<ExecutableStep> stepsToBuildList = new();
			List<ExecutableStep> stepsToConfirmList = new();

			foreach (Step step in steps)
			{
				if (step is ExecutableStep executableStep && (forceIncludeInBuild || executableStep.IncludeInBuild))
				{
					if (executableStep.PromptFirst)
					{
						stepsToConfirmList.Add(executableStep);
					}
					else
					{
						stepsToBuildList.Add(executableStep);
					}
				}
			}

			stepsToBuild = stepsToBuildList.ToArray();
			stepsToConfirm = stepsToConfirmList.ToArray();
		}

		private static void SetInCurrentBuild(ExecutableStep[] steps, bool state)
		{
			if (steps != null)
			{
				foreach (ExecutableStep step in steps)
				{
					step.InCurrentBuild = state;
				}
			}
		}

		private static int ChangeOption<T>(ref T member, T value)
		{
			int result = 0;

			if (!object.Equals(member, value))
			{
				member = value;
				result = 1;
			}

			return result;
		}

		private void AddToRecentFiles()
		{
			if (this.RecentFiles != null)
			{
				this.RecentFiles.Add(this.FileName);

				this.RecentFileAdded?.Invoke(this, EventArgs.Empty);
			}
		}

		private BuildStatus BuildExecutableSteps(ExecutableStep[] steps, string finalProgressMessage)
		{
			BuildStatus result = BuildStatus.Succeeded;

			// We'll check frequently for a StopBuild signal.
			if (this.StopBuilding)
			{
				result = this.status;
			}
			else
			{
				int baseIndent = this.indentOutput;
				try
				{
					int numSteps = steps.Length;
					int totalProgressSteps = numSteps + 1;
					for (int i = 0; i < numSteps; i++)
					{
						ExecutableStep step = steps[i];
						if (this.StopBuilding)
						{
							result = this.status;
							break;
						}

						// Indent the output for "indented" steps.
						this.indentOutput = step.Level + baseIndent;

						step.OutputStartId = Guid.NewGuid();
						this.FireBuildProgress("Step: " + step.Name, i + 1, totalProgressSteps, true, step.OutputStartId);

						// Some steps can be skipped if the parent didn't succeeded (e.g., failed, timed out, or was skipped).
						bool buildIt = !step.OnlyIfParentSucceeded || step.ParentSucceeded;
						if (buildIt)
						{
							// We can set the initial status to "Executing", but we can't set the final status if the
							// Execute doesn't succeed. Each Execute implementation will have to handle the failed,
							// timedout, canceled, etc. cases.
							step.Status = StepStatus.Executing;

							// Execute the step and record the time.
							DateTime stepStart = DateTime.UtcNow;
							bool execute = false;
							try
							{
								execute = step.Execute(this.executeArgs);
							}
							catch (Exception ex)
							{
								// Some step blew up (and didn't handle it's own exception), so we need to log the exception
								// and then set the step to a failure status.
								this.OutputLine("Step Exception: " + GetExceptionText(ex), OutputColors.Error, 0, true);
								step.Status = StepStatus.Failed;
								execute = false;
							}

							step.TotalExecutionTime = DateTime.UtcNow - stepStart;
							if (!execute)
							{
								// If we get here, then the step failed to build.
								if (this.StopBuilding)
								{
									result = this.status;
									break;
								}

								// The step should have set its own status in this case (to failure or timedout), but if not, we will.
								if (step.Status == StepStatus.Executing)
								{
									step.Status = StepStatus.Failed;
								}

								// If we're not supposed to ignore failures, then return a failure result.
								if (!step.IgnoreFailure)
								{
									result = BuildStatus.Failed;
									break;
								}
							}
							else
							{
								step.Status = StepStatus.Succeeded;
							}
						}
						else
						{
							step.Status = StepStatus.Skipped;
						}
					}

					if (result == BuildStatus.Succeeded)
					{
						this.FireBuildProgress(finalProgressMessage, totalProgressSteps, totalProgressSteps);
					}
				}
				finally
				{
					this.indentOutput = baseIndent;
				}
			}

			return result;
		}

		// Note: All events should be fired on the GUI thread if this.Form is set.
		// Using Control.BeginInvoke ensures apartment safety. Whether we're calling from the GUI thread
		// or not, this does something like PostThreadMessage, and the GUI thread will process it when the
		// message pump gets around to it. Since all of the non-GUI objects in this project can be safely
		// accessed by multiple threads, we're always safe firing the events from the GUI thread.
		private void BuildOutput(string message, Color color)
		{
			this.BuildOutput(message, color, Guid.Empty);
		}

		private void BuildOutput(string message, Color color, Guid outputId)
		{
			this.Output(string.Format("---------------------- {0} ----------------------\r\n", message), color, 0, false, outputId);
		}

		private ExecutableStep[] CombineConfirmedSteps(StepCategory category, ExecutableStep[] stepsToBuild, ExecutableStep[] stepsToConfirm)
		{
			// This method would be a lot slicker if it used a merge algorithm, but that
			// would take a lot more thought.  This way is fine for small to medium lists.
			//
			// Create an array large enough to hold the entire category of steps.
			int maxNumSteps = category == StepCategory.Build ? this.buildSteps.Count : this.failureSteps.Count;
			ExecutableStep[] steps = new ExecutableStep[maxNumSteps];

			// Put each step at its original place in the list.
			foreach (ExecutableStep step in stepsToBuild)
			{
				steps[step.GetIndex()] = step;
			}

			foreach (ExecutableStep step in stepsToConfirm)
			{
				steps[step.GetIndex()] = step;
			}

			// Now copy the non-null entries into the final array.
			ExecutableStep[] resultSteps = new ExecutableStep[stepsToBuild.Length + stepsToConfirm.Length];
			int currentResultIndex = 0;
			for (int i = 0; i < maxNumSteps; i++)
			{
				if (steps[i] != null)
				{
					resultSteps[currentResultIndex] = steps[i];
					currentResultIndex++;
				}
			}

			return resultSteps;
		}

		private void ExecuteBackgroundBuild()
		{
			// NOTE: This code is running in a background thread.
			Debug.Assert(this.executableBuildSteps != null && this.executableBuildSteps.Length > 0, "Must have executable steps.");

			this.buildStart = DateTime.UtcNow;
			this.status = BuildStatus.Started;
			this.indentOutput = this.baseIndentLevel;

			SetInCurrentBuild(this.executableBuildSteps, true);
			SetInCurrentBuild(this.executableFailureSteps, true);

			// Delete any old log file if necessary.
			if (this.logOutput && this.overwriteLog && this.logFile.Length > 0 && File.Exists(this.logFile))
			{
				File.Delete(this.logFile);
			}

			// Send this notification now because we've figured out exactly
			// which steps we're going to build, and we've set the initial flags.
			this.FireBuildStarted();

			BuildStatus localStatus = BuildStatus.Stopped;
			this.indentOutput++;
			try
			{
				// Build the steps
				localStatus = this.BuildExecutableSteps(this.executableBuildSteps, "Build Succeeded");
				if (localStatus == BuildStatus.Failed)
				{
					this.status = BuildStatus.Failing;
					this.FireBuildFailed();
					if (this.executableFailureSteps != null && this.executableFailureSteps.Length > 0)
					{
						// Execute the failure steps.  Ignore any return status because the build has already failed.
						this.BuildExecutableSteps(this.executableFailureSteps, "Build Failed");
					}
					else
					{
						this.FireBuildProgress("Build Failed");
					}
				}
			}
			finally
			{
				this.indentOutput--;
			}

			// Sleep just a little bit before returning, so the final status is visible in the progress bar.
			// This is especially important for failed builds.
			Thread.Sleep(Properties.Settings.Default.Project_FinishBuildSleepMilliseconds);

			// Call this to make sure that the correct final notifications are sent.
			this.StopBuild(localStatus);
		}

		private void FireBuildFailed()
		{
			Debug.WriteLine("*** BuildFailed ***");
			this.FireEventHandler(this.BuildFailed);

			this.BuildOutput("Build Failed", OutputColors.Error);
		}

		private void FireBuildProgress(string message)
		{
			this.FireBuildProgress(message, 0, 0, false, Guid.Empty);
		}

		private void FireBuildProgress(string message, int currentStep, int totalSteps)
		{
			this.FireBuildProgress(message, currentStep, totalSteps, true, Guid.Empty);
		}

		private void FireBuildProgress(string message, int currentStep, int totalSteps, bool useNumbers, Guid outputId)
		{
			Debug.WriteLine("*** BuildProgress ***");
			if (this.BuildProgress != null)
			{
				BuildProgressEventArgs e = useNumbers ? new BuildProgressEventArgs(message, currentStep, totalSteps) : new BuildProgressEventArgs(message);

				if (this.Form != null)
				{
					this.Form.BeginInvoke(this.BuildProgress, new object[] { this, e });
				}
				else
				{
					this.BuildProgress(this, e);
				}
			}

			this.BuildOutput(message, OutputColors.Heading, outputId);
		}

		private void FireBuildStarted()
		{
			Debug.WriteLine("*** BuildStarted ***");
			this.FireEventHandler(this.BuildStarted);

			this.BuildOutput("Build Started", OutputColors.Heading);
		}

		private bool FireBuildStarting()
		{
			Debug.WriteLine("*** BuildStarting ***");

			bool result = true;

			if (this.BuildStarting != null)
			{
				// This event is the one event that we will fire on the current thread. It should only be
				// invoked from the GUI thread because it allows the build to be canceled before the background
				// thread starts.
				CancelEventArgs e = new(false);
				this.BuildStarting(this, e);
				result = !e.Cancel;
			}

			return result;
		}

		private void FireBuildStopped()
		{
			Debug.WriteLine("*** BuildStopped ***");

			// Send the output notification first. If the build was started from the command-line and is
			// supposed to auto-exit, then we want the actual BuildStopped event to be the last thing sent to
			// the main window. If output arrives after the main window is closed, then exceptions can be
			// thrown after the resources are disposed.
			this.BuildOutput("Build Stopped", OutputColors.Heading);

			this.FireEventHandler(this.BuildStopped);
		}

		private void FireChanged(ProjectStepsChangedType changeType, Step step, int oldIndex, int newIndex)
		{
			// We can't set Modified = true here because some callers (e.g. Run-time status properties)
			// broadcast changes that don't imply persistent project changes.
			Debug.WriteLine("*** FileNameSet ***");
			if (this.ProjectStepsChanged != null && !this.loading)
			{
				ProjectStepsChangedEventArgs e = new(changeType, step, oldIndex, newIndex);

				if (this.Form != null)
				{
					this.Form.BeginInvoke(this.ProjectStepsChanged, new object[] { this, e });
				}
				else
				{
					this.ProjectStepsChanged(this, e);
				}
			}
		}

		private void FireContentsReset()
		{
			Debug.WriteLine("*** ContentsReset ***");
			this.FireEventHandler(this.ContentsReset);
		}

		private void FireContentsResetting()
		{
			Debug.WriteLine("*** ContentsResetting ***");
			this.FireEventHandler(this.ContentsResetting);
		}

		private void FireDisplayComments()
		{
			Debug.WriteLine("*** DisplayComments ***");
			this.FireEventHandler(this.DisplayComments);
		}

		private void FireEventHandler(Delegate eventHandler)
		{
			if (eventHandler != null)
			{
				if (this.Form != null)
				{
					this.Form.BeginInvoke(eventHandler, this.eventHandlerParams);
				}
				else
				{
					eventHandler.DynamicInvoke(this.eventHandlerParams);
				}
			}
		}

		private void FireFileNameSet()
		{
			Debug.WriteLine("*** FileNameSet ***");
			this.FireEventHandler(this.FileNameSet);
		}

		private void FireModifiedChanged()
		{
			Debug.WriteLine("*** ModifiedChanged ***");
			this.FireEventHandler(this.ModifiedChanged);
		}

		private void FireStepEdited(Step step)
		{
			// Don't send the notification if we're inserting.  A separate StepInserted notification will be sent.
			if (!this.insertingStep)
			{
				Debug.WriteLine("*** StepEdited ***");
				int index = step.GetIndex();
				this.FireChanged(ProjectStepsChangedType.StepEdited, step, index, index);
			}
		}

		private VSConfigurationList GetAllConfigurations()
		{
			VSConfigurationList configurations = new();

			// Get all the configurations from the build and failure steps.
			GetConfigurationsForSteps(configurations, this.buildSteps);
			GetConfigurationsForSteps(configurations, this.failureSteps);

			// Now merge in the previous settings.
			configurations.MergeStatesAndOrder(this.overrideConfigurations);

			return configurations;
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
		}

		private void StopBuild(BuildStatus status)
		{
			lock (this.buildSteps)
			{
				if (this.Building)
				{
					this.status = status;

					// Only let the background thread clear the flags. This ensures that other builds can't be
					// kicked off until the current one finishes.
					//
					// I'm keeping a separate Building flag because the foreground thread can set the status to
					// Stopped, but it can't change the Building flag.
					if (this.buildThread == Thread.CurrentThread)
					{
						try
						{
							// Change all executable steps's InCurrentBuild to false.
							SetInCurrentBuild(this.executableBuildSteps, false);
							this.executableBuildSteps = null;

							SetInCurrentBuild(this.executableFailureSteps, false);
							this.executableFailureSteps = null;
						}
						finally
						{
							this.buildThread = null;
							this.indentOutput = this.baseIndentLevel;
						}

						this.FireBuildStopped();
					}
				}
			}
		}

		#endregion

		#region Internal Types

		internal sealed class VariableDefinition
		{
			#region Constructors

			public VariableDefinition(string name, string value, bool expandPath)
			{
				this.Name = name;
				this.Value = value;
				this.ExpandPath = expandPath;
			}

			#endregion

			#region Public Properties

			public bool ExpandPath { get; }

			public string Name { get; }

			public string Value { get; }

			#endregion

			#region Public Methods

			public static List<VariableDefinition> Load(XmlKey optionsKey)
			{
				Dictionary<string, VariableDefinition> variables = new(StringComparer.CurrentCultureIgnoreCase);

				XmlKey variablesKey = optionsKey.GetSubkey(nameof(Variables));
				XmlKey[] subKeys = variablesKey.GetSubkeys();
				foreach (XmlKey subKey in subKeys)
				{
					string name = subKey.XmlKeyName;
					string value = subKey.GetValue(nameof(Value), string.Empty);
					bool expandPath = subKey.GetValue(nameof(ExpandPath), true);
					VariableDefinition definition = new(name, value, expandPath);
					variables[name] = definition;
				}

				var result = variables.Values.OrderBy(d => d.Name).ToList();
				return result;
			}

			public static void Save(XmlKey optionsKey, List<VariableDefinition> variableDefinitions)
			{
				XmlKey variablesKey = optionsKey.GetSubkey(nameof(Variables));
				foreach (VariableDefinition definition in variableDefinitions.OrderBy(d => d.Name))
				{
					XmlKey subKey = variablesKey.GetSubkey("Variable", definition.Name);
					subKey.SetValue(nameof(Value), definition.Value);
					subKey.SetValue(nameof(ExpandPath), definition.ExpandPath);
				}
			}

			#endregion
		}

		#endregion
	}
}
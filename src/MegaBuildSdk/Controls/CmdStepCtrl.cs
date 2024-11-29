namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.Windows.Forms;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	internal sealed partial class CmdStepCtrl : StepEditorControl
	{
		#region Private Data Members

		private CommandStep? step;

		#endregion

		#region Constructors

		public CmdStepCtrl()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Public Properties

		public override string DisplayName => "Command";

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CommandStep Step
		{
			set
			{
				if (this.step != value)
				{
					this.step = value;

					this.edtCommand.Text = this.step.Command;
					this.edtArguments.Text = this.step.Arguments;
					this.edtWorkingDirectory.Text = this.step.WorkingDirectory;
					this.cbWindowState.SelectedIndex = (int)this.step.WindowState;
					this.chkShellExecute.Checked = this.step.UseShellExecute;
					this.edtVerb.Text = this.step.Verb;
					this.numFirstSuccess.Value = this.step.FirstSuccessCode;
					this.numLastSuccess.Value = this.step.LastSuccessCode;
					this.RedirectStreams = this.step.RedirectStreams;

					this.UpdateControlStates(true);
				}
			}
		}

		#endregion

		#region Private Properties

		private RedirectStandardStreams RedirectStreams
		{
			get
			{
				RedirectStandardStreams result = RedirectStandardStreams.None;

				if (this.chkRedirectInput.Checked)
				{
					result |= RedirectStandardStreams.Input;
				}

				if (this.chkRedirectOutput.Checked)
				{
					result |= RedirectStandardStreams.Output;
				}

				if (this.chkRedirectError.Checked)
				{
					result |= RedirectStandardStreams.Error;
				}

				// Set this outside the Error.Checked condition so the returned enum will match the visible states.
				if (this.chkErrorAsOutput.Checked)
				{
					result |= RedirectStandardStreams.TreatErrorAsOutput;
				}

				return result;
			}

			set
			{
				this.chkRedirectInput.Checked = (value & RedirectStandardStreams.Input) != 0;
				this.chkRedirectOutput.Checked = (value & RedirectStandardStreams.Output) != 0;
				this.chkRedirectError.Checked = (value & RedirectStandardStreams.Error) != 0;
				this.chkErrorAsOutput.Checked = (value & RedirectStandardStreams.TreatErrorAsOutput) != 0;
			}
		}

		#endregion

		#region Public Methods

		public override bool OnOk()
		{
			bool result = false;

			if (this.edtCommand.Text.Trim().Length == 0)
			{
				WindowsUtility.ShowError(this, "You must enter a command.");
			}
			else if (this.numFirstSuccess.Value > this.numLastSuccess.Value)
			{
				WindowsUtility.ShowError(this, "The last success code must be greater than or equal to the first success code.");
			}
			else if (this.step != null)
			{
				this.step.Command = this.edtCommand.Text;
				this.step.Arguments = this.edtArguments.Text;
				this.step.WorkingDirectory = this.edtWorkingDirectory.Text;
				this.step.WindowState = (ProcessWindowStyle)this.cbWindowState.SelectedIndex;
				this.step.UseShellExecute = this.chkShellExecute.Checked;
				this.step.Verb = this.edtVerb.Text;
				this.step.FirstSuccessCode = (int)this.numFirstSuccess.Value;
				this.step.LastSuccessCode = (int)this.numLastSuccess.Value;
				this.step.RedirectStreams = this.RedirectStreams;
				result = true;
			}

			return result;
		}

		#endregion

		#region Private Methods

		private void BrowseArguments_Click(object sender, EventArgs e)
		{
			this.GetFileName(this.edtArguments, "Get Argument", false);
		}

		private void BrowseCmd_Click(object sender, EventArgs e)
		{
			this.GetFileName(this.edtCommand, "Get Command File", true);
		}

		private void BrowseDirectory_Click(object sender, EventArgs e)
		{
			string initialFolder = Manager.ExpandVariables(this.edtWorkingDirectory.Text);
			string? selectedFolder = WindowsUtility.SelectFolder(this, "Select Working Directory", initialFolder);
			if (selectedFolder.IsNotEmpty())
			{
				this.edtWorkingDirectory.Text = Manager.CollapseVariables(selectedFolder);
			}
		}

		private void GetFileName(TextBox edtFileName, string title, bool autoRedirect)
		{
			this.OpenDlg.Title = title;
			this.OpenDlg.FileName = Manager.ExpandVariables(edtFileName.Text);
			if (this.OpenDlg.ShowDialog(this) == DialogResult.OK)
			{
				string fileName = Manager.CollapseVariables(this.OpenDlg.FileName);
				edtFileName.Text = fileName;
				if (autoRedirect && CommandStep.HasBatchFileExt(fileName))
				{
					this.chkRedirectInput.Checked = true;
					this.chkRedirectOutput.Checked = true;
					this.chkRedirectError.Checked = true;
					this.chkErrorAsOutput.Checked = false;
				}
			}
		}

		private void RedirectStreams_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateControlStates(true);
		}

		private void ShellExecute_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateControlStates(false);
		}

		private void UpdateControlStates(bool priorityToRedirect)
		{
			bool bothChecked = (this.RedirectStreams != RedirectStandardStreams.None) && this.chkShellExecute.Checked;
			if (priorityToRedirect)
			{
				if (bothChecked)
				{
					this.chkShellExecute.Checked = false;
				}
			}
			else
			{
				if (bothChecked)
				{
					this.RedirectStreams = RedirectStandardStreams.None;

					// This is necessary because the above line actually sets
					// multiple check boxes, which can cause this to uncheck.
					this.chkShellExecute.Checked = true;
				}
			}

			this.lblVerb.Enabled = this.chkShellExecute.Checked;
			this.edtVerb.Enabled = this.chkShellExecute.Checked;

			this.chkErrorAsOutput.Enabled = this.chkRedirectError.Checked;
		}

		#endregion
	}
}
namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Drawing;
	using System.Net.Mail;
	using System.Windows.Forms;
	using Menees;
	using Menees.Windows.Forms;

	#endregion

	internal sealed partial class EmailStepCtrl : StepEditorControl
	{
		#region Private Data Members

		private EmailStep? step;

		#endregion

		#region Constructors

		public EmailStepCtrl()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Public Properties

		public override string DisplayName => "Email";

		public EmailStep Step
		{
			set
			{
				if (this.step != value)
				{
					this.step = value;

					this.edtFrom.Text = this.step.From;
					this.edtTo.Text = this.step.To;
					this.edtCC.Text = this.step.CC;
					this.edtSubject.Text = this.step.Subject;
					this.txtMessage.Text = this.step.Message;
					this.chkAppendOutput.Checked = this.step.AppendOutput;
					this.cbPriority.SelectedIndex = (int)this.step.Priority;
					this.edtSmtpServer.Text = this.step.SmtpServer;
				}
			}
		}

		#endregion

		#region Public Methods

		public override bool OnOk()
		{
			bool result = false;

			if (this.edtFrom.Text.Trim().Length == 0)
			{
				WindowsUtility.ShowError(this, "You must enter an email address in the \"From\" field.");
			}
			else if (this.edtTo.Text.Trim().Length == 0)
			{
				WindowsUtility.ShowError(this, "You must enter an email address in the \"To\" field.");
			}
			else if (this.edtSubject.Text.Trim().Length == 0 && this.txtMessage.Text.Trim().Length == 0 && !this.chkAppendOutput.Checked)
			{
				WindowsUtility.ShowError(this, "You must enter something that can be emailed (e.g. Subject, Message, or Append Output).");
			}
			else if (this.step != null)
			{
				this.step.From = this.edtFrom.Text;
				this.step.To = this.edtTo.Text;
				this.step.CC = this.edtCC.Text;
				this.step.Subject = this.edtSubject.Text;
				this.step.Message = this.txtMessage.Text;
				this.step.AppendOutput = this.chkAppendOutput.Checked;
				this.step.Priority = (MailPriority)this.cbPriority.SelectedIndex;
				this.step.SmtpServer = this.edtSmtpServer.Text;
				result = true;
			}

			return result;
		}

		#endregion
	}
}
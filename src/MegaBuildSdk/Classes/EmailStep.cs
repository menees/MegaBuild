namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Drawing;
	using System.Linq;
	using System.Net.Mail;
	using System.Text;
	using Menees;

	#endregion

	[StepDisplay("Email", "Sends an email to one or more recipients using an SMTP server.", "Images.EmailStep.ico")]
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Called by Reflection.")]
	internal sealed class EmailStep : ExecutableStep
	{
		#region Private Data Members

		private readonly List<string> attachments = new List<string>();
		private bool appendOutput = true;
		private MailPriority priority = MailPriority.Normal;
		private string cc = string.Empty;
		private string from = string.Empty;
		private string message = string.Empty;
		private string smtpServer = string.Empty;
		private string subject = string.Empty;
		private string to = string.Empty;

		#endregion

		#region Constructors

		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Reflection.")]
		public EmailStep(Project project, StepCategory category, StepTypeInfo info)
			: base(project, category, info, ExecSupports.None)
		{
		}

		#endregion

		#region Public Properties

		public bool AppendOutput
		{
			get
			{
				return this.appendOutput;
			}

			set
			{
				this.SetValue(ref this.appendOutput, value);
			}
		}

		public IList<string> Attachments
		{
			get
			{
				return this.attachments;
			}

			set
			{
				IEnumerable<string> target = value ?? Enumerable.Empty<string>();
				if (!this.attachments.SequenceEqual(target))
				{
					this.attachments.Clear();
					this.attachments.AddRange(target);
					this.SetModified();
				}
			}
		}

		public string CC
		{
			get
			{
				return this.cc;
			}

			set
			{
				this.SetValue(ref this.cc, value);
			}
		}

		public string From
		{
			get
			{
				return this.from;
			}

			set
			{
				this.SetValue(ref this.from, value);
			}
		}

		public string Message
		{
			get
			{
				return this.message;
			}

			set
			{
				this.SetValue(ref this.message, value);
			}
		}

		public MailPriority Priority
		{
			get
			{
				return this.priority;
			}

			set
			{
				this.SetValue(ref this.priority, value);
			}
		}

		public string SmtpServer
		{
			get
			{
				return this.smtpServer;
			}

			set
			{
				this.SetValue(ref this.smtpServer, value);
			}
		}

		public string Subject
		{
			get
			{
				return this.subject;
			}

			set
			{
				this.SetValue(ref this.subject, value);
			}
		}

		public string To
		{
			get
			{
				return this.to;
			}

			set
			{
				this.SetValue(ref this.to, value);
			}
		}

		#endregion

		#region Public Methods

		public override bool Execute(StepExecuteArgs args)
		{
			string from = Manager.ExpandVariables(this.From);
			string to = Manager.ExpandVariables(this.To);
			string cc = Manager.ExpandVariables(this.CC);

			using (MailMessage mail = new MailMessage())
			{
				if (!string.IsNullOrEmpty(from))
				{
					mail.From = new MailAddress(from);
				}

				if (!string.IsNullOrEmpty(to))
				{
					mail.To.Add(to);
				}

				if (!string.IsNullOrEmpty(cc))
				{
					mail.CC.Add(cc);
				}

				mail.Subject = Manager.ExpandVariables(this.Subject);
				mail.Priority = this.Priority;

				StringBuilder sb = new StringBuilder();
				sb.Append(Manager.ExpandVariables(this.Message));
				if (this.AppendOutput)
				{
					sb.Append("\r\n\r\nBuild Output:\r\n");

					// I'm intentionally not calling ExpandVariables on the output.
					sb.Append(Manager.GetOutput());
				}

				mail.Body = sb.ToString();

				bool result = false;
				if (!this.StopBuilding)
				{
					// Add any attachments
					foreach (string attachmentPath in this.attachments)
					{
						Attachment att = new Attachment(Manager.ExpandVariables(attachmentPath));
						mail.Attachments.Add(att);

						if (this.StopBuilding)
						{
							break;
						}
					}
				}

				if (!this.StopBuilding)
				{
					try
					{
						using (SmtpClient smtp = new SmtpClient(Manager.ExpandVariables(this.SmtpServer)))
						{
							smtp.Send(mail);
							result = true;
						}
					}
					catch (SmtpException ex)
					{
						this.Project.OutputLine(ex.Message, OutputColors.Error, 0, true);
					}
					catch (InvalidOperationException ex)
					{
						this.Project.OutputLine(ex.Message, OutputColors.Error, 0, true);
					}
				}

				return result;
			}
		}

		public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
		{
			base.GetStepEditorControls(controls);
			controls.Add(new EmailStepCtrl() { Step = this });
			controls.Add(new EmailAttachmentsCtrl() { Step = this });
		}

		#endregion

		#region Protected Methods

		protected internal override void Load(XmlKey key)
		{
			base.Load(key);
			this.From = key.GetValue("From", this.from);
			this.To = key.GetValue("To", this.to);
			this.CC = key.GetValue("CC", this.cc);
			this.Subject = key.GetValue("Subject", this.subject);
			this.Message = key.GetValue("Message", this.message);
			this.AppendOutput = key.GetValue("AppendOutput", this.appendOutput);
			this.Priority = key.GetValue("Priority", this.priority);
			this.SmtpServer = key.GetValue("SmtpServer", this.smtpServer);

			// Load attachments
			this.attachments.Clear();
			XmlKey attachmentsKey = key.GetSubkey("Attachments", string.Empty);
			foreach (XmlKey attKey in attachmentsKey.GetSubkeys())
			{
				Debug.Assert(attKey.KeyType == "Attachment", "Key type must be Attachment.");

				string attachment = attKey.GetValue("FileName", string.Empty);
				if (attachment.Length > 0)
				{
					this.attachments.Add(attachment);
				}
			}
		}

		protected internal override void Save(XmlKey key)
		{
			base.Save(key);
			key.SetValue("From", this.from);
			key.SetValue("To", this.to);
			key.SetValue("CC", this.cc);
			key.SetValue("Subject", this.subject);
			key.SetValue("Message", this.message);
			key.SetValue("AppendOutput", this.appendOutput);
			key.SetValue("Priority", this.priority);
			key.SetValue("SmtpServer", this.smtpServer);

			// Save attachments
			XmlKey attachmentsKey = key.GetSubkey("Attachments", string.Empty);
			int numAttachments = this.attachments.Count;
			for (int i = 0; i < numAttachments; i++)
			{
				XmlKey attKey = attachmentsKey.GetSubkey("Attachment", i.ToString());
				attKey.SetValue("FileName", this.attachments[i]);
			}
		}

		#endregion
	}
}
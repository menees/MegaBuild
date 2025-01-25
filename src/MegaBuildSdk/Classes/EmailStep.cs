namespace MegaBuild;

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

	private readonly List<string> attachments = [];
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

	public EmailStep(Project project, StepCategory category, StepTypeInfo info)
		: base(project, category, info, ExecSupports.None)
	{
	}

	#endregion

	#region Public Properties

	public bool AppendOutput
	{
		get => this.appendOutput;
		set => this.SetValue(ref this.appendOutput, value);
	}

	public IList<string> Attachments
	{
		get => this.attachments;
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
		get => this.cc;
		set => this.SetValue(ref this.cc, value);
	}

	public string From
	{
		get => this.from;
		set => this.SetValue(ref this.from, value);
	}

	public string Message
	{
		get => this.message;
		set => this.SetValue(ref this.message, value);
	}

	public MailPriority Priority
	{
		get => this.priority;
		set => this.SetValue(ref this.priority, value);
	}

	public string SmtpServer
	{
		get => this.smtpServer;
		set => this.SetValue(ref this.smtpServer, value);
	}

	public string Subject
	{
		get => this.subject;
		set => this.SetValue(ref this.subject, value);
	}

	public string To
	{
		get => this.to;
		set => this.SetValue(ref this.to, value);
	}

	#endregion

	#region Public Methods

	public override bool Execute(StepExecuteArgs args)
	{
		string from = Manager.ExpandVariables(this.From);
		string to = Manager.ExpandVariables(this.To);
		string cc = Manager.ExpandVariables(this.CC);

		using (MailMessage mail = new())
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

			StringBuilder sb = new();
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
					Attachment att = new(Manager.ExpandVariables(attachmentPath));
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
					using (SmtpClient smtp = new(Manager.ExpandVariables(this.SmtpServer)))
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

	[SuppressMessage("Usage", "CC0022:Should dispose object", Justification = "Caller disposes new controls.")]
	public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
	{
		base.GetStepEditorControls(controls);
		controls.Add(new EmailStepCtrl { Step = this });
		controls.Add(new EmailAttachmentsCtrl { Step = this });
	}

	#endregion

	#region Protected Methods

	protected internal override void Load(XmlKey key)
	{
		base.Load(key);
		this.From = key.GetValue(nameof(this.From), this.from);
		this.To = key.GetValue(nameof(this.To), this.to);
		this.CC = key.GetValue(nameof(this.CC), this.cc);
		this.Subject = key.GetValue(nameof(this.Subject), this.subject);
		this.Message = key.GetValue(nameof(this.Message), this.message);
		this.AppendOutput = key.GetValue(nameof(this.AppendOutput), this.appendOutput);
		this.Priority = key.GetValue(nameof(this.Priority), this.priority);
		this.SmtpServer = key.GetValue(nameof(this.SmtpServer), this.smtpServer);

		// Load attachments
		this.attachments.Clear();
		XmlKey attachmentsKey = key.GetSubkey(nameof(this.Attachments));
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
		key.SetValue(nameof(this.From), this.from);
		key.SetValue(nameof(this.To), this.to);
		key.SetValue(nameof(this.CC), this.cc);
		key.SetValue(nameof(this.Subject), this.subject);
		key.SetValue(nameof(this.Message), this.message);
		key.SetValue(nameof(this.AppendOutput), this.appendOutput);
		key.SetValue(nameof(this.Priority), this.priority);
		key.SetValue(nameof(this.SmtpServer), this.smtpServer);

		// Save attachments
		XmlKey attachmentsKey = key.GetSubkey(nameof(this.Attachments));
		int numAttachments = this.attachments.Count;
		for (int i = 0; i < numAttachments; i++)
		{
			XmlKey attKey = attachmentsKey.GetSubkey("Attachment", i.ToString());
			attKey.SetValue("FileName", this.attachments[i]);
		}
	}

	#endregion
}
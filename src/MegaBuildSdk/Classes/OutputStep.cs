namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Drawing;
	using Menees;

	#endregion

	[StepDisplay("Output", "Adds a message to the MegaBuild output window.", "Images.OutputStep.ico")]
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Called by Reflection.")]
	internal sealed class OutputStep : ExecutableStep
	{
		#region Private Data Members

		private bool includeTimestamp;
		private bool isHighlight;
		private int indentOutput;
		private string message = string.Empty;
		private Color textColor = SystemColors.WindowText;

		#endregion

		#region Constructors

		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Reflection.")]
		public OutputStep(Project project, StepCategory category, StepTypeInfo info)
			: base(project, category, info, ExecSupports.None)
		{
		}

		#endregion

		#region Public Properties

		public bool IncludeTimestamp
		{
			get
			{
				return this.includeTimestamp;
			}

			set
			{
				this.SetValue(ref this.includeTimestamp, value);
			}
		}

		public int IndentOutput
		{
			get
			{
				return this.indentOutput;
			}

			set
			{
				this.SetValue(ref this.indentOutput, value);
			}
		}

		public bool IsHighlight
		{
			get
			{
				return this.isHighlight;
			}

			set
			{
				this.SetValue(ref this.isHighlight, value);
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

		public Color TextColor
		{
			get
			{
				return this.textColor;
			}

			set
			{
				this.SetValue(ref this.textColor, value);
			}
		}

		#endregion

		#region Public Methods

		public override bool Execute(StepExecuteArgs args)
		{
			if (this.IncludeTimestamp)
			{
				this.Project.OutputLine(DateTime.UtcNow.ToLocalTime().ToString(), this.textColor, this.IndentOutput);
			}

			this.Project.OutputLine(Manager.ExpandVariables(this.Message), this.textColor, this.IndentOutput, this.IsHighlight);

			return true;
		}

		public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
		{
			base.GetStepEditorControls(controls);
			controls.Add(new OutputStepCtrl() { Step = this });
		}

		#endregion

		#region Protected Methods

		protected internal override void Load(XmlKey key)
		{
			base.Load(key);
			this.Message = key.GetValue("Message", this.message);
			this.IncludeTimestamp = key.GetValue("IncludeTimestamp", this.includeTimestamp);
			this.IndentOutput = key.GetValue("IndentOutput", this.indentOutput);
			this.TextColor = key.GetValue("TextColor", this.textColor);
			this.IsHighlight = key.GetValue("IsHighlight", this.isHighlight);
		}

		protected internal override void Save(XmlKey key)
		{
			base.Save(key);
			key.SetValue("Message", this.message);
			key.SetValue("IncludeTimestamp", this.includeTimestamp);
			key.SetValue("IndentOutput", this.indentOutput);
			key.SetValue("TextColor", this.textColor);
			key.SetValue("IsHighlight", this.isHighlight);
		}

		#endregion
	}
}
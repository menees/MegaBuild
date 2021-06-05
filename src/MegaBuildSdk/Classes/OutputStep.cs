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

		public OutputStep(Project project, StepCategory category, StepTypeInfo info)
			: base(project, category, info, ExecSupports.None)
		{
		}

		#endregion

		#region Public Properties

		public bool IncludeTimestamp
		{
			get => this.includeTimestamp;
			set => this.SetValue(ref this.includeTimestamp, value);
		}

		public int IndentOutput
		{
			get => this.indentOutput;
			set => this.SetValue(ref this.indentOutput, value);
		}

		public bool IsHighlight
		{
			get => this.isHighlight;
			set => this.SetValue(ref this.isHighlight, value);
		}

		public string Message
		{
			get => this.message;
			set => this.SetValue(ref this.message, value);
		}

		public Color TextColor
		{
			get => this.textColor;
			set => this.SetValue(ref this.textColor, value);
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

		[SuppressMessage("Usage", "CC0022:Should dispose object", Justification = "Caller disposes new controls.")]
		public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
		{
			base.GetStepEditorControls(controls);
			controls.Add(new OutputStepCtrl { Step = this });
		}

		#endregion

		#region Protected Methods

		protected internal override void Load(XmlKey key)
		{
			base.Load(key);
			this.Message = key.GetValue(nameof(this.Message), this.message);
			this.IncludeTimestamp = key.GetValue(nameof(this.IncludeTimestamp), this.includeTimestamp);
			this.IndentOutput = key.GetValue(nameof(this.IndentOutput), this.indentOutput);
			this.TextColor = key.GetValue(nameof(this.TextColor), this.textColor);
			this.IsHighlight = key.GetValue(nameof(this.IsHighlight), this.isHighlight);
		}

		protected internal override void Save(XmlKey key)
		{
			base.Save(key);
			key.SetValue(nameof(this.Message), this.message);
			key.SetValue(nameof(this.IncludeTimestamp), this.includeTimestamp);
			key.SetValue(nameof(this.IndentOutput), this.indentOutput);
			key.SetValue(nameof(this.TextColor), this.textColor);
			key.SetValue(nameof(this.IsHighlight), this.isHighlight);
		}

		#endregion
	}
}
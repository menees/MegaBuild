namespace SampleStep
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;
	using MegaBuild;

	#endregion

	[StepDisplay("Beep", "Plays a beep.", "BeepStep.ico")]
	public sealed class BeepStep : ExecutableStep
	{
		#region Private Data Members

		private int duration = 250;
		private int frequency = 1000;

		#endregion

		#region Constructors

		public BeepStep(Project project, StepCategory category, StepTypeInfo info)
			: base(project, category, info, ExecSupports.None)
		{
		}

		#endregion

		#region Public Properties

		public int Duration
		{
			get
			{
				return this.duration;
			}

			set
			{
				if (this.duration != value)
				{
					this.duration = value;
					this.SetModified();
				}
			}
		}

		public int Frequency
		{
			get
			{
				return this.frequency;
			}

			set
			{
				if (this.frequency != value)
				{
					this.frequency = value;
					this.SetModified();
				}
			}
		}

		#endregion

		#region Public Methods

		public override bool Execute(StepExecuteArgs args)
		{
			this.Project.OutputLine(string.Format("Beeping at {0} Hz for {1} milliseconds.", this.Frequency, this.Duration));

			bool result = false;
			try
			{
				if (!this.StopBuilding)
				{
					Console.Beep(this.Frequency, this.Duration);
					result = true;
				}
			}
			catch (ArgumentOutOfRangeException)
			{
			}

			return result;
		}

		public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
		{
			base.GetStepEditorControls(controls);
			controls.Add(new BeepStepCtrl() { Step = this });
		}

		#endregion

		#region Protected Methods

		protected override void Load(XmlKey key)
		{
			base.Load(key);
			this.Frequency = key.GetValue("Frequency", this.Frequency);
			this.Duration = key.GetValue("Duration", this.Duration);
		}

		protected override void Save(XmlKey key)
		{
			base.Save(key);
			key.SetValue("Frequency", this.Frequency);
			key.SetValue("Duration", this.Duration);
		}

		#endregion
	}
}
namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Threading;
	using Menees;

	#endregion

	[StepDisplay("Sleep", "Suspends execution for a specified amount of time.", "Images.SleepStep.ico")]
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Called by Reflection.")]
	internal sealed class SleepStep : ExecutableStep
	{
		#region Private Data Members

		private int sleepTimeMilliseconds;

		#endregion

		#region Constructors

		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Reflection.")]
		public SleepStep(Project project, StepCategory category, StepTypeInfo info)
			: base(project, category, info, ExecSupports.None)
		{
		}

		#endregion

		#region Public Properties

		public int SleepTimeMilliseconds
		{
			get
			{
				return this.sleepTimeMilliseconds;
			}

			set
			{
				this.SetValue(ref this.sleepTimeMilliseconds, value);
			}
		}

		public override string StepInformation => string.Format("{0} ms", this.SleepTimeMilliseconds);

		#endregion

		#region Public Methods

		public override bool Execute(StepExecuteArgs args)
		{
			this.Project.OutputLine(string.Format("Sleeping for {0} milliseconds.", this.SleepTimeMilliseconds));
			const int MillisecondsPerSecond = 1000;

			// Rather than sleep for the entire interval, we'll sleep one second at a time so we can check for a stop build signal.
			int wholeSeconds = this.sleepTimeMilliseconds / MillisecondsPerSecond;
			int remainingMilliseconds = this.sleepTimeMilliseconds % MillisecondsPerSecond;

			bool result = true;
			for (int i = 0; i < wholeSeconds; i++)
			{
				if (this.StopBuilding)
				{
					result = false;
					break;
				}

				Thread.Sleep(MillisecondsPerSecond);
			}

			if (remainingMilliseconds > 0)
			{
				if (this.StopBuilding)
				{
					result = false;
				}
				else
				{
					Thread.Sleep(remainingMilliseconds);
				}
			}

			return result;
		}

		[SuppressMessage("Usage", "CC0022:Should dispose object", Justification = "Caller disposes new controls.")]
		[SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller disposes new controls.")]
		public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
		{
			base.GetStepEditorControls(controls);
			controls.Add(new SleepStepCtrl { Step = this });
		}

		#endregion

		#region Protected Methods

		protected internal override void Load(XmlKey key)
		{
			base.Load(key);
			this.SleepTimeMilliseconds = key.GetValue("SleepTime", this.SleepTimeMilliseconds);
		}

		protected internal override void Save(XmlKey key)
		{
			base.Save(key);
			key.SetValue("SleepTime", this.SleepTimeMilliseconds);
		}

		#endregion
	}
}
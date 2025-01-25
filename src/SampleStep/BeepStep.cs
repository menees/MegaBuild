namespace SampleStep;

#region Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
		get => this.duration;

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
		get => this.frequency;

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
#pragma warning disable CC0004 // Catch block cannot be empty
		catch (ArgumentOutOfRangeException)
		{
			// The user asked for a frequencey too high or too low to play.
		}
#pragma warning restore CC0004 // Catch block cannot be empty

		return result;
	}

	[SuppressMessage("Usage", "CC0022:Should dispose object", Justification = "Caller disposes new controls.")]
	public override void GetStepEditorControls(ICollection<StepEditorControl> controls)
	{
		base.GetStepEditorControls(controls);
		controls.Add(new BeepStepCtrl { Step = this });
	}

	#endregion

	#region Protected Methods

	protected override void Load(XmlKey key)
	{
		base.Load(key);
		this.Frequency = key.GetValue(nameof(this.Frequency), this.Frequency);
		this.Duration = key.GetValue(nameof(this.Duration), this.Duration);
	}

	protected override void Save(XmlKey key)
	{
		base.Save(key);
		key.SetValue(nameof(this.Frequency), this.Frequency);
		key.SetValue(nameof(this.Duration), this.Duration);
	}

	#endregion
}
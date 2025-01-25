namespace MegaBuild;

#region Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Menees;

#endregion

[SuppressMessage(
	"Microsoft.Naming",
	"CA1716:IdentifiersShouldNotMatchKeywords",
	MessageId = nameof(Step),
	Justification = "Legacy support.  Also, I don't care about conflicting with a VB keyword.")]
public abstract class Step
{
	#region Private Data Members

	private readonly StepCategory category = StepCategory.Build;
	private readonly StepTypeInfo stepTypeInfo;
	private readonly Project project;

	private bool includeInBuild = true;
	private bool modified;
	private int level;
	private int updateLevel;
	private Step? parent;
	private StepCollection? categorySteps;
	private string description = string.Empty;
	private string name = string.Empty;

	#endregion

	#region Constructors

	protected Step(Project project, StepCategory category, StepTypeInfo info)
	{
		this.project = project;
		this.category = category;
		this.stepTypeInfo = info;
	}

	#endregion

	#region Public Properties

	public StepCategory Category => this.category;

	public StepCollection CategorySteps
	{
		get
		{
			this.categorySteps ??= this.category == StepCategory.Build ? this.project.BuildSteps : this.project.FailureSteps;

			return this.categorySteps;
		}
	}

	public string Description
	{
		get => this.description;
		set => this.SetValue(ref this.description, value);
	}

	public bool IncludeInBuild
	{
		get => this.includeInBuild;
		set
		{
			this.SetValue(ref this.includeInBuild, value);

			// Apply this recursively to all child steps. Do this even if the current step wasn't modified
			// so that it is easy to programmatically ensure a parent and child all have the same setting.
			StepCollection steps = this.CategorySteps;
			int numSteps = steps.Count;
			int thisStepLevel = this.Level;
			for (int i = this.GetIndex() + 1; i < numSteps; i++)
			{
				Step childStep = steps[i];
				if (childStep.Level > thisStepLevel)
				{
					childStep.IncludeInBuild = value;
				}
				else
				{
					// Quit since the level is <= our level, so there are no more child steps.
					break;
				}
			}
		}
	}

	public int Level => this.level;

	public string Name
	{
		get => this.name;
		set => this.SetValue(ref this.name, value);
	}

	public Step? Parent
	{
		get => this.parent;
		set => this.SetValue(ref this.parent, value);
	}

	public Project Project => this.project;

	public virtual string StepInformation => string.Empty;

	public StepTypeInfo StepTypeInfo => this.stepTypeInfo;

	#endregion

	#region Public Methods

	public void BeginUpdate()
	{
		this.updateLevel++;
	}

	public void EndUpdate()
	{
		this.updateLevel--;

		if (this.updateLevel == 0 && this.modified)
		{
			this.modified = false;

			// Call the virtual function first so derived classes can update any "calculated" members before
			// external things like the list views get the update notification.
			this.StepEdited();

			this.project.StepEdited(this);
		}
	}

	public virtual void ExecuteCustomVerb(string verb)
	{
	}

	public virtual string[]? GetCustomVerbs() => null;

	public int GetIndex() => this.CategorySteps.IndexOf(this);

	[SuppressMessage("Usage", "CC0022:Should dispose object", Justification = "Caller disposes new controls.")]
	public virtual void GetStepEditorControls(ICollection<StepEditorControl> controls)
	{
		controls.Add(new GeneralStepCtrl { Step = this });
	}

	public bool Indent() => this.SetLevel(this.Level + 1);

	public virtual void ProjectOptionsChanged()
	{
	}

	public bool Unindent() => this.SetLevel(this.Level - 1);

	#endregion

	#region Internal Members

	internal bool SetLevel(int newLevel)
	{
		bool result = false;

		if (newLevel >= 0)
		{
			this.level = newLevel;
			this.SetParent();
			this.SetModified();
			result = true;
		}

		return result;
	}

	internal void SetParent()
	{
		// Find the first item in the collection that has a lower level.
		// If none have lower level, then parent is null.
		this.parent = null;

		StepCollection steps = this.CategorySteps;
		int startingIndex = steps.IndexOf(this) - 1;
		for (int i = startingIndex; i >= 0; i--)
		{
			Step step = steps[i];
			if (step.Level < this.Level)
			{
				this.parent = step;
				break;
			}
		}
	}

	#endregion

	#region Protected Methods

	protected internal virtual void Load(XmlKey key)
	{
		this.Name = key.GetValue(nameof(this.Name), this.name);
		this.Description = key.GetValue(nameof(this.Description), this.description);
		this.IncludeInBuild = key.GetValue(nameof(this.IncludeInBuild), this.includeInBuild);
		this.SetLevel(key.GetValue(nameof(this.Level), this.level));
	}

	protected internal virtual void Save(XmlKey key)
	{
		key.SetValue(nameof(this.Name), this.name);
		key.SetValue(nameof(this.Description), this.description);
		key.SetValue(nameof(this.IncludeInBuild), this.includeInBuild);
		key.SetValue(nameof(this.Level), this.level);
	}

	protected void SetModified()
	{
		this.BeginUpdate();
		this.modified = true;
		this.EndUpdate();
	}

	protected bool SetValue<T>(ref T data, T value)
	{
		bool result = false;

		if (!Equals(data, value))
		{
			data = value;
			this.SetModified();
			result = true;
		}

		return result;
	}

	protected virtual void StepEdited()
	{
	}

	#endregion
}
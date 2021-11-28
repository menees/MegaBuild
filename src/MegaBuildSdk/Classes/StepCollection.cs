namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Drawing;
	using Menees;

	#endregion

	public sealed class StepCollection : IReadOnlyCollection<Step>
	{
		#region Private Data Members

		private readonly List<Step> steps = new();

		#endregion

		#region Constructors

		public StepCollection()
		{
		}

		#endregion

		#region Public Properties

		public int Count => this.steps.Count;

		public Step this[int index] => this.steps[index];

		#endregion

		#region Public Methods

		public bool Contains(Step step) => this.steps.Contains(step);

		public void CopyTo(Step[] steps)
		{
			this.steps.CopyTo(steps);
		}

		public void CopyTo(Step[] steps, int index)
		{
			this.steps.CopyTo(steps, index);
		}

		public void CopyTo(int sourceIndex, Step[] steps, int targetIndex, int count)
		{
			this.steps.CopyTo(sourceIndex, steps, targetIndex, count);
		}

		public int IndexOf(Step step) => this.steps.IndexOf(step);

		IEnumerator<Step> IEnumerable<Step>.GetEnumerator() => this.steps.GetEnumerator();

		public System.Collections.IEnumerator GetEnumerator() => this.steps.GetEnumerator();

		#endregion

		#region Internal Methods

		internal void Add(Step step)
		{
			this.steps.Add(step);
		}

		internal void Clear()
		{
			this.steps.Clear();
		}

		internal void Insert(int index, Step step)
		{
			this.steps.Insert(index, step);
		}

		internal void Load(XmlKey key, Project project, StepCategory category)
		{
			XmlKey[] stepKeys = key.GetSubkeys();
			int numSteps = stepKeys.Length;
			for (int i = 0; i < numSteps; i++)
			{
				XmlKey stepKey = stepKeys[i];
				string stepTypeName = stepKey.GetValue("StepTypeName", string.Empty);
				if (stepTypeName.Length > 0)
				{
					// The StepTypeInfo might not be available (e.g. if the step type's static CheckAvailability() method made the step unavailable).
					StepTypeInfo info = Manager.GetStepTypeInfo(stepTypeName);
					if (info != null)
					{
						Step step = project.CreateStep(category, info);

						// Add it to the collection now.
						this.Add(step);

						// Now that the step is in the collection, we can call Load. This order is necessary because
						// Load needs to find the parent for each step, which means the step must already be in the
						// collection. Use a Begin/EndUpdate pair so only one change notification gets sent.
						step.BeginUpdate();
						try
						{
							step.Load(stepKey);
						}
						finally
						{
							step.EndUpdate();
						}
					}
					else
					{
						project.Output(
							string.Format("Type '{0}' was not found when loading step '{1}'.  The step was skipped.", stepTypeName, key.XmlKeyName),
							OutputColors.Error,
							0,
							true);
					}
				}
			}
		}

		internal void Remove(Step step)
		{
			this.steps.Remove(step);
		}

		internal void RemoveAt(int index)
		{
			this.steps.RemoveAt(index);
		}

		internal void ResetStatuses()
		{
			foreach (Step step in this.steps)
			{
				ExecutableStep executableStep = step as ExecutableStep;
				if (executableStep != null)
				{
					executableStep.ResetStatus();
				}
			}
		}

		internal void Save(XmlKey key)
		{
			int numSteps = this.Count;
			for (int i = 0; i < numSteps; i++)
			{
				Step step = this[i];
				XmlKey stepKey = key.AddSubkey(nameof(Step));
				stepKey.SetValue("StepTypeName", step.GetType().FullName);
				step.Save(stepKey);
			}
		}

		#endregion
	}
}
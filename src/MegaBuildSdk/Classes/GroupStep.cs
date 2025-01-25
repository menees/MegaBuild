namespace MegaBuild;

#region Using Directives

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

[StepDisplay("Group", "Groups related steps together.", "Images.GroupStep.ico")]
[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Called by Reflection.")]
internal sealed class GroupStep : Step
{
	#region Constructors

	public GroupStep(Project project, StepCategory category, StepTypeInfo info)
		: base(project, category, info)
	{
	}

	#endregion
}
namespace MegaBuild
{
	#region Using Directives

	using System;

	#endregion

	public sealed class ProjectStepsChangedEventArgs : EventArgs
	{
		#region Constructors

		internal ProjectStepsChangedEventArgs(ProjectStepsChangedType changeType, Step step, int oldIndex, int newIndex)
		{
			this.ChangeType = changeType;
			this.Step = step;
			this.OldIndex = oldIndex;
			this.NewIndex = newIndex;
		}

		#endregion

		#region Public Properties

		public ProjectStepsChangedType ChangeType { get; }

		public int NewIndex { get; }

		public int OldIndex { get; }

		public Step Step { get; }

		#endregion
	}
}
namespace MegaBuild
{
	#region Using Directives

	using System;

	#endregion

	public sealed class BuildProgressEventArgs : EventArgs
	{
		#region Constructors

		internal BuildProgressEventArgs(string message)
		{
			this.Message = message;
		}

		internal BuildProgressEventArgs(string message, int currentStep, int totalSteps)
		{
			this.Message = message;
			this.CurrentStep = currentStep;
			this.TotalSteps = totalSteps;
			this.UseStepNumbers = true;
		}

		#endregion

		#region Public Properties

		public int CurrentStep { get; }

		public string Message { get; }

		public int TotalSteps { get; }

		public bool UseStepNumbers { get; }

		#endregion
	}
}
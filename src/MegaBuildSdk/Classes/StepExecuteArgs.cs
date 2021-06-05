namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Diagnostics.CodeAnalysis;

	#endregion

	public class StepExecuteArgs
	{
		#region Public Fields

		public static readonly StepExecuteArgs Empty = new();

		#endregion

		#region Constructors

		protected StepExecuteArgs()
		{
		}

		#endregion
	}
}
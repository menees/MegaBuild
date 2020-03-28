namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Diagnostics.CodeAnalysis;

	#endregion

	[SuppressMessage(
		"Microsoft.Design",
		"CA1052:StaticHolderTypesShouldBeSealed",
		Justification = "This is similar to EventArgs.  Derived classes exist (e.g., VSStepExecuteArgs).")]
	[SuppressMessage(
		"Microsoft.Design",
		"CA1053:StaticHolderTypesShouldNotHaveConstructors",
		Justification = "This is similar to EventArgs.  Derived classes exist (e.g., VSStepExecuteArgs).")]
	public class StepExecuteArgs
	{
		#region Public Fields

		[SuppressMessage(
			"Microsoft.Security",
			"CA2104:DoNotDeclareReadOnlyMutableReferenceTypes",
			Justification = "The base StepExecuteArgs class is immutable.")]
		public static readonly StepExecuteArgs Empty = new StepExecuteArgs();

		#endregion

		#region Constructors

		protected StepExecuteArgs()
		{
		}

		#endregion
	}
}
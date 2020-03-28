namespace MegaBuild
{
	#region Using Directives

	using System;

	#endregion

	[AttributeUsage(AttributeTargets.Class)]
	public sealed class StepDisplayAttribute : Attribute
	{
		#region Private Data Members

		private string description;
		private string iconResourceName;
		private string name;

		#endregion

		#region Constructors

		public StepDisplayAttribute(string name, string description, string iconResourceName)
		{
			this.name = name;
			this.description = description;
			this.iconResourceName = iconResourceName;
		}

		#endregion

		#region Public Properties

		public string Description => this.description;

		public string IconResourceName => this.iconResourceName;

		public string Name => this.name;

		#endregion
	}
}
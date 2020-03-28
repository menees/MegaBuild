namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;

	#endregion

	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class MegaBuildExtensionAttribute : Attribute
	{
	}
}

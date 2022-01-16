namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Text;
	using Microsoft.JScript;

	#endregion

	internal static class JavaScriptUtility
	{
		#region Public Methods

		public static string Escape(object value) => GlobalObject.escape(value);

		public static string Unescape(object value) => GlobalObject.unescape(value);

		#endregion
	}
}

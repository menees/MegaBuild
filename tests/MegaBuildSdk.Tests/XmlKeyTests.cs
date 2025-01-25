namespace MegaBuild;

#region Using Directives

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

#endregion

[TestClass]
public class XmlKeyTests
{
	#region Private Data Members

	private static readonly (string Original, string Escaped)[] TestCases =
	[
		(string.Empty, string.Empty),
		("X Y", "X Y"),
		("  X Y  ", "%20%20X Y%20%20"),
		("%", "%25"),
		(" \r X  Y Z \n ", "%20%0D X%20%20Y Z %0A%20"),
	];

	#endregion

	#region Public Methods

	[TestMethod]
	public void EscapeTest()
	{
		foreach ((string original, string escaped) in TestCases)
		{
			XmlKey.Escape(original).ShouldBe(escaped, original);
		}
	}

	[TestMethod]
	public void UnescapeTest()
	{
		foreach ((string original, string escaped) in TestCases)
		{
			XmlKey.Unescape(escaped).ShouldBe(original, escaped);
		}
	}

	#endregion
}
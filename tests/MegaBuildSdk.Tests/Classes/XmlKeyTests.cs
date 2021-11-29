namespace MegaBuild.Tests
{
	#region Using Directives

	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Shouldly;

	#endregion

	[TestClass]
	public class XmlKeyTests
	{
		#region Private Data Members

		private static readonly (string Original, string Escaped)[] TestCases = new[]
		{
			(string.Empty, string.Empty),
			("X Y", "X Y"),
			("  X Y  ", "%20%20X Y%20%20"),
			("%", "%25"),
			(" \r X  Y Z \n ", "%20%0D X%20%20Y Z %0A%20"),
		};

		#endregion

		#region Public Methods

		[TestMethod]
		public void EscapeTest()
		{
			foreach (var testCase in TestCases)
			{
				XmlKey.Escape(testCase.Original).ShouldBe(testCase.Escaped, testCase.Original);
			}
		}

		[TestMethod]
		public void UnescapeTest()
		{
			foreach (var testCase in TestCases)
			{
				XmlKey.Unescape(testCase.Escaped).ShouldBe(testCase.Original, testCase.Escaped);
			}
		}

		#endregion
	}
}
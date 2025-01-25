namespace MegaBuild;

#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

#endregion

[TestClass]
public class ManagerTests
{
	#region Public Methods

	[DoNotParallelize] // Loads a project file.
	[TestMethod]
	public void MegaBuildSetCommandTest()
	{
		Manager.Variables.Clear();

		using (ProjectContainer container = new())
		using (Project project = new(container))
		{
			project.Open(@"Content\MegaBuild.Set Project Variable.mgb").ShouldBeTrue();
			project.Variables.ShouldBe(new Dictionary<string, string> { ["%MB.Configuration%"] = "Debug", ["%MB.Target%"] = "Build",  });
			Manager.Variables.ShouldBeEmpty();

			Set("MB.Target", "Rebuild");
			project.Variables.ShouldBe(new Dictionary<string, string> { ["%MB.Configuration%"] = "Debug", ["%MB.Target%"] = "Rebuild", });
			Manager.Variables.ShouldBeEmpty();

			Set("MB.Configuration", "Release");
			project.Variables.ShouldBe(new Dictionary<string, string> { ["%MB.Configuration%"] = "Release", ["%MB.Target%"] = "Rebuild", });
			Manager.Variables.ShouldBeEmpty();

			Set("MB Global Var", "For Manager");
			project.Variables.ShouldBe(new Dictionary<string, string> { ["%MB.Configuration%"] = "Release", ["%MB.Target%"] = "Rebuild", });
			Manager.Variables.ShouldBe(new Dictionary<string, string> { ["%MB Global Var%"] = "For Manager" });

			static void Set(string name, string value) =>
				Manager.Output($"MegaBuild.Set {name}={value}", 0, Color.Black);
		}

		Manager.Variables.Clear();
	}

	#endregion

	#region Private Types

	private sealed class ProjectContainer : IContainer
	{
		public ComponentCollection Components => new([]);

		public void Add(IComponent? component)
		{
		}

		public void Add(IComponent? component, string? name)
		{
		}

		public void Dispose()
		{
		}

		public void Remove(IComponent? component)
		{
		}
	}

	#endregion
}

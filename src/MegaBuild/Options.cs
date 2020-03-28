namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Windows.Forms;
	using Menees;
	using Menees.Windows.Forms;
	using Microsoft.Win32;

	#endregion

	internal static class Options
	{
		#region Internal Constants

		internal static readonly IReadOnlyList<string> SupportedTimestampFormats = new[] { "(none)", "HH:mm:ss.fff", "HH:mm:ss", "mm:ss.fff", "mm:ss" };

		internal static readonly string DefaultTimestampFormat = SupportedTimestampFormats[1];

		// This has to be non-empty to (a) display it in the UI and (b) to ensure that ISettingsNode.GetValue will return it instead of defaultValue.
		internal static readonly string NoTimestampFormat = SupportedTimestampFormats[0];

		#endregion

		#region Public Properties

		public static bool AlwaysOnTop { get; set; }

		public static bool ClearOutputBeforeBuild { get; set; }

		public static bool NeverShowProjectComments { get; set; }

		public static bool ReloadLastProjectAtStartup { get; set; }

		public static bool SaveChangesBeforeBuild { get; set; }

		public static bool SwitchTabsOnFailure { get; set; }

		public static bool WordWrapOutputWindow { get; set; }

		public static bool ShowProgressInTaskbar { get; set; }

		public static string TimestampFormat { get; set; }

		#endregion

		#region Public Methods

		public static void Load(ISettingsNode node)
		{
			SaveChangesBeforeBuild = node.GetValue("SaveChangesBeforeBuild", SaveChangesBeforeBuild);
			ReloadLastProjectAtStartup = node.GetValue("ReloadLastProjectAtStartup", true);
			AlwaysOnTop = node.GetValue("AlwaysOnTop", AlwaysOnTop);
			NeverShowProjectComments = node.GetValue("NeverShowProjectComments", NeverShowProjectComments);
			ClearOutputBeforeBuild = node.GetValue("ClearOutputBeforeBuild", true);
			WordWrapOutputWindow = node.GetValue("WordWrapOutputWindow", WordWrapOutputWindow);
			SwitchTabsOnFailure = node.GetValue("SwitchTabsOnFailure", SwitchTabsOnFailure);
			ShowProgressInTaskbar = node.GetValue("ShowProgressInTaskbar", true);
			TimestampFormat = node.GetValue("TimestampFormat", DefaultTimestampFormat);
		}

		public static void Save(ISettingsNode node)
		{
			node.SetValue("SaveChangesBeforeBuild", SaveChangesBeforeBuild);
			node.SetValue("ReloadLastProjectAtStartup", ReloadLastProjectAtStartup);
			node.SetValue("AlwaysOnTop", AlwaysOnTop);
			node.SetValue("NeverShowProjectComments", NeverShowProjectComments);
			node.SetValue("ClearOutputBeforeBuild", ClearOutputBeforeBuild);
			node.SetValue("WordWrapOutputWindow", WordWrapOutputWindow);
			node.SetValue("SwitchTabsOnFailure", SwitchTabsOnFailure);
			node.SetValue("ShowProgressInTaskbar", ShowProgressInTaskbar);
			node.SetValue("TimestampFormat", TimestampFormat);
		}

		#endregion
	}
}
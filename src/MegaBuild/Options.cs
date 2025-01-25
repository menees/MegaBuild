namespace MegaBuild;

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

	internal static readonly IReadOnlyList<string> SupportedTimestampFormats = ["(none)", "HH:mm:ss.fff", "HH:mm:ss", "mm:ss.fff", "mm:ss"];

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

	public static string TimestampFormat { get; set; } = DefaultTimestampFormat;

	public static bool OutputWindowOnRight { get; set; }

	public static bool UseTimestampPrefix
		=> !string.IsNullOrEmpty(TimestampFormat) && TimestampFormat != NoTimestampFormat;

	#endregion

	#region Public Methods

	public static void Load(ISettingsNode node)
	{
		SaveChangesBeforeBuild = node.GetValue(nameof(SaveChangesBeforeBuild), SaveChangesBeforeBuild);
		ReloadLastProjectAtStartup = node.GetValue(nameof(ReloadLastProjectAtStartup), true);
		AlwaysOnTop = node.GetValue(nameof(AlwaysOnTop), AlwaysOnTop);
		NeverShowProjectComments = node.GetValue(nameof(NeverShowProjectComments), NeverShowProjectComments);
		ClearOutputBeforeBuild = node.GetValue(nameof(ClearOutputBeforeBuild), true);
		WordWrapOutputWindow = node.GetValue(nameof(WordWrapOutputWindow), WordWrapOutputWindow);
		SwitchTabsOnFailure = node.GetValue(nameof(SwitchTabsOnFailure), SwitchTabsOnFailure);
		ShowProgressInTaskbar = node.GetValue(nameof(ShowProgressInTaskbar), true);
		TimestampFormat = node.GetValue(nameof(TimestampFormat), DefaultTimestampFormat);
		OutputWindowOnRight = node.GetValue(nameof(OutputWindowOnRight), true);
	}

	public static void Save(ISettingsNode node)
	{
		node.SetValue(nameof(SaveChangesBeforeBuild), SaveChangesBeforeBuild);
		node.SetValue(nameof(ReloadLastProjectAtStartup), ReloadLastProjectAtStartup);
		node.SetValue(nameof(AlwaysOnTop), AlwaysOnTop);
		node.SetValue(nameof(NeverShowProjectComments), NeverShowProjectComments);
		node.SetValue(nameof(ClearOutputBeforeBuild), ClearOutputBeforeBuild);
		node.SetValue(nameof(WordWrapOutputWindow), WordWrapOutputWindow);
		node.SetValue(nameof(SwitchTabsOnFailure), SwitchTabsOnFailure);
		node.SetValue(nameof(ShowProgressInTaskbar), ShowProgressInTaskbar);
		node.SetValue(nameof(TimestampFormat), TimestampFormat);
		node.SetValue(nameof(OutputWindowOnRight), OutputWindowOnRight);
	}

	#endregion
}
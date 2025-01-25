namespace MegaBuild;

#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Menees;
using Menees.Windows.Forms;

#endregion

internal partial class CsiStepCtrl : StepEditorControl
{
	#region Private Data Members

	private CsiStep? step;

	#endregion

	#region Constructors

	public CsiStepCtrl()
	{
		this.InitializeComponent();
		MSBuildStepCtrl.AddVersions(this.cbToolsVersion, version => version >= CsiStep.MinSupportedCsiVersion);
	}

	#endregion

	#region Public Properties

	public override string DisplayName => "C# Interactive";

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public CsiStep Step
	{
		set
		{
			if (this.step != value)
			{
				this.step = value;

				this.edtScript.Text = this.step.ScriptFile;
				this.edtScriptArgs.Text = this.step.ScriptArguments;
				this.edtWorkingDirectory.Text = this.step.WorkingDirectory;
				this.cbToolsVersion.SelectedValue = this.step.CsiVersion;
				this.treatErrorAsOutput.Checked = this.step.TreatErrorStreamAsOutput;
				this.edtCsiOptions.Text = this.step.CsiOptions;
			}
		}
	}

	#endregion

	#region Public Methods

	public override bool OnOk()
	{
		bool result = false;

		if (string.IsNullOrEmpty(this.edtScript.Text.Trim()))
		{
			WindowsUtility.ShowError(this, "You must specify a script.");
		}
		else if (this.step != null)
		{
			this.step.ScriptFile = this.edtScript.Text;
			this.step.ScriptArguments = ScrubLines(this.edtScriptArgs.Text);
			this.step.WorkingDirectory = this.edtWorkingDirectory.Text;
			this.step.CsiVersion = (MSBuildToolsVersion?)this.cbToolsVersion.SelectedValue ?? default;
			this.step.TreatErrorStreamAsOutput = this.treatErrorAsOutput.Checked;
			this.step.CsiOptions = ScrubLines(this.edtCsiOptions.Text);
			result = true;
		}

		return result;
	}

	#endregion

	#region Private Methods

	private static void CollapseAndAppendValue(StringBuilder sb, string value)
	{
		bool quoteValue = value.Contains(' ');
		if (quoteValue)
		{
			sb.Append('"');
		}

		string collapsedValue = Manager.CollapseVariables(value);
		sb.Append(collapsedValue);

		if (quoteValue)
		{
			sb.Append('"');
		}
	}

	private static void AppendValue(TextBoxBase textBox, string prefix, string value)
	{
		StringBuilder line = new();
		if (textBox.TextLength > 0)
		{
			string text = textBox.Text;
			if (text.Length > 0 && !char.IsWhiteSpace(text[text.Length - 1]))
			{
				line.AppendLine();
			}
		}

		line.Append(prefix);
		CollapseAndAppendValue(line, value);

		string append = line.ToString();
		textBox.AppendText(append);
	}

	private static string ScrubLines(string text)
	{
		string result = string.Join(Environment.NewLine, text.Split('\r', '\n').Where(line => line.IsNotWhiteSpace()).Select(line => line.Trim()));
		return result;
	}

	private void AppendOption(string option, string value)
		=> AppendValue(this.edtCsiOptions, option, value);

	#endregion

	#region Private Event Handlers

	private void BrowseScript_Click(object sender, EventArgs e)
	{
		this.OpenCsxDlg.FileName = Manager.ExpandVariables(this.edtScript.Text);
		if (this.OpenCsxDlg.ShowDialog(this) == DialogResult.OK)
		{
			this.edtScript.Text = Manager.CollapseVariables(this.OpenCsxDlg.FileName);
		}
	}

	private void AddScriptArg_Click(object sender, EventArgs e)
	{
		this.OpenAllDlg.FileName = string.Empty;
		if (this.OpenAllDlg.ShowDialog(this) == DialogResult.OK)
		{
			foreach (string value in this.OpenAllDlg.FileNames)
			{
				AppendValue(this.edtScriptArgs, string.Empty, value);
			}
		}
	}

	private void BrowseDirectory_Click(object sender, EventArgs e)
	{
		string initialFolder = Manager.ExpandVariables(this.edtWorkingDirectory.Text);
		string? selectedFolder = WindowsUtility.SelectFolder(this, "Select Working Directory", initialFolder);
		if (selectedFolder.IsNotEmpty())
		{
			this.edtWorkingDirectory.Text = Manager.CollapseVariables(selectedFolder);
		}
	}

	private void AddReference_Click(object sender, EventArgs e)
	{
		this.OpenAsmDlg.FileName = string.Empty;
		if (this.OpenAsmDlg.ShowDialog(this) == DialogResult.OK)
		{
			this.AppendOption("/r:", this.OpenAsmDlg.FileName);
		}
	}

	private void AddLibPath_Click(object sender, EventArgs e)
	{
		string? libraryPath = WindowsUtility.SelectFolder(this, "Add Library Path", null);
		if (libraryPath.IsNotEmpty())
		{
			this.AppendOption("/lib:", libraryPath);
		}
	}

	private void AddUsing_Click(object sender, EventArgs e)
	{
		static bool IsValidCSharpIdentifier(string value)
		{
			bool result = false;

			if (value.IsNotWhiteSpace())
			{
				char ch = value[0];
				if (ch == '_' || char.IsLetter(ch))
				{
					result = value.Skip(1).All(ch => ch == '_' || char.IsLetterOrDigit(ch));
				}
			}

			return result;
		}

		string? input = WindowsUtility.ShowInputBox(
			this,
			"Enter a namespace to treat as a global using directive:",
			"Add Global Using",
			nameof(System),
			null,
			entered =>
			{
				List<string> errors = [];
				string[] parts = entered.Split('.');
				foreach (string part in parts)
				{
					// This simplistic validation doesn't handle several edge cases, but a user can type those in this.edtCsiOptions manually.
					// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/lexical-structure#743-identifiers
					if (!IsValidCSharpIdentifier(part))
					{
						errors.Add($"\"{part}\" is not a valid C# identifier.");
					}
				}

				string errorMessage = errors.Count > 0 ? string.Join(Environment.NewLine, errors) : string.Empty;
				return errorMessage;
			});

		if (input.IsNotWhiteSpace())
		{
			this.AppendOption("/u:", input);
		}
	}

	#endregion
}
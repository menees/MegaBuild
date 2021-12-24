﻿namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using Menees.Windows.Forms;

	#endregion

	internal sealed partial class ExecOutputCtrl : StepEditorControl
	{
		#region Private Data Members

		private ExecutableStep step;

		#endregion

		#region Constructors

		public ExecOutputCtrl()
		{
			this.InitializeComponent();
			this.cbStyle.Items.AddRange(((OutputStyle[])Enum.GetValues(typeof(OutputStyle))).OrderBy(v => v).Select(v => v.ToString()).ToArray());

			// Turn off ExtendedListView's default sorting so ListViewItemMover will work with it.
			this.lstPatterns.ListViewItemSorter = null;
		}

		#endregion

		#region Public Properties

		public override string DisplayName => "Output";

		public ExecutableStep Step
		{
			set
			{
				if (this.step != value)
				{
					this.step = value;
					this.chkAutoColorErrorsAndWarnings.Checked = this.step.AutoColorErrorsAndWarnings;

					ListView.ListViewItemCollection items = this.lstPatterns.Items;
					this.lstPatterns.BeginUpdate();
					try
					{
						items.Clear();
						if (this.step.CustomOutputStyles != null)
						{
							foreach (var tuple in this.step.CustomOutputStyles)
							{
								ListViewItem item = new(new[] { tuple.Style.ToString(), tuple.Pattern.ToString() });
								items.Add(item);
							}
						}

						this.lstPatterns.AutoSizeColumns();
					}
					finally
					{
						this.lstPatterns.EndUpdate();
					}

					this.UpdateControlStates();
				}
			}
		}

		#endregion

		#region Private Properties

		private ListViewItem SelectedItem
		{
			get
			{
				ListViewItem result = null;

				if (this.lstPatterns.SelectedIndices.Count > 0)
				{
					result = this.lstPatterns.SelectedItems[0];
				}

				return result;
			}
		}

		#endregion

		#region Public Methods

		public override bool OnOk()
		{
			bool result = true;

			List<(OutputStyle Style, Regex Pattern)> list = null;
			foreach (ListViewItem item in this.lstPatterns.Items)
			{
				if (Enum.TryParse(item.SubItems[0].Text, out OutputStyle style) &&
					ExecutableStep.TryParseRegex(item.SubItems[1].Text, out Regex regex))
				{
					list ??= new();
					list.Add((style, regex));
				}
				else
				{
					item.Selected = true;
					item.EnsureVisible();
					WindowsUtility.ShowError(this, "The selected custom style is invalid.");
					this.lstPatterns.Focus();
					item.Focused = true;
					result = false;
					break;
				}
			}

			if (result)
			{
				this.step.AutoColorErrorsAndWarnings = this.chkAutoColorErrorsAndWarnings.Checked;
				this.step.CustomOutputStyles = list;
			}

			return result;
		}

		#endregion

		#region Private Methods

		private bool UpdateControlStates()
		{
			bool hasSelection = this.SelectedItem != null;
			this.btnDelete.Enabled = hasSelection;
			this.cbStyle.Enabled = hasSelection;
			this.edtRegex.Enabled = hasSelection;
			return hasSelection;
		}

		private void Patterns_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool hasSelection = this.UpdateControlStates();

			if (hasSelection)
			{
				ListViewItem item = this.SelectedItem;
				this.cbStyle.SelectedItem = item.SubItems[0].Text;
				this.edtRegex.Text = item.SubItems[1].Text;
			}
			else
			{
				this.cbStyle.SelectedIndex = -1;
				this.edtRegex.Text = string.Empty;
			}
		}

		private void Add_Click(object sender, EventArgs e)
		{
			ListViewItem item = new(new string[] { nameof(OutputStyle.Normal), "^.*Match.*$" });
			this.lstPatterns.Items.Add(item);
			this.lstPatterns.AutoSizeColumns();
			item.Selected = true;
			this.cbStyle.Focus();
		}

		private void Delete_Click(object sender, EventArgs e)
		{
			ListViewItem item = this.SelectedItem;
			if (item != null)
			{
				item.Remove();
			}
		}

		private void Style_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateSelectedItem(this.cbStyle.SelectedItem?.ToString() ?? string.Empty, 0);
		}

		private void Regex_TextChanged(object sender, EventArgs e)
		{
			this.UpdateSelectedItem(this.edtRegex.Text, 1);
		}

		private void UpdateSelectedItem(string text, int subItemIndex)
		{
			ListViewItem item = this.SelectedItem;
			if (item != null)
			{
				item.SubItems[subItemIndex].Text = text;
			}
		}

		#endregion
	}
}
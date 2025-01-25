namespace MegaBuild;

#region Using Directives

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Menees;
using Menees.Windows.Forms;

#endregion

/// <summary>
/// Used to interactively moves list view items up and down.
/// </summary>
public partial class ListViewItemMover : ExtendedUserControl
{
	#region Private Data Members

	private ListView? listView;

	#endregion

	#region Constructors

	public ListViewItemMover()
	{
		this.InitializeComponent();
	}

	#endregion

	#region Public Events

	public event EventHandler? ItemMovedDown;

	public event EventHandler? ItemMovedUp;

	#endregion

	#region Public Properties

	[DefaultValue(null)]
	public ListView? ListView
	{
		get => this.listView;

		set
		{
			if (this.listView != value)
			{
				if (this.listView != null)
				{
					this.listView.SelectedIndexChanged -= this.List_SelectedIndexChanged;
				}

				this.listView = value;

				if (this.listView != null)
				{
					this.listView.SelectedIndexChanged += this.List_SelectedIndexChanged;
				}
			}
		}
	}

	#endregion

	#region Private Properties

	private ListViewItem? SelectedItem
	{
		get
		{
			ListViewItem? result = null;

			// Only work with single selection.  Moving multiple non-contiguous
			// items is too tricky to mess with since it is so rarely needed.
			if (this.listView != null && this.listView.SelectedItems.Count == 1)
			{
				result = this.listView.SelectedItems[0];
			}

			return result;
		}
	}

	#endregion

	#region Public Methods

	public void UpdateControlStates()
	{
		ListViewItem? item = this.SelectedItem;
		this.btnUp.Enabled = item != null && item.Index > 0;
		this.btnDown.Enabled = item != null && item.Index < (this.listView!.Items.Count - 1);
	}

	#endregion

	#region Private Methods

	private void Down_Click(object? sender, EventArgs e)
	{
		ListViewItem? item = this.SelectedItem;
		if (item != null && item.Index < (this.listView!.Items.Count - 1))
		{
			this.MoveItem(item, +1);
			this.ItemMovedDown?.Invoke(this, e);
		}
	}

	private void Up_Click(object? sender, EventArgs e)
	{
		ListViewItem? item = this.SelectedItem;
		if (item != null && item.Index > 0)
		{
			this.MoveItem(item, -1);
			this.ItemMovedUp?.Invoke(this, e);
		}
	}

	private void List_SelectedIndexChanged(object? sender, EventArgs e)
	{
		this.UpdateControlStates();
	}

	private void ListViewItemMover_Load(object? sender, EventArgs e)
	{
		this.UpdateControlStates();
	}

	private void MoveItem(ListViewItem item, int indexOffset)
	{
		Conditions.RequireReference(this.listView, nameof(this.listView));

		// The ListView control won't move items when sorting is enabled.
		// https://stackoverflow.com/a/30536933/1882616
		if (this.listView.ListViewItemSorter != null || this.listView.Sorting != SortOrder.None)
		{
			throw Exceptions.NewInvalidOperationException(
				$"{nameof(ListViewItemMover)} can't manually move items when ListView sorting is enabled.");
		}

		int newIndex = item.Index + indexOffset;
		item.Remove();
		this.listView.Items.Insert(newIndex, item);
		item.Selected = true;
		item.Focused = true;
		this.listView.ArrangeIcons();
		item.EnsureVisible();
	}

	#endregion
}
namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Windows.Forms;
	using Menees;

	#endregion

	internal sealed class VSConfigurationList
	{
		#region Private Data Members

		private List<string> names = new List<string>();
		private Dictionary<string, bool> nameToState = new Dictionary<string, bool>();

		#endregion

		#region Public Properties

		public int Count => this.names.Count;

		#endregion

		#region Public Methods

		public void Add(VSConfigurationList configurations)
		{
			foreach (var entry in configurations.nameToState)
			{
				if (!this.nameToState.ContainsKey(entry.Key))
				{
					this.names.Add((string)entry.Key);
				}

				this.nameToState[entry.Key] = entry.Value;
			}
		}

		public bool Contains(string configuration) => this.nameToState.ContainsKey(configuration);

		public bool GetFromListView(ListView listView)
		{
			// Build new collections
			List<string> newNames = new List<string>();
			Dictionary<string, bool> newNameToState = new Dictionary<string, bool>();
			foreach (ListViewItem item in listView.Items)
			{
				newNames.Add(item.Text);
				newNameToState[item.Text] = item.Checked;
			}

			// Compare the new collections to the current collections.  If anything at all is different, we'll just use the new ones and set the modified flag.
			bool modified = false;
			if (this.names.Count != newNames.Count)
			{
				modified = true;
			}
			else
			{
				int numConfigs = newNames.Count;
				for (int i = 0; i < numConfigs; i++)
				{
					if (this.names[i] != newNames[i] ||
						(bool)this.nameToState[this.names[i]] != (bool)newNameToState[newNames[i]])
					{
						modified = true;
						break;
					}
				}
			}

			if (modified)
			{
				this.names = newNames;
				this.nameToState = newNameToState;
			}

			return modified;
		}

		public string GetName(int index) => this.names[index];

		public bool GetState(int index) => (bool)this.nameToState[this.GetName(index)];

		public void Load(XmlKey key)
		{
			this.names.Clear();
			this.nameToState.Clear();

			XmlKey[] subKeys = key.GetSubkeys();
			foreach (XmlKey subKey in subKeys)
			{
				string name = subKey.XmlKeyName;
				this.names.Add(name);
				this.nameToState.Add(name, subKey.GetValue("Enabled", true));
			}
		}

		public void MergeStatesAndOrder(VSConfigurationList configurations)
		{
			// A merge is slightly different than Add because if configurations contains an entry that isn't
			// in this list, then we don't want to add it. We just want to skip it because it is no longer a
			// valid configuration.
			foreach (var entry in configurations.nameToState)
			{
				if (this.nameToState.ContainsKey(entry.Key))
				{
					this.nameToState[entry.Key] = entry.Value;
				}
			}

			// The names in this.names are currently in the order they were added. We need them to be in
			// the order they were in the passed in list. If we have any configurations that weren't in the
			// old list, then they should come at the end.
			List<string> newNames = new List<string>();

			// Add the old entries that still exist.
			foreach (string name in configurations.names)
			{
				if (this.nameToState.ContainsKey(name))
				{
					newNames.Add(name);
				}
			}

			// Add any entries from the new list that weren't in the old list.
			foreach (string name in this.names)
			{
				if (!configurations.nameToState.ContainsKey(name))
				{
					newNames.Add(name);
				}
			}

			this.names = newNames;
		}

		public void PutInListView(ListView listView)
		{
			listView.Items.Clear();

			int numNames = this.names.Count;
			for (int i = 0; i < numNames; i++)
			{
				string name = this.names[i];
				bool state = (bool)this.nameToState[name];

				ListViewItem item = new ListViewItem(name)
				{
					Checked = state,
				};

				listView.Items.Add(item);
			}
		}

		public void Save(XmlKey key)
		{
			int numNames = this.names.Count;
			for (int i = 0; i < numNames; i++)
			{
				string name = this.names[i];
				bool state = (bool)this.nameToState[name];

				XmlKey subKey = key.GetSubkey("Configuration", name);
				subKey.SetValue("Enabled", state);
			}
		}

		#endregion
	}
}
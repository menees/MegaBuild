namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Drawing;
	using System.Linq;
	using System.Xml.Linq;
	using Menees;

	#endregion

	public sealed class XmlKey
	{
		#region Private Data Members

		private readonly XElement element;

		#endregion

		#region Constructors

		internal XmlKey(XElement element)
		{
			this.element = element;
		}

		#endregion

		#region Public Properties

		public string KeyType => this.element.Name.LocalName;

		public string XmlKeyName => this.GetValue("XmlKeyName", string.Empty);

		#endregion

		#region Public Methods

		public XmlKey AddSubkey(string subkeyType, string xmlKeyName) => new XmlKey(this.AddSubkeyNode(subkeyType, xmlKeyName));

		public void DeleteSubkey(string subkeyType, string xmlKeyName)
		{
			XElement node = this.GetSubkeyNode(subkeyType, xmlKeyName, true);
			if (node != null)
			{
				node.Remove();
			}
		}

		public void DeleteValue(string name)
		{
			XAttribute attribute = this.element.Attribute(name);
			if (attribute != null)
			{
				attribute.Remove();
			}
		}

		public XmlKey GetSubkey(string subkeyType, string xmlKeyName) => new XmlKey(this.GetSubkeyNode(subkeyType, xmlKeyName, false));

		public XmlKey[] GetSubkeys()
		{
			List<XmlKey> lstKeys = new List<XmlKey>();
			if (this.element.HasElements)
			{
				foreach (XElement child in this.element.Elements())
				{
					lstKeys.Add(new XmlKey(child));
				}
			}

			return lstKeys.ToArray();
		}

		public string GetValue(string name, string defaultValue)
		{
			XAttribute attr = this.element.Attribute(name);

			string result = defaultValue;
			if (attr != null)
			{
				result = attr.Value;
				if (string.IsNullOrEmpty(result))
				{
					result = defaultValue;
				}
				else
				{
					// See comments in the static SetValue for why escaping was necessary.
					result = JavaScriptUtility.Unescape(result);
				}
			}

			return result;
		}

		public int GetValue(string name, int defaultValue)
		{
			int result = this.element.GetAttributeValue(name, defaultValue);
			return result;
		}

		public bool GetValue(string name, bool defaultValue)
		{
			bool result = this.element.GetAttributeValue(name, defaultValue);
			return result;
		}

		public Color GetValue(string name, Color defaultValue) => Color.FromArgb(this.GetValue(name, defaultValue.ToArgb()));

		public TEnum GetValue<TEnum>(string name, TEnum defaultValue)
			where TEnum : struct
		{
			Conditions.RequireArgument(defaultValue.GetType().IsEnum, "value must be an enum type.");

			TEnum result = defaultValue;

			// In v4.0 and up we save enum names.  Earlier versions saved enum integer values.
			// For [Flags] enums (e.g., RedirectStandardStreams), we can find comma-separated
			// names, or we can find integer values that aren't direct enum field definitions.
			// Fortunately, Enum.TryParse handles all these cases for us.
			//
			// In v4.1.1 the VSVersion values switched from an initial v to V to make StyleCop happy.
			// So we'll also ignore case here to make sure an old v2013 value loads as V2013.
			string textValue = this.GetValue(name, null);
			if (!string.IsNullOrEmpty(textValue) && !Enum.TryParse<TEnum>(textValue, true, out result))
			{
				result = defaultValue;
			}

			return result;
		}

		public string[] GetValueNames()
		{
			string[] result = this.element.Attributes().Select(a => a.Name.LocalName).ToArray();
			return result;
		}

		public void Prune()
		{
			// Make sure "empty" sub-elements aren't saved out (e.g., if no child elements and no attributes other than XmlKeyName).
			// Copy to an array first since we'll be removing nodes in the loop.
			XElement[] emptyElements = this.element.Descendants().Where(e => !e.HasElements &&
				(!e.HasAttributes || (e.Attributes().Count() == 1 && e.Attributes(nameof(this.XmlKeyName)) != null))).ToArray();
			foreach (XElement empty in emptyElements)
			{
				empty.Remove();
			}
		}

		public void SetValue(string name, string value)
		{
			SetValue(this.element, name, value);
		}

		public void SetValue(string name, int value)
		{
			this.SetValue(name, value.ToString());
		}

		public void SetValue(string name, bool value)
		{
			this.SetValue(name, value.ToString());
		}

		public void SetValue(string name, Color value)
		{
			this.SetValue(name, value.ToArgb());
		}

		public void SetValue<TEnum>(string name, TEnum value)
			where TEnum : struct
		{
			Conditions.RequireArgument(value.GetType().IsEnum, "value must be an enum type.");
			this.SetValue(name, value.ToString());
		}

		public bool ValueExists(string name)
		{
			bool result = this.element.Attribute(name) != null;
			return result;
		}

		#endregion

		#region Private Methods

		private static void SetValue(XElement element, string name, string value)
		{
			string escapedValue = null;

			// Treat an empty value the same as a null value, so we only write attributes with non-empty values.
			// Note: JScript's escape(null) returns "undefined" (which sucks), but escape(string.Empty) returns string.Empty.
			if (!string.IsNullOrEmpty(value))
			{
				// We're using attributes to store arbitrary string values, so we have to escape them first to preserve whitespace.
				// Otherwise, all XML DOMs will normalize the whitespace in ways we don't want (e.g., removing leading/trailing
				// whitespace and changing all remaining whitespace into spaces).
				//
				// We'll use the old JScript escape function because it works on strings of arbitrary length, and it generates
				// compact encodings for the most common characters (<= 255).  The similar Uri.EscapeDataString method
				// is limited to only 32766 characters, and it generates wider encodings for characters 128 through 255.
				escapedValue = JavaScriptUtility.Escape(value);
			}

			// If escapedValue is null this will remove the attribute.
			element.SetAttributeValue(name, escapedValue);
		}

		private XElement AddSubkeyNode(string subKeyType, string xmlKeyName)
		{
			XElement result = new XElement(subKeyType);

			if (!string.IsNullOrEmpty(xmlKeyName))
			{
				SetValue(result, nameof(this.XmlKeyName), xmlKeyName);
			}

			this.element.Add(result);
			return result;
		}

		private XElement GetSubkeyNode(string subkeyType, string xmlKeyName, bool allowNull)
		{
			// This must treat null and empty XmlKeyNames as equal.  We don't write out empty
			// XmlKeyNames, so they'll be read back in as null, which we'll default to empty
			// (on both sides of the comparison).
			XElement result = this.element.Elements(subkeyType)
				.FirstOrDefault(e => e.GetAttributeValue(nameof(this.XmlKeyName), string.Empty) == (xmlKeyName ?? string.Empty));

			if (result == null && !allowNull)
			{
				result = this.AddSubkeyNode(subkeyType, xmlKeyName);
			}

			return result;
		}

		#endregion
	}
}
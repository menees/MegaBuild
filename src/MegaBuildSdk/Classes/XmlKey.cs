namespace MegaBuild;

#region Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Menees;

#endregion

public sealed class XmlKey
{
	#region Private Data Members

	private const decimal SmartEncodingVersion = 5m;

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

	public static string Escape(string text)
	{
		int length = text.Length;

		StringBuilder sb = new(2 * length);
		for (int i = 0; i < length; i++)
		{
			char ch = text[i];
			if (ch == ' ')
			{
				// XML attributes will remove leading and trailing spaces and collapse sequences of spaces to a single space.
				// We don't want that so we have to encode non-safe spaces. It's a safe space if chars before and after are
				// non-spaces. This also encodes "X%20%20Y" when only one of the spaces needs to be encoded, but that's ok.
				// See "Attribute-Value Normalization" at https://www.w3.org/TR/REC-xml/#AVNormalize.
				if (i > 0 && i < length - 1 && text[i - 1] != ' ' && text[i + 1] != ' ')
				{
					// A safe space (i.e., surrounded by non-space chars).
					sb.Append(ch);
				}
				else
				{
					// Either a leading space, a trailing space, or in a sequence of spaces.
					sb.Append(JavaScriptUtility.Escape(ch));
				}
			}
			else if (ch != '%' && ch > ' ' && ch <= '~')
			{
				// Allow any "printable" char except '%' since that's the JavaScript escape char.
				sb.Append(ch);
			}
			else
			{
				// This will be used for control chars, which are invalid in XML, and for higher Unicode chars.
				sb.Append(JavaScriptUtility.Escape(ch));
			}
		}

		string result = sb.ToString();
		return result;
	}

	public static string Unescape(string text)
	{
		// We only escape an XML-safe subset of the JavaScript escaped characters,
		// so we can reverse things correctly using JavaScript's Unescape method.
		string result = JavaScriptUtility.Unescape(text);
		return result;
	}

	public XmlKey AddSubkey(string subkeyType) => this.AddSubkey(subkeyType, string.Empty);

	public XmlKey AddSubkey(string subkeyType, string xmlKeyName) => new(this.AddSubkeyNode(subkeyType, xmlKeyName));

	public XmlKey GetSubkey(string subkeyType) => this.GetSubkey(subkeyType, string.Empty);

	public XmlKey GetSubkey(string subkeyType, string xmlKeyName) => new(this.GetSubkeyNode(subkeyType, xmlKeyName));

	public XmlKey[] GetSubkeys()
	{
		List<XmlKey> lstKeys = [];
		if (this.element.HasElements)
		{
			foreach (XElement child in this.element.Elements())
			{
				lstKeys.Add(new XmlKey(child));
			}
		}

		return [.. lstKeys];
	}

	public string GetValue(string name, string defaultValue)
	{
		string result = this.GetValueN(name, defaultValue) ?? defaultValue;
		return result;
	}

	public string? GetValueN(string name, string? defaultValue)
	{
		XAttribute? attr = this.element.Attribute(name);

		string? result = defaultValue;
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
				decimal version = Project.GetVersion(this.element);
				if (version < SmartEncodingVersion)
				{
					result = JavaScriptUtility.Unescape(result);
				}
				else
				{
					result = Unescape(result);
				}
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
		string? textValue = this.GetValueN(name, null);
		if (!string.IsNullOrEmpty(textValue) && !Enum.TryParse(textValue, true, out result))
		{
			result = defaultValue;
		}

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

	public void SetValue(string name, string? value)
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

	private static void SetValue(XElement element, string name, string? value)
	{
		string? escapedValue = null;

		// Treat an empty value the same as a null value, so we only write attributes with non-empty values.
		// Note: JScript's escape(null) returns "undefined" (which sucks), but escape(string.Empty) returns string.Empty.
		if (value.IsNotEmpty())
		{
			// We're using attributes to store arbitrary string values, so we have to escape them first to preserve whitespace.
			// Otherwise, all XML DOMs will normalize the whitespace in ways we don't want (e.g., removing leading/trailing
			// whitespace and changing all remaining whitespace into spaces).
			decimal version = Project.GetVersion(element);
			if (version < SmartEncodingVersion)
			{
				// We'll use the old JScript escape function because it works on strings of arbitrary length, and it generates
				// compact encodings for the most common characters (<= 255).  The similar Uri.EscapeDataString method
				// is limited to only 32766 characters, and it generates wider encodings for characters 128 through 255.
				escapedValue = JavaScriptUtility.Escape(value);
			}
			else
			{
				escapedValue = Escape(value);
			}
		}

		// If escapedValue is null this will remove the attribute.
		element.SetAttributeValue(name, escapedValue);
	}

	private XElement AddSubkeyNode(string subKeyType, string xmlKeyName)
	{
		XElement result = new(subKeyType);
		this.element.Add(result);

		if (!string.IsNullOrEmpty(xmlKeyName))
		{
			SetValue(result, nameof(this.XmlKeyName), xmlKeyName);
		}

		return result;
	}

	private XElement GetSubkeyNode(string subkeyType, string xmlKeyName)
	{
		// This must treat null and empty XmlKeyNames as equal.  We don't write out empty
		// XmlKeyNames, so they'll be read back in as null, which we'll default to empty
		// (on both sides of the comparison).
		XElement? result = this.element.Elements(subkeyType)
			.FirstOrDefault(e => e.GetAttributeValue(nameof(this.XmlKeyName), string.Empty) == (xmlKeyName ?? string.Empty));

		result ??= this.AddSubkeyNode(subkeyType, xmlKeyName);

		return result;
	}

	#endregion
}
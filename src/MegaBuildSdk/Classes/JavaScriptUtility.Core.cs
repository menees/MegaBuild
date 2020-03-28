namespace MegaBuild
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Text;

	#endregion

	// This needs to escape and unescape in a way 100% compatible with .NET Framework's Microsoft.JScript.GlobalObject.
	// The Uri.EscapeDataString and Uri.UnescapeDataString methods are not 100% compatible as discussed in XmlKey.SetValue
	// and in the following StackOverflow question:
	// https://stackoverflow.com/questions/28038717/whats-different-microsoft-jscript-globalobject-escape-and-uri-escapeuristring
	internal static class JavaScriptUtility
	{
		#region Public Methods

		public static string Escape(object value)
		{
			// Note: This code was decompiled from .NET Framework's Microsoft.JScript v10.0.0.0 GlobalObject.escape().
			string text = Convert.ToString(value);
			const string HexDigits = "0123456789ABCDEF";
			int length = text.Length;
			StringBuilder stringBuilder = new StringBuilder(length * 2);
			int num = -1;
			while (++num < length)
			{
				char c = text[num];
#pragma warning disable MEN010 // Avoid magic numbers
				int num2 = c;
				if ((num2 < 65 || num2 > 90) && (num2 < 97 || num2 > 122) && (num2 < 48 || num2 > 57)
					&& c != '@' && c != '*' && c != '_' && c != '+' && c != '-' && c != '.' && c != '/')
				{
					stringBuilder.Append('%');
					if (num2 < 256)
					{
						stringBuilder.Append(HexDigits[num2 / 16]);
						c = HexDigits[num2 % 16];
					}
					else
					{
						stringBuilder.Append('u');
						stringBuilder.Append(HexDigits[(num2 >> 12) % 16]);
						stringBuilder.Append(HexDigits[(num2 >> 8) % 16]);
						stringBuilder.Append(HexDigits[(num2 >> 4) % 16]);
						c = HexDigits[num2 % 16];
					}
				}
#pragma warning restore MEN010 // Avoid magic numbers

				stringBuilder.Append(c);
			}

			return stringBuilder.ToString();
		}

		public static string Unescape(object value)
		{
			// Note: This code was decompiled from .NET Framework's Microsoft.JScript v10.0.0.0 GlobalObject.unescape().
#pragma warning disable MEN007 // Use a single return
			static int HexDigit(char c)
#pragma warning restore MEN007 // Use a single return
			{
#pragma warning disable MEN010 // Avoid magic numbers
				if (c >= '0' && c <= '9')
				{
					return c - 48;
				}

				if (c >= 'A' && c <= 'F')
				{
					return 10 + c - 65;
				}

				if (c >= 'a' && c <= 'f')
				{
					return 10 + c - 97;
				}
#pragma warning restore MEN010 // Avoid magic numbers

				return -1;
			}

			string text = Convert.ToString(value);
			int length = text.Length;
			StringBuilder stringBuilder = new StringBuilder(length);
			int num = -1;
			while (++num < length)
			{
				char c = text[num];
				if (c == '%')
				{
					int num2;
					int num3;
					int num4;
					int num5;
#pragma warning disable MEN010 // Avoid magic numbers
					if (num + 5 < length && text[num + 1] == 'u' && (num2 = HexDigit(text[num + 2])) != -1 && (num3 = HexDigit(text[num + 3])) != -1
						&& (num4 = HexDigit(text[num + 4])) != -1 && (num5 = HexDigit(text[num + 5])) != -1)
					{
						c = (char)((num2 << 12) + (num3 << 8) + (num4 << 4) + num5);
						num += 5;
					}
					else if (num + 2 < length && (num2 = HexDigit(text[num + 1])) != -1 && (num3 = HexDigit(text[num + 2])) != -1)
					{
						c = (char)((num2 << 4) + num3);
						num += 2;
					}
				}
#pragma warning restore MEN010 // Avoid magic numbers

				stringBuilder.Append(c);
			}

			return stringBuilder.ToString();
		}

		#endregion
	}
}

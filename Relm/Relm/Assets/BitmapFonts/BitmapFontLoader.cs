using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace Relm.Assets.BitmapFonts
{
	public static class BitmapFontLoader
	{
		public static BitmapFont LoadFontFromFile(string filename, bool premultiplyAlpha = false)
		{
			using (var file = TitleContainer.OpenStream(filename))
			{
				using (var reader = new StreamReader(file))
				{
					var line = reader.ReadLine();
					if (line.StartsWith("info "))
						return LoadFontFromTextFile(filename, premultiplyAlpha);
					else if (line.StartsWith("<?xml") || line.StartsWith("<font"))
						return LoadFontFromXmlFile(filename, premultiplyAlpha);
					else
						throw new InvalidDataException("Unknown file format.");
				}
			}
		}

		public static BitmapFont LoadFontFromTextFile(string filename, bool premultiplyAlpha = false)
		{
			var font = new BitmapFont();
			using (var stream = TitleContainer.OpenStream(filename))
				font.LoadText(stream);

			QualifyResourcePaths(font, Path.GetDirectoryName(filename));
			font.Initialize(premultiplyAlpha);

			return font;
		}

		public static BitmapFont LoadFontFromXmlFile(string filename, bool premultiplyAlpha = false)
		{
			var font = new BitmapFont();
			using (var stream = TitleContainer.OpenStream(filename))
				font.LoadXml(stream);

			QualifyResourcePaths(font, Path.GetDirectoryName(filename));
			font.Initialize(premultiplyAlpha);

			return font;
		}

		internal static bool GetNamedBool(string[] parts, string name, bool defaultValue = false)
		{
			var s = GetNamedString(parts, name);
			if (int.TryParse(s, out var v))
				return v > 0;

			return defaultValue;
		}

		internal static int GetNamedInt(string[] parts, string name, int defaultValue = 0)
		{
			var s = GetNamedString(parts, name);
			if (!int.TryParse(s, out var result))
				return defaultValue;

			return result;
		}

		internal static string GetNamedString(string[] parts, string name)
		{
			var result = string.Empty;
			foreach (string part in parts)
			{
				var nameEndIndex = part.IndexOf('=');
				if (nameEndIndex != -1)
				{
					var namePart = part.Substring(0, nameEndIndex);
					var valuePart = part.Substring(nameEndIndex + 1);

					if (string.Equals(name, namePart, StringComparison.OrdinalIgnoreCase))
					{
						var length = valuePart.Length;
						if (length > 1 && valuePart[0] == '"' && valuePart[length - 1] == '"')
							valuePart = valuePart.Substring(1, length - 2);

						result = valuePart;
						break;
					}
				}
			}

			return result;
		}

		internal static Padding ParsePadding(string s)
		{
			var parts = s.Split(',');
			return new Padding()
			{
				Left = Convert.ToInt32(parts[3].Trim()),
				Top = Convert.ToInt32(parts[0].Trim()),
				Right = Convert.ToInt32(parts[1].Trim()),
				Bottom = Convert.ToInt32(parts[2].Trim())
			};
		}

		internal static Point ParseInt2(string s)
		{
			var parts = s.Split(',');
			return new Point
			{
				X = Convert.ToInt32(parts[0].Trim()),
				Y = Convert.ToInt32(parts[1].Trim())
			};
		}

		internal static void QualifyResourcePaths(BitmapFont font, string resourcePath)
		{
			var pages = font.Pages;
			for (var i = 0; i < pages.Length; i++)
			{
				var page = pages[i];
				page.Filename = Path.Combine(resourcePath, page.Filename);
				pages[i] = page;
			}

			font.Pages = pages;
		}

		internal static string[] Split(string s, char delimiter)
		{
			if (s.IndexOf('"') != -1)
			{
				var partStart = -1;
				var parts = new List<string>();

				do
				{
					var quoteStart = s.IndexOf('"', partStart + 1);
					var quoteEnd = s.IndexOf('"', quoteStart + 1);
					var partEnd = s.IndexOf(delimiter, partStart + 1);

					if (partEnd == -1)
						partEnd = s.Length;

					var hasQuotes = quoteStart != -1 && partEnd > quoteStart && partEnd < quoteEnd;
					if (hasQuotes)
						partEnd = s.IndexOf(delimiter, quoteEnd + 1);

					parts.Add(s.Substring(partStart + 1, partEnd - partStart - 1));
					if (hasQuotes)
						partStart = partEnd - 1;

					partStart = s.IndexOf(delimiter, partStart + 1);
				} while (partStart != -1);

				return parts.ToArray();
			}

			return s.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
		}

		internal static T[] ToArray<T>(ICollection<T> values)
		{
			var result = new T[values.Count];
			values.CopyTo(result, 0);

			return result;
		}
	}
}
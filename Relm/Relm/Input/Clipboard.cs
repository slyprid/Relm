using System;
using System.Runtime.InteropServices;

namespace Relm.Input
{
	public class Clipboard : IClipboard
	{
		private static IClipboard _instance;

		[DllImport("SDL2.dll")]
		internal static extern void SDL_free(IntPtr memblock);

        [DllImport("SDL2.dll")]
        private static extern int SDL_SetClipboardText(string text);

        [DllImport("SDL2.dll")]
        private static extern IntPtr SDL_GetClipboardText();

		public static unsafe string UTF8_ToManaged(IntPtr s, bool freePtr = false)
		{
			if (s == IntPtr.Zero)
			{
				return null;
			}

			byte* ptr = (byte*)s;
			while (*ptr != 0)
			{
				ptr++;
			}

			int len = (int)(ptr - (byte*)s);
			if (len == 0)
			{
				return string.Empty;
			}
			char* chars = stackalloc char[len];
			int strLen = System.Text.Encoding.UTF8.GetChars((byte*)s, len, chars, len);
			string result = new string(chars, 0, strLen);

			if (freePtr)
			{
				SDL_free(s);
			}
			return result;
		}
		
		public static string GetContents()
		{
			if (_instance == null)
				_instance = new Clipboard();
			return _instance.GetContents();
		}

		public static void SetContents(string text)
		{
			if (_instance == null)
				_instance = new Clipboard();
			_instance.SetContents(text);
		}

		#region IClipboard implementation

		string IClipboard.GetContents()
		{
			return UTF8_ToManaged(SDL_GetClipboardText(), true);
		}

		void IClipboard.SetContents(string text)
		{
			SDL_SetClipboardText(text);
		}

		#endregion IClipboard implementation
	}
}
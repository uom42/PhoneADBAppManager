using System.Runtime.InteropServices;

#region Structures to interoperate with the Windows API

using hwnd = System.IntPtr;

#endregion

#nullable enable

namespace common.WinAPI
{
	internal static class Windows
	{
		public enum WindowMessages : int
		{
			WM_PAINT = 0xF,
			EM_SETCUEBANNER = 0x1501
		}

		[DllImport(Core.WINDLL_USER, SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
		internal static extern IntPtr SendMessage(
			[In] hwnd hwnd,
			[In, MarshalAs(UnmanagedType.I4)] WindowMessages wMsg,
			[In] int wParam,
			[In, MarshalAs(UnmanagedType.LPTStr)] string? lParam);

		[DllImport(Core.WINDLL_USER)]
		private static extern int GetClientRect(
			[In] hwnd hwnd,
			[In, Out] ref Rectangle rc);

		public static Rectangle GetClientRect(this IWin32Window wind)
		{
			var rcClient = new Rectangle();
			GetClientRect(wind.Handle, ref rcClient);
			return rcClient;
		}
	}

}

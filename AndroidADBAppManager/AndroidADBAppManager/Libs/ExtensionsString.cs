using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable enable

namespace common
{
	internal static class ExtensionsString
	{
		public const string CR = "\n";
		public const string LF = "\r";
		public const string CRLF = CR + LF;
		public const string CRLF2 = CRLF + CRLF;

		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string NormalizeConsoleOutput(this string source)
		{
			if (!string.IsNullOrEmpty(source))
			{
				source = source.Replace(CRLF2, CRLF).TrimEnd(new char[] { '\n', '\r' });// '//'  .e_TrimEnd(vbCrLf).e_TrimEnd(vbCr).e_TrimEnd(vbLf)
				source = source.Trim();
			}
			return source;
		}

		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] ToArrayOf<T>(this T rObject) => new T[] { rObject };

		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNullOrWhiteSpace(this string? text) => string.IsNullOrWhiteSpace(text);

		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNullOrEmpty(this string? text) => string.IsNullOrEmpty(text);

		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TestFileExist(this string path)
		{
			//Just test file exist to throw error if not.
			var atr = System.IO.File.GetAttributes(path);
			return !(atr.HasFlag(FileAttributes.Directory));
		}

	}
}

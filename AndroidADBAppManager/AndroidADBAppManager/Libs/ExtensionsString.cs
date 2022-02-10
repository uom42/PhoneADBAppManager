using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable enable

namespace AndroidADBAppManager.Libs
{
    internal static class ExtensionsString
    {
        public const string CRLF = "\n\r";
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
        public static string[] ToArrayOfStrings(this string text) => new string[] { text };

        [DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrWhiteSpace(this string? text) => string.IsNullOrWhiteSpace(text);

        [DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this string? text) => string.IsNullOrEmpty(text);
    }
}

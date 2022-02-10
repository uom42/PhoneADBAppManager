using System.Diagnostics;
using System.Runtime.CompilerServices;

using NLog;

#nullable enable

namespace AndroidADBAppManager.Libs
{
	internal static class ExtensionsNLog
	{

		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Display(
			this Exception ex,
			MessageBoxIcon icon = MessageBoxIcon.Error,
			ILogger? logger = null)
		{
			logger ??= LogManager.GetCurrentClassLogger();
			logger.Error(ex);
			MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, icon);
		}

		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Display(
			this Exception ex,
			ILogger? logger = null)
			=> ex.Display(MessageBoxIcon.Error, logger);

	}
}

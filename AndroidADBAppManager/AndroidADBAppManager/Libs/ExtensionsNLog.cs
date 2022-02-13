using System.Diagnostics;
using System.Runtime.CompilerServices;

using NLog;

#nullable enable

namespace common
{
	internal static class ExtensionsNLog
	{

		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Handle(
			this Exception ex,
			bool showError = true,
			ILogger? logger = null,
			MessageBoxIcon icon = MessageBoxIcon.Error)
		{
			logger ??= LogManager.GetCurrentClassLogger();
			logger.Error(ex);

			if (showError)
				MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, icon);
		}
	}

}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AndroidADBAppManager.ADB
{

	[DebuggerStepThrough]
	internal static class ExtansionsADB
	{


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ThrowIfFailed(this string sMessage)
		{
			if (sMessage.ToLower().Contains("error:")) throw new Exception(sMessage);
			if (sMessage.ToLower().Contains("failed")) throw new Exception(sMessage);
			if (sMessage.ToLower().Contains("failure ")) throw new Exception(sMessage);

			//'ADB.EXE -Good.Answer 'Failure - not installed for 0
			//'ADB.EXE -Good.Answer 'Success
		}

	}
}

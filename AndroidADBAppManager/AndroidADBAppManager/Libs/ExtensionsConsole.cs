using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

using NLog;

#nullable enable
namespace common
{
	[DebuggerStepThrough]
	internal static class ExtensionsConsole
	{
		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ConsoleOutput RunConsoleApp(
			this FileInfo fiExe,
			string? arguments = null,
			string? workingDir = null,
			int waitTimeoutSec = 5,
			Encoding? outputEncoding = null,
			bool killIfHang = true,
			bool throwOnErrorStreamData = true)
		{
			ILogger logger = LogManager.GetCurrentClassLogger();
			logger.Debug($"*** RunConsole '{fiExe.FullName}', Args: '{arguments}'");
			workingDir ??= fiExe.DirectoryName;

			ProcessStartInfo psi = new()
			{
				FileName = fiExe.FullName,
				WorkingDirectory = workingDir,
				Arguments = arguments,
				UseShellExecute = false,

				RedirectStandardOutput = true,
				RedirectStandardError = true,

				LoadUserProfile = true,
				CreateNoWindow = true
			};
			if (null != outputEncoding)
			{
				psi.StandardOutputEncoding = outputEncoding;
				psi.StandardErrorEncoding = outputEncoding;
			}

			int waitMiliseconds = (waitTimeoutSec * 1_000);

			using (Process? prcExe = Process.Start(psi))
			{
				if (null == prcExe) throw new ArgumentException($"Failed to start '{fiExe.FullName}'!");

				StringBuilder sbOutput = new();
				StringBuilder sbError = new();
				prcExe!.OutputDataReceived += (s, e) => { lock (sbOutput) sbOutput.AppendLine(e.Data); };
				prcExe.ErrorDataReceived += (s, e) => { lock (sbError) sbError.AppendLine(e.Data); };
				prcExe.BeginOutputReadLine();
				prcExe.BeginErrorReadLine();
				var waitResult = prcExe.WaitForExit(waitMiliseconds);
				prcExe.CancelOutputRead();
				prcExe.CancelErrorRead();

				if (!waitResult)
				{
					var sErr = $"Process '{fiExe.FullName}' has not finished at '{waitTimeoutSec}' sec!";
					logger.Debug(sErr);
					if (killIfHang)
					{
						logger.Debug($"Killing '{fiExe.FullName}'...");
						prcExe.Close();
						logger.Debug($"'{fiExe.FullName}' - Closed!");
					}
					throw new Exception(sErr);
				}

				string sError = sbError.ToString().NormalizeConsoleOutput();
				if (throwOnErrorStreamData && !string.IsNullOrWhiteSpace(sError)) throw new Exception(sError);
				string sOutput = sbOutput.ToString().NormalizeConsoleOutput();
				ConsoleOutput result = new(sOutput, sError);
				logger.Debug($"*** Console:\n{result}");
				return result;
			}
		}
	}

	internal class ConsoleOutput
	{
		public readonly string Output;
		public readonly string Errors;


		internal ConsoleOutput(string? o, string? e)
		{
			Output = o ?? string.Empty;
			Errors = e ?? string.Empty;
		}


		public override string ToString() => $"Output Stream: '{Output}',\nErrors Stream: '{Errors}'";


		public void ThrowOutputOrDefault(string defaultError = "Unknown error!")
		{
			string sErr = defaultError;
			if (!string.IsNullOrWhiteSpace(Errors))
				sErr = Errors;
			else
				if (!string.IsNullOrWhiteSpace(Output)) sErr = Output;

			throw new Exception(sErr);
		}
	}
}

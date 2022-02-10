
using System.Diagnostics.CodeAnalysis;

using NLog;

using AndroidADBAppManager.Libs;

#nullable enable

namespace AndroidADBAppManager.ADBCore
{
	internal class ADBServer
	{
		private string adbPath = string.Empty;

		private readonly Lazy<ILogger> llogger = new(LogManager.GetCurrentClassLogger());
		private ILogger logger => llogger.Value;

		public readonly ADBDevice CurrentDevice;

		public ADBServer([NotNull] string pathADBexe)
		{
			pathADBexe = pathADBexe.Trim();
			if (string.IsNullOrEmpty(pathADBexe)) throw new ArgumentNullException(nameof(pathADBexe));

			adbPath = pathADBexe.Trim();
			logger.Debug($"TestADBConnection '{adbPath}'");
			TestFileExist(adbPath);
			logger.Debug($"TestFileExist {adbPath} = OK");
			{
				Properties.Settings.Default.ADBPath = adbPath;
				Properties.Settings.Default.Save();
				logger.Debug($"Saves settings ADBPath: '{adbPath}'");
			}

			//if(.chkRestartADBBeforeConnect.Checked) Then
			const string ADB_RESULT_STARTED_OK = "daemon started successfully";

			var closeResult = ExecADB("kill-server");
			var startResult = ExecADB("start-server");

			bool bConnected = startResult.Errors.ToLower().Contains(ADB_RESULT_STARTED_OK.ToLower());
			if (!bConnected) startResult.ThrowOutputOrDefault("Failed to connect to ADB!");

			//var aDevices = AsyncHelpers.RunSync(() => GetAttachedDevices());
			var aDevices = GetAttachedDevices().RunSync();
			if (!aDevices!.Any()) throw new Exception("Not found any attached devices!");
			logger.Debug($"Attached devices found ({aDevices!.Length}):\n{ADBDevice.AllConnectedDevicesToString(aDevices)}");

			if (aDevices.Length > 1)
				throw new Exception("Only one device must be attached!");

			//Only one device is connected.
			CurrentDevice = aDevices.First();
		}






		public static string GetSettings_ADBPath()
		{
			string path = Properties.Settings.Default.ADBPath;
			LogManager.GetCurrentClassLogger().Debug($"Load settings ADBPath: {path}");
			return Properties.Settings.Default.ADBPath;
		}

		public static bool TestFileExist(string path)
		{
			//Just test file exist to throw error if not.
			var atr = System.IO.File.GetAttributes(path);
			return !(atr.HasFlag(FileAttributes.Directory));
		}


		/*
		
	'Public Async Function GetDevicesSerialNo() As Task(Of String())
	'    'Dim S2 = Await GetDeviceList333()
	'    '#adb Get-serialno
	'    Dim sDevicesIDs = Await ExecADB("get-serialno")

	'    Const ERROR_DEVICES = "unknown"
	'    If (String.IsNullOrWhiteSpace(sDevicesIDs) OrElse (ERROR_DEVICES = sDevicesIDs.ToLower.Trim)) Then Return {}
	'    Return {sDevicesIDs}
	'End Function
		 */

		public async Task<ADBDevice[]> GetAttachedDevices()
		{
			#region Samples
			/*
				'Sample 1:
				'D:\_111\ADB>adb devices -l
				'* daemon Not running. starting it now on port 5037 *
				'* daemon started successfully *
				'List of devices attached


				'Sample 2:
				'ADB.EXE -Good.Answer 'List of devices attached
				'G8M9XA1762212914       device product: LUA-U22 model: HUAWEI_LUA_U22 device: HWLUA-U6582
				'42007db1c61c8300       device product:j3xltejt model: SM_J320F device: j3xlte '
			 */
			#endregion

			const string DEVICES_PREFIX = "List of devices attached";

			string sDevicesList = (await ExecADBAsync("devices -l")).Output;

			int prefixPos = sDevicesList.ToLower().IndexOf(DEVICES_PREFIX.ToLower());
			if (prefixPos < 0) throw new Exception(sDevicesList);

			sDevicesList = sDevicesList.Substring(prefixPos + DEVICES_PREFIX.Length);
			if (sDevicesList.IsNullOrWhiteSpace()) return Array.Empty<ADBDevice>();

			var asDevicesList = sDevicesList.Split(ExtensionsString.CRLF.ToArrayOfStrings(), StringSplitOptions.RemoveEmptyEntries);
			var lDevices = asDevicesList.Select(sDeviceLine => new ADBDevice(sDeviceLine)).ToArray();
			return lDevices;
		}


		#region ExecADB
		private const int DEFAULT_ADB_TIMEOUT_SEC = 10;

		private async Task<ConsoleOutput> ExecADBAsync(
			string adbCommand,
			int timeout_sec = DEFAULT_ADB_TIMEOUT_SEC)
				=> await ExecADBAsync_Core(adbPath, adbCommand, timeout_sec);

		private static async Task<ConsoleOutput> ExecADBAsync_Core(
			string adbPath,
			string adbCommand,
			int timeout_sec = DEFAULT_ADB_TIMEOUT_SEC)
		{
			using (Task<ConsoleOutput> tskExecADB = new(
				() => ExecADB(adbPath, adbCommand, timeout_sec),
				TaskCreationOptions.LongRunning))
			{
				tskExecADB.Start();
				return await tskExecADB;
			}
		}

		private static ConsoleOutput ExecADB(
			string adbPath,
			string adbCommand,
			int timeout_sec = DEFAULT_ADB_TIMEOUT_SEC)
				=> ConsoleTools.RunConsole(adbPath, adbCommand, null, timeout_sec, null, true, false);

		private ConsoleOutput ExecADB(
			string adbCommand,
			int timeout_sec = DEFAULT_ADB_TIMEOUT_SEC)
				=> ExecADB(adbPath, adbCommand, timeout_sec);

		#endregion


	}
}


using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

using common;

using NLog;

#nullable enable

namespace AndroidADBAppManager.ADB
{
	internal class ADBServer
	{

		//Поиск в многострочном тексте
		internal const RegexOptions C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Singleline =
			RegexOptions.IgnoreCase
			| RegexOptions.IgnorePatternWhitespace
			| RegexOptions.Singleline
			| RegexOptions.CultureInvariant;

		//Поиск в многострочном тексте, в каждой строке по-отдельности
		internal const RegexOptions C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Multiline =
			RegexOptions.IgnoreCase
			| RegexOptions.IgnorePatternWhitespace
			| RegexOptions.Multiline
			| RegexOptions.CultureInvariant;


		//Поиск в многострочном тексте, и в каждой строке по-отдельности
		internal const RegexOptions C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Multiline_Singleline =
			RegexOptions.IgnoreCase
			| RegexOptions.IgnorePatternWhitespace
			| RegexOptions.Multiline
			| RegexOptions.Singleline
			| RegexOptions.CultureInvariant;


		private string adbPath = string.Empty;

		private readonly Lazy<ILogger> llogger = new(LogManager.GetCurrentClassLogger());
		private ILogger logger => llogger.Value;

		public readonly Device CurrentDevice;

		public ADBServer([NotNull] string pathADBexe)
		{
			pathADBexe = pathADBexe.Trim();
			if (string.IsNullOrEmpty(pathADBexe)) throw new ArgumentNullException(nameof(pathADBexe));

			adbPath = pathADBexe.Trim();
			logger.Debug($"TestADBConnection '{adbPath}'");
			adbPath.TestFileExist();
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
			var aDevices = GetAttachedDevices().ExecSync();
			if (!aDevices!.Any()) throw new Exception("Not found any attached devices!");
			logger.Debug($"Attached devices found ({aDevices!.Length}):\n{Device.AllConnectedDevicesToString(aDevices)}");

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




		#region ExecADB
		private const int DEFAULT_ADB_TIMEOUT_SEC = 10;

		internal async Task<ConsoleOutput> ExecADBAsync(
			string adbCommand,
			int timeout_sec = DEFAULT_ADB_TIMEOUT_SEC)
				=> await ExecADBAsync_Core(adbPath, adbCommand, timeout_sec);

		internal static async Task<ConsoleOutput> ExecADBAsync_Core(
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

		internal static ConsoleOutput ExecADB(
			string adbPath,
			string adbCommand,
			int timeout_sec = DEFAULT_ADB_TIMEOUT_SEC)
				=> new FileInfo(adbPath).RunConsoleApp(adbCommand, null, timeout_sec, null, true, false);

		internal ConsoleOutput ExecADB(
			string adbCommand,
			int timeout_sec = DEFAULT_ADB_TIMEOUT_SEC)
				=> ExecADB(adbPath, adbCommand, timeout_sec);

		#endregion


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

		public async Task<Device[]> GetAttachedDevices()
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
			if (sDevicesList.IsNullOrWhiteSpace()) return Array.Empty<Device>();

			var asDevicesList = sDevicesList.Split(ExtensionsString.CRLF.ToArrayOf(), StringSplitOptions.RemoveEmptyEntries);
			var lDevices = asDevicesList.Select(sDeviceLine => new Device(sDeviceLine)).ToArray();
			return lDevices;
		}

		#region GetProps

		public async Task<Property[]> GetProps()
		{
			var sProps = (await ExecADBAsync("shell getprop")).Output;

			var reFlags = C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Multiline;
			string rePattern = @"^\[(?<PropKey>.+)\]\:\s\[(?<PropValue>.+)(?:\r\n)*\]";
			Regex rRegExp = new(rePattern, reFlags);

			var props = rRegExp.Matches(sProps)?.
				Select(m =>
				{
					string sKey = m.Groups["PropKey"].Value;
					string sValue = m.Groups["PropValue"].Value;
					return new Property(sKey, sValue);
				})?
				.OrderBy(p => p.Name)?
				.ToArray();

			return props ?? Array.Empty<Property>();
		}

		#endregion


		#region GetPackages

		public async Task<Package[]> GetPackages()
		{
			var aAllPackagesNoUnInstalled = await GetPackages_Core(GET_PACKAGES_FLAGS.All);
			var aAllPackagesWithUnInstalled = await GetPackages_Core(GET_PACKAGES_FLAGS.All | GET_PACKAGES_FLAGS.IncludeUninstalled);
			var aSystemPackages = await GetPackages_Core(GET_PACKAGES_FLAGS.OnlySystem | GET_PACKAGES_FLAGS.IncludeUninstalled);
			var a3rdPackages = await GetPackages_Core(GET_PACKAGES_FLAGS.OnlyThirdParty | GET_PACKAGES_FLAGS.IncludeUninstalled);
			var aDisabledPackages = await GetPackages_Core(GET_PACKAGES_FLAGS.OnlyDisabled | GET_PACKAGES_FLAGS.IncludeUninstalled);

			foreach (var pDisabled in aDisabledPackages)
			{
				var aPackages =
					from pAll in aAllPackagesWithUnInstalled
					where pAll == pDisabled
					select pAll;

				if (aPackages.Any()) aPackages.First().SetStateFlag(Package.E_PACKAGE_STATES.Disabled_Frozen);
			}

			var asAllPackagesNoUnInstalledNames =
				(from P in aAllPackagesNoUnInstalled
				 select P.Name.ToLower()).ToArray();

			var aUnInstalledPackages =
				(from P in aAllPackagesWithUnInstalled
				 where !asAllPackagesNoUnInstalledNames.Contains(P.Name.ToLower())
				 select P).ToArray();

			foreach (var pUnIstalled in aUnInstalledPackages)
			{
				var aPackages =
					from pAll in aAllPackagesWithUnInstalled
					where pAll == pUnIstalled
					select pAll;

				if (aPackages.Any())
					aPackages.First().SetStateFlag(Package.E_PACKAGE_STATES.Uninstalled);
			}

			foreach (var pSystem in aSystemPackages)
			{
				var aPackages =
					from pAll in aAllPackagesWithUnInstalled
					where pAll == pSystem
					select pAll;

				if (aPackages.Any())
					aPackages.First().SetStateFlag(Package.E_PACKAGE_STATES.System);
			}

			foreach (var p3rd in a3rdPackages)
			{
				var aPackages =
					from pAll in aAllPackagesWithUnInstalled
					where pAll == p3rd
					select pAll;

				if (aPackages.Any())
					aPackages.First().SetStateFlag(Package.E_PACKAGE_STATES.ThirdParty);
			}

			aAllPackagesWithUnInstalled = (from pAll in aAllPackagesWithUnInstalled
										   orderby pAll.Name, pAll.Path
										   select pAll).ToArray();

			return aAllPackagesWithUnInstalled;
		}

		[Flags]
		public enum GET_PACKAGES_FLAGS : int
		{
			/// <summary>All except Uninstalled</summary>
			All = 0,
			/// <summary>'-d: Filter to only show disabled packages.</summary>
			OnlyDisabled = 2,
			/// <summary>'-e: Filter to only show enabled packages.</summary>
			OnlyEnabled = 4,
			/// <summary>'-s: Filter to only show system packages.</summary>
			OnlySystem = 8,
			/// <summary>'-3: Filter to only show third party packages.</summary>
			OnlyThirdParty = 16,
			/// <summary>-u: Also include uninstalled packages.</summary>
			IncludeUninstalled = 32
		}

		private async Task<Package[]> GetPackages_Core(GET_PACKAGES_FLAGS Flags = GET_PACKAGES_FLAGS.All)
		{
			// http://adbcommand.com/adbshell/pm

			// -f: See their associated file.
			string sAdbCmd = "shell pm list packages -f";

			// --user user_id: The user space To query.
			// -i: See the installer For the packages.

			if (Flags != GET_PACKAGES_FLAGS.All)
			{
				if (Flags.HasFlag(GET_PACKAGES_FLAGS.OnlyDisabled)) sAdbCmd += " -d";
				if (Flags.HasFlag(GET_PACKAGES_FLAGS.OnlyEnabled)) sAdbCmd += " -e";
				if (Flags.HasFlag(GET_PACKAGES_FLAGS.OnlySystem)) sAdbCmd += " -s";
				if (Flags.HasFlag(GET_PACKAGES_FLAGS.OnlyThirdParty)) sAdbCmd += " -3";
				if (Flags.HasFlag(GET_PACKAGES_FLAGS.IncludeUninstalled)) sAdbCmd += " -u";
			}

			string sPackagesListString = (await ExecADBAsync(sAdbCmd, 20)).Output;
			if (string.IsNullOrWhiteSpace(sPackagesListString)) throw new Exception("Not Found Any Packages!");

			// error: device not found
			sPackagesListString.ThrowIfFailed();

			var packageStrings = sPackagesListString.Split(ExtensionsString.LF.ToArrayOf(), StringSplitOptions.RemoveEmptyEntries).ToList();
			if (!packageStrings.Any()) throw new Exception("Not Found Any Packages!");

			var aPackages = packageStrings.Select(s => new Package(this, s)).ToArray();
			return aPackages;
		}
		#endregion


	}
}

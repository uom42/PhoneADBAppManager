using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace AndroidADBAppManager.ADB
{
	[Serializable()]
	public class Package
	{

		private const string C_USER0_RegExp = @"User\s0\:
(\s+) 
installed\=(?<installed>true|false)
(\s+) 
hidden\=(?<hidden>true|false)
(\s+) 
stopped\=(?<stopped>true|false)
(\s+) 
notLaunched\=(?<notlaunched>true|false)
(\s+) 
enabled\=(?<enabled>\d)";

		private static RegexOptions _REO = ADBServer.C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Singleline;
		private static Regex _RegExp = new(C_USER0_RegExp, _REO);

		[Flags]
		public enum E_PACKAGE_STATES : int
		{
			Unknown = 0,
			System = 1,
			User = System * 2,
			ThirdParty = User * 2,
			Disabled_Frozen = ThirdParty * 2,
			Uninstalled = Disabled_Frozen * 2,
			Critical = Uninstalled * 2,
			Hidden = Critical * 2
			// Dumpsys_Installed = (Dumpsys_Hidden* 2)
			// Dumpsys_Stopped = (Dumpsys_Hidden * 2)
			// Dumpsys_NotLaunched = (Dumpsys_Stopped * 2)
			// Dumpsys_Enabled = (Dumpsys_NotLaunched * 2)
		}

		[XmlAttribute("Name")]
		public string Name { get; set; } = string.Empty;

		[XmlAttribute("Path")]
		public string Path { get; set; } = string.Empty;

		[XmlElement("State")]
		public E_PACKAGE_STATES State { get; set; } = E_PACKAGE_STATES.Unknown;

		//internal KNOWN_PACKAGE KnownPackageData { get; private set; } = null; // Description As String = vbNullString

		internal string? Dumpsys_RAW { get; private set; } = null;
		internal ADBDumpsysRecord? Dumpsys_Root { get; private set; } = null;

		private ADBServer? srv = null;

		/// <summary>Only FOR Serialization!</summary>
		[Obsolete("Only for Serialization!", true)]
		public Package() : base() { }


		internal Package(ADBServer parent, string sPackageString) : base()
		{
			const string GOOD_PACKAGE_PREFIX = "package:";
			const string C_OLD_SEPARATOR = "=";

			srv = parent;
			sPackageString = sPackageString.Trim();
			// package:/system/priv-app/AutomationTest_FB/AutomationTest_FB.apk=com.sec.android.app.DataCreate'

			if (!sPackageString.ToLower().StartsWith(GOOD_PACKAGE_PREFIX))
				throw new Exception(sPackageString + " NOT FOUND PREFIX " + GOOD_PACKAGE_PREFIX);

			sPackageString = sPackageString.Substring(GOOD_PACKAGE_PREFIX.Length);

			#region Примеры путей
			// Примеры старых путей"
			// package:/system/app/WallpaperPicker/WallpaperPicker.apk=com.android.wallpaperpicker
			// package:/system/priv-app/ContactsProvider/ContactsProvider.apk=com.android.providers.contacts
			// package:/system/app/CaptivePortalLogin/CaptivePortalLogin.apk=com.android.captiveportallogin 

			// Примеры новых путей
			// package:/data/app/com.logame.eliminateintruder3d-fgSessTvROV8p5dhbLmPSg==/base.apk=com.logame.eliminateintruder3d
			// package:/data/app/wps_lite/wps_lite.apk=cn.wps.xiaomi.abroad.lite                                                                                               package:/data/app/com.alibaba.aliexpresshd-Nm-jrNq_V9nXMXIHPNH3Kw==/base.apk=com.alibaba.aliexpresshd                                                           package:/data/app/com.block.puzzle.game.hippo.mi-NbyA9e4Lj6jJzFIvymKYXg==/base.apk=com.block.puzzle.game.hippo.mi                                               package:/data/app/com.teslacoilsw.launcher-MoZG4lGLIErWKe6O1HoBlw==/base.apk=com.teslacoilsw.launcher                                                           package:/data/app/com.yandex.zenkitpartnerconfig-ySE03xBo5tW-JBUVr7VT2w==/base.apk=com.yandex.zenkitpartnerconfig                                               package:/data/app/com.google.android.deskclock-XlbCoXgwJ-_g2fuMu017zg==/base.apk=com.google.android.deskclock                                                   package:/data/app/XMRemoteController/XMRemoteController.apk=com.duokan.phone.remotecontroller                                                                   package:/data/app/com.google.android.apps.docs-gO4QAKBg-e30Sc7ITD-w9w==/base.apk=com.google.android.apps.docs                                                   package:/data/app/com.pop.bubble.shooter.blast.mi-GXqzztWJWxV2s4PSOUMJrw==/base.apk=com.pop.bubble.shooter.blast.mi                                             package:/data/app/com.opera.browser-A_6_Rr8taca-CGLsueCWbQ==/base.apk=com.opera.browser                                                                         package:/data/app/com.mi.global.bbs-TTguGrPa7DBGwFkYrQWdBQ==/base.apk=com.mi.global.bbs                                                                         package:/data/app/com.zhiliaoapp.musically-tGjjEKVhybKZGvikmQziBw==/base.apk=com.zhiliaoapp.musically                                                           package:/data/app/Photos/Photos.apk=com.google.android.apps.photos                                                                                              package:/data/app/ru.yandex.money-6u1wyac592FlNJIhZZlUOw==/base.apk=ru.yandex.money                                                                             package:/data/app/ru.yandex.searchplugin-3TNnAajoGvm231689eecWw==/base.apk=ru.yandex.searchplugin                                                               package:/data/app/com.facebook.katana-r19n2jaZyPe7W5ZS4NhwQg==/base.apk=com.facebook.katana                                                                     package:/data/app/ru.yandex.money.service-Xx3LeMmHLLtTR00eAZuArA==/base.apk=ru.yandex.money.service                                                             package:/data/app/PeelMiRemote/PeelMiRemote.apk=com.duokan.phone.remotecontroller.peel.plugin                                                                   package:/data/app/com.mi.global.shop-fOx19KW4r-3_fvJzZ7uRQQ==/base.apk=com.mi.global.shop                                                                       package:/data/app/MiCreditInStub/MiCreditInStub.apk=com.micredit.in                                                                                             package:/data/app/com.bubble.free.bubblestory-FoW2oF3BzO-JFOOCSz9Vmw==/base.apk=com.bubble.free.bubblestory                                                     package:/data/app/com.opera.preinstall-bDGZIAL8xFrH9fg0IMY1fw==/base.apk=com.opera.preinstall                                                                   package:/data/app/com.yandex.zen-72wtte9NxUvC5pYZzjuYTQ==/base.apk=com.yandex.zen                                                                               package:/data/app/com.facemoji.lite.xiaomi-YOlDmbEjZ4Mo6wXlcfJ9Eg==/base.apk=com.facemoji.lite.xiaomi                                                           package:/data/app/cn.wps.moffice_eng-PJk2tdlhDeSrh7umLO95Xg==/base.apk=cn.wps.moffice_eng        
			#endregion

			int iSeparatorOld = sPackageString.LastIndexOf(C_OLD_SEPARATOR);
			if (iSeparatorOld < 1)
				throw new Exception($"{sPackageString} NOT FOUND Path/PackageName SEPARATOR ('{C_OLD_SEPARATOR}')!");

			Path = sPackageString.Substring(0, iSeparatorOld).Trim();
			Name = sPackageString.Substring(iSeparatorOld + 1).Trim();
			GetInfoFromKnownPackagesDatabase();
		}

		internal void SetStateFlag(E_PACKAGE_STATES F, bool bSet = true)
		{
			if (bSet) State |= F;
			else State &= ~F;
		}

		private bool IsCriticalPackage
		{
			get
			{
				//	return KnownPackageData is object && KnownPackageData.IsCritical;
				return false;
			}
		}

		/// <summary>Ищем в базе известных / критических приложений</summary>
		public void GetInfoFromKnownPackagesDatabase()
		{
			/*
			KnownPackageData = ADBManager.mKnownPackages.g_KnownPachagesDatabase.GetPackageInfo(this);
			bool bIsCritical = IsCriticalPackage;
			SetStateFlag(E_PACKAGE_STATES.Critical, bIsCritical);
			 */
		}

		public override string ToString() => $"Name: '{Name}', Path: '{Path}'";


		/*

		#region Enable / Disable (Freeze / Unfreeze)

		/// <summary>Freeze / Unfreeze</summary>
		internal async Task Enable(ADBServer Server, bool bEnable)
		{
			// pm enable com.jamworks.bxactions - чтобы включить обратно если вы передумали
			// pm disable com.jamworks.bxactions - чтобы отключить

			// adb Shell pm disable-user --user 0 имя_приложения
			// adb Shell pm disable-user -user 0 <package_to_disable>
			// adb Shell pm enable --user 0 имя_приложения
			// pm uninstall - k - -user 0 com.miui.miservice



			// adb shell pm disable-user --user 0 com.miui.cleanmaster
			// adb shell pm enable <package_to_enable>

			string sCMD = "shell pm ";
			if (bEnable)
			{
				sCMD += "enable --user 0 ";
			}
			else
			{
				sCMD += "disable-user --user 0 ";
			}

			sCMD += " " + Name;
			string sActionResult = (await Server.ExecADB(sCMD, 30)).Output;
			sActionResult = sActionResult.e_CheckNullOrWhiteSpace();
			string sActionResultL = sActionResult.ToLower().Trim();
			const string C_ERROR_NEED_ROOT = "Error: java.lang.SecurityException: Permission Denial:";
			if (sActionResultL.StartsWith(C_ERROR_NEED_ROOT.ToLower()))
			{
				var EX = new System.Security.SecurityException(sActionResult);
				throw EX;
			}

			string sGoodResult = String.Empty;
			if (bEnable)
			{
				// Package com.google.android.youtube New state: enabled
				const string C_RESULT_OK = "state: enabled";
				sGoodResult = C_RESULT_OK;
			}
			else // DISABLE
			{
				// Package com.miui.home New state: Default
				// Package com.google.android.youtube New state: disabled-user
				const string C_RESULT_OK = "state: disabled-user";
				sGoodResult = C_RESULT_OK;
			}

			sGoodResult = sGoodResult.ToLower().Trim();
			if (!sActionResultL.EndsWith(sGoodResult))
				throw new Exception(sActionResult);
			SetStateFlag(E_PACKAGE_STATES.Disabled_Frozen, !bEnable);
			// Dim bResultOk = (sActionResult.ToLower = "Success".ToLower)
			// If (Not bResultOk) Then Throw New Exception(sActionResult)
			// Call Me.SetStateFlag(PACKAGE_STATES.Uninstalled)
		}

		#endregion

		#region Install / Uninstall
		internal async Task Install(ADBServer Server, bool bInstall)
		{
			if (bInstall)
			{
				// adb shell pm install APK_FILE
				// shell pm install -r --user 0 /data/app/com.google.android.play.games-1/*.apk
				string sCMD = "shell pm install -r --user 0 " + Path;
				string sActionResult = (await Server.ExecADB(sCMD, 30)).Output;

				// Dim bResultOk = (sActionResult.ToLower = "Success".ToLower)
				// If (Not bResultOk) Then Throw New Exception(sActionResult)
				sActionResult.ThrowIfFailed();
				SetStateFlag(E_PACKAGE_STATES.Uninstalled);
			}
			else
			{
				// adb uninstall --user 0 com.samsung.android.email.widget
				string sCMD = "uninstall --user 0 " + Name;
				string sActionResult = (await Server.ExecADB(sCMD, 30)).Output;

				// ADB.EXE -Good.Answer 'Failure - not installed for 0
				// ADB.EXE -Good.Answer 'Success

				bool bResultOk = (sActionResult.ToLower() ?? "") == ("Success".ToLower() ?? "");
				if (!bResultOk)
					throw new Exception(sActionResult);
				sActionResult.ThrowIfFailed();
				SetStateFlag(E_PACKAGE_STATES.Uninstalled);
			}
		}
		#endregion

		internal async Task<System.IO.FileInfo> Download(ADBManager.ADBServer Server, string sLocalPCFolderToSave)
		{
			// adb pull -a /system/priv-app/SOAgent/SOAgent.apk t:\1.apk
			// 1928 KB/s (39502 bytes in 0.020s)

			var diFile = new System.IO.FileInfo(Path);
			string sPCAPKFile = Name + "." + diFile.Name;
			string sFullLocalPath = System.IO.Path.Combine(sLocalPCFolderToSave, sPCAPKFile);
			string sCMD = "pull -a " + Path + " \"" + sFullLocalPath + "\"";
			string sDownloadResult = (await Server.ExecADB(sCMD, 60)).Output;
			// 1928 KB/s (39502 bytes in 0.020s)
			if (System.IO.File.Exists(sFullLocalPath))
			{
				diFile = new System.IO.FileInfo(sFullLocalPath);
				return diFile;
			}

			throw new Exception(sDownloadResult);
		}
		 */


		public static bool operator ==(Package P1, Package P2)
			=> (P1.ToString().ToLower() ?? "") == (P2.ToString().ToLower() ?? "");

		public static bool operator !=(Package P1, Package P2)
			=> (P1.ToString().ToLower() ?? "") != (P2.ToString().ToLower() ?? "");

		internal async Task GetDumpsysInfo()
		{
			ArgumentNullException.ThrowIfNull(srv);

			string sCMD = $"shell dumpsys package {Name}";
			string dumpSys = (await srv!.ExecADBAsync(sCMD, 30)).Output;
			if (string.IsNullOrEmpty(dumpSys)) return;

			Dumpsys_RAW = dumpSys;
			// Dim bResultOk = (sActionResult.ToLower = "Success".ToLower)
			// If (Not bResultOk) Then Throw New Exception(sActionResult)
			// Call sActionResult.ThrowIfFailed

			Dumpsys_Root = ADBDumpsysRecord.ParseRAW(dumpSys);
			var rPackages = Dumpsys_Root.Find("Packages:");
			if (rPackages != null)
			{
				if (rPackages.Childrens.Count == 1)
				{
					var rPack = rPackages.Childrens.First();
					var rUserInfo = rPack.FindStartsWith("User 0:");
					if (null != rUserInfo)
					{
						string sDumpsys_PackageUser0 = rUserInfo.Title;
						var rMatch = _RegExp.Match(sDumpsys_PackageUser0);
						if (rMatch.Success)
						{
							// Dim aGroups = _RegExp.GetGroupNames
							var rGroups = rMatch.Groups;
							string sInstalled = rGroups["installed"].Value;
							string sHidden = rGroups["hidden"].Value;
							string sStopped = rGroups["stopped"].Value;
							string sNotLaunched = rGroups["notlaunched"].Value;
							string sEnabled = rGroups["enabled"].Value;
							bool binstalled = bool.Parse(sInstalled);
							bool bHidden = bool.Parse(sHidden);
							bool bStopped = bool.Parse(sStopped);
							bool bNotLaunched = bool.Parse(sNotLaunched);
							int iEnabled = int.Parse(sEnabled);
							bool bEnabled = (iEnabled == 0) ? false : true;
							var F = E_PACKAGE_STATES.Unknown;
							// If binstalled Then F = F Or E_PACKAGE_STATES.Dumpsys_Installed
							if (bHidden) F |= E_PACKAGE_STATES.Hidden;
							// If bStopped Then F = F Or E_PACKAGE_STATES.Dumpsys_Stopped
							// If bNotLaunched Then F = F Or E_PACKAGE_STATES.Dumpsys_NotLaunched
							// If bEnabled Then F = F Or E_PACKAGE_STATES.Dumpsys_Enabled

							if (F != E_PACKAGE_STATES.Unknown) SetStateFlag(F);
						}
					}
				}
				else
				{
					// LogDebug("******************** rPackages.Childrens.Count > 1")
				}
			}
		}

	}
}

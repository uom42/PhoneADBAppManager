using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualBasic;

namespace AndroidADBAppManager.ADB
{
	internal class ADBDumpsysRecord
	{
		private const int LEVEL_ROOT = -1;


		public int Level { get; private set; } = 0;
		public string Title { get; private set; } = String.Empty;
		public ADBDumpsysRecord? Parent { get; private set; } = null;
		public List<ADBDumpsysRecord> Childrens { get; private set; } = new List<ADBDumpsysRecord>();


		private ADBDumpsysRecord(string sTitle, int iLevel, ADBDumpsysRecord? rParent) : base()
		{
			Title = sTitle;
			Level = iLevel;
			Parent = rParent;
		}

		/// <summary>Creates New Root Folder</summary>
		private ADBDumpsysRecord() : this("Root", LEVEL_ROOT, null) { }

		public bool HasChildrens => Childrens.Any();

		public bool IsRoot => Level == LEVEL_ROOT;


		public override string ToString()
		{
			StringBuilder sbResult = new();
			if (!IsRoot) sbResult.AppendLine(new string('-', Level) + Title);
			foreach (var C in Childrens) sbResult.AppendLine(C.ToString());
			return sbResult.ToString().Trim();
		}

		public ADBDumpsysRecord? Find(string S)
		{
			if (!IsRoot && (Title ?? "") == (S ?? "")) return this;

			foreach (var C in Childrens)
			{
				var F = C.Find(S);
				if (F != null) return F;
			}
			return null;
		}

		public ADBDumpsysRecord? FindStartsWith(string S)
		{
			if (!IsRoot && Title.StartsWith(S)) return this;

			foreach (var C in Childrens)
			{
				var F = C.FindStartsWith(S);
				if (F != null) return F;
			}
			return null;
		}

		public static ADBDumpsysRecord ParseRAW(string sRAW)
		{
			var aLines = sRAW.Split(common.ExtensionsString.CRLF, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
			var lLines = aLines.ToList();
			var rRoot = new ADBDumpsysRecord();
			ParseChildRows(lLines, rRoot);
			return rRoot;
		}

		private static void ParseChildRows(List<string> lLines, ADBDumpsysRecord FolderToAdd)
		{
			while (lLines.Any())
			{
				string sLine = lLines.First();
				if (!string.IsNullOrWhiteSpace(sLine))
				{
					int iSpaces = sLine.TakeWhile(c => c == ' ').Count();
					int iRowLevel = (int)Math.Round(iSpaces / 2d);
					sLine = sLine.Trim();
					if (iRowLevel <= FolderToAdd.Level) return; // Родительский, или того же уровня

					// Дочерний элемент
					lLines.RemoveAt(0);
					var rChild = new ADBDumpsysRecord(sLine, iRowLevel, FolderToAdd);
					FolderToAdd.Childrens.Add(rChild);
					ParseChildRows(lLines, rChild);
				}
			}
		}
	}
}



#region Sampe
// Activity Resolver Table:
// Full MIME Types
// vnd.android.cursor.dir/ vnd.samsung.calendar.event
// 31605726 com.android.calendar/.agenda.AgendaPickActivity
// vnd.android.cursor.item/ vnd.samsung.calendar.*
// 31605726 com.android.calendar/.agenda.AgendaPickActivity
// vnd.android.cursor.item/ vnd.samsung.calendar.event
// 31605726 com.android.calendar/.agenda.AgendaPickActivity
// vnd.android.cursor.dir/ vnd.samsung.calendar.task
// 31605726 com.android.calendar/.agenda.AgendaPickActivity
// vnd.android.cursor.dir/event:
// 2df6df67 com.android.calendar/.EditEventActivity
// 31605726 com.android.calendar/.agenda.AgendaPickActivity
// application/ ics
// 3ad8b114 com.android.calendar/.vcal.VCalListActivity
// text/ calendar: 
// 1dafadbd com.android.calendar/.NfcImportActivity
// 3ad8b114 com.android.calendar/.vcal.VCalListActivity
// time/ epoch: 
// 7eabfb2 com.android.calendar/.AllInOneActivity
// */*
// 3ad8b114 com.android.calendar/.vcal.VCalListActivity
// text/ x - vcalendar: 
// 1dafadbd com.android.calendar/.NfcImportActivity
// 3ad8b114 com.android.calendar/.vcal.VCalListActivity
// text/ x - vCalendar: 
// 1dafadbd com.android.calendar/.NfcImportActivity
// 3ad8b114 com.android.calendar/.vcal.VCalListActivity
// vnd.android.cursor.item/ vnd.samsung.calendar.task
// 31605726 com.android.calendar/.agenda.AgendaPickActivity
// text/ x - vtodo
// 1b080b03 com.android.calendar/.vcal.VTaskListActivity
// 1dafadbd com.android.calendar/.NfcImportActivity
// text/ plain
// 31605726 com.android.calendar/.agenda.AgendaPickActivity
// vnd.android.cursor.item/event:
// 2df6df67 com.android.calendar/.EditEventActivity
// 31605726 com.android.calendar/.agenda.AgendaPickActivity
// 3d19ca80 com.android.calendar/.detail.EventInfoActivity
// application/ octet - stream
// 1b080b03 com.android.calendar/.vcal.VTaskListActivity (2 filters)
// vnd.android.cursor.dir/ vnd.samsung.calendar.*
// 31605726 com.android.calendar/.agenda.AgendaPickActivity

// Base MIME Types
// vnd.android.cursor.dir : 
// 2df6df67 com.android.calendar/.EditEventActivity
// 31605726 com.android.calendar/.agenda.AgendaPickActivity (4 filters)
// vnd.android.cursor.item : 
// 2df6df67 com.android.calendar/.EditEventActivity
// 31605726 com.android.calendar/.agenda.AgendaPickActivity (4 filters)
// 3d19ca80 com.android.calendar/.detail.EventInfoActivity
// text:
// 1b080b03 com.android.calendar/.vcal.VTaskListActivity
// 1dafadbd com.android.calendar/.NfcImportActivity (4 filters)
// 31605726 com.android.calendar/.agenda.AgendaPickActivity
// 3ad8b114 com.android.calendar/.vcal.VCalListActivity (3 filters)
// time:
// 7eabfb2 com.android.calendar/.AllInOneActivity
// application:
// 1b080b03 com.android.calendar/.vcal.VTaskListActivity (2 filters)
// 3ad8b114 com.android.calendar/.vcal.VCalListActivity

// Wild MIME Types:
// *
// 3ad8b114 com.android.calendar/.vcal.VCalListActivity

// Schemes:
// mailto-action: 
// 3eae68b9 com.android.calendar/.LinkActionChooserActivity
// geo-action: 
// 3eae68b9 com.android.calendar/.LinkActionChooserActivity
// url-action - no - chooser: 
// 3eae68b9 com.android.calendar/.LinkActionChooserActivity
// tel-action: 
// 3eae68b9 com.android.calendar/.LinkActionChooserActivity
// file:
// 1b080b03 com.android.calendar/.vcal.VTaskListActivity
// 3ad8b114 com.android.calendar/.vcal.VCalListActivity
// http:
// c4124fe com.android.calendar/.GoogleCalendarUriIntentFilter
// https:
// c4124fe com.android.calendar/.GoogleCalendarUriIntentFilter
// tel-action - no - chooser: 
// 3eae68b9 com.android.calendar/.LinkActionChooserActivity
// content:
// 7eabfb2 com.android.calendar/.AllInOneActivity
// 1b080b03 com.android.calendar/.vcal.VTaskListActivity
// 2b76545f com.android.calendar/.timezone.CalendarTimezoneActivity
// mailto-action - no - chooser: 
// 3eae68b9 com.android.calendar/.LinkActionChooserActivity
// url-action: 
// 3eae68b9 com.android.calendar/.LinkActionChooserActivity

// Non-Data Actions
// android.intent.action.MAIN : 
// 7eabfb2 com.android.calendar/.AllInOneActivity
// 2f910eac com.android.calendar/.preference.CalendarSoundSettingsActivity
// android.intent.action.VIEW : 
// 3843d775 com.android.calendar/.detail.TaskInfoActivity
// 3d19ca80 com.android.calendar/.detail.EventInfoActivity
// com.sec.android.intent.action.SEC_APPLICATION_SETTINGS
// 3a06930a com.android.calendar/.preference.CalendarSettingsActivity
// com.sec.android.intent.calendar.setting
// 3a06930a com.android.calendar/.preference.CalendarSettingsActivity
// com.sec.android.intent.calendar.OPEN_FESTIVAL_LIST : 
// 3955d77b com.android.calendar/.agenda.AgendaFestivalListActivity
// com.sec.android.calendar.event.action.accountvalidationcheck
// 35566998 com.android.calendar/.event.AccountValidationCheckActivity
// com.sec.android.intent.calendar.soundsettings : 
// 2f910eac com.android.calendar/.preference.CalendarSoundSettingsActivity
// android.intent.action.SEARCH : 
// 7eabfb2 com.android.calendar/.AllInOneActivity
// d8775f1 com.android.calendar/.CalendarSearchActivity
// 31605726 com.android.calendar/.agenda.AgendaPickActivity

// MIME Typed Actions
// android.intent.action.EDIT : 
// 2df6df67 com.android.calendar/.EditEventActivity (2 filters)
// android.intent.action.PICK
// 31605726 com.android.calendar/.agenda.AgendaPickActivity
// android.intent.action.VIEW : 
// 7eabfb2 com.android.calendar/.AllInOneActivity
// 1b080b03 com.android.calendar/.vcal.VTaskListActivity (3 filters)
// 3ad8b114 com.android.calendar/.vcal.VCalListActivity (3 filters)
// 3d19ca80 com.android.calendar/.detail.EventInfoActivity
// android.intent.action.IMPORT
// 1b080b03 com.android.calendar/.vcal.VTaskListActivity (3 filters)
// 3ad8b114 com.android.calendar/.vcal.VCalListActivity (2 filters)
// android.intent.action.INSERT : 
// 2df6df67 com.android.calendar/.EditEventActivity (2 filters)
// android.nfc.action.NDEF_DISCOVERED : 
// 1dafadbd com.android.calendar/.NfcImportActivity

// Receiver Resolver Table:
// Full MIME Types:
// */*
// 3202d5d6 com.android.calendar/.common.receiver.VCalReceiver

// Wild MIME Types:
// *
// 3202d5d6 com.android.calendar/.common.receiver.VCalReceiver

// Schemes:
// file:
// 3202d5d6 com.android.calendar/.common.receiver.VCalReceiver
// content:
// 9148744 com.android.calendar/.alerts.TaskAlertReceiver (2 filters)
// 21e07057 com.android.calendar/.alerts.AlertReceiver (2 filters)

// Non-Data Actions
// android.intent.action.BCS_REQUEST
// 18c802d com.android.calendar/.common.receiver.CpiCommandReceiver
// com.samsung.android.intent.action.REQUEST_RESTORE_CALENDAR_SETTING
// 7ae7962 com.android.calendar/.preference.KiesBackupReceiver
// com.sec.android.calendar.ACTION_VCAL : 
// 3202d5d6 com.android.calendar/.common.receiver.VCalReceiver
// com.samsung.android.intent.action.REQUEST_BACKUP_CALENDAR_SETTING
// 7ae7962 com.android.calendar/.preference.KiesBackupReceiver
// com.samsung.accessory.saproviders.sacalendar.DISMISS_SNOOZE : 
// 21e07057 com.android.calendar/.alerts.AlertReceiver
// com.samsung.intent.action.SETTINGS_SOFT_RESET
// 3211baf3 com.android.calendar/.common.receiver.ResetSettingsReceiver
// osp.signin.SAMSUNG_ACCOUNT_SIGNOUT : 
// 374d3b0 com.android.calendar/.common.receiver.FacebookEventUpdateReceiver
// com.samsung.android.calendar.preference.CscReceiver
// 3b5df229 com.android.calendar/.preference.CscReceiver
// com.sec.android.app.sns.action.LOGIN : 
// 374d3b0 com.android.calendar/.common.receiver.FacebookEventUpdateReceiver
// android.intent.action.LOCALE_CHANGED
// 9148744 com.android.calendar/.alerts.TaskAlertReceiver
// 21e07057 com.android.calendar/.alerts.AlertReceiver
// com.android.calendar.DISMISS_SNOOZE : 
// 21e07057 com.android.calendar/.alerts.AlertReceiver
// android.intent.action.TIME_SET
// 9148744 com.android.calendar/.alerts.TaskAlertReceiver
// 21e07057 com.android.calendar/.alerts.AlertReceiver
// com.sec.android.app.sns3.action.SYNC_EVENT : 
// 374d3b0 com.android.calendar/.common.receiver.FacebookEventUpdateReceiver
// android.intent.action.BOOT_COMPLETED
// 9148744 com.android.calendar/.alerts.TaskAlertReceiver
// 21e07057 com.android.calendar/.alerts.AlertReceiver
// 3b5df229 com.android.calendar/.preference.CscReceiver
// com.sec.android.app.view.calendars
// 1b7ec9ae com.android.calendar/.common.receiver.GearReceiver
// com.samsung.android.theme.themecenter.THEME_APPLY
// 3b5df229 com.android.calendar/.preference.CscReceiver
// com.sec.android.app.sns.action.UPDATE_SCHEDULE : 
// 374d3b0 com.android.calendar/.common.receiver.FacebookEventUpdateReceiver

// MIME Typed Actions
// com.sec.android.calendar.ACTION_VCAL_UPDATE : 
// 3202d5d6 com.android.calendar/.common.receiver.VCalReceiver

// Permissions:
// Permission [com.sec.android.app.calendar.permission.READ_CALENDAR_SETTINGS] (103134F)
// sourcePackage = com.android.calendar
// uid = 10097 gids=[] type=0 prot=normal
// packageSetting = PackageSetting{1c4b7adc com.android.calendar/10097}
// perm = Permission{2Dba87e5 com.sec.android.app.calendar.permission.READ_CALENDAR_SETTINGS}
// Permission [com.samsung.android.calendar.permission.LAUNCH_SELECT_MAP] (df1d2ba)
// sourcePackage = com.android.calendar
// uid = 10097 gids=[] type=0 prot=normal
// packageSetting = PackageSetting{1c4b7adc com.android.calendar/10097}
// perm = Permission{1407956b com.samsung.android.calendar.permission.LAUNCH_SELECT_MAP}
// Permission [com.samsung.android.calendar.permission.LAUNCH_SELECT_MAP_CHINA] (128368c8)
// sourcePackage = com.android.calendar
// uid = 10097 gids=[] type=0 prot=normal
// packageSetting = PackageSetting{1c4b7adc com.android.calendar/10097}
// perm = Permission{3E90bd61 com.samsung.android.calendar.permission.LAUNCH_SELECT_MAP_CHINA}
// Permission [com.sec.android.app.calendar.permission.OPEN_CALENDAR_SETTINGS] (175a6086)
// sourcePackage = com.android.calendar
// uid = 10097 gids=[] type=0 prot=normal
// packageSetting = PackageSetting{1c4b7adc com.android.calendar/10097}
// perm = Permission{28881d47 com.sec.android.app.calendar.permission.OPEN_CALENDAR_SETTINGS}
// Permission [com.sec.android.app.calendar.permission.WRITE_CALENDAR_SETTINGS] (26ca4974)
// sourcePackage = com.android.calendar
// uid = 10097 gids=[] type=0 prot=normal
// packageSetting = PackageSetting{1c4b7adc com.android.calendar/10097}
// perm = Permission{2a3ace9d com.sec.android.app.calendar.permission.WRITE_CALENDAR_SETTINGS}
// Permission [com.sec.android.app.calendar.permission.USE_VCAL_COMPONENT] (106bff12)
// sourcePackage = com.android.calendar
// uid = 10097 gids=[] type=0 prot=normal
// packageSetting = PackageSetting{1c4b7adc com.android.calendar/10097}
// perm = Permission{3F9F46E3 com.sec.android.app.calendar.permission.USE_VCAL_COMPONENT}

// Registered ContentProviders
// com.android.calendar/ .preference.PreferenceProvider
// Provider{14.0Fc88e0 com.android.calendar/.preference.PreferenceProvider}
// com.android.calendar/ .globalSearch.CalendarSuggestionsProvider
// Provider{10000000.0ab799 com.android.calendar/.globalSearch.CalendarSuggestionsProvider}
// com.android.calendar/ android.support.v4.content.FileProvider
// Provider{3786.0Fa5e com.android.calendar/android.support.v4.content.FileProvider}

// ContentProvider Authorities : 
// [com.sec.android.calendar.preference]
// Provider{14.0Fc88e0 com.android.calendar/.preference.PreferenceProvider}
// applicationInfo = ApplicationInfo{7756E3F com.android.calendar}
// [com.android.calendar.vcs]
// Provider{3786.0Fa5e com.android.calendar/android.support.v4.content.FileProvider}
// applicationInfo = ApplicationInfo{7756E3F com.android.calendar}
// [com.sec.android.calendar.CalendarSuggestionsProvider]
// Provider{10000000.0ab799 com.android.calendar/.globalSearch.CalendarSuggestionsProvider}
// applicationInfo = ApplicationInfo{7756E3F com.android.calendar}

// Key Set Manager:
// [com.android.calendar]
// Signing KeySets :  1

// Packages:
// Package [com.android.calendar] (1c4b7adc)
// userId = 10097 gids=[3003, 1028, 1015, 1023, 3002]
// pkg = Package{3851530c com.android.calendar}
// codePath =/ system / app / SPlanner_Essential
// resourcePath =/ system / app / SPlanner_Essential
// legacyNativeLibraryDir =/ system / app / SPlanner_Essential /lib
// primaryCpuAbi = null
// secondaryCpuAbi = null
// nativeLibraryRootDir =/ system / app / SPlanner_Essential /lib
// nativeLibraryDir =/ system / app / SPlanner_Essential /lib/arm
// secondaryNativeLibraryDir = null
// nativeLibraryRootRequiresIsa = True
// dexMode = unknown
// versionCode = 17082120 targetSdk=21
// versionName = 3.1.3.17082120
// splits = [base]
// applicationInfo = ApplicationInfo{7756E3F com.android.calendar}
// flags =[ SYSTEM HAS_CODE ALLOW_CLEAR_USER_DATA ALLOW_BACKUP KILL_AFTER_RESTORE ]
// dataDir =/ data / data / com.android.calendar
// supportsScreens =[small, medium, large, xlarge, resizeable, anyDensity]
// usesOptionalLibraries:
// touchwiz
// usesLibraryFiles:
// /system/framework/twframework.jar
// timeStamp = 2018 - 1 - 17 08:42:06
// firstInstallTime = 2018 - 1 - 17 08:42:06
// lastUpdateTime = 2018 - 1 - 17 08:42:06
// signatures = PackageSignatures{9363455 [3c6c2333]}
// permissionsFixed = False haveGids=True installStatus=1
// pkgFlags =[ SYSTEM HAS_CODE ALLOW_CLEAR_USER_DATA ALLOW_BACKUP KILL_AFTER_RESTORE ]
// User 0: installed = False hidden=False stopped=True notLaunched=True enabled=0
// grantedPermissions:
// android.permission.WRITE_SETTINGS
// android.permission.READ_CALENDAR
// android.permission.USE_CREDENTIALS
// android.permission.MANAGE_ACCOUNTS
// android.permission.SYSTEM_ALERT_WINDOW
// android.permission.SEND_RESPOND_VIA_MESSAGE
// android.permission.CHANGE_COMPONENT_ENABLED_STATE
// android.permission.NFC
// android.permission.RECEIVE_BOOT_COMPLETED
// com.wssnps.permission.COM_WSSNPS
// com.sec.android.settings.permission.SOFT_RESET
// android.permission.DEVICE_POWER
// android.permission.READ_PROFILE
// android.permission.BLUETOOTH
// com.samsung.android.email.permission.ACCESS_PROVIDER
// android.permission.WRITE_MEDIA_STORAGE
// android.permission.INTERNET
// com.google.android.googleapps.permission.GOOGLE_AUTH.mail
// android.permission.READ_EXTERNAL_STORAGE
// android.permission.INTERACT_ACROSS_USERS_FULL
// android.permission.ACCESS_COARSE_LOCATION
// android.permission.READ_PHONE_STATE
// android.permission.CALL_PHONE
// android.permission.WRITE_CONTACTS
// android.permission.CHANGE_WIFI_STATE
// android.permission.MANAGE_USERS
// android.permission.ACCESS_NETWORK_STATE
// android.permission.DISABLE_KEYGUARD
// android.permission.CHANGE_CONFIGURATION
// android.permission.INTERACT_ACROSS_USERS
// android.permission.WRITE_CALENDAR
// com.sec.enterprise.knox.MDM_CONTENT_PROVIDER
// android.permission.READ_SYNC_STATS
// android.permission.READ_SYNC_SETTINGS
// android.permission.GET_ACCOUNTS
// android.permission.WRITE_EXTERNAL_STORAGE
// android.permission.VIBRATE
// android.permission.ACCESS_WIFI_STATE
// android.permission.STATUS_BAR
// com.sec.android.app.calendar.permission.CHANGE_SHARE
// android.permission.WAKE_LOCK
// android.permission.READ_CONTACTS
// android.permission.INJECT_EVENTS
// com.sec.android.app.sns.permission.READ_SNSDB
// android.permission.UPDATE_APP_OPS_STATS
// mPackagesOnlyForOwnerUser:
// mComponentsOnlyForOwnerUser:
#endregion


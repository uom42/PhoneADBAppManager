#nullable enable

using uom.Extensions;

using static AndroidADBAppManager.ADB.Package;

namespace AndroidADBAppManager.ADB
{

	internal class PackageListViewItem : ListViewItem
	{
		private const int C_PACKAGE_COLUMNS = 4;

		private static readonly Color CLR_BACK_DEFAULT = SystemColors.Window;
		private static readonly Color CLR_BACK_FROZEN = Color.LightSkyBlue; // (ARGB = &HFF87CEFA) (H;S;B; = 202,9565; 0,9200001; 0,754902;)
		private static readonly Color CLR_BACK_DUMPSYS_DISABLED = Color.LightGray;
		private static readonly Color CLR_BACK_UNINSTALLED = Color.Gray; // (ARGB = &HFF808080) (H;S;B; = 0; 0; 0,5019608;)
		private static readonly Color CLR_BACK_SAFE_TO_DELETE = Color.LightGreen;
		private static readonly Color CLR_FORE_DEFAULT = SystemColors.WindowText;
		private static readonly Color CLR_FORE_CRITICAL = Color.Red; // (ARGB = &HFFFF0000) (H;S;B; = 0; 1; 0,5;)


		public readonly Package Package;

		public PackageListViewItem(Package p) : base(p.Name)
		{
			Package = p;
			this.AddFakeSubitems(C_PACKAGE_COLUMNS);
			UpdateLI();
		}

		public bool HasDumpsysInfo => !string.IsNullOrWhiteSpace(Package.Dumpsys_RAW);


		public void UpdateLI()
		{
			// Call .Images.Add(C_ICON_APPLICATION, SystemIcons.Application)
			// Call .Images.Add(C_ICON_CRITICAL, SystemIcons.Exclamation)
			// Call .Images.Add(C_ICON_ERROR, SystemIcons.Error)

			var clrFore = CLR_FORE_DEFAULT;
			var clrBack = CLR_BACK_DEFAULT;
			//string sIconKey = ADBManager.mMain.C_ICON_APPLICATION;
			if (Package.State.HasFlag(E_PACKAGE_STATES.Critical))
			{
				//sIconKey = ADBManager.mMain.C_ICON_CRITICAL;
				clrFore = CLR_FORE_CRITICAL;
			}
			else
			{
				clrBack = CLR_BACK_SAFE_TO_DELETE;
			}

			string sState = Package.State.ToString();
			if (Package.State.HasFlag(E_PACKAGE_STATES.Hidden)) clrBack = CLR_BACK_DUMPSYS_DISABLED;
			if (Package.State.HasFlag(E_PACKAGE_STATES.Disabled_Frozen)) clrBack = CLR_BACK_FROZEN; // CLR_BACK_DISABLED
			if (Package.State.HasFlag(E_PACKAGE_STATES.Uninstalled)) clrBack = CLR_BACK_UNINSTALLED;
			ForeColor = clrFore;
			BackColor = clrBack;
			//ImageKey = sIconKey;

			string sDescr = String.Empty;
			//if (Package.KnownPackageData is object) sDescr = Package.KnownPackageData.Description;
			//sDescr = sDescr.e_CheckNullOrWhiteSpace();

			this.UpdateTexts(Package.Name, Package.Path, sState, sDescr);
		}

		public bool CheckFilter(string sFilter)
		{
			bool bCheck = Package.Name.ToLower().Contains(sFilter) || Package.Path.ToLower().Contains(sFilter);
			return bCheck;
		}
	}
}

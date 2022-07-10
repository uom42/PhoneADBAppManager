
using AndroidADBAppManager.ADB;

using common;

using uom.Extensions;

namespace AndroidADBAppManager
{
	internal partial class frmMain : Form
	{


		private readonly ADBServer ADB;


		protected frmMain()
		{
			InitializeComponent();
		}


		public frmMain(ADBServer s) : this()
		{
			ADB = s;
			Text = ADB.CurrentDevice.ToString();

			InitUI_Main();

			Package_Init();
			Props_Init();

			this.e_RunDelay(ReloadAll());
		}


		private void InitUI_Main()
		{
			cmdGetRoot.Visible = false;
			cmdReloadAll.Click += async (_, _) => await ReloadAll();
		}


		private async Task ReloadAll()
		{
			await Props_Reload();
			await Package_Reload();
		}


	}
}

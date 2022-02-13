
using AndroidADBAppManager.ADB;

using common;

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


			Package_Init();
			Props_Init();

			this.ExecDelay(ReloadAll());
		}

		private async Task ReloadAll()
		{
			await Props_Reload();
			await Package_Reload();
		}
	}
}

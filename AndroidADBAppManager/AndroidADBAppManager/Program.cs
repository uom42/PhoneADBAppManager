using AndroidADBAppManager.ADB;

namespace AndroidADBAppManager
{
	internal static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

			/*
			Application.ThreadException += (s, e) =>
			{
				MessageBox.Show(e.Exception.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(1);
			};
			 */

			try
			{
				var srv = frmADBConnect.Connect();
				if (srv == null) return;

				Application.Run(new frmMain(srv!));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			/*		 
			//if (!System.Diagnostics.Debugger.IsAttached)
			{
				

				AppDomain.CurrentDomain.FirstChanceException += (s, e) =>
				{
					MessageBox.Show(e.Exception.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					Environment.Exit(1);
				};

				AppDomain.CurrentDomain.UnhandledException += (s, e) =>
				{
					MessageBox.Show(e.ToString());
					Environment.Exit(1);
				};
			}
		

			try
			{
				var srv = frmADBConnect.Connect();
				if (srv == null)
				{
					throw new Exception("Failed to Connect to ADB!");
					return;
				}

				Application.Run(new frmMain());
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
				*/
		}
	}
}

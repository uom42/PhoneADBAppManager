using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AndroidADBAppManager.ADBCore;

namespace AndroidADBAppManager
{
    internal partial class frmMain : Form
    {
        private ADBServer srv;

        protected frmMain()
        {
            InitializeComponent();
        }

        public frmMain(ADBServer s) : this()
        {
            srv = s;

            Text = srv.CurrentDevice.ToString();

            //this.Shown += new EventHandler(OnShown);

        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void OnShown(object sender, EventArgs e)
        {
            try
            {
                //srv = frmADBConnect.Connect();
                //if (srv == null) throw new Exception("Failed to Connect to ADB!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
    }
}

using AndroidADBAppManager.ADB;

using common;

using NLog;

using uom.Extensions;

#nullable enable

namespace AndroidADBAppManager
{
	internal partial class frmADBConnect : Form
	{
		private ILogger logger = LogManager.GetCurrentClassLogger();
		private ADBServer? srv = null;


		internal frmADBConnect()
		{
			InitializeComponent();
			txtPath!.Text = ADBServer.GetSettings_ADBPath();
			pbProgress!.Visible = false;
		}


		public static ADBServer? Connect()
		{
			using (frmADBConnect fc = new())
			{
				if (fc.ShowDialog() != DialogResult.OK) return null;
				return fc.srv;
			}
		}


		private async void OnConnect(object sender, EventArgs e)
		{
			btnConnect.Enabled = false;
			txtPath.Enabled = false;
			UseWaitCursor = true;
			pbProgress.Style = ProgressBarStyle.Marquee;
			pbProgress.Visible = true;
			try
			{
				using (Task<ADBServer> tskConnect = new(() =>
				{
					string path = txtPath.Text.Trim();
					var srv = new ADBServer(path);
					return srv;
				}, TaskCreationOptions.LongRunning))
				{
					tskConnect.Start();
					srv = await tskConnect;
				}
				if (srv != null) DialogResult = DialogResult.OK;
			}
			catch (Exception ex)
			{

				ex.Handle(true, logger);
			}
			finally
			{
				pbProgress.Visible = false;
				txtPath.Enabled = true;
				btnConnect.Enabled = true;
				UseWaitCursor = false;
			}
		}

		private void txtPath_TextChanged(object sender, EventArgs e)
		{
			bool fileFound = false;
			try
			{
				txtPath.Text.Trim().e_throwIfNotExist();
				fileFound = true;
				err.SetError(txtPath, String.Empty);
			}
			catch (Exception ex) { err.SetError(txtPath, ex.Message); }
			finally { btnConnect.Enabled = fileFound; }

		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.lblPathTitle = new System.Windows.Forms.Label();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.btnConnect = new System.Windows.Forms.Button();
			this.pbProgress = new System.Windows.Forms.ProgressBar();
			this.err = new System.Windows.Forms.ErrorProvider(this.components);
			this.tlpMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
			this.SuspendLayout();
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 1;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Controls.Add(this.lblPathTitle, 0, 0);
			this.tlpMain.Controls.Add(this.txtPath, 0, 1);
			this.tlpMain.Controls.Add(this.btnConnect, 0, 3);
			this.tlpMain.Controls.Add(this.pbProgress, 0, 2);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMain.Location = new System.Drawing.Point(0, 0);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.Padding = new System.Windows.Forms.Padding(24);
			this.tlpMain.RowCount = 4;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tlpMain.Size = new System.Drawing.Size(493, 157);
			this.tlpMain.TabIndex = 0;
			// 
			// lblPathTitle
			// 
			this.lblPathTitle.AutoSize = true;
			this.lblPathTitle.Location = new System.Drawing.Point(27, 24);
			this.lblPathTitle.Name = "lblPathTitle";
			this.lblPathTitle.Size = new System.Drawing.Size(98, 15);
			this.lblPathTitle.TabIndex = 0;
			this.lblPathTitle.Text = "Path to \'adb.exe\':";
			// 
			// txtPath
			// 
			this.txtPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.txtPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
			this.txtPath.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtPath.Location = new System.Drawing.Point(27, 42);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(439, 23);
			this.txtPath.TabIndex = 1;
			this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged!);
			// 
			// btnConnect
			// 
			this.btnConnect.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnConnect.Location = new System.Drawing.Point(391, 96);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(75, 34);
			this.btnConnect.TabIndex = 2;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.OnConnect!);
			// 
			// pbProgress
			// 
			this.pbProgress.Dock = System.Windows.Forms.DockStyle.Top;
			this.pbProgress.Location = new System.Drawing.Point(27, 71);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(439, 8);
			this.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.pbProgress.TabIndex = 3;
			this.pbProgress.Value = 100;
			// 
			// err
			// 
			this.err.ContainerControl = this;
			// 
			// frmADBConnect
			// 
			this.AcceptButton = this.btnConnect;
			this.ClientSize = new System.Drawing.Size(493, 157);
			this.Controls.Add(this.tlpMain);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmADBConnect";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Connect to ADB";
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
			this.ResumeLayout(false);

		}

		private TableLayoutPanel tlpMain;
		private Label lblPathTitle;
		private TextBox txtPath;
		private Button btnConnect;
		private ErrorProvider err;
		private System.ComponentModel.IContainer components;
		private ProgressBar pbProgress;
	}
}

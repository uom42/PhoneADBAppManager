using System.Diagnostics;

namespace AndroidADBAppManager
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		// NOTE: The following procedure is required by the Windows Form Designer
		// It can be modified using the Windows Form Designer.  
		// Do not modify it using the code editor.
		[DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tcMain = new System.Windows.Forms.TabControl();
			this.tcPackages = new System.Windows.Forms.TabPage();
			this.splitPackages_1 = new System.Windows.Forms.SplitContainer();
			this.lvwPackages = new common.Controls.ListViewEx();
			this.colPackage_Name = new System.Windows.Forms.ColumnHeader();
			this.colPackage_Path = new System.Windows.Forms.ColumnHeader();
			this.colPackage_State = new System.Windows.Forms.ColumnHeader();
			this.colPackage_Description = new System.Windows.Forms.ColumnHeader();
			this.txtPackageFilter = new System.Windows.Forms.TextBox();
			this.chkPackageFilter_ShowOnlyUnknown = new System.Windows.Forms.CheckBox();
			this.txtDumpsys = new TextBox();
			this.tbPackage = new System.Windows.Forms.ToolStrip();
			this.cmdPackage_Disable = new System.Windows.Forms.ToolStripButton();
			this.cmdPackage_Enable = new System.Windows.Forms.ToolStripButton();
			this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.cmdPackage_Istall = new System.Windows.Forms.ToolStripButton();
			this.cmdPackage_UnIstall = new System.Windows.Forms.ToolStripButton();
			this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.cmdPackage_Download = new System.Windows.Forms.ToolStripButton();
			this.cmdPackage_InstallFromPC = new System.Windows.Forms.ToolStripButton();
			this.ToolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.cmdKnownPackagesDatabase_Show = new System.Windows.Forms.ToolStripButton();
			this.cmdAddUnKnownPackagesToDatabase = new System.Windows.Forms.ToolStripButton();
			this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.cmdPackage_SaveSnapshot = new System.Windows.Forms.ToolStripButton();
			this.pbPackage_Progress = new System.Windows.Forms.ProgressBar();
			this.tcSettings = new System.Windows.Forms.TabPage();
			this.SplitProps_1 = new System.Windows.Forms.SplitContainer();
			this.lvwPropsList = new System.Windows.Forms.ListView();
			this.colProps_ShortName = new System.Windows.Forms.ColumnHeader();
			this.colProps_Value = new System.Windows.Forms.ColumnHeader();
			this.colProps_FullName = new System.Windows.Forms.ColumnHeader();
			this.txtPropsList_Filter = new System.Windows.Forms.TextBox();
			this.TextBox2 = new System.Windows.Forms.TextBox();
			this.BackgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.imlPackagelIcons = new System.Windows.Forms.ImageList(this.components);
			this.tbMain = new System.Windows.Forms.ToolStrip();
			this.cmdReloadAll = new System.Windows.Forms.ToolStripButton();
			this.ToolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.cmdGetRoot = new System.Windows.Forms.ToolStripButton();
			this.tcMain.SuspendLayout();
			this.tcPackages.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitPackages_1)).BeginInit();
			this.splitPackages_1.Panel1.SuspendLayout();
			this.splitPackages_1.Panel2.SuspendLayout();
			this.splitPackages_1.SuspendLayout();
			this.tbPackage.SuspendLayout();
			this.tcSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SplitProps_1)).BeginInit();
			this.SplitProps_1.Panel1.SuspendLayout();
			this.SplitProps_1.Panel2.SuspendLayout();
			this.SplitProps_1.SuspendLayout();
			this.tbMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// tcMain
			// 
			this.tcMain.Controls.Add(this.tcPackages);
			this.tcMain.Controls.Add(this.tcSettings);
			this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tcMain.Location = new System.Drawing.Point(5, 30);
			this.tcMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tcMain.Name = "tcMain";
			this.tcMain.SelectedIndex = 0;
			this.tcMain.Size = new System.Drawing.Size(1291, 698);
			this.tcMain.TabIndex = 3;
			// 
			// tcPackages
			// 
			this.tcPackages.Controls.Add(this.splitPackages_1);
			this.tcPackages.Controls.Add(this.tbPackage);
			this.tcPackages.Controls.Add(this.pbPackage_Progress);
			this.tcPackages.Location = new System.Drawing.Point(4, 24);
			this.tcPackages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tcPackages.Name = "tcPackages";
			this.tcPackages.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tcPackages.Size = new System.Drawing.Size(1283, 670);
			this.tcPackages.TabIndex = 0;
			this.tcPackages.Text = "Packages";
			this.tcPackages.UseVisualStyleBackColor = true;
			// 
			// splitPackages_1
			// 
			this.splitPackages_1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitPackages_1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitPackages_1.Location = new System.Drawing.Point(4, 28);
			this.splitPackages_1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.splitPackages_1.Name = "splitPackages_1";
			// 
			// splitPackages_1.Panel1
			// 
			this.splitPackages_1.Panel1.Controls.Add(this.lvwPackages);
			this.splitPackages_1.Panel1.Controls.Add(this.txtPackageFilter);
			this.splitPackages_1.Panel1.Controls.Add(this.chkPackageFilter_ShowOnlyUnknown);
			// 
			// splitPackages_1.Panel2
			// 
			this.splitPackages_1.Panel2.Controls.Add(this.txtDumpsys);
			this.splitPackages_1.Size = new System.Drawing.Size(1275, 630);
			this.splitPackages_1.SplitterDistance = 740;
			this.splitPackages_1.SplitterWidth = 9;
			this.splitPackages_1.TabIndex = 1;
			// 
			// lvwPackages
			// 
			this.lvwPackages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.colPackage_Name,
			this.colPackage_Path,
			this.colPackage_State,
			this.colPackage_Description});
			this.lvwPackages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvwPackages.FullRowSelect = true;
			this.lvwPackages.GridLines = true;
			this.lvwPackages.Location = new System.Drawing.Point(0, 42);
			this.lvwPackages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.lvwPackages.Name = "lvwPackages";
			this.lvwPackages.Size = new System.Drawing.Size(740, 588);
			this.lvwPackages.TabIndex = 0;
			this.lvwPackages.UseCompatibleStateImageBehavior = false;
			this.lvwPackages.View = System.Windows.Forms.View.Details;
			// 
			// colPackage_Name
			// 
			this.colPackage_Name.Text = "Пакет";
			// 
			// colPackage_Path
			// 
			this.colPackage_Path.Text = "Путь";
			// 
			// colPackage_State
			// 
			this.colPackage_State.Text = "Статус";
			// 
			// colPackage_Description
			// 
			this.colPackage_Description.Text = "Описание";
			// 
			// txtPackageFilter
			// 
			this.txtPackageFilter.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtPackageFilter.Location = new System.Drawing.Point(0, 19);
			this.txtPackageFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.txtPackageFilter.Name = "txtPackageFilter";
			this.txtPackageFilter.Size = new System.Drawing.Size(740, 23);
			this.txtPackageFilter.TabIndex = 1;
			// 
			// chkPackageFilter_ShowOnlyUnknown
			// 
			this.chkPackageFilter_ShowOnlyUnknown.AutoSize = true;
			this.chkPackageFilter_ShowOnlyUnknown.Dock = System.Windows.Forms.DockStyle.Top;
			this.chkPackageFilter_ShowOnlyUnknown.Location = new System.Drawing.Point(0, 0);
			this.chkPackageFilter_ShowOnlyUnknown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.chkPackageFilter_ShowOnlyUnknown.Name = "chkPackageFilter_ShowOnlyUnknown";
			this.chkPackageFilter_ShowOnlyUnknown.Size = new System.Drawing.Size(740, 19);
			this.chkPackageFilter_ShowOnlyUnknown.TabIndex = 2;
			this.chkPackageFilter_ShowOnlyUnknown.Text = "Показать только неизвестные пакеты";
			this.chkPackageFilter_ShowOnlyUnknown.UseVisualStyleBackColor = true;
			// 
			// txtDumpsys
			// 
			this.txtDumpsys.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtDumpsys.Location = new System.Drawing.Point(0, 0);
			this.txtDumpsys.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.txtDumpsys.Multiline = true;
			this.txtDumpsys.Name = "txtDumpsys";
			this.txtDumpsys.PlaceholderText = "Select package for details...";
			this.txtDumpsys.ReadOnly = true;
			this.txtDumpsys.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtDumpsys.Size = new System.Drawing.Size(526, 630);
			this.txtDumpsys.TabIndex = 0;
			// 
			// tbPackage
			// 
			this.tbPackage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.cmdPackage_Disable,
			this.cmdPackage_Enable,
			this.ToolStripSeparator3,
			this.cmdPackage_Istall,
			this.cmdPackage_UnIstall,
			this.ToolStripSeparator2,
			this.cmdPackage_Download,
			this.cmdPackage_InstallFromPC,
			this.ToolStripSeparator4,
			this.cmdKnownPackagesDatabase_Show,
			this.cmdAddUnKnownPackagesToDatabase,
			this.ToolStripSeparator1,
			this.cmdPackage_SaveSnapshot});
			this.tbPackage.Location = new System.Drawing.Point(4, 3);
			this.tbPackage.Name = "tbPackage";
			this.tbPackage.Size = new System.Drawing.Size(1275, 25);
			this.tbPackage.TabIndex = 2;
			this.tbPackage.Text = "ToolStrip1";
			// 
			// cmdPackage_Disable
			// 
			this.cmdPackage_Disable.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdPackage_Disable.Name = "cmdPackage_Disable";
			this.cmdPackage_Disable.Size = new System.Drawing.Size(93, 22);
			this.cmdPackage_Disable.Text = "Freeze (Disable)";
			this.cmdPackage_Disable.ToolTipText = "Заморозить приложение у текущего пользователя";
			// 
			// cmdPackage_Enable
			// 
			this.cmdPackage_Enable.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdPackage_Enable.Name = "cmdPackage_Enable";
			this.cmdPackage_Enable.Size = new System.Drawing.Size(105, 22);
			this.cmdPackage_Enable.Text = "UnFreeze (Enable)";
			this.cmdPackage_Enable.ToolTipText = "Разморозить приложение у текущего пользователя";
			// 
			// ToolStripSeparator3
			// 
			this.ToolStripSeparator3.Name = "ToolStripSeparator3";
			this.ToolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// cmdPackage_Istall
			// 
			this.cmdPackage_Istall.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdPackage_Istall.Name = "cmdPackage_Istall";
			this.cmdPackage_Istall.Size = new System.Drawing.Size(42, 22);
			this.cmdPackage_Istall.Text = "Install";
			this.cmdPackage_Istall.ToolTipText = "Установить приложение для текущего пользователя";
			// 
			// cmdPackage_UnIstall
			// 
			this.cmdPackage_UnIstall.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdPackage_UnIstall.Name = "cmdPackage_UnIstall";
			this.cmdPackage_UnIstall.Size = new System.Drawing.Size(57, 22);
			this.cmdPackage_UnIstall.Text = "UnInstall";
			this.cmdPackage_UnIstall.ToolTipText = "Выполнить \"удаление\" приложение у текущего пользователя (приложение останется на " +
	"устройстве)";
			// 
			// ToolStripSeparator2
			// 
			this.ToolStripSeparator2.Name = "ToolStripSeparator2";
			this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// cmdPackage_Download
			// 
			this.cmdPackage_Download.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdPackage_Download.Name = "cmdPackage_Download";
			this.cmdPackage_Download.Size = new System.Drawing.Size(90, 22);
			this.cmdPackage_Download.Text = "Скачать на ПК";
			this.cmdPackage_Download.ToolTipText = "Скачать APK-файл на рабочий стол ПК";
			// 
			// cmdPackage_InstallFromPC
			// 
			this.cmdPackage_InstallFromPC.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdPackage_InstallFromPC.Name = "cmdPackage_InstallFromPC";
			this.cmdPackage_InstallFromPC.Size = new System.Drawing.Size(101, 22);
			this.cmdPackage_InstallFromPC.Text = "Установить с ПК";
			this.cmdPackage_InstallFromPC.ToolTipText = "Установить APK-файл с ПК на устройство";
			// 
			// ToolStripSeparator4
			// 
			this.ToolStripSeparator4.Name = "ToolStripSeparator4";
			this.ToolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// cmdKnownPackagesDatabase_Show
			// 
			this.cmdKnownPackagesDatabase_Show.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdKnownPackagesDatabase_Show.Name = "cmdKnownPackagesDatabase_Show";
			this.cmdKnownPackagesDatabase_Show.Size = new System.Drawing.Size(141, 22);
			this.cmdKnownPackagesDatabase_Show.Text = "База известных пакетов";
			// 
			// cmdAddUnKnownPackagesToDatabase
			// 
			this.cmdAddUnKnownPackagesToDatabase.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdAddUnKnownPackagesToDatabase.Name = "cmdAddUnKnownPackagesToDatabase";
			this.cmdAddUnKnownPackagesToDatabase.Size = new System.Drawing.Size(172, 22);
			this.cmdAddUnKnownPackagesToDatabase.Text = "Добавить в базу неизвестные";
			// 
			// ToolStripSeparator1
			// 
			this.ToolStripSeparator1.Name = "ToolStripSeparator1";
			this.ToolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// cmdPackage_SaveSnapshot
			// 
			this.cmdPackage_SaveSnapshot.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdPackage_SaveSnapshot.Name = "cmdPackage_SaveSnapshot";
			this.cmdPackage_SaveSnapshot.Size = new System.Drawing.Size(87, 22);
			this.cmdPackage_SaveSnapshot.Text = "Save Snapshot";
			// 
			// pbPackage_Progress
			// 
			this.pbPackage_Progress.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pbPackage_Progress.Location = new System.Drawing.Point(4, 658);
			this.pbPackage_Progress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.pbPackage_Progress.Name = "pbPackage_Progress";
			this.pbPackage_Progress.Size = new System.Drawing.Size(1275, 9);
			this.pbPackage_Progress.TabIndex = 3;
			// 
			// tcSettings
			// 
			this.tcSettings.Controls.Add(this.SplitProps_1);
			this.tcSettings.Location = new System.Drawing.Point(4, 24);
			this.tcSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tcSettings.Name = "tcSettings";
			this.tcSettings.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tcSettings.Size = new System.Drawing.Size(1283, 670);
			this.tcSettings.TabIndex = 1;
			this.tcSettings.Text = "Properties";
			this.tcSettings.UseVisualStyleBackColor = true;
			// 
			// SplitProps_1
			// 
			this.SplitProps_1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SplitProps_1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.SplitProps_1.Location = new System.Drawing.Point(4, 3);
			this.SplitProps_1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.SplitProps_1.Name = "SplitProps_1";
			// 
			// SplitProps_1.Panel1
			// 
			this.SplitProps_1.Panel1.Controls.Add(this.lvwPropsList);
			this.SplitProps_1.Panel1.Controls.Add(this.txtPropsList_Filter);
			// 
			// SplitProps_1.Panel2
			// 
			this.SplitProps_1.Panel2.Controls.Add(this.TextBox2);
			this.SplitProps_1.Size = new System.Drawing.Size(1275, 664);
			this.SplitProps_1.SplitterDistance = 1019;
			this.SplitProps_1.SplitterWidth = 9;
			this.SplitProps_1.TabIndex = 4;
			// 
			// lvwPropsList
			// 
			this.lvwPropsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.colProps_ShortName,
			this.colProps_Value,
			this.colProps_FullName});
			this.lvwPropsList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvwPropsList.FullRowSelect = true;
			this.lvwPropsList.GridLines = true;
			this.lvwPropsList.Location = new System.Drawing.Point(0, 23);
			this.lvwPropsList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.lvwPropsList.Name = "lvwPropsList";
			this.lvwPropsList.Size = new System.Drawing.Size(1019, 641);
			this.lvwPropsList.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvwPropsList.TabIndex = 0;
			this.lvwPropsList.UseCompatibleStateImageBehavior = false;
			this.lvwPropsList.View = System.Windows.Forms.View.Details;
			// 
			// colProps_ShortName
			// 
			this.colProps_ShortName.Text = "Имя";
			// 
			// colProps_Value
			// 
			this.colProps_Value.Text = "Значение";
			// 
			// colProps_FullName
			// 
			this.colProps_FullName.Text = "Полное имя";
			// 
			// txtPropsList_Filter
			// 
			this.txtPropsList_Filter.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtPropsList_Filter.Location = new System.Drawing.Point(0, 0);
			this.txtPropsList_Filter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.txtPropsList_Filter.Name = "txtPropsList_Filter";
			this.txtPropsList_Filter.Size = new System.Drawing.Size(1019, 23);
			this.txtPropsList_Filter.TabIndex = 1;
			// 
			// TextBox2
			// 
			this.TextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TextBox2.Location = new System.Drawing.Point(0, 0);
			this.TextBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.TextBox2.Multiline = true;
			this.TextBox2.Name = "TextBox2";
			this.TextBox2.ReadOnly = true;
			this.TextBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.TextBox2.Size = new System.Drawing.Size(247, 664);
			this.TextBox2.TabIndex = 0;
			// 
			// imlPackagelIcons
			// 
			this.imlPackagelIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imlPackagelIcons.ImageSize = new System.Drawing.Size(16, 16);
			this.imlPackagelIcons.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// tbMain
			// 
			this.tbMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.cmdReloadAll,
			this.ToolStripSeparator9,
			this.cmdGetRoot});
			this.tbMain.Location = new System.Drawing.Point(5, 5);
			this.tbMain.Name = "tbMain";
			this.tbMain.Size = new System.Drawing.Size(1291, 25);
			this.tbMain.TabIndex = 3;
			this.tbMain.Text = "ToolStrip1";
			// 
			// cmdReloadAll
			// 
			this.cmdReloadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdReloadAll.Name = "cmdReloadAll";
			this.cmdReloadAll.Size = new System.Drawing.Size(47, 22);
			this.cmdReloadAll.Text = "Reload";
			// 
			// ToolStripSeparator9
			// 
			this.ToolStripSeparator9.Name = "ToolStripSeparator9";
			this.ToolStripSeparator9.Size = new System.Drawing.Size(6, 25);
			// 
			// cmdGetRoot
			// 
			this.cmdGetRoot.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdGetRoot.Name = "cmdGetRoot";
			this.cmdGetRoot.Size = new System.Drawing.Size(57, 22);
			this.cmdGetRoot.Text = "Get Root";
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1301, 733);
			this.Controls.Add(this.tcMain);
			this.Controls.Add(this.tbMain);
			this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.Name = "frmMain";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Packages";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.tcMain.ResumeLayout(false);
			this.tcPackages.ResumeLayout(false);
			this.tcPackages.PerformLayout();
			this.splitPackages_1.Panel1.ResumeLayout(false);
			this.splitPackages_1.Panel1.PerformLayout();
			this.splitPackages_1.Panel2.ResumeLayout(false);
			this.splitPackages_1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitPackages_1)).EndInit();
			this.splitPackages_1.ResumeLayout(false);
			this.tbPackage.ResumeLayout(false);
			this.tbPackage.PerformLayout();
			this.tcSettings.ResumeLayout(false);
			this.SplitProps_1.Panel1.ResumeLayout(false);
			this.SplitProps_1.Panel1.PerformLayout();
			this.SplitProps_1.Panel2.ResumeLayout(false);
			this.SplitProps_1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.SplitProps_1)).EndInit();
			this.SplitProps_1.ResumeLayout(false);
			this.tbMain.ResumeLayout(false);
			this.tbMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		internal common.Controls.ListViewEx lvwPackages;
		internal ColumnHeader colPackage_Name;
		internal ColumnHeader colPackage_Path;
		internal ColumnHeader colPackage_State;
		internal SplitContainer splitPackages_1;
		internal TextBox txtDumpsys;
		internal System.ComponentModel.BackgroundWorker BackgroundWorker1;
		internal ToolStrip tbPackage;
		internal ToolStripButton cmdPackage_Download;
		internal ToolStripButton cmdPackage_Disable;
		internal ToolStripButton cmdPackage_Enable;
		internal ToolStripSeparator ToolStripSeparator2;
		internal ToolStripButton cmdPackage_UnIstall;
		internal ToolStripSeparator ToolStripSeparator3;
		internal TextBox txtPackageFilter;
		internal ToolStripButton cmdPackage_Istall;
		internal ImageList imlPackagelIcons;
		internal TabControl tcMain;
		internal TabPage tcPackages;
		internal TabPage tcSettings;
		internal SplitContainer SplitProps_1;
		internal ListView lvwPropsList;
		internal ColumnHeader colProps_ShortName;
		internal ColumnHeader colProps_Value;
		internal ColumnHeader colProps_FullName;
		internal TextBox txtPropsList_Filter;
		internal TextBox TextBox2;
		internal ToolStrip tbMain;
		internal ToolStripButton cmdReloadAll;
		internal ToolStripSeparator ToolStripSeparator9;
		internal ToolStripButton cmdGetRoot;
		internal ProgressBar pbPackage_Progress;
		internal ColumnHeader colPackage_Description;
		internal ToolStripButton cmdPackage_InstallFromPC;
		internal ToolStripButton cmdKnownPackagesDatabase_Show;
		internal ToolStripSeparator ToolStripSeparator4;
		internal ToolStripButton cmdAddUnKnownPackagesToDatabase;
		internal ToolStripSeparator ToolStripSeparator1;
		internal ToolStripButton cmdPackage_SaveSnapshot;
		internal CheckBox chkPackageFilter_ShowOnlyUnknown;
	}
}

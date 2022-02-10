
using AndroidADBAppManager.ADB;

using common;

using Microsoft.VisualBasic;

using uom.Extensions;

#nullable enable

namespace AndroidADBAppManager
{
	partial class frmMain
	{


		private Control[] _aPackages_UIControls { get; set; } = Array.Empty<Control>();


		private void Package_ShowDetailsForFirstSelectedPackage()
		{
			Package_UpdateUI();
			var aSelected = lvwPackages.e_SelectedItemsAs<PackageListViewItem>().ToArray();
			string sText = "";
			try
			{
				if (aSelected == null || !aSelected.Any() || aSelected.Length > 1) return;

				sText = "NO EXT DATA";
				var li = aSelected.First();
				if (li.HasDumpsysInfo) sText = li.Package.Dumpsys_RAW!;
			}
			finally { txtDumpsys.Text = sText; }
		}


		private async Task Package_RunForSelected(
			Func<PackageListViewItem, Task> cbAsyncPackageAction,
			bool bUpdateAfter = true)
		{
			var aSelected = lvwPackages.e_SelectedItemsAs<PackageListViewItem>().ToArray();
			if (!aSelected.Any()) return;
			this.UseWaitCursor = true;

			var tsk = async () =>
			{
				foreach (var LI in aSelected)
				{
					try
					{
						await cbAsyncPackageAction(LI);
						if (bUpdateAfter) LI.UpdateLI();
					}
					catch (Exception ex)
					{
						Interaction.MsgBox(ex.Message, MsgBoxStyle.Critical);
					}
				}
			};

			await _aPackages_UIControls.e_RunOnDisabledAsync(tsk, this);
		}


		private async void On_Package_Enable_Disable_Click(object sender, EventArgs e)
		{
			bool bEnable = (sender == cmdPackage_Enable);

			Func<PackageListViewItem, Task> cbAction = async LI =>
			{
				try
				{
					await LI.Package.Enable(ADB, bEnable);
				}
				catch (System.Security.SecurityException sex)
				{
					throw new System.Security.SecurityException($"{sex.Message}\n\nMay be Root need!");
				}
			};
			await Package_RunForSelected(cbAction);
		}


		private async void On_Package_Istall_Click(object sender, EventArgs e)
		{
			bool bInstall = object.ReferenceEquals(sender, this.cmdPackage_Istall);
			Func<PackageListViewItem, Task> cbAction = async LI => await LI.Package.Install(ADB, bInstall);
			await Package_RunForSelected(cbAction);
		}


		#region PC Install / Download


		private async void Package_DownloadAPKToDesktop()
		{
			string sDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			Func<PackageListViewItem, Task> cbAction = async LI =>
			{
				var diFile = await LI.Package.Download(ADB, sDesktop);
			};
			await Package_RunForSelected(cbAction);
		}


		private async void Package_InstallFromPC()
		{
			string sAppPath = this.e_OpenLoadFileDialog(
				"apk",
				"Android applications (apk)|*.apk",
				initialDirectory: Environment.SpecialFolder.DesktopDirectory);

			if (sAppPath.e_IsNullOrWhiteSpace()) return;

			// adb install [-l] [-r] [-s] <название_приложения.apk> Послать приложение на устройство и установить его.
			// Пример: adb install c: /adb/app/autostarts.apk Установить файл autostarts.apk лежащий в папке /adb/app/ на диске с
			// Ключи:
			// -l Блокировка приложения
			// -r Переустановить приложение, с сохранением данных
			// -s Установить приложение на карту памяти

			string sCMD = "install ";
			sCMD += sAppPath.e_Enclose();
			this.UseWaitCursor = true;
			try
			{
				await _aPackages_UIControls.e_RunOnDisabledAsync(async () =>
				{
					try
					{
						const string CS_RESULT_OK = "Success";
						const int C_WAIT_MIN = 3;
						string sResult = (await ADB.e_runADBAsync(sCMD, 60 * C_WAIT_MIN)).NormalizedOutput();
						if (sResult.e_IsNOTNullOrWhiteSpace() && sResult.ToLower().Trim().EndsWith(CS_RESULT_OK.ToLower()))
						{
							Interaction.MsgBox("Success!", Constants.vbOKOnly | Constants.vbInformation);
							return;
						}
						throw new Exception(sResult);
					}
					catch (Exception ex)
					{
						ex.Handle(true);
					}
				});
			}
			finally
			{
				this.UseWaitCursor = false;
			}

			await this.Package_Reload();
		}


		#endregion

		/*
		
		/// <summary>Добавить в базу неизвестные пакеты</summary>
		private void AddUnKnownPackagesToDatabase()
		{
			var aAllInGrid = this.lvwPackages.ExtCtl_ItemsAs<ADBPackageListViewItem>();
			var aUnknownPackages = (from Li in aAllInGrid
									where Li.Package.KnownPackageData is null
									select Li.Package).ToArray();
			if (!aUnknownPackages.Any())
			{
				Interaction.MsgBox("Нет неизвестных пакетов!", MsgBoxStyle.OkOnly | MsgBoxStyle.Information);
				return;
			}

			string sPhone = ADBManager.mMain.gPhoneDevice.Model.e_CheckNullOrWhiteSpace();
			var lNewPackages = new List<ADBManager.KNOWN_PACKAGE>();
			aUnknownPackages.ToList().ForEach(P =>
			{
				var KP = ADBManager.KNOWN_PACKAGE.FromPackageName(P.Name, ADBManager.mKnownPackages.g_KnownPachagesDatabase);
				string sDescr = Constants.vbNullString;
				if (sPhone.e_IsNullOrWhiteSpace())
				{
					sDescr = P.Path;
				}
				else
				{
					sDescr = "{0} ({1})".e_Format(P.Path, sPhone);
				}

				sDescr = sDescr.Trim();
				KP.Description = sDescr;
				lNewPackages.Add(KP);
			});
			string sMsg = "Добавить неизвестные пакеты ({0}шт.) в базу ?".e_Format(lNewPackages.Count);
			const int C_TOP10 = 15;

			// Выводим первые 10 этих пакетов
			var top10 = aUnknownPackages.Take(C_TOP10);
			if (top10.Any())
			{
				string sTop10Names = ADBManager.mUOM_NETExtensions_Strings.e_Join(from P in top10
																				  select P.Name, Constants.vbCrLf).e_CheckNullOrWhiteSpace();
				if (sTop10Names.e_IsNOTNullOrWhiteSpace())
				{
					sMsg += Constants.vbCrLf + Constants.vbCrLf;
					if (aUnknownPackages.Count() > top10.Count())
					{
						sMsg += "Первые {0}:".e_Format(C_TOP10) + Constants.vbCrLf + Constants.vbCrLf + sTop10Names + Constants.vbCrLf + "...";
					}
					else
					{
						sMsg += sTop10Names;
						// sMsg &= "Неизвестные:" & vbCrLf & sTop10Names
					}
				}
			}

			if (!ADBManager.mUOM_NETExtensions_Controls.ExtCtl_MsgBox_IsYes(sMsg))
				return;
			ADBManager.mKnownPackages.g_KnownPachagesDatabase.AddRange(lNewPackages, false);
			this.RefreshPackagesInfoInKnownPackagesDatabase();
		}

		private void Packages_SaveSateSnapshot()
		{
			if (!this._aAllPackages.Any())
				return;
			if (!this._aAllPackages.Any())
				return;
			var aPackages = (from Li in this._aAllPackages
							 select Li.Package).ToArray();
			string sPath = this.ExtCtl_OpenSaveFileDialog();
			if (sPath.e_IsNullOrWhiteSpace())
				return;
			aPackages.ExtSer_SerializeXML(sPath);
		}

		private void _Load(object sender, EventArgs e) => _Load();
		private void Package_ShowDetailsForFirstSelectedPackage(object sender, EventArgs e) => Package_ShowDetailsForFirstSelectedPackage();
		private void Package_DownloadAPKToDesktop(object sender, EventArgs e) => Package_DownloadAPKToDesktop();
		private void Package_InstallFromPC(object sender, EventArgs e) => Package_InstallFromPC();
		private void AddUnKnownPackagesToDatabase(object sender, EventArgs e) => AddUnKnownPackagesToDatabase();
		private void Packages_SaveSateSnapshot(object sender, EventArgs e) => Packages_SaveSateSnapshot();
		private void _ShowOnlyUnknown_CheckedChanged(object sender, EventArgs e) => _ShowOnlyUnknown_CheckedChanged();
		private void GetRoot(object sender, EventArgs e) => GetRoot();
		private void KnownPackagesDatabase_Show(object sender, EventArgs e) => KnownPackagesDatabase_Show();
		private void ReloadAll(object sender, EventArgs e) => ReloadAll();

		*/




#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
		private async void GetRoot()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		{
			throw new NotImplementedException();
			/*

			// Dim aSelected = Me.lvwPackages.ExtCtl_SelectedItemsAs(Of ADBPackageListViewItem).ToArray
			// If (Not aSelected.Any) Then Return

			this.UseWaitCursor = true;
			this.tbPackage.Enabled = false;
			this.lvwPackages.Enabled = false;
			try
			{
				await ADBManager.mMain.gADB.GetRoot();
			}
			catch (Exception ex)
			{
				Interaction.MsgBox(ex.Message, MsgBoxStyle.Critical);
			}
			finally
			{
				this.UseWaitCursor = false;
				this.tbPackage.Enabled = true;
				this.lvwPackages.Enabled = true;
			}
			 */
		}
	}


}

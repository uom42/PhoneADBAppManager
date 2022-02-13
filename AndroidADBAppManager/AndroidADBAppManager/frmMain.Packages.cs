
using AndroidADBAppManager.ADB;

using common;

using static AndroidADBAppManager.ADB.Package;

#nullable enable

namespace AndroidADBAppManager
{
	partial class frmMain
	{
		private const int C_PACHAGES_LIST_ICON_SIZE = 16;

		private readonly ListViewGroup _lvgpPackages_System = new("Систмные", "Систмные");
		private readonly ListViewGroup _lvgpPackages_3rd = new("Сторонние", "Сторонние");
		private readonly ListViewGroup _lvgpPackages_Other = new("Прочие", "Прочие");


		private PackageListViewItem[] _aAllPackages = Array.Empty<PackageListViewItem>();
		private bool _ListUpdated = false;

		private void Package_Init()
		{
			txtPackageFilter.AttachDelayedFilter(this.Package_FillWithFilter);
			_aPackages_UIControls = new Control[] { tbPackage, splitPackages_1 };
			{
				ImageList iml = imlPackagelIcons;
				iml.Images.Clear();
				iml.ColorDepth = ColorDepth.Depth24Bit;
				iml.ImageSize = new Size(C_PACHAGES_LIST_ICON_SIZE, C_PACHAGES_LIST_ICON_SIZE);
				//iml.Images.Add(ADBManager.mMain.C_ICON_APPLICATION, SystemIcons.Application);
				//iml.Images.Add(ADBManager.mMain.C_ICON_CRITICAL, SystemIcons.Exclamation);
				//iml.Images.Add(ADBManager.mMain.C_ICON_ERROR, SystemIcons.Error);
			}

			{
				ListView lvw = lvwPackages;
				//lvw.SmallImageList = this.imlPackagelIcons;
				//lvw.LargeImageList = this.imlPackagelIcons;
				lvw.ClearItemsAndGroups();
				lvw.Groups.AddRange(new[] { _lvgpPackages_System, _lvgpPackages_Other, _lvgpPackages_3rd });


				lvw.SetAllGroupsState();
				_lvgpPackages_Other.CollapsedState = ListViewGroupCollapsedState.Expanded;
				_lvgpPackages_3rd.CollapsedState = ListViewGroupCollapsedState.Expanded;

				lvw.SelectedIndexChanged += (_, _) => Package_ShowDetailsForFirstSelectedPackage();
			}
			pbPackage_Progress.Visible = false;
		}


		private async Task Package_Reload()
		{
			_aAllPackages = Array.Empty<PackageListViewItem>();
			_ListUpdated = true;
			{
				ListView lvw = this.lvwPackages;
				//lvw.EmptyText = "Получаем список пакетов с устройства {0}...".e_Format(ADBManager.mMain.gPhoneDevice.Product.e_CheckNullOrWhiteSpace());
				lvw.ClearItems();
			}

			Package_ShowDetailsForFirstSelectedPackage();
			{
				pbPackage_Progress.Style = ProgressBarStyle.Marquee;
				pbPackage_Progress.Visible = true;
			}

			UseWaitCursor = true;
			try
			{
				var aPackages = await ADB.GetPackages();
				_aAllPackages = aPackages.Select(p => new PackageListViewItem(p)).ToArray();
				_ListUpdated = true;
				Package_FillWithFilter();
			}
			finally
			{
				UseWaitCursor = false;
				Package_UpdateButtonsState();
				pbPackage_Progress.Visible = false;
			}
			await Package_GetDetails();
		}


		private void Package_FillWithFilter()
		{
			try
			{
				ListView lvw = lvwPackages;

				/*
				if (!_aAllPackages.Any()) 
					lvw.EmptyText = "Не найдено пакетов на устройстве {0}".e_Format(ADBManager.mMain.gPhoneDevice.Product.e_CheckNullOrWhiteSpace());
				else
					withBlock.EmptyText = "Фильтруем пакеты...";
				 */

				bool bShowOnlyUnknownPackages = chkPackageFilter_ShowOnlyUnknown.Checked;
				lvw.ExecOnLockedUpdate(() =>
				{
					lvw.Items.Clear();
					Package_ShowDetailsForFirstSelectedPackage();
					try
					{
						PackageListViewItem[] aFiltered = _aAllPackages;
						if (!aFiltered.Any()) return;

						#region Фильтр: Только неизвестные пакеты
						// Dim aAllInGrid = Me.lvwPackages.ExtCtl_ItemsAs(Of ADBPackageListViewItem)
						if (bShowOnlyUnknownPackages)
						{
							/*
							aFiltered = (from Li in aFiltered
										 where Li.Package.KnownPackageData is null
										 select Li).ToArray();
							 */
						}
						#endregion


						#region Фильтр: Текстовый
						{
							string sFilter = this.txtPackageFilter.Text.Trim().ToLower();
							if (!sFilter.IsNullOrWhiteSpace())
							{
								aFiltered = aFiltered.Where(li => li.CheckFilter(sFilter)).ToArray();
								//if (!aFiltered.Any()) lvw.EmptyText = "Нет элементов для фильтра '" + sFilter + "'";
							}
						}
						#endregion

						aFiltered.ToList().ForEach(li =>
						{
							ListViewGroup lvg = _lvgpPackages_Other;
							if (li.Package.State.HasFlag(E_PACKAGE_STATES.System))
							{
								lvg = _lvgpPackages_System;
							}
							else if (li.Package.State.HasFlag(E_PACKAGE_STATES.ThirdParty))
							{
								lvg = _lvgpPackages_3rd;
							}
							li.Group = lvg;
						});
						lvw.Items.AddRange(aFiltered);
					}
					finally { lvw.SetGroupsTitlesFast(); }
				}, true);
			}
			finally { Package_UpdateButtonsState(); }
		}


		private async Task Package_GetDetails()
		{
			var aLI = _aAllPackages;
			{
				var withBlock = pbPackage_Progress;
				pbPackage_Progress.Style = ProgressBarStyle.Continuous;
				pbPackage_Progress.Maximum = aLI.Count();
				pbPackage_Progress.Visible = true;
			}

			try
			{
				_ListUpdated = false;
				//ADBManager.NLogExtansions.LogDebug("Package_GetDetails Started");
				foreach (var LI in aLI)
				{
					pbPackage_Progress.Increment(1);
					await LI.Package.GetDumpsysInfo();
					if (_ListUpdated) return;
					LI.UpdateLI();
				}
				//ADBManager.NLogExtansions.LogDebug("Package_GetDetails Finished!");
			}
			finally { pbPackage_Progress.Visible = false; }
		}

		private void Package_UpdateButtonsState()
		{
			/*

			var aAllInGrid = this.lvwPackages.ExtCtl_ItemsAs<ADBPackageListViewItem>();
			// Dim aSelected = Me.lvwPackages.ExtCtl_SelectedItemsAs(Of ADBPackageListViewItem)
			var aSelected = (from Li in aAllInGrid
							 where Li.Selected
							 select Li).ToArray();
			var aButtons = new[] { this.cmdPackage_Disable, this.cmdPackage_Enable, this.cmdPackage_Istall, this.cmdPackage_UnIstall, this.cmdPackage_Download };
			aSelected.Any().IIFEnableControl(aButtons);
			var aUnknownPackages = (from Li in aAllInGrid
									where Li.Package.KnownPackageData is null
									select Li).ToArray();
			this.cmdAddUnKnownPackagesToDatabase.Enabled = aUnknownPackages.Any();
			 */
		}


		private void _ShowOnlyUnknown_CheckedChanged()
		{
			Package_FillWithFilter();
		}








		private void KnownPackagesDatabase_Show()
		{
			//ADBManager.mKnownPackages.g_KnownPachagesDatabase.Edit(this);
			//RefreshPackagesInfoInKnownPackagesDatabase();
		}

		private void RefreshPackagesInfoInKnownPackagesDatabase()
		{
			/*
			_aAllPackages.ToList().ForEach(LI =>
			{
				var PKG = LI.Package;
				PKG.GetInfoFromKnownPackagesDatabase();
				LI.UpdateLI();
			});
			Package_FillWithFilter();
			 */
		}


	}
}

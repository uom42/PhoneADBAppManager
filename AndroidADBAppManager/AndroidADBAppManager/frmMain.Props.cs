using AndroidADBAppManager.ADB;

using common;

using uom.Extensions;

#nullable enable

namespace AndroidADBAppManager
{
	partial class frmMain
	{
		private const string GROUP_COMMON = "Общие";


		private PropertyListViewItem[] _aAllProps = Array.Empty<PropertyListViewItem>();
		private ListViewGroup _lvgpProps_0 = new(GROUP_COMMON, GROUP_COMMON);

		private void Props_Init()
		{
			txtPropsList_Filter.e_AttachDelayedFilter(Property_FillWithFilter);
			{
				var lvw = lvwPropsList;
				lvw.e_ClearItemsAndGroups();
				lvw.Groups.AddRange(new[] { _lvgpProps_0 });
				// .SetAllGroupsState();
			}
		}

		private async Task Props_Reload()
		{
			_aAllProps = Array.Empty<PropertyListViewItem>();
			{
				var withBlock = this.lvwPropsList;
				//lvwPropsList.EmptyText = "Получаем список свойств...";
				lvwPropsList.Items.Clear();
			}

			UseWaitCursor = true;
			try
			{
				Property[] aProps = await ADB.GetProps();
				_aAllProps = aProps.Select(p => new PropertyListViewItem(p)).ToArray();
				Property_FillWithFilter();
			}
			finally
			{
				UseWaitCursor = false;
				// Call Me.Package_UpdateButtonsState()
			}
		}

		private void Property_FillWithFilter()
		{
			if (!_aAllProps.Any())
			{
				//lvwPropsList.EmptyText = "Не найдено свойств.";
			}
			else
			{
				//lvwPropsList.EmptyText = "Фильтруем свойства...";
			}

			lvwPropsList.e_runOnLockedUpdate(() =>
			{
				lvwPropsList.e_ClearItems();
				var aFiltered = _aAllProps;
				try
				{
					if (!aFiltered.Any()) return;

					string sFilter = txtPropsList_Filter.Text.Trim().ToLower();
					if (!sFilter.e_IsNullOrWhiteSpace())
					{
						aFiltered = aFiltered.Where(p => p.CheckFilter(sFilter)).ToArray();
						//if (!aFiltered.Any()) withBlock.EmptyText = "Нет элементов для фильтра '" + sFilter + "'";
					}

					foreach (var P in aFiltered)
					{
						ListViewGroup lvg = _lvgpProps_0;
						string sKey = P.Property.Name;
						if (sKey.Contains('.'))
						{
							string groupHeader = Path.GetFileNameWithoutExtension(sKey);
							var crg = lvwPropsList.e_GroupsCreateGroupByKey(groupHeader, null, ListViewGroupCollapsedState.Expanded);
							lvg = crg.Group;
						}
						P.Group = lvg;
					}
					lvwPropsList.Items.AddRange(aFiltered);
				}
				finally { lvwPropsList.e_SetGroupsTitlesFast(); }
			}, true);
		}
	}
}

#nullable enable

using common;

using uom.Extensions;

namespace AndroidADBAppManager.ADB
{
	internal class PropertyListViewItem : ListViewItem
	{
		public readonly Property Property;

		public PropertyListViewItem(Property p) : base(p.Name)
		{
			Property = p;
			string sFullName = p.Name;
			string sShortName = sFullName;
			if (sShortName.Contains(".")) sShortName = Path.GetExtension(sShortName).Substring(1).Trim();

			this.AddFakeSubitems(3);
			this.UpdateTexts(sShortName, p.Value, sFullName);

			// Call Me.UpdateLI()
		}

		// Public Sub UpdateLI()
		// 'Dim clrBack = SystemColors.Window ' Color.LightGray

		// 'Dim sIconKey = C_ICON_APPLICATION
		// 'If ((Me.Property.State And ADBProperty.PACKAGE_STATES.SafeToDelete) <> 0) Then
		// 'End If
		// 'If bHasDetails Then clrBack = Color.LightGreen
		// 'clrBack = Color.LightGreen
		// 'Else
		// 'sIconKey = C_ICON_CRITICAL
		// 'End If
		// 'Call .Images.Add(C_ICON_APPLICATION, SystemIcons.Application)
		// 'Call .Images.Add(C_ICON_CRITICAL, SystemIcons.Exclamation)
		// 'Call .Images.Add(C_ICON_ERROR, SystemIcons.Error)


		// 'If (Me.Property.State And ADBProperty.PACKAGE_STATES.Disabled) <> 0 Then clrBack = Color.LightGray
		// 'If (Me.Property.State And ADBProperty.PACKAGE_STATES.Uninstalled) <> 0 Then clrBack = Color.Salmon '(ARGB = &HFFFA8072) (H;S;B; = 6,176474; 0,9315069; 0,7137255;)

		// 'Me.BackColor = clrBack
		// 'Me.ImageKey = sIconKey
		// End Sub


		public bool CheckFilter(string sFilter)
		{
			bool bCheck = Property.Name.ToLower().Contains(sFilter) || Property.Value.ToLower().Contains(sFilter);
			return bCheck;
		}
	}
}

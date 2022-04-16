using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable enable

namespace common
{

	[DebuggerStepThrough]
	public static class ExtensionsControls
	{

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetVistaCueBanner(this TextBox ctl, string? BannerText = null)
		{
			ArgumentNullException.ThrowIfNull(ctl);
			ctl.ExecWhenHandleReady(tb => WinAPI.Windows.SendMessage(
				tb.Handle,
				WinAPI.Windows.WindowMessages.EM_SETCUEBANNER,
				0,
				BannerText));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ExecWhenHandleReady<T>(this T ctl, Action<Control> HandleReadyAction) where T : Control
		{
			_ = ctl ?? throw new ArgumentNullException(nameof(ctl));
			if (ctl.Disposing || ctl.IsDisposed) return;

			if (ctl.IsHandleCreated)    //Control handle already Exist, run immediate
				HandleReadyAction?.Invoke(ctl);
			else            //Delay action when handle will be ready...
				ctl.HandleCreated += (s, e) => HandleReadyAction?.Invoke((T)s);
		}


		/// <summary>
		/// Usually used when you need to do an action with a slight delay after exiting the current method. 
		/// For example, if some data will be ready only after exiting the control event handler processing branch
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ExecDelayed(this Control ctl, Action delayedAction, int DelayInterval = 100)
		{
			ArgumentNullException.ThrowIfNull(ctl);
			ArgumentNullException.ThrowIfNull(delayedAction);

			//Use 'System.Windows.Forms.Timer' that uses some thread with caller to raise events
			System.Windows.Forms.Timer tmrDelay = new()
			{
				Interval = DelayInterval,
				Enabled = false //do not start timer untill we finish it's setup
			};
			tmrDelay.Tick += (_, _) =>
			{
				//first stop and dispose our timer, to avoid double execution
				tmrDelay.Stop();
				tmrDelay.Dispose();

				//Now start action incontrols UI thread
				ctl.Invoke(delayedAction);
			};

			//Start delay timer
			tmrDelay.Start();
		}


		#region AttachDelayedFilter

		private const int DEFAULT_TEXT_EDIT_DELAY = 1000;
		private const string DEFAULT_FILTER_CUEBANNER = "Filter";


		/// <summary>
		/// Attaches a deferred text change event handler that makes it possible to react to text changes with some delay, 
		/// allowing the user to correct erroneous input, 
		/// or complete input, rather than reacting immediately to each letter.
		/// </summary>
		/// <param name="OnTextChangedCallBack">TextChanged Handler</param>
		/// <param name="TextEditiDelay">Delay (ms.) during which repeated input will not call the handler</param>
		/// <param name="VistaCueBanner">Vista cueabanner text</param>
		/// <param name="SetBackColorAsSystemTipColor">Sets the background color for textbox to Systemcolors.Info</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AttachDelayedFilter(
			this TextBox txtCtl,
			Action<string> OnTextChangedCallBack,
			int TextEditiDelay = DEFAULT_TEXT_EDIT_DELAY,
			string VistaCueBanner = DEFAULT_FILTER_CUEBANNER,
			bool SetBackColorAsSystemTipColor = true)
		{
			var TMR = new System.Windows.Forms.Timer() { Interval = TextEditiDelay };
			txtCtl.Tag = TMR; //Сохраняем ссылку на таймер хоть где-то, чтобы GC его не грохнул.

			if (!string.IsNullOrWhiteSpace(VistaCueBanner))
			{
				txtCtl.SetVistaCueBanner(VistaCueBanner);
			}

			if (SetBackColorAsSystemTipColor)
			{
				txtCtl.BackColor = SystemColors.Info;
			}

			TMR.Tick += (s, e) =>
			{
				TMR.Stop(); //Останавливаем таймер
				var sNewText = txtCtl.Text;
				OnTextChangedCallBack.Invoke(sNewText);
			};
			txtCtl.TextChanged += (s, e) =>
			{
				//Restart timer...
				TMR.Stop();
				TMR.Start();
			};
		}

		/// <inheritdoc cref="AttachDelayedFilter" />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AttachDelayedFilter(
			this TextBox txtCtl,
			Action TextChangedCallBack,
			int iDelay_ms = DEFAULT_TEXT_EDIT_DELAY,
			string VistaCueBanner = DEFAULT_FILTER_CUEBANNER,
			bool SetBackColorAsSystemTipColor = true)
		{
			Action<string> DummyCallback = new((s) => TextChangedCallBack.Invoke());
			txtCtl.AttachDelayedFilter(
				DummyCallback,
				iDelay_ms,
				VistaCueBanner,
				SetBackColorAsSystemTipColor);
		}

		#endregion


		/// <summary>ThreadSafe excute action in control UI thread
		/// Any errors will be ignored!</summary>
		/// <param name="ctl">Control in which UI thread action executes</param>
		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ExecInUIThread(
			this Control ctl,
			Action? a,
			bool useBeginInvoke = false)
		{
			ArgumentNullException.ThrowIfNull(ctl);
			if (a == null) return;

			try
			{
				if (!ctl.IsHandleCreated || ctl.IsDisposed) return;
				if (useBeginInvoke) { ctl.BeginInvoke(a); return; }
				if (ctl.InvokeRequired) { ctl.Invoke(a); return; }
				a.Invoke();
			}
			catch (ObjectDisposedException) { }//just ignore			
		}
	}

	[DebuggerStepThrough]
	public static class ExtensionsListview
	{
		/*	 


<DebuggerNonUserCode, DebuggerStepThrough>
<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
Friend Sub  AddSubitems(ByVal li As Global.System.Windows.Forms.ListViewItem,
							  ByVal ParamArray aSubItemsText() As String)

	If (li Is Nothing) Then Return

	For Each S In aSubItemsText
		Call li.SubItems.Add(S)
	Next
End Sub
	 */


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AutoSizeColumns(this ListView? lvw)
		{
			if (lvw == null) return;

			if (lvw.Items.Count > 0)
				lvw.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			else
				lvw.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
		}

		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ClearItemsAndGroups(
			this ListView? lvw,
			bool autoSizeColumns = true,
			bool clearGroups = true,
			bool clearColumns = false)
		{
			lvw?.ExecOnLockedUpdate(() =>
			{
				lvw?.Items.Clear();
				if (clearGroups) lvw?.Groups.Clear();
				if (clearColumns) lvw?.Columns.Clear();
			}, autoSizeColumns);
		}


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ClearItems(this ListView lvw, bool autoSizeColumns = false)
			=> lvw.ClearItemsAndGroups(autoSizeColumns, false);


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AddFakeSubitems(
			this ListViewItem? li,
			int ListViewColumnsCount,
			string fakeText = "")
		{
			if (li != null) for (int i = 0; i < ListViewColumnsCount; i++) li.SubItems.Add(fakeText);
		}


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AddFakeSubitems(
			this ListViewItem? li,
			ListView? lvw = null,
			string fakeText = "")
		{
			if (li == null) return;

			lvw ??= li.ListView;
			ArgumentNullException.ThrowIfNull(lvw);
			li?.AddFakeSubitems(lvw.Columns.Count, fakeText);
		}


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void UpdateTexts(
			 this ListViewItem? li,
			 int startIndex,
			 params string?[] aSubItemsText)
		{
			if (li == null || !aSubItemsText.Any()) return;

			li.ListView.ExecOnLockedUpdate(() =>
			{
				int i = 0;
				aSubItemsText.ToList().ForEach(text =>
				{
					if (!text.IsNullOrEmpty()) li.SubItems[(startIndex + i)].Text = text;
					i++;
				});
			});
		}


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void UpdateTexts(this ListViewItem? li, params string[] aSubItemsText)
			=> UpdateTexts(li, 0, aSubItemsText);


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<ListViewGroup> GroupsAsIEnumerable(this ListView lvw)
			=> lvw.Groups.Cast<ListViewGroup>();


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static (ListViewGroup Group, bool Created) GroupsCreateGroupByKey(
			this ListView lvw,
			string key,
			string? header = null,
			ListViewGroupCollapsedState newGroupState = ListViewGroupCollapsedState.Collapsed,
			Action<ListViewGroup>? onNewGroup = null
			)
		{
			ListViewGroup? grp = lvw.GroupsAsIEnumerable()?.Where(g => (g.Name == key)).FirstOrDefault();
			bool exist = (grp != null);
			if (!exist)
			{
				grp = new ListViewGroup(key, header ?? key);
				lvw.Groups.Add(grp);
				onNewGroup?.Invoke(grp);
				grp.CollapsedState = newGroupState;
			}
			return (grp!, !exist);
		}


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static (ListViewGroup Group, bool Created) GroupsCreateGroupByKey(
			Dictionary<string, ListViewGroup> dicGroups,
			string key,
			string? header = null,
			Action<ListViewGroup>? onNewGroup = null)
		{
			bool exist = dicGroups.TryGetValue(key, out ListViewGroup? grp);
			if (!exist)
			{
				header ??= key;
				grp = new ListViewGroup(key, header);
				dicGroups.Add(key, grp);
				onNewGroup?.Invoke(grp);
			}
			return (grp!, !exist);
		}


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetGroupsTitlesFast(
			this ListView? lvw,
			Func<ListViewGroup, string>? getGroupHeader = null)
			=> lvw?.GroupsAsIEnumerable().ToList()
			.ForEach(g =>
			 {
				 string sTitle = g.Name ?? "";
				 if (getGroupHeader != null)
					 sTitle = getGroupHeader.Invoke(g);
				 else
					 sTitle = $"{sTitle} ({g.Items.Count:N0})".Trim();

				 if (!string.IsNullOrWhiteSpace(sTitle)) g.Header = sTitle;
			 });


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetAllGroupsState(
			this ListView? lvw,
			ListViewGroupCollapsedState state = ListViewGroupCollapsedState.Collapsed)
				=> lvw?.GroupsAsIEnumerable().ToList().ForEach(g => g.CollapsedState = state);


		///<summary>MT Safe!!!</summary>
		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ExecOnLockedUpdate(
			this ListView? lvw,
			Action? a,
			bool autoSizeColumns = false,
			bool fastUpdateGroupHeaders = false)
		{
			if (a == null) return;

			Action a2 = new(() =>
			{
				lvw?.BeginUpdate();
				try { a.Invoke(); }
				finally
				{
					if (autoSizeColumns) lvw?.AutoSizeColumns();
					if (fastUpdateGroupHeaders) lvw?.SetGroupsTitlesFast();
					lvw?.EndUpdate();
				}
			});

			if (lvw != null && lvw.InvokeRequired)
				lvw.ExecInUIThread(a2);
			else
				a2.Invoke();
		}


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task<T?> ExecOnLockedUpdateAsync<T>(
			this ListView lvw,
			Task<T?> tsk,
			bool autoSizeColumns = false,
			bool fastUpdateGroupHeaders = false) where T : class
		{
			ArgumentNullException.ThrowIfNull(tsk);

			lvw?.BeginUpdate();
			try
			{
				tsk.Start();
				T? ret = await tsk;
				return ret;
			}
			finally
			{
				if (autoSizeColumns) lvw?.AutoSizeColumns();
				if (fastUpdateGroupHeaders) lvw?.SetGroupsTitlesFast();
				lvw?.EndUpdate();
			}
		}


		//************************************** OLD









		public static void SelectFirstItem(this ListView lvw)
		{
			var first = lvw.ItemsAsIEnumerable().FirstOrDefault();
			if (first == default) return;

			first.Selected = true;
			first.Focused = true;
			first.EnsureVisible();
		}


		public static void AddSubitems(this ListViewItem? li, params string[] subitems)
			=> subitems?.ToList().ForEach(s => li?.SubItems.Add(s));


		public static async void FlashAsync(this ListViewItem? li, int flashCount = 10)
		{
			if (li == null) return;

			Color clrFore = li.ForeColor; Color clrBack = li.BackColor;
			try
			{
				for (int i = 1, loopTo = flashCount * 2; i <= loopTo; i++)
				{
					var clrTemp = li.ForeColor;
					li.ForeColor = li.BackColor;
					li.BackColor = clrTemp;
					await Task.Delay(100);
				}
			}
			finally //Restore original colors
			{ li.ForeColor = clrFore; li.BackColor = clrBack; }
		}


		public static void ItemsRemoveRange(
			this ListView? lvw,
			IEnumerable<ListViewItem> ItemsToRemove,
			bool aAutoSizeColumnsAtFinish = false)
			=> lvw?.ExecOnLockedUpdate(() => lvw?.Items.ItemsRemoveRange(ItemsToRemove), aAutoSizeColumnsAtFinish);

		public static void ItemsRemoveRange(
			this ListView.ListViewItemCollection liC,
			IEnumerable<ListViewItem> ItemsToRemove)
				=> ItemsToRemove.ToList().ForEach(li => liC.Remove(li));


		#region ItemsAsIEnumerable

		public static IEnumerable<ListViewItem> ItemsAsIEnumerable(this ListView lvw) => lvw.Items.Cast<ListViewItem>();
		public static IEnumerable<ListViewItem> ItemsAsIEnumerable(this ListViewGroup G) => G.Items.Cast<ListViewItem>();
		public static IEnumerable<ListViewItem> SelectedItemsAsIEnumerable(this ListView lvw) => lvw.SelectedItems.Cast<ListViewItem>();
		public static IEnumerable<ListViewItem> CheckedItemsAsIEnumerable(this ListView lvw) => lvw.CheckedItems.Cast<ListViewItem>();

		#endregion


		#region ItemsAndTags

		public static T? TagAs<T>(this ListViewGroup lvg) => (T?)lvg.Tag;
		public static T? TagAs<T>(this ListViewItem li) => (T?)li.Tag;
		public static T? TagAs<T>(this TreeNode nd) => (T?)nd.Tag;
		public static T? TagAs<T>(this Control ctl) => (T?)ctl.Tag;

		public static int ItemsCount_Selected(this ListView lvw) => lvw.SelectedItems.Count;
		public static int ItemsCount_Checked(this ListView lvw) => lvw.CheckedItems.Count;
		public static int ItemsCount(this ListView lvw) => lvw.Items.Count;

		#region ItemsAndTags2

		public sealed class ListViewItemAndTag<T>
		{
			public ListViewItem Item { get; set; }

			public ListViewItemAndTag(ListViewItem li) : base() { Item = li; }

			public T? Tag => Item.TagAs<T>();
		}


		public static IEnumerable<T?> Tags<T>(this IEnumerable<ListViewItemAndTag<T>> A)
			=> A.Select(li => li.Tag);


		public static IEnumerable<ListViewItem> Items<T>(this IEnumerable<ListViewItemAndTag<T>> A)
			=> A.Select(li => li.Item);


		public static IEnumerable<ListViewItemAndTag<T>> ItemsAndTags<T>(this IEnumerable<ListViewItem> A)
			=> A.Select(li => new ListViewItemAndTag<T>(li));


		public static IEnumerable<ListViewItemAndTag<T>> ItemsAndTags<T>(this ListViewGroup G)
			=> ItemsAndTags<T>(G.ItemsAsIEnumerable());


		public static IEnumerable<ListViewItemAndTag<T>> ItemsAndTags<T>(this ListView lvw)
			=> ItemsAndTags<T>(lvw.ItemsAsIEnumerable());


		public static IEnumerable<ListViewItemAndTag<T>> SelectedItemsAndTags<T>(this ListView lvw)
			=> ItemsAndTags<T>(lvw.SelectedItemsAsIEnumerable());


		public static IEnumerable<ListViewItemAndTag<T>> CheckedItemsAndTags<T>(this ListView lvw)
			=> ItemsAndTags<T>(lvw.CheckedItemsAsIEnumerable());


		#endregion

		#region ItemsAs

		public static IEnumerable<T> ItemsAs<T>(this ListViewGroup lvg) where T : ListViewItem => lvg.Items.Cast<T>();
		public static IEnumerable<T> ItemsAs<T>(this ListView lvw) where T : ListViewItem => lvw.Items.Cast<T>();
		public static IEnumerable<T> SelectedItemsAs<T>(this ListView lvw) where T : ListViewItem => lvw.SelectedItems.Cast<T>();
		public static IEnumerable<T> CheckedItemsAs<T>(this ListView lvw) where T : ListViewItem => lvw.CheckedItems.Cast<T>();

		#endregion

		#endregion


		public static IEnumerable<ColumnHeader> ColumnsAsIEnumerable(this ListView lvw) => lvw.Columns.Cast<ColumnHeader>();

		public static ListViewGroup? GroupsGetByKey(this ListView lvw, string? key)
			=> lvw.GroupsAsIEnumerable().Where(grp => (grp.Name ?? "") == (key ?? "")).FirstOrDefault();


		/// <summary>Предыдущий элемент (Index меньше на 1)</summary>
		public static ListViewItem? Previous(this ListViewItem li)
			=> (li.Index <= 0) ? null : li.ListView.Items[li.Index - 1];

		/// <summary>Предыдущий элемент в той же группе (Index меньше на 1)</summary>
		public static ListViewItem? PreviousInGroup(this ListViewItem li)
		{
			var liPrev = li.Previous();
			if (liPrev != null && object.ReferenceEquals(liPrev.Group, li.Group)) return liPrev;
			return null;
		}


		/// <summary>Next элемент (Index +1)</summary>
		public static ListViewItem? Next(this ListViewItem li)
			=> (li.Index >= (li.ListView.Items.Count - 1)) ? null : li.ListView.Items[li.Index + 1];


		/// <summary>Next элемент в той же группе (Index +1)</summary>
		public static ListViewItem? NextInGroup(this ListViewItem li)
		{
			var liNext = li.Next();
			if (liNext != null && object.ReferenceEquals(liNext.Group, li.Group)) return liNext;
			return null;
		}
	}

}

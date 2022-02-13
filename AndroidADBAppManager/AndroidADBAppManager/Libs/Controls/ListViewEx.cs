using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Controls
{
	internal class ListViewEx : ListView
	{
		public ListViewEx() : base()
		{
			SetStyle(
				ControlStyles.ResizeRedraw
				| ControlStyles.DoubleBuffer
				| ControlStyles.OptimizedDoubleBuffer
				| ControlStyles.AllPaintingInWmPaint
				, true);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			//e.Graphics.DrawString("FUCK!", Font, Brushes.Red, ClientRectangle);
		}
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			//base.OnPaintBackground(pevent);
		}
	}
}

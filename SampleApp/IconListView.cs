using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using TsudaKageyu;

namespace SampleApp
{
    internal class IconListView : ListView
    {
        public IconListView() : base()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw, true);
        }
    }

    internal class IconListViewItem : ListViewItem
    {
        public Bitmap Bitmap { get; set; }
        public int BitCount { get; set; }
    }
}

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

            OwnerDraw = true;
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            base.OnDrawItem(e);

            var item = e.Item as IconListViewItem;

            // Draw item

            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.Clip = new Region(e.Bounds);

            if (e.Item.Selected)
                e.Graphics.FillRectangle(SystemBrushes.MenuHighlight, e.Bounds);

            int w = Math.Min(128, item.Bitmap.Width);
            int h = Math.Min(128, item.Bitmap.Height);

            int x = e.Bounds.X + (e.Bounds.Width - w) / 2;
            int y = e.Bounds.Y + (e.Bounds.Height - h) / 2;
            var dstRect = new Rectangle(x, y, w, h);
            var srcRect = new Rectangle(Point.Empty, item.Bitmap.Size);

            e.Graphics.DrawImage(item.Bitmap, dstRect, srcRect, GraphicsUnit.Pixel);

            var textRect = new Rectangle(
                e.Bounds.Left, e.Bounds.Bottom - Font.Height - 4,
                e.Bounds.Width, Font.Height + 2);
            TextRenderer.DrawText(e.Graphics, item.ToolTipText, Font, textRect, ForeColor);

            e.Graphics.Clip = new Region();
            e.Graphics.DrawRectangle(SystemPens.ControlLight, e.Bounds);
        }
    }

    internal class IconListViewItem : ListViewItem
    {
        public Bitmap Bitmap { get; set; }
        public int BitCount { get; set; }
    }
}

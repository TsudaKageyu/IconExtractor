using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using TsudaKageyu;

namespace SampleApp
{
    public partial class Form1 : Form
    {
        private class IconListViewItem : ListViewItem
        {
            public Icon Icon { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private int GetIconBitDepth(Icon icon)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            using (var stream = new MemoryStream())
            {
                icon.Save(stream);
                return BitConverter.ToInt16(stream.ToArray(), 12);
            }
        }

        private void ClearAllIcons()
        {
            foreach (var item in lvwIcons.Items)
                ((IconListViewItem)item).Icon.Dispose();

            lvwIcons.Items.Clear();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClearAllIcons();
        }

        private void btnPickFile_Click(object sender, EventArgs e)
        {
            var result = iconPickerDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                var fileName = iconPickerDialog.FileName;
                var index = iconPickerDialog.IconIndex;

                txtFileName.Text = String.Format("{0}, {1}", fileName, index);

                Icon icon = null;
                Icon[] splitIcons = null;
                try
                {
                    if (Path.GetExtension(iconPickerDialog.FileName).ToLower() == ".ico")
                    {
                        icon = new Icon(iconPickerDialog.FileName);
                    }
                    else
                    {
                        var extractor = new IconExtractor(fileName);
                        icon = extractor.GetIcon(index);
                    }

                    splitIcons = IconExtractor.SplitIcon(icon);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update icons.

                Icon = icon;
                icon.Dispose();

                lvwIcons.BeginUpdate();
                ClearAllIcons();

                foreach (var i in splitIcons)
                {
                    var item = new IconListViewItem();
                    item.ToolTipText = String.Format(
                        "{0} x {1}, {2}bits", i.Width, i.Height, GetIconBitDepth(i));
                    item.Icon = i;

                    lvwIcons.Items.Add(item);
                }

                lvwIcons.EndUpdate();

            }
        }

        private void lvwIcons_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            var item = e.Item as IconListViewItem;

            // Draw item

            e.DrawBackground();

            int w, h;
            if (item.Icon.Width <= 128 && item.Icon.Height <= 128)
            {
                w = item.Icon.Width;
                h = item.Icon.Height;
            }
            else
            {
                w = 128;
                h = 128;
            }

            int x = e.Bounds.X + (e.Bounds.Width - w) / 2;
            int y = e.Bounds.Y + (e.Bounds.Height - h) / 2;
            var dstRect = new Rectangle(x, y, w, h);
            var srcRect = new Rectangle(Point.Empty, item.Icon.Size);

            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.Clip = new Region(e.Bounds);

            using (var bmp = IconExtractor.IconToBitmap(item.Icon))
            {
                e.Graphics.DrawImage(bmp, dstRect, srcRect, GraphicsUnit.Pixel);
            }

            var textRect = new Rectangle(
                e.Bounds.Left, e.Bounds.Bottom - Font.Height - 4,
                e.Bounds.Width, Font.Height + 2);
            TextRenderer.DrawText(e.Graphics, item.ToolTipText, Font, textRect, ForeColor);

            e.Graphics.Clip = new Region();
            e.Graphics.DrawRectangle(SystemPens.ControlLight, e.Bounds);
        }
    }
}

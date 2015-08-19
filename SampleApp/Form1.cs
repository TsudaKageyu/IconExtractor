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
            public Bitmap Bitmap { get; set; }
            public int BitCount { get; set; }
        }

        IconExtractor m_iconExtractor = null;
        int m_iconIndex = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void ClearAllIcons()
        {
            foreach (var item in lvwIcons.Items)
                ((IconListViewItem)item).Bitmap.Dispose();

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
                m_iconIndex = iconPickerDialog.IconIndex;

                Icon icon = null;
                Icon[] splitIcons = null;
                try
                {
                    if (Path.GetExtension(iconPickerDialog.FileName).ToLower() == ".ico")
                    {
                        m_iconExtractor = null;
                        icon = new Icon(iconPickerDialog.FileName);
                    }
                    else
                    {
                        m_iconExtractor = new IconExtractor(fileName);
                        icon = m_iconExtractor.GetIcon(m_iconIndex);
                    }

                    splitIcons = IconUtil.Split(icon);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                txtFileName.Text = String.Format(
                    "{0}, #{1}, {2} variations", fileName, m_iconIndex, splitIcons.Length);

                // Update icons.

                Icon = icon;
                icon.Dispose();

                lvwIcons.BeginUpdate();
                ClearAllIcons();

                foreach (var i in splitIcons)
                {
                    var item = new IconListViewItem();
                    var size = i.Size;
                    item.BitCount = IconUtil.GetBitCount(i);
                    item.Bitmap = IconUtil.ToBitmap(i);
                    item.ToolTipText = String.Format("{0}x{1}, {2} bits", size.Width, size.Height, item.BitCount);
                    i.Dispose();

                    lvwIcons.Items.Add(item);
                }

                lvwIcons.EndUpdate();

                btnSaveAsIco.Enabled = (m_iconExtractor != null);
            }
        }

        private void cbShowChecker_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowChecker.Checked)
                lvwIcons.BackgroundImage = Properties.Resources.Checker;
            else
                lvwIcons.BackgroundImage = null;
        }

        private void btnSaveAsIco_Click(object sender, EventArgs e)
        {
            var result = saveIcoDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                using (var fs = File.OpenWrite(saveIcoDialog.FileName))
                {
                    m_iconExtractor.Save(m_iconIndex, fs);
                }
            }
        }

        private void btnSaveAsPng_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                int count = lvwIcons.Items.Count;
                for (int i = 0; i < count; ++i)
                {
                    var item = (IconListViewItem)lvwIcons.Items[i];
                    var fileName = String.Format(
                        "{0}x{1}, {2} bits.png", item.Bitmap.Width, item.Bitmap.Height, item.BitCount);

                    fileName = Path.Combine(folderBrowserDialog.SelectedPath, fileName);

                    item.Bitmap.Save(fileName);
                }
            }
        }

        private void lvwIcons_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
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

    class MyListView : ListView
    {
        public MyListView() : base()
        {
            DoubleBuffered = true;
        }
    }
}

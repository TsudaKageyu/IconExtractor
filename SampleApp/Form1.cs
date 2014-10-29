using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using TsudaKageyu;

namespace SampleApp
{
    public partial class Form1 : Form
    {
        private class IconListViewItem : ListViewItem
        {
            public Icon Icon { get; set; }
        }

        private static readonly Padding TilePadding = new Padding(2, 1, 2, 1);

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

            int x = e.Bounds.X + (e.Bounds.Width - item.Icon.Width) / 2;
            int y = e.Bounds.Y + (e.Bounds.Height - item.Icon.Height) / 2;
            var iconRect = new Rectangle(x, y, item.Icon.Width, item.Icon.Height);
            e.Graphics.Clip = new Region(e.Bounds);
            e.Graphics.DrawIcon(item.Icon, iconRect);

            var textRect = new Rectangle(
                e.Bounds.Left, e.Bounds.Bottom - Font.Height - 4,
                e.Bounds.Width, Font.Height + 2);
            TextRenderer.DrawText(e.Graphics, item.ToolTipText, Font, textRect, ForeColor);

            e.Graphics.Clip = new Region();
            e.Graphics.DrawRectangle(SystemPens.ControlLight, e.Bounds);
        }
    }
}

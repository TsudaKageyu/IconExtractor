namespace SampleApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.btnSelectIcon = new System.Windows.Forms.Button();
            this.lvwIcons = new System.Windows.Forms.ListView();
            this.btnSaveAsIco = new System.Windows.Forms.Button();
            this.saveIcoDialog = new System.Windows.Forms.SaveFileDialog();
            this.btnSaveAsPng = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.iconPickerDialog = new SampleApp.IconPickerDialog();
            this.SuspendLayout();
            // 
            // txtFileName
            // 
            this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileName.Location = new System.Drawing.Point(110, 12);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(370, 19);
            this.txtFileName.TabIndex = 1;
            // 
            // btnSelectIcon
            // 
            this.btnSelectIcon.Location = new System.Drawing.Point(12, 12);
            this.btnSelectIcon.Name = "btnSelectIcon";
            this.btnSelectIcon.Size = new System.Drawing.Size(92, 19);
            this.btnSelectIcon.TabIndex = 0;
            this.btnSelectIcon.Text = "Select Icon...";
            this.btnSelectIcon.UseVisualStyleBackColor = true;
            this.btnSelectIcon.Click += new System.EventHandler(this.btnPickFile_Click);
            // 
            // lvwIcons
            // 
            this.lvwIcons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwIcons.Location = new System.Drawing.Point(12, 37);
            this.lvwIcons.MultiSelect = false;
            this.lvwIcons.Name = "lvwIcons";
            this.lvwIcons.OwnerDraw = true;
            this.lvwIcons.Size = new System.Drawing.Size(468, 291);
            this.lvwIcons.TabIndex = 2;
            this.lvwIcons.TileSize = new System.Drawing.Size(132, 130);
            this.lvwIcons.UseCompatibleStateImageBehavior = false;
            this.lvwIcons.View = System.Windows.Forms.View.Tile;
            this.lvwIcons.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.lvwIcons_DrawItem);
            // 
            // btnSaveAsIco
            // 
            this.btnSaveAsIco.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAsIco.Location = new System.Drawing.Point(164, 334);
            this.btnSaveAsIco.Name = "btnSaveAsIco";
            this.btnSaveAsIco.Size = new System.Drawing.Size(155, 23);
            this.btnSaveAsIco.TabIndex = 3;
            this.btnSaveAsIco.Text = "Save as Single .ico...";
            this.btnSaveAsIco.UseVisualStyleBackColor = true;
            this.btnSaveAsIco.Click += new System.EventHandler(this.btnSaveAsIco_Click);
            // 
            // saveIcoDialog
            // 
            this.saveIcoDialog.Filter = "Icon files|*.ico";
            // 
            // btnSaveAsPng
            // 
            this.btnSaveAsPng.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAsPng.Location = new System.Drawing.Point(325, 334);
            this.btnSaveAsPng.Name = "btnSaveAsPng";
            this.btnSaveAsPng.Size = new System.Drawing.Size(155, 23);
            this.btnSaveAsPng.TabIndex = 4;
            this.btnSaveAsPng.Text = "Save as Multiple .png...";
            this.btnSaveAsPng.UseVisualStyleBackColor = true;
            this.btnSaveAsPng.Click += new System.EventHandler(this.btnSaveAsPng_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 369);
            this.Controls.Add(this.btnSaveAsPng);
            this.Controls.Add(this.btnSaveAsIco);
            this.Controls.Add(this.lvwIcons);
            this.Controls.Add(this.btnSelectIcon);
            this.Controls.Add(this.txtFileName);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "Form1";
            this.Text = "IconExtractor Sample App";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private IconPickerDialog iconPickerDialog;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button btnSelectIcon;
        private System.Windows.Forms.ListView lvwIcons;
        private System.Windows.Forms.Button btnSaveAsIco;
        private System.Windows.Forms.SaveFileDialog saveIcoDialog;
        private System.Windows.Forms.Button btnSaveAsPng;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}


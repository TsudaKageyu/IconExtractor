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
            this.lvwIcons = new SampleApp.IconListView();
            this.btnSaveAsIco = new System.Windows.Forms.Button();
            this.saveIcoDialog = new System.Windows.Forms.SaveFileDialog();
            this.btnSaveAsPng = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.cbShowChecker = new System.Windows.Forms.CheckBox();
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
            this.lvwIcons.BackgroundImageTiled = true;
            this.lvwIcons.Location = new System.Drawing.Point(12, 37);
            this.lvwIcons.MultiSelect = false;
            this.lvwIcons.Name = "lvwIcons";
            this.lvwIcons.Size = new System.Drawing.Size(468, 291);
            this.lvwIcons.TabIndex = 2;
            this.lvwIcons.TileSize = new System.Drawing.Size(132, 130);
            this.lvwIcons.UseCompatibleStateImageBehavior = false;
            this.lvwIcons.View = System.Windows.Forms.View.Tile;
            // 
            // btnSaveAsIco
            // 
            this.btnSaveAsIco.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAsIco.Location = new System.Drawing.Point(164, 334);
            this.btnSaveAsIco.Name = "btnSaveAsIco";
            this.btnSaveAsIco.Size = new System.Drawing.Size(155, 23);
            this.btnSaveAsIco.TabIndex = 4;
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
            this.btnSaveAsPng.TabIndex = 5;
            this.btnSaveAsPng.Text = "Save as Multiple .png...";
            this.btnSaveAsPng.UseVisualStyleBackColor = true;
            this.btnSaveAsPng.Click += new System.EventHandler(this.btnSaveAsPng_Click);
            // 
            // cbShowChecker
            // 
            this.cbShowChecker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbShowChecker.AutoSize = true;
            this.cbShowChecker.Location = new System.Drawing.Point(12, 338);
            this.cbShowChecker.Name = "cbShowChecker";
            this.cbShowChecker.Size = new System.Drawing.Size(97, 16);
            this.cbShowChecker.TabIndex = 3;
            this.cbShowChecker.Text = "Show Checker";
            this.cbShowChecker.UseVisualStyleBackColor = true;
            this.cbShowChecker.CheckedChanged += new System.EventHandler(this.cbShowChecker_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 369);
            this.Controls.Add(this.cbShowChecker);
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
        private SampleApp.IconListView lvwIcons;
        private System.Windows.Forms.Button btnSaveAsIco;
        private System.Windows.Forms.SaveFileDialog saveIcoDialog;
        private System.Windows.Forms.Button btnSaveAsPng;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.CheckBox cbShowChecker;
    }
}


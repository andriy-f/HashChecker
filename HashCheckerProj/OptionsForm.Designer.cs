namespace HashChecker.WinForms
{
    partial class OptionsForm
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.clbAssoc = new System.Windows.Forms.CheckedListBox();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpAssoc = new System.Windows.Forms.TabPage();
            this.cbAddComp2Clip = new System.Windows.Forms.CheckBox();
            this.cbSelectAll = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tpAssoc.SuspendLayout();
            this.SuspendLayout();
            // 
            // clbAssoc
            // 
            this.clbAssoc.CheckOnClick = true;
            this.clbAssoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbAssoc.FormattingEnabled = true;
            this.clbAssoc.Items.AddRange(new object[] {
            "*.sfv (CRC32)",
            "*.md5 (MD5)",
            "*.sha (SHA1)",
            "*.sha1 (SHA1)",
            "*.sha256 (SHA256)",
            "*.sha384 (SHA384)",
            "*.sha512 (SHA512)"});
            this.clbAssoc.Location = new System.Drawing.Point(4, 4);
            this.clbAssoc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.clbAssoc.Name = "clbAssoc";
            this.clbAssoc.Size = new System.Drawing.Size(381, 234);
            this.clbAssoc.TabIndex = 0;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Location = new System.Drawing.Point(175, 278);
            this.bOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(100, 28);
            this.bOK.TabIndex = 1;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.Location = new System.Drawing.Point(283, 278);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(100, 28);
            this.bCancel.TabIndex = 2;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpAssoc);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(397, 271);
            this.tabControl1.TabIndex = 3;
            // 
            // tpAssoc
            // 
            this.tpAssoc.Controls.Add(this.cbSelectAll);
            this.tpAssoc.Controls.Add(this.cbAddComp2Clip);
            this.tpAssoc.Controls.Add(this.clbAssoc);
            this.tpAssoc.Location = new System.Drawing.Point(4, 25);
            this.tpAssoc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpAssoc.Name = "tpAssoc";
            this.tpAssoc.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpAssoc.Size = new System.Drawing.Size(389, 242);
            this.tpAssoc.TabIndex = 0;
            this.tpAssoc.Text = "Associations";
            this.tpAssoc.UseVisualStyleBackColor = true;
            // 
            // cbAddComp2Clip
            // 
            this.cbAddComp2Clip.AutoSize = true;
            this.cbAddComp2Clip.Location = new System.Drawing.Point(9, 180);
            this.cbAddComp2Clip.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbAddComp2Clip.Name = "cbAddComp2Clip";
            this.cbAddComp2Clip.Size = new System.Drawing.Size(349, 21);
            this.cbAddComp2Clip.TabIndex = 1;
            this.cbAddComp2Clip.Text = "Add \'Compare to hash in Clipboard\' file menu entry";
            this.cbAddComp2Clip.UseVisualStyleBackColor = true;
            // 
            // cbSelectAll
            // 
            this.cbSelectAll.AutoSize = true;
            this.cbSelectAll.Location = new System.Drawing.Point(9, 151);
            this.cbSelectAll.Margin = new System.Windows.Forms.Padding(4);
            this.cbSelectAll.Name = "cbSelectAll";
            this.cbSelectAll.Size = new System.Drawing.Size(88, 21);
            this.cbSelectAll.TabIndex = 2;
            this.cbSelectAll.Text = "Select All";
            this.cbSelectAll.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 321);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.tabControl1.ResumeLayout(false);
            this.tpAssoc.ResumeLayout(false);
            this.tpAssoc.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clbAssoc;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpAssoc;
        private System.Windows.Forms.CheckBox cbAddComp2Clip;
        private System.Windows.Forms.CheckBox cbSelectAll;
    }
}
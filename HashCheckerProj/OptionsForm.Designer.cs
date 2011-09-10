namespace HashCheckerProj
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
            this.clbAssoc = new System.Windows.Forms.CheckedListBox();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpAssoc = new System.Windows.Forms.TabPage();
            this.cbAddComp2Clip = new System.Windows.Forms.CheckBox();
            this.tpMisc = new System.Windows.Forms.TabPage();
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
            this.clbAssoc.Location = new System.Drawing.Point(3, 3);
            this.clbAssoc.Name = "clbAssoc";
            this.clbAssoc.Size = new System.Drawing.Size(284, 188);
            this.clbAssoc.TabIndex = 0;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Location = new System.Drawing.Point(131, 226);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 1;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.Location = new System.Drawing.Point(212, 226);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
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
            this.tabControl1.Controls.Add(this.tpMisc);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(298, 220);
            this.tabControl1.TabIndex = 3;
            // 
            // tpAssoc
            // 
            this.tpAssoc.Controls.Add(this.cbAddComp2Clip);
            this.tpAssoc.Controls.Add(this.clbAssoc);
            this.tpAssoc.Location = new System.Drawing.Point(4, 22);
            this.tpAssoc.Name = "tpAssoc";
            this.tpAssoc.Padding = new System.Windows.Forms.Padding(3);
            this.tpAssoc.Size = new System.Drawing.Size(290, 194);
            this.tpAssoc.TabIndex = 0;
            this.tpAssoc.Text = "Associations";
            this.tpAssoc.UseVisualStyleBackColor = true;
            // 
            // cbAddComp2Clip
            // 
            this.cbAddComp2Clip.AutoSize = true;
            this.cbAddComp2Clip.Location = new System.Drawing.Point(6, 125);
            this.cbAddComp2Clip.Name = "cbAddComp2Clip";
            this.cbAddComp2Clip.Size = new System.Drawing.Size(261, 17);
            this.cbAddComp2Clip.TabIndex = 1;
            this.cbAddComp2Clip.Text = "Add \'Compare to hash in Clipboard\' file menu entry";
            this.cbAddComp2Clip.UseVisualStyleBackColor = true;
            // 
            // tpMisc
            // 
            this.tpMisc.Location = new System.Drawing.Point(4, 22);
            this.tpMisc.Name = "tpMisc";
            this.tpMisc.Padding = new System.Windows.Forms.Padding(3);
            this.tpMisc.Size = new System.Drawing.Size(290, 194);
            this.tpMisc.TabIndex = 1;
            this.tpMisc.Text = "Misc";
            this.tpMisc.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 261);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
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
        private System.Windows.Forms.TabPage tpMisc;
        private System.Windows.Forms.CheckBox cbAddComp2Clip;
    }
}
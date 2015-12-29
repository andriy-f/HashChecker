namespace HashChecker.WinForms
{
    partial class SingleFileCheckForm
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
            this.lblFile = new System.Windows.Forms.Label();
            this.tbFile = new System.Windows.Forms.TextBox();
            this.pbValidateProgress = new System.Windows.Forms.ProgressBar();
            this.bClose = new System.Windows.Forms.Button();
            this.bAbout = new System.Windows.Forms.Button();
            this.labelResult = new System.Windows.Forms.Label();
            this.labelResultText = new System.Windows.Forms.Label();
            this.lblHashInClipboard = new System.Windows.Forms.Label();
            this.tbHashInClipboard = new System.Windows.Forms.TextBox();
            this.lblActualHash = new System.Windows.Forms.Label();
            this.tbActualHash = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(13, 12);
            this.lblFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(34, 17);
            this.lblFile.TabIndex = 0;
            this.lblFile.Text = "File:";
            // 
            // tbFile
            // 
            this.tbFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFile.Location = new System.Drawing.Point(142, 9);
            this.tbFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbFile.Name = "tbFile";
            this.tbFile.ReadOnly = true;
            this.tbFile.Size = new System.Drawing.Size(545, 22);
            this.tbFile.TabIndex = 1;
            // 
            // pbValidateProgress
            // 
            this.pbValidateProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbValidateProgress.Location = new System.Drawing.Point(20, 113);
            this.pbValidateProgress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbValidateProgress.Name = "pbValidateProgress";
            this.pbValidateProgress.Size = new System.Drawing.Size(668, 44);
            this.pbValidateProgress.TabIndex = 2;
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bClose.Location = new System.Drawing.Point(588, 220);
            this.bClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(100, 28);
            this.bClose.TabIndex = 3;
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bAbout
            // 
            this.bAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAbout.Location = new System.Drawing.Point(16, 220);
            this.bAbout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bAbout.Name = "bAbout";
            this.bAbout.Size = new System.Drawing.Size(100, 28);
            this.bAbout.TabIndex = 4;
            this.bAbout.Text = "About";
            this.bAbout.UseVisualStyleBackColor = true;
            this.bAbout.Click += new System.EventHandler(this.bAbout_Click);
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Location = new System.Drawing.Point(16, 169);
            this.labelResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(52, 17);
            this.labelResult.TabIndex = 5;
            this.labelResult.Text = "Result:";
            // 
            // labelResultText
            // 
            this.labelResultText.AutoSize = true;
            this.labelResultText.Location = new System.Drawing.Point(73, 169);
            this.labelResultText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelResultText.Name = "labelResultText";
            this.labelResultText.Size = new System.Drawing.Size(0, 17);
            this.labelResultText.TabIndex = 6;
            // 
            // lblHashInClipboard
            // 
            this.lblHashInClipboard.AutoSize = true;
            this.lblHashInClipboard.Location = new System.Drawing.Point(13, 44);
            this.lblHashInClipboard.Name = "lblHashInClipboard";
            this.lblHashInClipboard.Size = new System.Drawing.Size(122, 17);
            this.lblHashInClipboard.TabIndex = 7;
            this.lblHashInClipboard.Text = "Hash in clipboard:";
            // 
            // tbHashInClipboard
            // 
            this.tbHashInClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbHashInClipboard.Location = new System.Drawing.Point(142, 41);
            this.tbHashInClipboard.Margin = new System.Windows.Forms.Padding(4);
            this.tbHashInClipboard.Name = "tbHashInClipboard";
            this.tbHashInClipboard.ReadOnly = true;
            this.tbHashInClipboard.Size = new System.Drawing.Size(545, 22);
            this.tbHashInClipboard.TabIndex = 8;
            // 
            // lblActualHash
            // 
            this.lblActualHash.AutoSize = true;
            this.lblActualHash.Location = new System.Drawing.Point(13, 82);
            this.lblActualHash.Name = "lblActualHash";
            this.lblActualHash.Size = new System.Drawing.Size(86, 17);
            this.lblActualHash.TabIndex = 9;
            this.lblActualHash.Text = "Actual hash:";
            // 
            // tbActualHash
            // 
            this.tbActualHash.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbActualHash.Location = new System.Drawing.Point(142, 79);
            this.tbActualHash.Margin = new System.Windows.Forms.Padding(4);
            this.tbActualHash.Name = "tbActualHash";
            this.tbActualHash.ReadOnly = true;
            this.tbActualHash.Size = new System.Drawing.Size(545, 22);
            this.tbActualHash.TabIndex = 10;
            // 
            // SingleFileCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 263);
            this.Controls.Add(this.tbActualHash);
            this.Controls.Add(this.lblActualHash);
            this.Controls.Add(this.tbHashInClipboard);
            this.Controls.Add(this.lblHashInClipboard);
            this.Controls.Add(this.labelResultText);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.bAbout);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.pbValidateProgress);
            this.Controls.Add(this.tbFile);
            this.Controls.Add(this.lblFile);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(461, 211);
            this.Name = "SingleFileCheckForm";
            this.Text = "Hash Checker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SingleFileCheckForm_FormClosing);
            this.Load += new System.EventHandler(this.SingleFileCheckForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.TextBox tbFile;
        private System.Windows.Forms.ProgressBar pbValidateProgress;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bAbout;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Label labelResultText;
        private System.Windows.Forms.Label lblHashInClipboard;
        private System.Windows.Forms.TextBox tbHashInClipboard;
        private System.Windows.Forms.Label lblActualHash;
        private System.Windows.Forms.TextBox tbActualHash;
    }
}
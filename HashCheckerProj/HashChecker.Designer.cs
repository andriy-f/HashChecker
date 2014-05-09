namespace HashCheckerProj
{
    partial class HashChecker
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
            this.bStop = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            this.bChFile = new System.Windows.Forms.Button();
            this.tbChSumFile = new System.Windows.Forms.TextBox();
            this.tbDir = new System.Windows.Forms.TextBox();
            this.bQCheck = new System.Windows.Forms.Button();
            this.labelChecksumFile = new System.Windows.Forms.Label();
            this.labelRootDIrectory = new System.Windows.Forms.Label();
            this.bBrowseFile = new System.Windows.Forms.Button();
            this.bBrowseDir = new System.Windows.Forms.Button();
            this.panelSetUp = new System.Windows.Forms.Panel();
            this.cbPriority = new System.Windows.Forms.ComboBox();
            this.labelThresPriority = new System.Windows.Forms.Label();
            this.bOptions = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.bAbout = new System.Windows.Forms.Button();
            this.labelTotalProgress = new System.Windows.Forms.Label();
            this.labelEntryProgress = new System.Windows.Forms.Label();
            this.progressTotal = new System.Windows.Forms.ProgressBar();
            this.progressEntry = new System.Windows.Forms.ProgressBar();
            this.panelSetUp.SuspendLayout();
            this.SuspendLayout();
            // 
            // bStop
            // 
            this.bStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bStop.Enabled = false;
            this.bStop.Location = new System.Drawing.Point(344, 460);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(75, 23);
            this.bStop.TabIndex = 1;
            this.bStop.Text = "Stop";
            this.bStop.UseVisualStyleBackColor = true;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bClose.Location = new System.Drawing.Point(425, 460);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(75, 23);
            this.bClose.TabIndex = 2;
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bChFile
            // 
            this.bChFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bChFile.Location = new System.Drawing.Point(431, 32);
            this.bChFile.Name = "bChFile";
            this.bChFile.Size = new System.Drawing.Size(69, 49);
            this.bChFile.TabIndex = 3;
            this.bChFile.Text = "Check File";
            this.bChFile.UseVisualStyleBackColor = true;
            this.bChFile.Click += new System.EventHandler(this.bChFile_Click);
            // 
            // tbChSumFile
            // 
            this.tbChSumFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbChSumFile.Location = new System.Drawing.Point(35, 34);
            this.tbChSumFile.Name = "tbChSumFile";
            this.tbChSumFile.Size = new System.Drawing.Size(357, 20);
            this.tbChSumFile.TabIndex = 4;
            // 
            // tbDir
            // 
            this.tbDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDir.Location = new System.Drawing.Point(35, 60);
            this.tbDir.Name = "tbDir";
            this.tbDir.Size = new System.Drawing.Size(357, 20);
            this.tbDir.TabIndex = 5;
            // 
            // bQCheck
            // 
            this.bQCheck.Location = new System.Drawing.Point(173, 3);
            this.bQCheck.Name = "bQCheck";
            this.bQCheck.Size = new System.Drawing.Size(143, 22);
            this.bQCheck.TabIndex = 6;
            this.bQCheck.Text = "Quick Check...";
            this.bQCheck.UseVisualStyleBackColor = true;
            this.bQCheck.Click += new System.EventHandler(this.bQCheck_Click);
            // 
            // labelChecksumFile
            // 
            this.labelChecksumFile.AutoSize = true;
            this.labelChecksumFile.Location = new System.Drawing.Point(3, 37);
            this.labelChecksumFile.Name = "labelChecksumFile";
            this.labelChecksumFile.Size = new System.Drawing.Size(26, 13);
            this.labelChecksumFile.TabIndex = 7;
            this.labelChecksumFile.Text = "File:";
            // 
            // labelRootDIrectory
            // 
            this.labelRootDIrectory.AutoSize = true;
            this.labelRootDIrectory.Location = new System.Drawing.Point(3, 63);
            this.labelRootDIrectory.Name = "labelRootDIrectory";
            this.labelRootDIrectory.Size = new System.Drawing.Size(23, 13);
            this.labelRootDIrectory.TabIndex = 8;
            this.labelRootDIrectory.Text = "Dir:";
            // 
            // bBrowseFile
            // 
            this.bBrowseFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bBrowseFile.Image = global::HashCheckerProj.Properties.Resources.openfolderHS;
            this.bBrowseFile.Location = new System.Drawing.Point(398, 32);
            this.bBrowseFile.Name = "bBrowseFile";
            this.bBrowseFile.Size = new System.Drawing.Size(27, 23);
            this.bBrowseFile.TabIndex = 9;
            this.bBrowseFile.UseVisualStyleBackColor = true;
            this.bBrowseFile.Click += new System.EventHandler(this.bBrowseFile_Click);
            // 
            // bBrowseDir
            // 
            this.bBrowseDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bBrowseDir.Image = global::HashCheckerProj.Properties.Resources.openfolderHS;
            this.bBrowseDir.Location = new System.Drawing.Point(398, 58);
            this.bBrowseDir.Name = "bBrowseDir";
            this.bBrowseDir.Size = new System.Drawing.Size(27, 23);
            this.bBrowseDir.TabIndex = 10;
            this.bBrowseDir.UseVisualStyleBackColor = true;
            this.bBrowseDir.Click += new System.EventHandler(this.bBrowseDir_Click);
            // 
            // panelSetUp
            // 
            this.panelSetUp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSetUp.Controls.Add(this.cbPriority);
            this.panelSetUp.Controls.Add(this.labelThresPriority);
            this.panelSetUp.Controls.Add(this.bQCheck);
            this.panelSetUp.Controls.Add(this.bChFile);
            this.panelSetUp.Controls.Add(this.bBrowseDir);
            this.panelSetUp.Controls.Add(this.labelChecksumFile);
            this.panelSetUp.Controls.Add(this.bBrowseFile);
            this.panelSetUp.Controls.Add(this.tbChSumFile);
            this.panelSetUp.Controls.Add(this.labelRootDIrectory);
            this.panelSetUp.Controls.Add(this.tbDir);
            this.panelSetUp.Location = new System.Drawing.Point(0, 0);
            this.panelSetUp.Name = "panelSetUp";
            this.panelSetUp.Size = new System.Drawing.Size(505, 128);
            this.panelSetUp.TabIndex = 11;
            // 
            // cbPriority
            // 
            this.cbPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPriority.FormattingEnabled = true;
            this.cbPriority.Items.AddRange(new object[] {
            "Highest",
            "Above Normal",
            "Normal",
            "Below Normal",
            "Lowest"});
            this.cbPriority.Location = new System.Drawing.Point(379, 96);
            this.cbPriority.Name = "cbPriority";
            this.cbPriority.Size = new System.Drawing.Size(121, 21);
            this.cbPriority.TabIndex = 16;
            // 
            // labelThresPriority
            // 
            this.labelThresPriority.AutoSize = true;
            this.labelThresPriority.Location = new System.Drawing.Point(296, 99);
            this.labelThresPriority.Name = "labelThresPriority";
            this.labelThresPriority.Size = new System.Drawing.Size(77, 13);
            this.labelThresPriority.TabIndex = 15;
            this.labelThresPriority.Text = "Thread priority:";
            // 
            // bOptions
            // 
            this.bOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bOptions.Location = new System.Drawing.Point(12, 460);
            this.bOptions.Name = "bOptions";
            this.bOptions.Size = new System.Drawing.Size(75, 23);
            this.bOptions.TabIndex = 12;
            this.bOptions.Text = "Options...";
            this.bOptions.UseVisualStyleBackColor = true;
            this.bOptions.Click += new System.EventHandler(this.bOptions_Click);
            // 
            // rtbLog
            // 
            this.rtbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLog.HideSelection = false;
            this.rtbLog.Location = new System.Drawing.Point(0, 134);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(505, 277);
            this.rtbLog.TabIndex = 13;
            this.rtbLog.Text = "";
            // 
            // bAbout
            // 
            this.bAbout.Location = new System.Drawing.Point(95, 460);
            this.bAbout.Name = "bAbout";
            this.bAbout.Size = new System.Drawing.Size(75, 23);
            this.bAbout.TabIndex = 15;
            this.bAbout.Text = "About";
            this.bAbout.UseVisualStyleBackColor = true;
            this.bAbout.Click += new System.EventHandler(this.bAbout_Click);
            // 
            // labelTotalProgress
            // 
            this.labelTotalProgress.AutoSize = true;
            this.labelTotalProgress.Location = new System.Drawing.Point(3, 414);
            this.labelTotalProgress.Name = "labelTotalProgress";
            this.labelTotalProgress.Size = new System.Drawing.Size(77, 13);
            this.labelTotalProgress.TabIndex = 16;
            this.labelTotalProgress.Text = "Total progress:";
            // 
            // labelEntryProgress
            // 
            this.labelEntryProgress.AutoSize = true;
            this.labelEntryProgress.Location = new System.Drawing.Point(4, 435);
            this.labelEntryProgress.Name = "labelEntryProgress";
            this.labelEntryProgress.Size = new System.Drawing.Size(70, 13);
            this.labelEntryProgress.TabIndex = 17;
            this.labelEntryProgress.Text = "Current entry:";
            // 
            // progressTotal
            // 
            this.progressTotal.Location = new System.Drawing.Point(83, 417);
            this.progressTotal.Name = "progressTotal";
            this.progressTotal.Size = new System.Drawing.Size(417, 12);
            this.progressTotal.TabIndex = 18;
            // 
            // progressEntry
            // 
            this.progressEntry.Location = new System.Drawing.Point(83, 436);
            this.progressEntry.Name = "progressEntry";
            this.progressEntry.Size = new System.Drawing.Size(417, 12);
            this.progressEntry.TabIndex = 19;
            // 
            // HashChecker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 495);
            this.Controls.Add(this.progressEntry);
            this.Controls.Add(this.progressTotal);
            this.Controls.Add(this.labelEntryProgress);
            this.Controls.Add(this.labelTotalProgress);
            this.Controls.Add(this.bAbout);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.bOptions);
            this.Controls.Add(this.panelSetUp);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.bStop);
            this.Name = "HashChecker";
            this.Text = "Hash Checker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HashChecker_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HashChecker_FormClosed);
            this.Load += new System.EventHandler(this.HashChecker_Load);
            this.panelSetUp.ResumeLayout(false);
            this.panelSetUp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bChFile;
        private System.Windows.Forms.TextBox tbChSumFile;
        private System.Windows.Forms.TextBox tbDir;
        private System.Windows.Forms.Button bQCheck;
        private System.Windows.Forms.Label labelChecksumFile;
        private System.Windows.Forms.Label labelRootDIrectory;
        private System.Windows.Forms.Button bBrowseFile;
        private System.Windows.Forms.Button bBrowseDir;
        private System.Windows.Forms.Panel panelSetUp;
        private System.Windows.Forms.Label labelThresPriority;
        private System.Windows.Forms.ComboBox cbPriority;
        private System.Windows.Forms.Button bOptions;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Button bAbout;
        private System.Windows.Forms.Label labelTotalProgress;
        private System.Windows.Forms.Label labelEntryProgress;
        private System.Windows.Forms.ProgressBar progressTotal;
        private System.Windows.Forms.ProgressBar progressEntry;
    }
}
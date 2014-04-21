namespace HashCheckerProj
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
            this.lblFile = new System.Windows.Forms.Label();
            this.tbFile = new System.Windows.Forms.TextBox();
            this.pbValidateProgress = new System.Windows.Forms.ProgressBar();
            this.bClose = new System.Windows.Forms.Button();
            this.bAbout = new System.Windows.Forms.Button();
            this.labelResult = new System.Windows.Forms.Label();
            this.labelResultText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(12, 9);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(26, 13);
            this.lblFile.TabIndex = 0;
            this.lblFile.Text = "File:";
            // 
            // tbFile
            // 
            this.tbFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFile.Location = new System.Drawing.Point(44, 6);
            this.tbFile.Name = "tbFile";
            this.tbFile.ReadOnly = true;
            this.tbFile.Size = new System.Drawing.Size(278, 20);
            this.tbFile.TabIndex = 1;
            // 
            // pbValidateProgress
            // 
            this.pbValidateProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbValidateProgress.Location = new System.Drawing.Point(15, 32);
            this.pbValidateProgress.Name = "pbValidateProgress";
            this.pbValidateProgress.Size = new System.Drawing.Size(307, 36);
            this.pbValidateProgress.TabIndex = 2;
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bClose.Location = new System.Drawing.Point(247, 107);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(75, 23);
            this.bClose.TabIndex = 3;
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bAbout
            // 
            this.bAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAbout.Location = new System.Drawing.Point(12, 107);
            this.bAbout.Name = "bAbout";
            this.bAbout.Size = new System.Drawing.Size(75, 23);
            this.bAbout.TabIndex = 4;
            this.bAbout.Text = "About";
            this.bAbout.UseVisualStyleBackColor = true;
            this.bAbout.Click += new System.EventHandler(this.bAbout_Click);
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Location = new System.Drawing.Point(12, 81);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(40, 13);
            this.labelResult.TabIndex = 5;
            this.labelResult.Text = "Result:";
            // 
            // labelResultText
            // 
            this.labelResultText.AutoSize = true;
            this.labelResultText.Location = new System.Drawing.Point(55, 81);
            this.labelResultText.Name = "labelResultText";
            this.labelResultText.Size = new System.Drawing.Size(0, 13);
            this.labelResultText.TabIndex = 6;
            // 
            // SingleFileCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 142);
            this.Controls.Add(this.labelResultText);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.bAbout);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.pbValidateProgress);
            this.Controls.Add(this.tbFile);
            this.Controls.Add(this.lblFile);
            this.MinimumSize = new System.Drawing.Size(350, 180);
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
    }
}
namespace HashCheckerProj
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// Form for validating hash of file against a hash in clipboard
    /// </summary>
    public partial class SingleFileCheckForm : Form
    {
        private readonly string fileToValidate;

        private FileHashCalculator fileHashCalculator;

        private Thread validatingThread;

        public SingleFileCheckForm(string fileToValidate)
        {
            this.InitializeComponent();
            this.fileToValidate = fileToValidate;
            this.tbFile.Text = fileToValidate;
        }

        private void SingleFileCheckForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.ValidateFile();
            }
            catch (Exception ex)
            {
                CustomMessageBoxes.Error(ex.Message);
#if DEBUG
                throw;
#endif
            }
        }

        private void bAbout_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void SetResultTextThreadSafe(string message, bool success)
        {
            if (this.labelResultText.InvokeRequired)
            {
                Action<string, bool> setResultMethod = this.SetResultText;
                this.labelResultText.Invoke(setResultMethod, message, success);
            }
            else
            {
                this.SetResultText(message, success);
            }
        }

        private void SetResultText(string message, bool success)
        {
            if (success)
            {
                this.labelResultText.ForeColor = Color.Green;
            }
            else
            {
                this.labelResultText.ForeColor = Color.Red;
            }

            this.labelResultText.Text = message;
        }

        private void SetProgressThreadSafe(int percents)
        {
            if (this.pbValidateProgress.InvokeRequired)
            {
                Action<int> action = this.SetProgress;
                this.pbValidateProgress.Invoke(action, percents); // TODO: find why unhandled exception if called without "percents"
            }
            else
            {
                this.SetProgress(percents);
            }
        }

        private void SetProgress(int percents)
        {
            if (percents < 0 || percents > 100)
            {
                throw new ArgumentOutOfRangeException("percents", @"Invalid percent value");
            }

            this.pbValidateProgress.Value = percents;
        }

        private void ValidateFile()
        {
            try
            {
                if (!Clipboard.ContainsText())
                {
                    CustomMessageBoxes.Warning("No hash in Clipboard to compare");
                    Application.Exit();
                }
                
                string expectedHash = Clipboard.GetText();

                HashType hashType;
                try
                {
                    hashType = CryptoUtils.DetectHashType(expectedHash.Length);
                }
                catch (ArgumentException ex)
                {
                    throw new InvalidHashException("That's not a valid hash in clipboard", ex);
                }

                this.fileHashCalculator = new FileHashCalculator { HashAlgorithm = CryptoUtils.ToHashAlgorithm(hashType) };
                this.fileHashCalculator.ChunkProcessed += (processed, total) => this.SetProgressThreadSafe((int)((processed * 100) / total));

                this.fileHashCalculator.Calculated += (actualHash) =>
                {
                    bool isValid = ConvertUtils.ByteArraysEqual(actualHash, ConvertUtils.ToBytes(expectedHash));
                    if (isValid)
                    {
                        this.SetResultTextThreadSafe("Correct", true);
                    }
                    else
                    {
                        this.SetResultTextThreadSafe("Wrong", false);
                    }
                };
                
                this.fileHashCalculator.Finished += () =>
                    {
                        if (!fileHashCalculator.HashCalculated)
                        {
                            this.SetResultTextThreadSafe("Stopped", false);
                        }
                    };
                
                this.validatingThread = new Thread(this.CalculateHashAsync);
                this.validatingThread.Start(new ValidateHashParams()
                {
                    FilePath = this.fileToValidate,
                    ExpectedHash = expectedHash,
                    FileHashCalculator = this.fileHashCalculator
                });
            }
            catch (InvalidHashException ex)
            {
                CustomMessageBoxes.Error(ex.Message);
                Application.Exit();
            }
            catch (Exception ex)
            {
                CustomMessageBoxes.Error("Error while comparing to hash in clipboard: " + ex.Message);
                Application.Exit();
            }
        }

        private void CalculateHashAsync(object ob)
        {
            try
            {
                var inputParams = (ValidateHashParams)ob;
                inputParams.FileHashCalculator.CalculateFileHash(inputParams.FilePath);
            }
            catch (ThreadAbortException)
            {
                this.SetResultTextThreadSafe("Aborted", false);
            }
            catch (Exception ex)
            {
                this.SetResultTextThreadSafe("Error: " + ex.Message, false);
#if DEBUG
                throw;
#endif
            }
        }

        private class ValidateHashParams
        {
            public string FilePath { get; set; }

            public FileHashCalculator FileHashCalculator { get; set; }

            public string ExpectedHash { get; set; }
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SingleFileCheckForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.fileHashCalculator != null && this.fileHashCalculator.State == FileHashCalculator.StateType.InProgress)
            {
                this.fileHashCalculator.RequestStop();
                this.SetResultText("Stopping...", false);
                this.fileHashCalculator.Finished += this.CloseThreadSafe;
                e.Cancel = true;
            }
        }

        private void CloseThreadSafe()
        {
            if (this.InvokeRequired)
            {
                Action closeAction = this.CloseThreadSafe;
                this.Invoke(closeAction);
            }
            else
            {
                this.Close();
            }
        }
    }
}

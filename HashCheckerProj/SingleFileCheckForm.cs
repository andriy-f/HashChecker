namespace HashChecker.WinForms
{
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using global::HashChecker.Core;

    /// <summary>
    /// Form for validating hash of file against a hash in clipboard
    /// </summary>
    public partial class SingleFileCheckForm : Form
    {
        private string fileToValidate;

        public string FileToValidate
        {
            get { return this.fileToValidate; }
            set
            {
                this.fileToValidate = value;
                this.tbFile.Text = value;
            }
        }

        private FileHashCalculator fileHashCalculator;

        private Thread validatingThread;

        public SingleFileCheckForm()
        {
            this.InitializeComponent();
        }

        private void SingleFileCheckForm_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

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
            this.labelResultText.ForeColor = success ? Color.Green : Color.Red;
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

                this.tbHashInClipboard.Text = string.Format("{0} ({1})", expectedHash.ToUpperInvariant(), hashType);
                this.fileHashCalculator = new FileHashCalculator { HashAlgorithm = CryptoUtils.ToHashAlgorithm(hashType) };
                this.fileHashCalculator.ChunkProcessed += (processed, total) => this.SetProgressThreadSafe((int)((processed * 100) / total));

                this.fileHashCalculator.Calculated += (actualHash) =>
                {
                    var actualHashString = ConvertUtils.ToString(actualHash);
                    this.tbActualHash.Invoke(new Action(() => { this.tbActualHash.Text = actualHashString; }));
                    bool isValid = ConvertUtils.ByteArraysEqual(actualHash, ConvertUtils.ToBytes(expectedHash));
                    this.SetResultTextThreadSafe(isValid ? "Correct" : "Wrong", isValid);
                };
                
                this.fileHashCalculator.Finished += () =>
                    {
                        if (!this.fileHashCalculator.HashCalculated)
                        {
                            this.SetResultTextThreadSafe("Stopped", false);
                        }
                    };
                
                this.validatingThread = new Thread(this.CalculateHashAsync);
                this.validatingThread.Start(new ValidateHashParams
                {
                    FilePath = this.FileToValidate,
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
                this.Invoke((Action)this.CloseThreadSafe);
            }
            else
            {
                this.Close();
            }
        }
    }
}

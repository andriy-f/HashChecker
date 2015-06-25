namespace HashChecker.WinForms
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;
    using global::HashChecker.Core;
    using global::HashChecker.WinForms.Properties;

    public partial class HashChecker : Form
    {
        #region Fields Consts
        
        // locals
        private const string ChecksumFileFilter = @"All Supported|*.sfv;*.md5;*.sha;*.sha1;*.sha256;*.sha384;*.sha512|sfv|*.sfv|md5|*.md5|sha1|*.sha;*.sha1|sha256|*.sha256|sha384|*.sha384|sha512|*.sha512|All(*.*)|*.*";

        private readonly string cmdlineFName;

        private volatile HashValidator hashValidator;

        private volatile bool exiting;

        private Thread checkingThread;

        #endregion

        #region Constructors

        public HashChecker(ProgramMode mode, string filePath = null)
        {
            this.InitializeComponent();
            this.Icon = Resources.Key;
            
            switch (mode)
            {
                case ProgramMode.Standard:
                    break;
                case ProgramMode.ValidateWithClipboard:
                    throw new ArgumentException("Use new form for checking clipboard hash");
                case ProgramMode.ValidateChecksumFile:
                    this.cmdlineFName = filePath;
                    break;
                default:
                    throw new ArgumentException(Resources.HashChecker_Unsupported_mode, "mode");
            }

            try
            { 
                this.cbPriority.SelectedIndex = Settings.Default.ThreadPriority;
            }
            catch (System.Configuration.ConfigurationException)
            {
                CustomMessageBoxes.Warning("Failed to load settings, restoring defaults");
                this.cbPriority.SelectedIndex = 4;
#if DEBUG
                throw;
#endif
            }
        }

        #endregion

        #region Methods

        #region Cross Thread related

        private void LogThreadSafe(string text)
        {
            this.LogThreadSafe(text, Color.Black);
        }

        private void LogThreadSafe(string text, Color color)
        {
            if (this.rtbLog.InvokeRequired)
            {
                this.Invoke((Action<string, Color>)this.LogThreadSafe, text, color);
            }
            else
            {
                WinFormsUtils.AppendText(this.rtbLog, color, text);
            }
        }

        private void SetToInitialStateThreadSafe()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)this.SetToInitialStateThreadSafe);
            }
            else
            {
                this.panelSetUp.Enabled = true;
                this.bStop.Enabled = false;
                this.Text = Resources.HashChecker_setFormToNormal_Hash_Checker;
            }
        }

        private void InitProgressSafe(int entriesCount = 0)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action<int>)this.InitProgressSafe, entriesCount);
            }
            else
            {
                this.ShowTotalProgressSafe(0, 1);
                this.ShowEntryProcessedProgressSafe(0, 1);
                if (entriesCount > 0)
                {
                    this.Text = string.Format("Hash Checker (0/{0} entries processed)", entriesCount);
                }
            }
        }

        #endregion

        private void StartValidatingAsync()
        {
            var checksumFile = new ChecksumFile(this.tbChSumFile.Text);
            var entries = checksumFile.Parse().ToArray();
            this.InitProgressSafe(entries.Count());

            this.hashValidator = new HashValidator();
            this.hashValidator.BaseDir = this.tbDir.Text;
            this.hashValidator.DefaultHashType = CryptoUtils.InferHashTypeFromExtension(checksumFile.Ext);
            this.hashValidator.StartProcessingEntry += entry => this.LogThreadSafe(entry.Path + "... ");
            this.hashValidator.EntryProcessed += (result, processed, count) =>
                {
                    this.ShowTotalProgressSafe(processed, count);

                    Color color;
                    switch (result.ResultType)
                    {
                        case EntryResultType.Wrong:
                        case EntryResultType.Aborted:
                            color = Color.Red;
                            break;
                        case EntryResultType.NotFound:
                            color = Color.BlueViolet;
                            break;
                        default:
                            color = Color.Black;
                            break;
                    }

                    this.LogThreadSafe(result.ResultType.ToString() + Environment.NewLine, color);
                };

            this.hashValidator.EntryChunkProcessed += this.ShowEntryProcessedProgressSafe;

            this.checkingThread = new Thread(
                () =>
                    {
                        try
                        {
                            this.hashValidator.ValidateEntries(entries, checksumFile.Ext);

                            if (this.exiting)
                            {
                                return;
                            }

                            // Display final message
                            var endMessage = string.Format(
                                "{0}Correct: {1}, Wrong: {2}, Not Found: {3}, Total: {4}",
                                Environment.NewLine,
                                this.hashValidator.ValidCount,
                                this.hashValidator.WrongCount,
                                this.hashValidator.NotFoundCount,
                                this.hashValidator.EntriesCount);

                            Color color;
                            if (this.hashValidator.WrongCount > 0)
                            {
                                color = Color.Red;
                            } 
                            else if (this.hashValidator.NotFoundCount > 0)
                            {
                                color = Color.BlueViolet;
                            }
                            else
                            {
                                color = Color.Black;
                            }

                            this.LogThreadSafe(endMessage, color);
                            this.SetToInitialStateThreadSafe();
                        }
                        catch (Exception ex)
                        {
                            this.ShowErrorThreadSafe(ex);
                        }
                    });

            this.checkingThread.Priority = this.SelectedThreadPriority;
            this.checkingThread.IsBackground = true;
            this.checkingThread.Start();
        }

        private ThreadPriority SelectedThreadPriority
        {
            get
            {
                switch (this.cbPriority.SelectedIndex)
                {
                    case 0:
                        return ThreadPriority.Highest;
                    case 1:
                        return ThreadPriority.AboveNormal;
                    case 2:
                        return ThreadPriority.Normal;
                    case 3:
                        return ThreadPriority.BelowNormal;
                    case 4:
                        return ThreadPriority.Lowest;
                    default:
                        throw new InvalidOperationException("Invalid index in cbPriority");
                }
            }
        }

        #endregion

        #region Form Methods

        private void HashChecker_Load(object sender, EventArgs e)
        {
            if (this.cmdlineFName != null)
            {
                // Parse hashFile(cmdlineFName) and verify hashes
                if (File.Exists(this.cmdlineFName))
                {
                    this.tbChSumFile.Text = this.cmdlineFName;
                    this.tbDir.Text = Utils.GetFileDirectory(this.cmdlineFName);
                    this.panelSetUp.Enabled = false;
                    this.bStop.Enabled = true;
                    this.rtbLog.Clear();
                    this.StartValidatingAsync();
                }
                else
                {
                    CustomMessageBoxes.Error(string.Format("Specified hashfile '{0}' doesn't exist", this.cmdlineFName));
                }
            }
        }

        private void bQCheck_Click(object sender, EventArgs e)
        {
            var openFileDlg1 = new OpenFileDialog { Filter = ChecksumFileFilter };
            if ((openFileDlg1.ShowDialog() == DialogResult.OK) && File.Exists(openFileDlg1.FileName))
            {
                this.tbChSumFile.Text = openFileDlg1.FileName;
                this.tbDir.Text = Utils.GetFileDirectory(openFileDlg1.FileName);

                this.panelSetUp.Enabled = false;
                this.bStop.Enabled = true;
                this.rtbLog.Clear();
                this.StartValidatingAsync();
            }
        }        

        private void bBrowseFile_Click(object sender, EventArgs e)
        {
            var openFileDlg1 = new OpenFileDialog { Filter = ChecksumFileFilter };
            if ((openFileDlg1.ShowDialog() == DialogResult.OK) && File.Exists(openFileDlg1.FileName))
            {
                this.tbChSumFile.Text = openFileDlg1.FileName;
                this.tbDir.Text = Utils.GetFileDirectory(openFileDlg1.FileName);                
            }
        }

        private void bBrowseDir_Click(object sender, EventArgs e)
        {
            var fdbd1 = new FolderBrowserDialog();
            fdbd1.ShowNewFolderButton = false;
            if (fdbd1.ShowDialog() == DialogResult.OK)
            {
                this.tbDir.Text = fdbd1.SelectedPath;
            }
        }

        private void bChFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(this.tbChSumFile.Text))
                {
                    CustomMessageBoxes.Error(string.Format("Checksum file '{0}' not found", this.tbChSumFile.Text));
                }

                if (!Directory.Exists(this.tbDir.Text))
                {
                    CustomMessageBoxes.Error(string.Format("Root directory '{0}' not found", this.tbDir.Text));
                }

                this.panelSetUp.Enabled = false;
                this.bStop.Enabled = true;
                this.rtbLog.Clear();
                this.StartValidatingAsync();
            }
            catch (Exception ex)
            {
                CustomMessageBoxes.Error(ex.Message);
            }
        }        

        private void bStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.hashValidator != null)
                {
                    this.hashValidator.RequestStop();
                }
            }
            catch (Exception ex)
            {
                CustomMessageBoxes.Error(ex.Message);
            }
        }
        
        private void bClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                CustomMessageBoxes.Error(ex.Message);
            }
        }

        private void HashChecker_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.hashValidator != null && this.hashValidator.State == HashValidator.StateType.InProgress)
            {
                e.Cancel = true;
                this.exiting = true;
                this.hashValidator.Finished += this.CloseThreadSafe;
                this.hashValidator.RequestStop();
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

        private void HashChecker_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // TODO: move to options
                Settings.Default.ThreadPriority = this.cbPriority.SelectedIndex;
                Settings.Default.Save();
            }
            catch (System.Configuration.ConfigurationException ex)
            {
                CustomMessageBoxes.Error("Failed to save settings: " + ex.Message);
            }
        }
        
        private void bOptions_Click(object sender, EventArgs e)
        {
            new OptionsForm().ShowDialog();
        }

        #endregion

        private void ShowTotalProgressSafe(int processed, int total)
        {
            if (this.progressTotal.InvokeRequired)
            {
                this.Invoke((Action<int, int>)this.ShowTotalProgressSafe, processed, total);
            }
            else
            {
                this.progressTotal.Value = (processed * 100) / total;
                this.Text = string.Format("Hash Checker ({0}/{1} entries processed)", processed, total);
            }
        }

        private void ShowEntryProcessedProgressSafe(long bytesProcessed, long fileSize)
        {
            if (this.progressEntry.InvokeRequired)
            {
                this.Invoke((Action<long, long>)this.ShowEntryProcessedProgressSafe, bytesProcessed, fileSize);
            }
            else
            {
                if (bytesProcessed > fileSize)
                {
                    throw new ArgumentException(Resources.HashChecker_Must_not_be_bigger_than_fileSize, "bytesProcessed");
                }

                int percent;
                if (fileSize == 0 && bytesProcessed == 0)
                {
                    percent = 100;
                }
                else
                {
                    percent = (int)((bytesProcessed * 100) / fileSize);
                }

                this.progressEntry.Value = percent;
            }
        }

        private void ShowErrorThreadSafe(Exception ex)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action<Exception>)this.ShowErrorThreadSafe, ex);
            }
            else
            {
                CustomMessageBoxes.Error(ex.Message);
            }
        }

        private void bAbout_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }
    }
}
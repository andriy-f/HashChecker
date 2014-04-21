namespace HashCheckerProj
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;

    using HashCheckerProj.Properties;
    
    public partial class HashChecker : Form
    {
        #region Fields Consts
        
        // locals
        private readonly string cmdlineFName;

        private volatile bool exitAttempt;

        private Thread myThread;

        private int checkboxLogShowSelIndex = 1;

        private int filesProcessed, totalNumOfFilesToProcess;

        #endregion

        #region Constructors

        public HashChecker(ProgramMode mode, string filePath = null)
        {
            this.InitializeComponent();
            
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
                this.cbLogShow.SelectedIndex = Settings.Default.LogShow;
            }
            catch (System.Configuration.ConfigurationException)
            {
                CustomMessageBoxes.Warning("Failed to load settings, restoring defaults");
                this.cbPriority.SelectedIndex = 4;
                this.cbLogShow.SelectedIndex = 0;
#if DEBUG
                throw;
#endif
            }
        }

        #endregion

        private delegate void TextBoxAppendText(string text, Color color);

        #region Methods

        public enum EntryProcessingResult
        {
            NotFound = -1,
            Wrong = 0,
            Correct = 1
        }

        #region Cross Thread related

        private void RtbLogAppendText(string text)
        {
            if (this.rtbLog.InvokeRequired)
            {
                TextBoxAppendText d = this.RtbLogAppendText;
                this.Invoke(d, new object[] { text, Color.Black });
            }
            else
            {
                this.AppendText(this.rtbLog, Color.Black, text);
            }
        }

        private void RtbLogAppendText(string text, Color color)
        {
            if (this.rtbLog.InvokeRequired)
            {
                TextBoxAppendText d = this.RtbLogAppendText;
                this.Invoke(d, new object[] { text, color });
            }
            else
            {
                this.AppendText(this.rtbLog, color, text);
            }
        }
        
        private void AppendText(RichTextBox box, Color color, string text)
        {
            int start = box.TextLength;
            box.AppendText(text);
            int end = box.TextLength;

            // Textbox may transform chars, so (end-start) != text.Length
            box.Select(start, end - start);
            {
                box.SelectionColor = color;
                //// Could set box.SelectionBackColor, box.SelectionFont too.
            }

            box.SelectionLength = 0; // clear
        }

        private void SetFormToNormal()
        {
            if (this.panel1.InvokeRequired)
            {
                Action d = () => this.panel1.Enabled = true;
                this.Invoke(d, null);
            }
            else
            {
                this.panel1.Enabled = true;
            }

            if (this.bStop.InvokeRequired)
            {
                Action d = delegate { this.bStop.Enabled = false; };
                this.Invoke(d, null);
            }
            else
            {
                this.bStop.Enabled = false;
            }

            if (this.InvokeRequired)
            {
                Action d = delegate { this.Text = Resources.HashChecker_setFormToNormal_Hash_Checker; };
                this.Invoke(d, null);
            }
            else
            {
                this.Text = Resources.HashChecker_setFormToNormal_Hash_Checker;
            }
        }

        private void DisplayProgressThreadSafe(long bytesProcessed, long totalBytesProcessed)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action<long, long>)this.DisplayProgressThreadSafe, new object[] { bytesProcessed, totalBytesProcessed });
            }
            else
            {
                this.Text = string.Format(
                    "Hash Checker (Progress: {0}/{1} files, {2}/{3} bytes of current file)", 
                    this.filesProcessed, 
                    this.totalNumOfFilesToProcess,
                    bytesProcessed,
                    totalBytesProcessed);
            }
        }

        private void ProgressStep()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)this.ProgressStep, null);
            }
            else
            {
                this.Text = string.Format("Hash Checker ({0}/{1} entries processed)", ++this.filesProcessed, this.totalNumOfFilesToProcess);
            }
        }

        private void InitProgress()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)this.InitProgress, null);
            }
            else
            {
                this.filesProcessed = 0;
                this.Text = string.Format("Hash Checker ({0}/{1} entries processed)", 0, this.totalNumOfFilesToProcess);
            }
        }

        #endregion

        private void CallMyThread()
        {
            this.myThread = new Thread(this.PerformCheck) { Priority = this.SelectedThreadPriority, IsBackground = true };
            this.myThread.Start();
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

        /// <summary>
        /// Method for calling inside new thread
        /// </summary>
        private void PerformCheck()
        {
            try
            {
                var checksumFile = new ChecksumFile(this.tbChSumFile.Text);
                var entries = checksumFile.Parse().ToArray();
                this.totalNumOfFilesToProcess = entries.Count();
                this.InitProgress();
                
                var hchRes = new HashCheckRes(); 

                foreach (var entry in entries)
                {
                    string hashType = entry.ChecksumType ?? checksumFile.Ext;
                    var result = this.ValidateHashAndLog(entry.Path, hashType, entry.Hash);
                    hchRes.Append(result);
                    this.ProgressStep();
                }

                // Display message
                var message = string.Format(
                    "{0}Correct: {1}, Wrong: {2}, Not Found: {3}, Total: {4}",
                    Environment.NewLine,
                    hchRes.Correct,
                    hchRes.Wrong,
                    hchRes.NotFound,
                    hchRes.GetSum());
                var color = hchRes.Wrong == 0 ? (hchRes.NotFound == 0 ? Color.Black : Color.BlueViolet) : Color.Red;
                this.RtbLogAppendText(message, color);
            }
            catch (ThreadAbortException)
            {
                if (this.exitAttempt)
                {
                    return;
                }

                this.RtbLogAppendText("Check Aborted", Color.Red);
                ////MessageBox.Show(String.Format("{0}", ex));
            }
            catch (Exception)
            {
                 CustomMessageBoxes.Error("Error while performing check: Probably invalid file format");   
            }
            finally
            {
                if (!this.exitAttempt)
                {
                    this.SetFormToNormal();                    
                }
            }
        }

        /// <summary>
        /// Validate entry in checksum file
        /// </summary>
        /// <param name="entry">Path to file which will be validated</param>
        /// <param name="hashtype">Type of hash</param>
        /// <param name="hash">Hex string</param>
        /// <returns></returns>
        private EntryProcessingResult ValidateHashAndLog(string entry, string hashtype, string hash)
        {
            string fname = entry;
            if (File.Exists(fname) || File.Exists(fname = this.tbDir.Text + "\\" + fname))
            {
                var fi = new FileInfo(fname);
                switch (this.checkboxLogShowSelIndex)
                {
                    case 0: this.RtbLogAppendText(entry + "... "); 
                        break;
                    case 1: this.RtbLogAppendText(fi.FullName + "... "); 
                        break;
                    case 2: this.RtbLogAppendText(fi.Name + "... "); 
                        break;
                }

                bool correct = this.ValidateFileHash(fname, hashtype, hash);
                this.RtbLogAppendText((correct ? "OK" : "WRONG!") + Environment.NewLine, correct ? Color.Black : Color.Red);
                return correct ? EntryProcessingResult.Correct : EntryProcessingResult.Wrong;
            }
            else
            {
                this.RtbLogAppendText(entry + "... Not Found" + Environment.NewLine, Color.BlueViolet);
                return EntryProcessingResult.NotFound;
            }
        }

        private bool ValidateFileHash(string fname, string hashtype, string hash)
        {
            var validator = new FileHashCalculator { HashAlgorithm = CryptoUtils.ToHashAlgorithm(hashtype) };
            validator.ChunkProcessed += this.DisplayProgressThreadSafe;

            return ConvertUtils.ByteArraysEqual(
                validator.CalculateFileHash(fname),
                ConvertUtils.ToBytes(hash));
        }

        #endregion

        #region Form Methods

        private void HashChecker_Load(object sender, EventArgs e)
        {
            if (this.cmdlineFName != null)
            {
                // Parse hashFile(cmdlineFName) and verify hashes
                if (File.Exists(cmdlineFName))
                {
                    this.tbChSumFile.Text = this.cmdlineFName;
                    this.tbDir.Text = Utils.GetFileDirectory(this.cmdlineFName);
                    this.panel1.Enabled = false;
                    this.bStop.Enabled = true;
                    this.rtbLog.Clear();
                    this.CallMyThread();
                }
                else
                {
                    CustomMessageBoxes.Error(string.Format("Specified hashfile '{0}' doesn't exist", cmdlineFName));
                }
            }
        }

        private void bQCheck_Click(object sender, EventArgs e)
        {
            var openFileDlg1 = new OpenFileDialog();
            openFileDlg1.Filter = @"All Supported|*.sfv;*.md5;*.sha;*.sha1;*.sha256;*.sha384;*.sha512|sfv|*.sfv|md5|*.md5|sha1|*.sha;*.sha1|sha256|*.sha256|sha384|*.sha384|sha512|*.sha512|All(*.*)|*.*";
            if ((openFileDlg1.ShowDialog() == DialogResult.OK) && File.Exists(openFileDlg1.FileName))
            {
                this.tbChSumFile.Text = openFileDlg1.FileName;
                this.tbDir.Text = Utils.GetFileDirectory(openFileDlg1.FileName);

                this.panel1.Enabled = false;
                this.bStop.Enabled = true;
                this.rtbLog.Clear();
                this.CallMyThread();
            }
        }        

        private void bBrowseFile_Click(object sender, EventArgs e)
        {
            var openFileDlg1 = new OpenFileDialog();
            openFileDlg1.Filter = @"All Supported|*.md5;*.sha;*.sha1;*.sha256;*.sha384;*.sha512|md5|*.md5|sha1|*.sha;*.sha1|sha256|*.sha256|sha384|*.sha384|sha512|*.sha512|All(*.*)|*.*";
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
            if (File.Exists(this.tbChSumFile.Text) &&
                (this.tbDir.Text == string.Empty || Directory.Exists(this.tbDir.Text)))
            {
                this.panel1.Enabled = false;
                this.bStop.Enabled = true;
                this.rtbLog.Clear();
                this.CallMyThread();
            }
        }        

        private void bStop_Click(object sender, EventArgs e)
        {
            if (this.myThread != null)
            {
                this.myThread.Abort();
                this.myThread = null;
            }
        }
        
        private void bClose_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        private void HashChecker_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.exitAttempt = true;
            if (this.myThread != null)
            {
                this.myThread.Abort();
                this.myThread = null;
            }            
        }

        private void HashChecker_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Settings.Default.ThreadPriority = cbPriority.SelectedIndex;
                Settings.Default.LogShow = this.cbLogShow.SelectedIndex;
                Settings.Default.Save();
            }
            catch (System.Configuration.ConfigurationException ex)
            {
                CustomMessageBoxes.Error("Failed to save settings: " + ex.Message);
            }
        }

        private void cbLogShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.checkboxLogShowSelIndex = this.cbLogShow.SelectedIndex;
        }
        
        private void bOptions_Click(object sender, EventArgs e)
        {
            new OptionsForm().ShowDialog();
        }

        private void bHelp_Click(object sender, EventArgs e)
        {
            CustomMessageBoxes.Info(
                string.Format(
                    "Simple usage:\nClick Options and select associations. After that "
                    + "you can just open hash files in Explorer"));
        }

        #endregion
        
        public struct Comp2ClipHashParams
        {
            public string Filename { get; set; }

            public string Hash { get; set; }

            public int Hashtype { get; set; }
        }

        public class HashCheckRes
        {
            public HashCheckRes()
            {
                this.Correct = 0; 
                this.Wrong = 0; 
                this.NotFound = 0;
            }

            public HashCheckRes(int correct, int wrong, int notFound)
            {
                this.Correct = correct;
                this.Wrong = wrong;
                this.NotFound = notFound;
            }

            public int Correct { get; private set; }

            public int Wrong { get; private set; }

            public int NotFound { get; private set; }

            public void Append(EntryProcessingResult res)
            {
                switch (res)
                {
                    case EntryProcessingResult.NotFound:
                        this.NotFound++;
                        break;
                    case EntryProcessingResult.Wrong:
                        this.Wrong++;
                        break;
                    case EntryProcessingResult.Correct:
                        this.Correct++;
                        break;
                }
            }

            public void Append(HashCheckRes other)
            {
                this.Correct += other.Correct;
                this.Wrong += other.Wrong;
                this.NotFound += other.NotFound;
            }

            public int GetSum()
            {
                return this.Correct + this.Wrong + this.NotFound;
            }
        }

        private void bAbout_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }
    }
}
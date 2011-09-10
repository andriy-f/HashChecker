using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using HashCheckerProj.Properties;
//using System.Globalization;


namespace HashCheckerProj
{
    public partial class HashChecker : Form
    {
        #region Fields Consts
        //consts
        private const int ChSumTypesCnt = 7;
        //private enum ChSumTypes { sfv, md5, sha1, sha256, sha384, sha512 };
        private readonly string[] ChSumTypeStrs = { "sfv", "md5", "sha", "sha1", "sha256", "sha384", "sha512" };


        //locals
        private volatile bool exitAttempt = false;
        private string cmdlineFName = null;      
        private Thread myThread=null;
        private int cbLogShowSelIndex = 1;
        private int progress, progrMax;
        private bool comp2clipboard=false;

        delegate void TextBoxAppendText(string text, Color color);
        //delegate void SomeAction();

        #endregion

        #region Constructors

        public HashChecker(string[] args)
        {
            InitializeComponent();

            if (args.Length > 0)
            {
                if (args[0] == "-comp2clipboard" && args.Length > 1)
                {
                    cmdlineFName = args[1];
                    comp2clipboard = true;
                }
                else cmdlineFName = args[0];
            }

            try
            {
                cbPriority.SelectedIndex = HashCheckerProj.Properties.Settings.Default.ThreadPriority;
                cbLogShow.SelectedIndex = HashCheckerProj.Properties.Settings.Default.LogShow;
            }
            catch (System.Configuration.ConfigurationException ex)
            {
                Program.DefMsgBoxWarning("Failed to load settings, restoring defaults");
                cbPriority.SelectedIndex = 4;
                cbLogShow.SelectedIndex = 0;
#if DEBUG
                throw;
#endif
            }
        }

        #endregion

        #region Methods

        #region Cross Thread related

        private void RtbLogAppendText(string text)
        {
            if (rtbLog.InvokeRequired)
            {
                TextBoxAppendText d = RtbLogAppendText;
                Invoke(d, new object[] { text, Color.Black });
            }
            else AppendText(rtbLog, Color.Black, text);
        }

        private void RtbLogAppendText(string text, Color color)
        {
            if (rtbLog.InvokeRequired)
            {
                TextBoxAppendText d = RtbLogAppendText;
                Invoke(d, new object[] { text, color });
            }
            else AppendText(rtbLog, color, text);
        }

        

        void AppendText(RichTextBox box, Color color, string text)
        {
            int start = box.TextLength;
            box.AppendText(text);
            int end = box.TextLength;

            // Textbox may transform chars, so (end-start) != text.Length
            box.Select(start, end - start);
            {
                box.SelectionColor = color;
                // could set box.SelectionBackColor, box.SelectionFont too.
            }
            box.SelectionLength = 0; // clear
        }

        private void setFormToNormal()
        {
            if (panel1.InvokeRequired)
            {
                //SomeAction d = new SomeAction(delegate(){panel1.Enabled = true;});
                //SomeAction d = delegate() { panel1.Enabled = true; };
                Action d= ()=> panel1.Enabled = true;
                Invoke(d, null);
            }
            else panel1.Enabled = true;

            if (bStop.InvokeRequired)
            {
                Action d = delegate() { bStop.Enabled = false; };
                Invoke(d, null);
            }
            else bStop.Enabled = false;

            if (InvokeRequired)
            {
                Action d = delegate() { this.Text = Resources.HashChecker_setFormToNormal_Hash_Checker; };
                Invoke(d, null);
            }
            else Text = Resources.HashChecker_setFormToNormal_Hash_Checker;
        }

        private void progressStep()
        {
            if (InvokeRequired)
            {
                //SomeAction d = ;
                Invoke((Action)progressStep, null);
            }
            else            
                Text = String.Format("Hash Checker ({0}/{1} entries processed)", ++progress, progrMax);
        }

        private void initProgress()
        {
            if (InvokeRequired)
            {
                //SomeAction d = initProgress;
                Invoke((Action)initProgress, null);
            }
            else
            {
                progress = 0;
                Text = String.Format("Hash Checker ({0}/{1} entries processed)", 0, progrMax);
            }
        }

        #endregion

        private void callMyThread()
        {
            myThread = new Thread(PerformCheck)
                           {Priority = cbIndex2Priority(cbPriority.SelectedIndex), IsBackground = true};
            myThread.Start();
        }

        private ThreadPriority cbIndex2Priority(int idx)
        {
            switch (idx)
            {
                case 0: return System.Threading.ThreadPriority.Highest;
                case 1: return System.Threading.ThreadPriority.AboveNormal;
                case 2: return System.Threading.ThreadPriority.Normal;
                case 3: return System.Threading.ThreadPriority.BelowNormal;
                case 4: return System.Threading.ThreadPriority.Lowest;
                default: throw new Exception("cbIndex2Priority wrong index");
            }
        }

        private string extractFileDir(string fName)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(fName);
            return fi.DirectoryName;
        }

        private void PerformClipboardCheck(Object ob)//for thread
        {
            try
            {
                Comp2ClipHashParams c2CParams = (Comp2ClipHashParams)ob;
                
                EntryProcessingResult res=chHashAndLog(c2CParams.Filename, c2CParams.Hashtype, c2CParams.Hash);
                switch(res)
                {
                    case EntryProcessingResult.NotFound:
                        Program.DefMsgBox(string.Format("Input file not found: {0}", 
                            c2CParams.Filename));
                        break;
                    case EntryProcessingResult.Wrong:
                        Program.DefMsgBox(string.Format("File '{0}' is CORRUPT.\n\nIt doesn't have hash {1}",
                            c2CParams.Filename, c2CParams.Hash));
                        break;
                    case EntryProcessingResult.Correct:
                        Program.DefMsgBox(string.Format("File '{0}' is OK:\n\n(Hash: {1})", 
                            c2CParams.Filename, c2CParams.Hash));
                        break;
                }
                Close();
            }
            catch (ThreadAbortException)
            {
                if (exitAttempt) return;
                RtbLogAppendText("Check Aborted", Color.Red);
            }
            finally
            {
                if (!exitAttempt)
                {
                    setFormToNormal();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hashLength">Length of string hash</param>
        /// <returns></returns>
        private int HashLength2HashType(int hashLength)
        {
            switch (hashLength)
            {
                case 8: //CRC32: 32 bit = 4 bytes = 8 characters
                    return 0;
                case 32: //md5: 128 bit = 16 bytes = 32 characters
                    return 1;
                case 40: //sha1: 160 bit = 20 bytes = 40 characters
                    return 3;
                case 64: //sha256: 256 bit = 32 bytes = 64 characters                      
                    return 4;
                case 96: //sha384: 384 bit = 48 bytes = 96 characters   
                    return 5;
                case 128: //sha512: 512 bit = 64 bytes = 128 characters   
                    return 6;
                default:
                    return -1;
            }
        }

        private void PerformCheck()//for thread
        {
            try
            {                
                string[] lines = File.ReadAllLines(tbChSumFile.Text, Encoding.Default);
                progrMax = lines.Length;
                initProgress();

                
                HashCheckRes hchRes = new HashCheckRes(); 

                System.IO.FileInfo fi = new System.IO.FileInfo(tbChSumFile.Text);                
                string chsumfExtLower = fi.Extension.Remove(0, 1).ToLowerInvariant();//ala "md5"
                //Identifying Checksum type from ext
                int DefChSumType = -1;//Unknown
                for (int i = 0; i < ChSumTypesCnt; i++)
                    if (chsumfExtLower == ChSumTypeStrs[i]) { DefChSumType = i; break; }
                
                foreach (string ln in lines)
                {
                    if (ln.Length > 0 && ln[0] != ';')
                    {
                        int i = 0;
                        int firstspace = ln.IndexOf(' ');
                        string hash;
                        string fname;//file with hashes
                        if (firstspace > -1)
                        {
                            string beforespace = ln.Substring(0, firstspace).ToLower();
                            while (i < ChSumTypesCnt)//checking if any line in file is UNIX-style
                            {
                                if (beforespace==ChSumTypeStrs[i])
                                {
                                    parseUNIXSString(ln, out fname, out hash);
                                    hchRes.append(chHashAndLog(fname, i, hash));
                                    break;
                                }
                                i++;
                            }
                        }
                        if (i == ChSumTypesCnt)//string ln is not UNIX-style -> ChSumType is defined by extension (DefChSumType)
                        {
                            if (DefChSumType == 0)
                                parseSFVString(ln, out fname, out hash);                            
                            else 
                                parseDefString(ln, out fname, out hash);
                            hchRes.append(chHashAndLog(fname, DefChSumType, hash));                            
                        }
                    }
                    progressStep();
                }
                RtbLogAppendText(String.Format(Environment.NewLine+"Correct: {0}, Wrong: {1}, Not Found: {2}, " +
                                               "Total: {3}", 
                                               hchRes.correct, 
                                               hchRes.wrong, 
                                               hchRes.notfound,
                                               hchRes.getSum()), 
                                               hchRes.wrong==0?
                                               (hchRes.notfound==0?Color.Black:Color.BlueViolet)
                                               :Color.Red);
            }
            catch (ThreadAbortException)
            {
                if (exitAttempt) return;
                RtbLogAppendText("Check Aborted", Color.Red);
                //MessageBox.Show(String.Format("{0}", ex));
            }
            catch(Exception ex)
            {
                 Utils.MsgBoxError("Error while performing check: "+ex.Message);   
            }
            finally
            {
                if (!exitAttempt)
                {
                    setFormToNormal();                    
                }
            }
        }

        private void parseUNIXSString(string s, out string fname, out string hash)//parses UNIX-style string for fname and hash
        {            
            if ((!s.Contains("(")) || (!s.Contains("(")) || (!s.Contains("="))) { throw new Exception("Bad string format"); /*return;*/ }
            int stbracket = s.IndexOf('(');//Start bracket
            fname = s.Substring(stbracket + 1, s.IndexOf(')') - stbracket - 1);
            int steq = s.IndexOf("=");//first index of '='
            hash = s.Substring(steq + 1).TrimStart(' ');
        }

        private void parseDefString(string s, out string fname, out string hash)//parses Default string for fname and hash
        {
            int stbracket = s.IndexOf(' ');
            hash = s.Substring(0, stbracket);
            fname = s.Substring(stbracket + 1);
            if (fname[0] == '*') { fname = fname.Substring(1); }
            fname = fname.Trim();
        }

        private void parseSFVString(string s, out string fname, out string hash)//parses Default string for fname and hash
        {
            int stbracket = s.LastIndexOf(' ');
            fname = s.Substring(0, stbracket);
            hash = s.Substring(stbracket + 1);
            if (hash[0] == '*') { fname = fname.Substring(1); }
            fname = fname.Trim();
            hash = hash.Trim();
        }

        private EntryProcessingResult chHashAndLog(string entry, int hashtype, string hash)//entry - fileentry in ChSumFile
        {
            string fname = entry;
            if (File.Exists(fname) || File.Exists(fname = tbDir.Text + "\\" + fname))
            {
                FileInfo fi = new System.IO.FileInfo(fname);
                switch (cbLogShowSelIndex)
                {
                    case 0: RtbLogAppendText(entry + "... "); 
                        break;
                    case 1: RtbLogAppendText(fi.FullName + "... "); 
                        break;
                    case 2: RtbLogAppendText(fi.Name + "... "); 
                        break;
                }
                bool correct=CheckFileHash(fname, hashtype, hash);
                RtbLogAppendText((correct ? "OK" : "WRONG!") + Environment.NewLine, correct ? Color.Black : Color.Red);
                return correct ? EntryProcessingResult.Correct : EntryProcessingResult.Wrong;
            }
            else
            {
                RtbLogAppendText(entry + "... Not Found" + Environment.NewLine, Color.BlueViolet);
                return EntryProcessingResult.NotFound;
            }
        }

        private bool CheckFileHash(string fname, int hashtype, string hash)
        {
            using (Stream sr = new FileStream(fname, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                StringBuilder sBuilder = new StringBuilder();
                byte[] result;                
                switch (hashtype)
                {
                    case 0://sfv - CRC32                        
                        CRC32 crc32 = new CRC32();                        
                        result = crc32.ComputeHash(sr);
                        break;
                    case 1://md5
                        MD5 md5Hasher = MD5.Create();
                        result = md5Hasher.ComputeHash(sr);
                        break;
                    case 2: case 3://sha or sha1
                        SHA1 sha1 = new SHA1CryptoServiceProvider();
                        result = sha1.ComputeHash(sr);
                        break;
                    case 4://sha256                        
                        SHA256 sha256 = new SHA256Managed();
                        result = sha256.ComputeHash(sr);
                        break;
                    case 5://sha384
                        SHA384 sha384 = new SHA384Managed();
                        result = sha384.ComputeHash(sr);
                        break;
                    case 6://sha512
                        SHA512 sha512 = new SHA512Managed();
                        result = sha512.ComputeHash(sr);
                        break;
                    default:
                        return false;
                }
                for (int i = 0; i < result.Length; i++)
                { sBuilder.Append(result[i].ToString("x2")); }

                hash=hash.ToLower();
                return (String.Compare(sBuilder.ToString(), hash) == 0);
            }
        }        

        #endregion

        #region Form Methods

        private void HashChecker_Load(object sender, EventArgs e)
        {
            if (comp2clipboard)//Compare file to clipboard hash string
            {
                ClipboardCompare();
            }
            else if (cmdlineFName != null)//Parse hashFile(cmdlineFName) and verify hashes
            {
                if (File.Exists(cmdlineFName))
                {
                    tbChSumFile.Text = cmdlineFName;
                    tbDir.Text = extractFileDir(cmdlineFName);
                    panel1.Enabled = false;
                    bStop.Enabled = true;
                    rtbLog.Clear();
                    callMyThread();
                }
                else
                    Utils.MsgBoxError(String.Format("Specified hashfile '{0}' doesn't exist", cmdlineFName));
            }
        }

        private void bQCheck_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg1 = new OpenFileDialog();
            openFileDlg1.Filter = "All Supported|*.sfv;*.md5;*.sha;*.sha1;*.sha256;*.sha384;*.sha512|sfv|*.sfv|md5|*.md5|sha1|*.sha;*.sha1|sha256|*.sha256|sha384|*.sha384|sha512|*.sha512|All(*.*)|*.*";
            if ((openFileDlg1.ShowDialog() == DialogResult.OK) && (System.IO.File.Exists(openFileDlg1.FileName)))
            {
                tbChSumFile.Text = openFileDlg1.FileName;
                tbDir.Text = extractFileDir(openFileDlg1.FileName);

                panel1.Enabled = false;
                bStop.Enabled = true;
                rtbLog.Clear();
                callMyThread();
            }
        }        

        private void bBrowseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg1 = new OpenFileDialog();
            openFileDlg1.Filter = "All Supported|*.md5;*.sha;*.sha1;*.sha256;*.sha384;*.sha512|md5|*.md5|sha1|*.sha;*.sha1|sha256|*.sha256|sha384|*.sha384|sha512|*.sha512|All(*.*)|*.*";
            if ((openFileDlg1.ShowDialog() == DialogResult.OK) && (System.IO.File.Exists(openFileDlg1.FileName)))
            {                
                tbChSumFile.Text = openFileDlg1.FileName;
                tbDir.Text = extractFileDir(openFileDlg1.FileName);                
            }
        }

        private void bBrowseDir_Click(object sender, EventArgs e)
        {            
            FolderBrowserDialog fdbd1 = new FolderBrowserDialog();
            fdbd1.ShowNewFolderButton = false;
            if (fdbd1.ShowDialog() == DialogResult.OK) tbDir.Text = fdbd1.SelectedPath;
        }

        private void bChFile_Click(object sender, EventArgs e)
        {
            if (File.Exists(tbChSumFile.Text) &&
                (tbDir.Text == "" || Directory.Exists(tbDir.Text)))
            {
                panel1.Enabled = false;
                bStop.Enabled = true;
                rtbLog.Clear();
                callMyThread();
            }
        }        

        private void bStop_Click(object sender, EventArgs e)
        {            
            if (myThread != null)
            {
                myThread.Abort();
                myThread = null;
            }
        }
        
        private void bClose_Click(object sender, EventArgs e)
        {            
            Close();
        }

        private void HashChecker_FormClosing(object sender, FormClosingEventArgs e)
        {
            exitAttempt = true;
            if (myThread != null)
            {
                myThread.Abort();
                myThread = null;
            }            
        }

        private void HashChecker_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Settings.Default.ThreadPriority = cbPriority.SelectedIndex;
                Settings.Default.LogShow = cbLogShow.SelectedIndex;
                Settings.Default.Save();
            }
            catch (System.Configuration.ConfigurationException ex)
            {
                Utils.MsgBoxError("Failed to save settings: "+ex.Message);
            }
        }

        private void cbLogShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbLogShowSelIndex = cbLogShow.SelectedIndex;
        }

        

        private void bOptions_Click(object sender, EventArgs e)
        {
            new OptionsForm().ShowDialog();
        }

        private void bHelp_Click(object sender, EventArgs e)
        {
            Utils.MsgBoxInfo(string.Format("Simple usage:\nClick Options and select associations. After that " +
                                          "you can just open hash files in Explorer" +
                                          "\nAuthor: {0}", Application.CompanyName));
        }

        #endregion

        private void ClipboardCompare()
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    string hash = Clipboard.GetText();
                    int hashtype = HashLength2HashType(hash.Length);
                    if (hashtype != -1)
                    {
                        tbChSumFile.Text = "";
                        tbDir.Text = "";
                        panel1.Enabled = false;
                        bStop.Enabled = true;
                        rtbLog.Clear();

                        myThread = new Thread(PerformClipboardCheck);
                        myThread.Start(new Comp2ClipHashParams
                                           {
                                               Filename = cmdlineFName,
                                               Hash = hash,
                                               Hashtype = hashtype
                                           });

                    }
                    else
                    {
                        Utils.MsgBoxExclamation("That's not a hash in Clipboard");
                        Application.Exit();
                    }

                }
                else
                {
                    Utils.MsgBoxExclamation("No hash in Clipboard to compare");
                    Application.Exit();
                }
            }
            catch(Exception ex)
            {
                Utils.MsgBoxError("Error while comparing to hash in clipboard: "+ex.Message);
            }
        }
        
        void DefMsgBoxInvoker(string str)
        {
            if (InvokeRequired)
            {
                Action<string> d = DefMsgBoxInvoker;
                Invoke(d, new object[] { str });
            }
            else
                Utils.MsgBoxInfo(str);
            
        }
    }

    #region Class HashCheckRes

    public enum EntryProcessingResult
    {
        NotFound = -1,
        Wrong = 0,
        Correct = 1
    };

    public class HashCheckRes
    {
        public int correct, wrong, notfound;

        public HashCheckRes()
        { correct = 0; wrong = 0; notfound = 0; }
        public HashCheckRes(int _correct, int _wrong, int _notfound)
        { correct = _correct; wrong = _wrong; notfound = _notfound; }

        public void append(EntryProcessingResult res)
        {
            switch (res)
            {
                case EntryProcessingResult.NotFound: notfound++; break;
                case EntryProcessingResult.Wrong: wrong++; break;
                case EntryProcessingResult.Correct: correct++; break;
            }
        }

        public void append(HashCheckRes other)
        { correct += other.correct; wrong += other.wrong; notfound += other.notfound; }

        public int getSum()
        { return correct + wrong + notfound; }
    }

    #endregion

    #region Comp2ClipHashParams

    class Comp2ClipHashParams
    {
        public string Filename;
        public string Hash;
        public int Hashtype;
        //public string Hash { get; set; }
    }

    #endregion
}
namespace HashChecker.WinForms
{
    using System;
    using System.Windows.Forms;
    using global::HashChecker.WinForms.Properties;
    using Microsoft.Win32;

    public partial class OptionsForm : Form
    {
        const string RootPath = @"HKEY_CURRENT_USER\";
        const string SoftClassesPath = @"Software\Classes\";
        private const string FullClassesPath = RootPath + SoftClassesPath;
        const string HchkEntry = @"HashChecker.Hash";
        private const string Shellopencommand = @"shell\open\command";
        private readonly string _iconRegValue = "\"" + Application.ExecutablePath + "\",0";
        private readonly string _shellopencommandRegValue = "\"" + Application.ExecutablePath + "\" \"%1\"";
        private readonly string _shellCmp2HshClpbrdRegValue = "\"" + Application.ExecutablePath + 
                                                                "\" \"-comp2clipboard\" \"%1\"";

        private const string Cmp2HshCommandFullPath = RootPath + Cmp2HshPath + @"\command";
        const string Cmp2HshPath = @"Software\Classes\*\shell\Compare to hash in Clipboard";

        public OptionsForm()
        {
            this.InitializeComponent();

            //Verify what is associated with this app
            if ((string)Registry.GetValue(FullClassesPath + HchkEntry + @"\" + Shellopencommand, "", "def") ==
                this._shellopencommandRegValue)
            {
                for (int i = 0; i < 7; i++)
                    this.clbAssoc.SetItemChecked(i,
                                            (string)
                                            Registry.GetValue(RootPath + SoftClassesPath + this.clbIndex2HashFileType(i), 
                                            "", "def") == HchkEntry);
            }
            else
                for (int i = 0; i < 7; i++)
                    this.clbAssoc.SetItemChecked(i, false);

            //Compare to hash in Clipboard File Context entry
            this.cbAddComp2Clip.Checked=(
                                            (string)
                                            Registry.GetValue(Cmp2HshCommandFullPath,
                                            "", "def") == this._shellCmp2HshClpbrdRegValue);
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            try
            {
                //HashChecker common entry
                RegistryKey hchkEntryKey = Registry.CurrentUser.CreateSubKey(SoftClassesPath + HchkEntry);
                if (hchkEntryKey != null)
                {
                    hchkEntryKey.CreateSubKey("DefaultIcon").SetValue("", this._iconRegValue);
                    
                    hchkEntryKey.CreateSubKey(Shellopencommand).SetValue("", this._shellopencommandRegValue);
                    
                    hchkEntryKey.Close();
                }

                // File associations
                for (int i = 0; i < 7; i++)
                    if (this.clbAssoc.GetItemChecked(i))
                        Registry.SetValue(FullClassesPath + this.clbIndex2HashFileType(i), string.Empty, HchkEntry);

                // Compare to hash in Clipboard file entry
                if(this.cbAddComp2Clip.Checked)
                    Registry.SetValue(Cmp2HshCommandFullPath, "", this._shellCmp2HshClpbrdRegValue);
                else this.DelSubKeyTreeNn(Cmp2HshPath);
            }
            catch (Exception)
            {
                CustomMessageBoxes.Error(Resources.OptionsForm_bOK_Click_Error_writing_to_registry_while_saving_data);
#if DEBUG
                throw;
#endif
            }
            
            this.Close();
        }

        private string clbIndex2HashFileType(int i)
        {
            switch (i)
            {
                case 0:
                    return ".sfv";
                case 1:
                    return ".md5";
                case 2:
                    return ".sha";
                case 3:
                    return ".sha1";
                case 4:
                    return ".sha256";
                case 5:
                    return ".sha384";
                case 6:
                    return ".sha512";
                default:
                    return null;
            }
        }

        private void DelSubKeyTreeNn(string path)
        {
            try
            {
                Registry.CurrentUser.DeleteSubKeyTree(path);
            }
            catch (ArgumentException)
            {
                //All right, path doesn't exist
            }
        }
    }


}

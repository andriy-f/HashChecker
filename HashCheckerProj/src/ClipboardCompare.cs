using System.Windows.Forms;
using HashCheckerProj;

namespace System.Text
{
    public class ClipboardCompare//Take: hash, fname, hashtype, Returns:Text 2 
    {
        private string fname;
        private string hash;

        public void Work(string fname)
        {
            if (Clipboard.ContainsText())
            {
                hash = Clipboard.GetText();
                //switch(hash.Length)
                //{
                //    case 8://CRC32

                //}
            }
            else Program.DefMsgBox("No hash in Clipboard to compare");
        }
    }
}
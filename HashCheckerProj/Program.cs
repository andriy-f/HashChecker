using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HashCheckerProj
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //MessageBox.Show(string.Format("{0}-{1}", Application.ExecutablePath, Application.StartupPath));
                Application.Run(new HashChecker(args));
            }
            catch (Exception ex)
            {
                DefMsgBox(ex.Message);
#if DEBUG
                throw;
#endif
            }
        }

        internal static void DefMsgBox(string str)
        {
            MessageBox.Show(str, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        internal static void DefMsgBoxAndExit(string str)
        {
            MessageBox.Show(str, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

        internal static void DefMsgBoxWarning(string str)
        {
            MessageBox.Show(str, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        internal static void DefMsgBoxError(string str)
        {
            MessageBox.Show(str, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

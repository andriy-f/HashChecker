namespace HashCheckerProj
{
    using System;
    using System.Windows.Forms;

    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new HashChecker(args));
            }
            catch (Exception ex)
            {
                CustomMessageBoxes.Simple(ex.Message);
#if DEBUG
                throw;
#endif
            }
        }
    }
}

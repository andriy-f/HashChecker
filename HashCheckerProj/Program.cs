namespace HashChecker.WinForms
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using global::HashChecker.WinForms.Properties;

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
                AppDomain.CurrentDomain.UnhandledException += (sender, e) => HandleException(e.ExceptionObject as Exception);
                Application.ThreadException += (sender, e) => HandleException(e.Exception);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                switch (args.Length)
                {
                    case 0:
                        Application.Run(new HashChecker(ProgramMode.Standard));
                        break;
                    case 1:
                        if (args[0] == "/h" || args[0] == "-h")
                        {
                            CustomMessageBoxes.Info(Resources.CommandLineArgumentsHelp);
                        }
                        else
                        {
                            if (!File.Exists(args[0]))
                            {
                                throw new Exception("First command line argument provided should be valid checksum file path");
                            }

                            Application.Run(new HashChecker(ProgramMode.ValidateChecksumFile, args[0]));
                        }

                        break;
                    case 2:
                        if (args[0] == "-comp2clipboard" || args[0] == "/comp2clipboard")
                        {
                            Application.Run(new SingleFileCheckForm(args[1]));
                        }
                        else
                        {
                            CustomMessageBoxes.Error(string.Format("Invalid program arguments: '{0}', execute <exename> /h for help", string.Join(" ", args)));
                        }

                        break;
                    default:
                        CustomMessageBoxes.Error(string.Format("Invalid program arguments: '{0}', execute <exename> /h for help", string.Join(" ", args)));
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomMessageBoxes.Error(ex.Message);
#if DEBUG
                throw;
#endif
            }
        }

        public static void HandleException(Exception ex)
        {
            CustomMessageBoxes.Error(ex.Message);
        }
    }
}

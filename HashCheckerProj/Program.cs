namespace HashCheckerProj
{
    using System;
    using System.Linq;
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

                switch (args.Length)
                {
                    case 0:
                        Application.Run(new HashChecker(ProgramMode.Standard));
                        break;
                    case 1:
                        if (args[0] == "/h" || args[0] == "-h")
                        {
                            CustomMessageBoxes.Info(@"
Command line arguments:
no arguments : Standatd interface
<path to exe> <filepath> : validate entries in <filepath> checksum file;
<path to exe> -comp2clipboard <filepath> : chack if file <filepath> has hash equal to hash in clipboard
<path to exe> /h : show this help");
                        }
                        else
                        {
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
    }
}

namespace HashChecker.WinForms
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using global::HashChecker.WinForms.Properties;
    using SimpleInjector;
    using SimpleInjector.Diagnostics;

    public static class Program
    {
        private static Container container;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                SetUpErrorHandling();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                SetUpDI();

                switch (args.Length)
                {
                    case 0:
                        var mainForm = container.GetInstance<HashChecker>();
                        mainForm.Mode = ProgramMode.Standard;
                        
                        Application.Run(mainForm);
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

                            var form = container.GetInstance<HashChecker>(); 
                            form.Mode = ProgramMode.ValidateChecksumFile;
                            form.ChecksumFileToCheck = args[0];
                            Application.Run(form);
                        }

                        break;
                    case 2:
                        if (args[0] == "-comp2clipboard" || args[0] == "/comp2clipboard")
                        {
                            var singleFileCheckForm = container.GetInstance<SingleFileCheckForm>();
                            singleFileCheckForm.FileToValidate = args[1];

                            Application.Run(singleFileCheckForm);
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

        private static void SetUpErrorHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => HandleException(e.ExceptionObject as Exception);
            Application.ThreadException += (sender, e) => HandleException(e.Exception);
        }

        private static void SetUpDI()
        {
            // Create the container as usual.
            container = new Container();

            var singleFileCheckFormiRegistration = Lifestyle.Transient.CreateRegistration(typeof(SingleFileCheckForm), container);
            container.AddRegistration(typeof(SingleFileCheckForm), singleFileCheckFormiRegistration);
            singleFileCheckFormiRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Windows Form (SingleFileCheckForm) will be automatically disposed by runtime as it is registered using Application.Run()");

            var mainFormRegistration = Lifestyle.Transient.CreateRegistration(typeof(HashChecker), container);
            container.AddRegistration(typeof(HashChecker), mainFormRegistration);
            mainFormRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Main Windows Form (HashChecker) will be automatically disposed by runtime as it is registered using Application.Run()");

            // Optionally verify the container.
            container.Verify();
        }
    }
}

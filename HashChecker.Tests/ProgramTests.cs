namespace HashChecker.Tests
{
    using System;
    using System.IO;
    using System.Reflection;

    using NUnit.Framework;

    [TestFixture]
    public class ProgramTests
    {
        private const string File1Path = @"Data\file01.bin";
        private const string NonExistingPath = @"Data\NoFile.newerwhere";
        private const string File1Md5HashUpper = "5738077ABBE757E9D2DA3741115074B6";
        private const string File1Md5HashInvalidFormat = "5738077ABBE757E9D2DA3741115074B67";
        private const string File1Md5HashInvalid = "5738077ABBE757E9D2DA3741115074B8";

        [Test]
        [STAThread]
        public void MainTest()
        {
            var uriDllPath = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            string assemblyFolder = Path.GetDirectoryName(uriDllPath.LocalPath);

            Assert.NotNull(assemblyFolder);

            var file1FullPath = Path.Combine(assemblyFolder, File1Path);
            System.Windows.Forms.Clipboard.SetText(File1Md5HashUpper);
            HashCheckerProj.Program.Main(new[] { "-comp2clipboard", file1FullPath });
        }

        [Test]
        [STAThread]
        public void MainTestWrong()
        {
            var uriDllPath = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            string assemblyFolder = Path.GetDirectoryName(uriDllPath.LocalPath);

            Assert.NotNull(assemblyFolder);

            var file1FullPath = Path.Combine(assemblyFolder, File1Path);
            System.Windows.Forms.Clipboard.SetText(File1Md5HashInvalid);
            HashCheckerProj.Program.Main(new[] { "-comp2clipboard", file1FullPath });
        }

        [Test]
        [STAThread]
        public void MainTestNotFound()
        {
            // TODO: fix
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Assert.NotNull(assemblyFolder);
            var file1FullPath = Path.Combine(assemblyFolder, NonExistingPath);
            System.Windows.Forms.Clipboard.SetText(File1Md5HashInvalid);
            HashCheckerProj.Program.Main(new[] { "-comp2clipboard", file1FullPath });
        }
    }
}

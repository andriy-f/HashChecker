namespace HashChecker.Core
{
    using System.IO;

    public static class Utils
    {
        public static string GetFileDirectory(string fileName)
        {
            var fi = new FileInfo(fileName);
            return fi.DirectoryName;
        }
    }
}

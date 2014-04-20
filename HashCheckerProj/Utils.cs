using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCheckerProj
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

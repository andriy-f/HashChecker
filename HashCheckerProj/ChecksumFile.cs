namespace HashCheckerProj
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Class for prsing checksum files
    /// TODO:
    /// 1. Throw exception if not checksum file (for example, *.bin)
    /// </summary>
    public class ChecksumFile
    {
        public ChecksumFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Checksum file not found", filePath);
            }
            
            this.FilePath = filePath;
            var fileInfo = new FileInfo(this.FilePath);
            this.Ext = string.IsNullOrEmpty(fileInfo.Extension)
                           ? null
                           : fileInfo.Extension.Substring(1).ToLowerInvariant();
        }

        /// <summary>
        /// .sfv, .md5, sha, sha1, etc file path
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Lovercase file extension ('md5', for example)
        /// </summary>
        public string Ext { get; private set; }
        
        /// <summary>
        /// Parse Checksum File
        /// </summary>
        public IEnumerable<ChecksumFileEntry> Parse()
        {
            var entries = new List<ChecksumFileEntry>();
            
            using (var contents = new StreamReader(this.FilePath))
            {
                while (!contents.EndOfStream)
                {
                    var line = contents.ReadLine();
                    if (string.IsNullOrEmpty(line) || line[0] == ';')
                    {
                        continue;
                    }

                    if (IsUnixTypeEntry(line))
                    {
                        entries.Add(ChecksumFileEntry.ParseUnixLine(line));
                    }
                    else if (CryptoUtils.IsChecksumType(this.Ext))
                    {
                        // string line is not UNIX-style -> ChSumType is defined by extension (DefChSumType)
                        entries.Add(this.Ext == "sfv" ? ChecksumFileEntry.ParseSfvFileLine(line) : ChecksumFileEntry.ParseDefaultLine(line));
                    }
                }
            }

            return entries;
        }

        /// <summary>
        /// Check if line is UNIX-style,
        /// Example: MD5 (filename.ext) = hash
        /// </summary>
        /// <param name="line">checksum file line</param>
        /// <returns></returns>
        private static bool IsUnixTypeEntry(string line)
        {
            int firstSpaceIndex = line.IndexOf(' ');
            if (firstSpaceIndex > -1)
            {
                string linePrefix = line.Substring(0, firstSpaceIndex).ToLower();
                if (CryptoUtils.IsChecksumType(linePrefix))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

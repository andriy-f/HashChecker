namespace HashCheckerProj
{
    using System;
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
        public IEnumerable<Entry> Parse()
        {
            var entries = new List<Entry>();
            
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
                        entries.Add(Entry.ParseUnixLine(line));
                    }
                    else if (CryptoUtils.IsChecksumType(this.Ext))
                    {
                        // string line is not UNIX-style -> ChSumType is defined by extension (DefChSumType)
                        entries.Add(this.Ext == "sfv" ? Entry.ParseSfvFileLine(line) : Entry.ParseDefaultLine(line));
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

        public struct Entry
        {
            public string Hash { get; set; }

            public string Path { get; set; }

            public string ChecksumType { get; set; }

            /// <summary>
            /// Parses UNIX-style string for fname and hash 
            /// </summary>
            /// <param name="line">Format: SFV|MD5|SHA|SHA1|SHA256|sha384|SHA512 (path.ext) = hash</param>
            public static Entry ParseUnixLine(string line)
            {
                try
                {
                    int leftBracketIndex = line.IndexOf('('); // Start bracket
                    if (leftBracketIndex == -1)
                    {
                        throw new FormatException(string.Format("'{0}' line has invalid  format: no left bracket '('", line));
                    }

                    var hashType = line.Substring(0, leftBracketIndex).Trim();

                    int rightBracketIndex = line.IndexOf(')');
                    if (rightBracketIndex == -1)
                    {
                        throw new FormatException(string.Format("'{0}' line has invalid  format: no right bracket '('", line));
                    }

                    var entryPath = line.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1);
                    int equalIndex = line.IndexOf('='); // first index of '='
                    if (equalIndex == -1)
                    {
                        throw new FormatException(string.Format("'{0}' line has invalid  format: no equal sight '='", line));
                    }

                    var entryHash = line.Substring(equalIndex + 1).TrimStart(' ');

                    return new Entry { Hash = entryHash, Path = entryPath, ChecksumType = hashType };
                }
                catch (FormatException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new FormatException(string.Format("'{0}' line has invalid  format: should be 'SFV|MD5|SHA|SHA1|SHA256|sha384|SHA512 (path.ext) = hash'", line), ex);
                }
            }

            public static Entry ParseSfvFileLine(string line)
            {
                try
                {
                    string trimmedLine = line.Trim();
                    int lastWhitespaceIndex = trimmedLine.LastIndexOf(' ');
                    string fname = trimmedLine.Substring(0, lastWhitespaceIndex);
                    string hash = trimmedLine.Substring(lastWhitespaceIndex + 1);
                    if (hash[0] == '*')
                    {
                        fname = fname.Substring(1);
                    }

                    fname = fname.Trim();
                    hash = hash.Trim();

                    return new Entry { Hash = hash, Path = fname };
                }
                catch (FormatException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new FormatException(string.Format("'{0}' line has invalid  format: should be 'path.ext hash'", line), ex);
                }
            }

            /// <summary>
            /// Parse checksum file entry
            /// Samples: 
            ///     5738077ABBE757E9D2DA3741115074B6 *file01.bin
            ///     5738077ABBE757E9D2DA3741115074B6 file01.bin
            /// </summary>
            /// <param name="line"></param>
            public static Entry ParseDefaultLine(string line)
            {
                try
                {
                    string trimmedLine = line.Trim();
                    int firstWhitespaceIndex = trimmedLine.IndexOf(' ');
                    var hash = trimmedLine.Substring(0, firstWhitespaceIndex);
                    var fname = trimmedLine[firstWhitespaceIndex + 1] == '*'
                                    ? trimmedLine.Substring(firstWhitespaceIndex + 2)
                                    : trimmedLine.Substring(firstWhitespaceIndex + 1);
                    
                    fname = fname.Trim();

                    return new Entry { Hash = hash, Path = fname };
                }
                catch (FormatException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new FormatException(string.Format("'{0}' line has invalid  format: should be 'hash [*]path.ext'", line), ex);
                }
            }
        }
    }
}

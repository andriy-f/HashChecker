namespace HashCheckerProj
{
    using System;

    public struct ChecksumFileEntry
    {
        public string Hash { get; set; }

        public string Path { get; set; }

        public string HashType { get; set; }

        /// <summary>
        /// Parses UNIX-style string for fname and hash 
        /// </summary>
        /// <param name="line">Format: SFV|MD5|SHA|SHA1|SHA256|sha384|SHA512 (path.ext) = hash</param>
        public static ChecksumFileEntry ParseUnixLine(string line)
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

                return new ChecksumFileEntry { Hash = entryHash, Path = entryPath, HashType = hashType };
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

        public static ChecksumFileEntry ParseSfvFileLine(string line)
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

                return new ChecksumFileEntry { Hash = hash, Path = fname };
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
        public static ChecksumFileEntry ParseDefaultLine(string line)
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

                return new ChecksumFileEntry { Hash = hash, Path = fname };
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
namespace HashCheckerProj
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public class CryptoUtils
    {
        /// <summary>
        /// ValidateFileHashChunked
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="hashAlgorithm">MD5.Create()</param>
        /// <param name="hash"></param>
        /// <param name="reportProgress"></param>
        /// <returns></returns>
        public static bool ValidateFileHashChunked(string fname, HashAlgorithm hashAlgorithm, string hash, Action<long, long> reportProgress)
        {
            long totalBytesRead = 0;
            using (var cryptoStream = new CryptoStream(Stream.Null, hashAlgorithm, CryptoStreamMode.Write))
            {
                using (var fileStream = new FileStream(fname, FileMode.Open, FileAccess.Read))
                {
                    long fileByteLength = fileStream.Length;
                    var chunk = new byte[Consts.ChunkSize];
                    int bytesRead;
                    do
                    {
                        bytesRead = fileStream.Read(chunk, 0, Consts.ChunkSize);
                        totalBytesRead += bytesRead;
                        cryptoStream.Write(chunk, 0, bytesRead);
                        reportProgress(totalBytesRead, fileByteLength);
                    }
                    while (bytesRead > 0);
                }
            }

            return string.Compare(hash, ByteToHexBitFiddle(hashAlgorithm.Hash), StringComparison.OrdinalIgnoreCase) == 0;
        }

        private static string ByteToHexBitFiddle(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];
            int b;
            for (int i = 0; i < bytes.Length; i++)
            {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
            }

            return new string(c);
        }
    }
}

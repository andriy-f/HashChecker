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
        /// <param name="filePath"></param>
        /// <param name="hashAlgorithm">MD5.Create()</param>
        /// <param name="hash"></param>
        /// <param name="reportProgress"></param>
        /// <returns></returns>
        public static bool ValidateFileHashChunked(string filePath, HashAlgorithm hashAlgorithm, string hash, Action<long, long> reportProgress)
        {
            var fileHashValidator = new FileHashCalculator { HashAlgorithm = hashAlgorithm };
            fileHashValidator.ChunkProcessed += reportProgress;

            return ConvertUtils.ByteArraysEqual(
                fileHashValidator.CalculateFileHash(filePath),
                ConvertUtils.ToBytes(hash));
        }
    }
}

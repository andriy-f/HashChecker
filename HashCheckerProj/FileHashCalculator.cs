namespace HashCheckerProj
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    /// <summary>
    /// Calculate hash with progress
    /// </summary>
    public class FileHashCalculator
    {
        private volatile bool shouldStop;

        public FileHashCalculator()
        {
            this.ChunkSize = 64 * 1024 * 1024;
        }

        public event Action<long, long> ChunkProcessed;

        public HashAlgorithm HashAlgorithm { get; set; }

        public int ChunkSize { get; set; }
        
        public byte[] CalculateFileHash(string filePath)
        {
            if (HashAlgorithm == null)
            {
                throw new InvalidOperationException("'HashAlgorithm' must be initialized");
            }

            using (var cryptoStream = new CryptoStream(Stream.Null, HashAlgorithm, CryptoStreamMode.Write))
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    long fileLength = fileStream.Length;
                    long fileBytesRead = 0;
                    var chunk = new byte[this.ChunkSize];
                    int tmpBytesRead;
                    do
                    {
                        tmpBytesRead = fileStream.Read(chunk, 0, this.ChunkSize);
                        fileBytesRead += tmpBytesRead;
                        cryptoStream.Write(chunk, 0, tmpBytesRead);
                        this.OnChunkProcessed(fileBytesRead, fileLength);
                    }
                    while (!this.shouldStop && tmpBytesRead > 0);
                }
            }

            if (this.shouldStop)
            {
                return null;
            }

            return HashAlgorithm.Hash;
        }

        public void RequestStop()
        {
            this.shouldStop = true;
        }

        protected virtual void OnChunkProcessed(long bytesProcessed, long fileSize)
        {
            if (this.ChunkProcessed != null)
            {
                this.ChunkProcessed(bytesProcessed, fileSize);
            }
        }
    }
}

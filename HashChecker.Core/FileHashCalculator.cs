namespace HashChecker.Core
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    /// <summary>
    /// Calculate hash with progress
    /// TODO: Make thread safe
    /// </summary>
    public class FileHashCalculator
    {
        public const int DefaultChunkSize = 64 * 1024 * 1024;

        private volatile bool shouldStop;

        private volatile bool hashCalculated;

        private volatile StateType state;

        public FileHashCalculator(int chunkSize = DefaultChunkSize)
        {
            this.ChunkSize = chunkSize;
            this.state = StateType.Initial;
        }

        public event Action<long, long> ChunkProcessed;
        
        /// <summary>
        /// Calculation of hash complete
        /// </summary>
        public event Action<byte[]> Calculated;

        /// <summary>
        /// Processing finished (with result or without)
        /// </summary>
        public event Action Finished;

        public enum StateType
        {
            Initial,
            InProgress,
            Inconclusive,
            Calculated
        }

        /// <summary>
        /// Gets the value of the computed hash code
        /// </summary>
        public byte[] Hash { get; private set; }
        
        public HashAlgorithm HashAlgorithm { private get; set; }

        public int ChunkSize { get; set; }

        public bool HashCalculated
        {
            get
            {
                return this.hashCalculated;
            }
        }

        public StateType State
        {
            get
            {
                return this.state;
            }
        }

        public byte[] CalculateFileHash(string filePath)
        {
            if (this.HashAlgorithm == null)
            {
                throw new InvalidOperationException("'HashAlgorithm property' must be set");
            }

            if (this.State != StateType.Initial)
            {
                throw new InvalidOperationException("Object is not in initial state. Create new object");
            }
            
            this.state = StateType.InProgress;
            try
            {
                using (var cryptoStream = new CryptoStream(Stream.Null, this.HashAlgorithm, CryptoStreamMode.Write))
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
                        this.hashCalculated = fileBytesRead == fileLength;
                    }
                }
            }
            finally
            {
                this.state = this.hashCalculated ? StateType.Calculated : StateType.Inconclusive;
                if (this.hashCalculated)
                {
                    this.Hash = this.HashAlgorithm.Hash;
                    this.OnCalculated(this.HashAlgorithm.Hash);
                }

                this.OnFinished();
            }

            if (this.shouldStop || !this.hashCalculated)
            {
                return null;
            }

            return this.HashAlgorithm.Hash;
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

        protected virtual void OnCalculated(byte[] calculatedHash)
        {
            if (this.Calculated != null)
            {
                this.Calculated(calculatedHash);
            }
        }

        protected virtual void OnFinished()
        {
            if (this.Finished != null)
            {
                this.Finished();
            }
        }
    }
}

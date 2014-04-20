namespace HashCheckerProj
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class HashValidator
    {
        public event Action<int, int> FileProcessed;

        public event Action<long, long> ChunkProcessed;

        public int EntriesCount { get; private set; }

        public int EntriesProcessed { get; private set; }

        public string BaseDir { get; set; }

        public void ValidateEntries(IEnumerable<ChecksumFileEntry> checksumFileEntries, string defaultHashAlgorithm)
        {
            var entries = checksumFileEntries.ToArray();
            this.EntriesCount = entries.Length;
            this.EntriesProcessed = 0;

            var results = new List<EntryResult>();
            foreach (var entry in entries)
            {
                var validator = new FileHashCalculator { HashAlgorithm = CryptoUtils.ToHashAlgorithm(entry.ChecksumType) };
                validator.ChunkProcessed += this.OnChunkProcessed;

                bool valid = ConvertUtils.ByteArraysEqual(
                    validator.CalculateFileHash(Path.Combine(this.BaseDir, entry.Path)),
                    ConvertUtils.ToBytes(entry.Path));

                validator.ChunkProcessed -= this.OnChunkProcessed;
                var entryResult = new EntryResult(entry.Hash, entry.Path, entry.ChecksumType, valid);
                results.Add(entryResult);

                this.EntriesProcessed++;
                this.OnEntryProcessed(this.EntriesProcessed, this.EntriesCount);
            }
        }

        protected virtual void OnEntryProcessed(int entriesProcessed, int entriesCount)
        {
            if (this.FileProcessed != null)
            {
                this.FileProcessed(entriesProcessed, entriesCount);
            }
        }

        protected virtual void OnChunkProcessed(long bytesProcessed, long fileSize)
        {
            if (this.ChunkProcessed != null)
            {
                this.ChunkProcessed(bytesProcessed, fileSize);
            }
        }

        public struct EntryResult
        {
            public EntryResult(string hash, string path, string type, bool valid)
                : this()
            {
                this.Hash = hash;
                this.Path = path;
                this.ChecksumType = type;
                this.Valid = valid;
            }

            public string Hash { get; private set; }

            public string Path { get; private set; }

            public string ChecksumType { get; private set; }

            public bool Valid { get; private set; }
        }
    }
}

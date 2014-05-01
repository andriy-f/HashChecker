namespace HashCheckerProj
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class HashValidator
    {
        private volatile int entriesCount;

        private volatile int entriesProcessed;

        private volatile FileHashCalculator entryCalculator;

        private volatile int notFoundCount;

        private volatile bool shouldStop;

        private volatile StateType state;

        private volatile int validCount;

        private volatile int wrongCount;

        public delegate void EntryProcessedEventHandler(
            EntryCheckResult checkResult,
            int entriesProcessed,
            int entriesCount);

        public delegate void EntryChunkProcessedEventHandler(long bytesProcessed, long fileSize);

        public event EntryProcessedEventHandler EntryProcessed;

        public event EntryChunkProcessedEventHandler EntryChunkProcessed;

        public event Action Finished;

        public event Action<ChecksumFileEntry> StartProcessingEntry;

        public enum StateType
        {
            Initial,
            InProgress,
            Finished
        }

        public int EntriesCount
        {
            get
            {
                return this.entriesCount;
            }
        }

        public int EntriesProcessed
        {
            get
            {
                return this.entriesProcessed;
            }
        }

        public int ValidCount
        {
            get
            {
                return this.validCount;
            }
        }

        public int WrongCount
        {
            get
            {
                return this.wrongCount;
            }
        }

        public int NotFoundCount
        {
            get
            {
                return this.notFoundCount;
            }
        }

        public string BaseDir { get; set; }

        /// <summary>
        /// Is used when entry hashType is not defined
        /// </summary>
        public HashType? DefaultHashType { get; set; }

        public StateType State
        {
            get
            {
                return this.state;
            }
        }

        public void HashChecker()
        {
            this.state = StateType.Initial;
        }

        public IEnumerable<EntryCheckResult> ValidateEntries(IEnumerable<ChecksumFileEntry> checksumFileEntries, string defaultHashAlgorithm)
        {
            try
            {
                this.state = StateType.InProgress;
                var entries = checksumFileEntries.ToArray();
                this.entriesCount = entries.Length;
                this.entriesProcessed = 0;

                var results = new List<EntryCheckResult>();
                foreach (var entry in entries)
                {
                    if (this.shouldStop)
                    {
                        break;
                    }

                    this.StartProcessingEntry(entry);
                    this.entryCalculator = new FileHashCalculator();
                                               
                    if (entry.HashType != null)
                    {
                        this.entryCalculator.HashAlgorithm = CryptoUtils.ToHashAlgorithm(entry.HashType);
                    }
                    else if (entry.HashType == null && this.DefaultHashType != null)
                    {
                        this.entryCalculator.HashAlgorithm = CryptoUtils.ToHashAlgorithm(this.DefaultHashType.Value);
                    }
                    else
                    {
                        throw new InvalidOperationException("Property 'DefaultHashType' must be set if individual entry hash type is undefined");
                    }
                    
                    this.entryCalculator.ChunkProcessed += this.OnEntryChunkProcessed;

                    string fullEntryPath;
                    if (Path.IsPathRooted(entry.Path))
                    {
                        fullEntryPath = entry.Path;
                    }
                    else
                    {
                        if (!Directory.Exists(this.BaseDir))
                        {
                            throw new InvalidOperationException("Property BaseDir is required, but directory not found");
                        }

                        fullEntryPath = Path.Combine(this.BaseDir, entry.Path);
                    }

                    EntryResultType entryResultType;
                    if (!File.Exists(fullEntryPath))
                    {
                        entryResultType = EntryResultType.NotFound;
                        this.notFoundCount++;
                    }
                    else
                    {
                        var calculatedHash = this.entryCalculator.CalculateFileHash(fullEntryPath);

                        if (this.entryCalculator.HashCalculated)
                        {
                            bool valid = ConvertUtils.ByteArraysEqual(calculatedHash, ConvertUtils.ToBytes(entry.Hash));
                            entryResultType = valid ? EntryResultType.Correct : EntryResultType.Wrong;
                            if (valid)
                            {
                                this.validCount++;
                            }
                            else
                            {
                                this.wrongCount++;
                            }
                        }
                        else
                        {
                            entryResultType = EntryResultType.Aborted;
                        }
                    }

                    var entryResult = new EntryCheckResult(entry.Hash, entry.Path, entry.HashType, entryResultType);
                    results.Add(entryResult);

                    this.entriesProcessed++;
                    this.OnEntryProcessed(entryResult);

                    // Free
                    this.entryCalculator.ChunkProcessed -= this.OnEntryChunkProcessed;
                    this.entryCalculator = null;
                }

                return results;
            }
            finally
            {
                this.state = StateType.Finished;
                this.OnFinished();
            }
        }

        public void RequestStop()
        {
            this.shouldStop = true;
            if (this.entryCalculator != null)
            {
                this.entryCalculator.RequestStop();
            }
        }

        protected virtual void OnStartProcessingEntry(ChecksumFileEntry entry)
        {
            if (this.StartProcessingEntry != null)
            {
                this.StartProcessingEntry(entry);
            }
        }

        protected virtual void OnEntryProcessed(EntryCheckResult checkResult)
        {
            if (this.EntryProcessed != null)
            {
                this.EntryProcessed(checkResult, this.entriesProcessed, this.entriesCount);
            }
        }

        protected virtual void OnEntryChunkProcessed(long bytesProcessed, long fileSize)
        {
            if (this.EntryChunkProcessed != null)
            {
                this.EntryChunkProcessed(bytesProcessed, fileSize);
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

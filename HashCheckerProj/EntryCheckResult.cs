namespace HashCheckerProj
{
    /// <summary>
    /// Immutable result of validating a hash of a checksum file entry
    /// </summary>
    public struct EntryCheckResult
    {
        public EntryCheckResult(string hash, string path, string type, EntryResultType resultType)
            : this()
        {
            this.Hash = hash;
            this.Path = path;
            this.ChecksumType = type;
            this.ResultType = resultType;
        }

        public string Hash { get; private set; }

        public string Path { get; private set; }

        public string ChecksumType { get; private set; }

        public EntryResultType ResultType { get; private set; }
    }
}
namespace HashChecker.Tests
{
    using System.Security.Cryptography;

    using HashCheckerProj;

    using NUnit.Framework;

    class FileHashCalculatorTests
    {
        private const string File1Path = @"Data\file01.bin";
        private const string File1Md5HashUpper = "5738077ABBE757E9D2DA3741115074B6";

        [Test]
        public void CalculateFileHashTest()
        {
            var fileHashValidator = new FileHashCalculator { HashAlgorithm = MD5.Create() };
            var actual = fileHashValidator.CalculateFileHash(File1Path);
            var expected = ConvertUtils.ToBytes(File1Md5HashUpper);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateFileHashTestToLower()
        {
            var fileHashValidator = new FileHashCalculator { HashAlgorithm = MD5.Create() };
            var actual = fileHashValidator.CalculateFileHash(File1Path);
            var expected = ConvertUtils.ToBytes(File1Md5HashUpper.ToLowerInvariant());

            Assert.AreEqual(expected, actual);
        }
    }
}

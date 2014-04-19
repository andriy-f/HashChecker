namespace HashChecker.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using HashCheckerProj;

    using NUnit.Framework;

    [TestFixture]
    public class ChecksumFileTests
    {
        private const string NonExisingFile = @"Data\file01.not";
        private const string ChecksumInvalidFile = @"Data\file01.bin";
        private const string ChecksumUnixFile = @"Data\file01.chsum";
        private const string ChecksumSfv = @"Data\file01.sfv";
        private const string ChecksumMd5 = @"Data\file01.md5";
        private const string ChecksumMd5V2 = @"Data\file01-v2.md5";

        private static readonly IEnumerable<ChecksumFile.Entry> ChecksumSfvExpected = new List<ChecksumFile.Entry>
                               {
                                   new ChecksumFile.Entry
                                       {
                                           Hash = "5BE4C5DF",
                                           Path = "file01.bin"
                                       }
                               };

        private static readonly IEnumerable<ChecksumFile.Entry> ChecksumMd5Expected = new List<ChecksumFile.Entry>
                               {
                                   new ChecksumFile.Entry
                                       {
                                           Hash = "5738077ABBE757E9D2DA3741115074B6",
                                           Path = "file01.bin"
                                       }
                               };

        public void ParseNonExisingFileTest()
        {
            Assert.Throws(
                Is.TypeOf<FileNotFoundException>().And.Message.EqualTo("Checksum file not found").And.Property("FileName").EqualTo(NonExisingFile),
                this.ParseNonExistingFile);
        }

        [Test]
        public void ParseInvalidFileTest()
        {
            var checksumFile = new ChecksumFile(ChecksumInvalidFile);
            var actual = checksumFile.Parse();
            Assert.IsEmpty(actual);
        }
        
        [Test]
        public void ParseUnixFileTest()
        {
            var checksumFile = new ChecksumFile(ChecksumUnixFile);
            var actual = checksumFile.Parse();

            var expected = new List<ChecksumFile.Entry>
                               {
                                   new ChecksumFile.Entry
                                       {
                                           Hash = "5738077ABBE757E9D2DA3741115074B6",
                                           Path = "file01.bin",
                                           ChecksumType = "MD5"
                                       }
                               };

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSfv()
        {
            var checksumFile = new ChecksumFile(ChecksumSfv);
            var actual = checksumFile.Parse();

            Assert.AreEqual(ChecksumSfvExpected, actual);
        }

        [Test]
        public void TestMd5()
        {
            var checksumFile = new ChecksumFile(ChecksumMd5);
            var actual = checksumFile.Parse();

            Assert.AreEqual(ChecksumMd5Expected, actual);
        }

        [Test]
        public void TestMd5V2()
        {
            var checksumFile = new ChecksumFile(ChecksumMd5V2);
            var actual = checksumFile.Parse();
            Assert.AreEqual(ChecksumMd5Expected, actual);
        }

        private void ParseNonExistingFile()
        {
            var checksumFile = new ChecksumFile(NonExisingFile);
            checksumFile.Parse();
        }
    }
}

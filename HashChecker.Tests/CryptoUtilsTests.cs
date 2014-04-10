namespace HashChecker.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    using NUnit.Framework;

    [TestFixture]
    public class CryptoUtilsTests
    {
        private const string File1Path = @"Data\file01.bin";
        private const string File1Md5HashUpper = "5738077ABBE757E9D2DA3741115074B6";

        [Test]
        public void T01ValidateFileHashChunked()
        {
            Assert.True(
                HashCheckerProj.CryptoUtils.ValidateFileHashChunked(
                    File1Path, 
                    MD5.Create(), 
                    File1Md5HashUpper, 
                    (i, j) => { }));
        }

        [Test]
        public void T02ValidateFileHashChunked()
        {
            Assert.True(
                HashCheckerProj.CryptoUtils.ValidateFileHashChunked(
                    File1Path,
                    MD5.Create(),
                    File1Md5HashUpper.ToLowerInvariant(),
                    (i, j) => { }));
        }
    }
}

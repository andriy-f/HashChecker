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
        [Test]
        public void T01ValidateFileHashChunked()
        {
            const string File = @"Data\file01.bin";
            const string ExpectedMd5Hash = "5738077ABBE757E9D2DA3741115074B6";

            Assert.True(HashCheckerProj.CryptoUtils.ValidateFileHashChunked(File, MD5.Create(), ExpectedMd5Hash, (i, j) => { }));
        }
    }
}

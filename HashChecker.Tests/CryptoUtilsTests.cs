namespace HashChecker.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    using HashCheckerProj;

    using NUnit.Framework;

    [TestFixture]
    public class CryptoUtilsTests
    {
        [Test]
        public void T01IsChecksumType()
        {
            Assert.True(CryptoUtils.IsChecksumType("md5"));
        }

        [Test]
        public void T02IsChecksumType()
        {
            Assert.False(CryptoUtils.IsChecksumType("7z"));
        }
    }
}

namespace HashChecker.Tests
{
    using HashChecker.Core;

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

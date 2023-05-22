using NUnit.Framework;
using System;
using user.Helper;
using user.tests.Constants;

namespace user.tests
{
    public class MD5HelperTest
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void NullTest()
        {
            // Arrange
            string? data = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                MD5Helper.Hash(data);
            });
        }

        [Test]
        public void EmptyTest()
        {
            // Arrange
            string data = string.Empty;

            // Act
            var hash = MD5Helper.Hash(data);

            // Assert
            Assert.AreEqual(hash, Md5HashConstants.EmptyStringHash);
        }

        [Test]
        public void WhiteSpaceTest()
        {
            // Arrange
            string data = StringConstants.WhiteSpace;

            // Act
            var hash = MD5Helper.Hash(data);

            // Assert
            Assert.AreEqual(hash, Md5HashConstants.WhiteSpaceHash);
        }

        [Test]
        public void IsNotOrWhiteSpaceTest()
        {
            // Arrange
            var data = "password";

            // Act
            var hash = MD5Helper.Hash(data);

            // Assert
            Assert.AreEqual(hash, Md5HashConstants.PasswordStringHash);
        }
    }
}
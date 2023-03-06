using NUnit.Framework;
using System;
using user.Helper;

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
            string data = "";

            // Act
            var hash = MD5Helper.Hash(data);

            // Assert
            Assert.AreEqual(hash, "D41D8CD98F00B204E9800998ECF8427E");
        }

        [Test]
        public void WhiteSpaceTest()
        {
            // Arrange
            string data = " ";

            // Act
            var hash = MD5Helper.Hash(data);

            // Assert
            Assert.AreEqual(hash, "7215EE9C7D9DC229D2921A40E899EC5F");
        }

        [Test]
        public void IsNotOrWhiteSpaceTest()
        {
            // Arrange
            var data = "password";

            // Act
            var hash = MD5Helper.Hash(data);

            // Assert
            Assert.AreEqual(hash, "5F4DCC3B5AA765D61D8327DEB882CF99");
        }
    }
}
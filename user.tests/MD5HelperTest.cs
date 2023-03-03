using NUnit.Framework;
using System;
using System.Security.Cryptography;
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
        public void HashTest([Values(null, "", " ", "password")] string? data)
        {
            // Arrange

            // Act
            var hash = MD5Helper.Hash(data);

            // Assert
            Assert.AreEqual(hash, Hash(data));

        }

        private string Hash(string input)
        {
            if (input == null)
                return null;

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }
    }
}
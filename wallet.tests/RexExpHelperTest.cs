using NUnit.Framework;
using wallet.Helpers;

namespace wallet.tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GenerateStringTest()
        {
            // Arrange
            var pattern = "[0-9]{2}[a-z]{12}";

            // Act
            var answer = RegExpHelper.GenerateString(pattern);

            // Assert
            Assert.That(answer, Does.Match(pattern));
        }
    }
}
using payment.Helpers;

namespace payment.tests
{
    internal class AmountHelperTest
    {
        [Test]
        public void TestGreaterThanZero([Values(15)] decimal amount, [Values(0.6)] decimal rate)
        {
            // Arrange

            // Act
            var result = AmountHelper.Compute(amount, rate);

            // Assert
            Assert.That(result, Is.GreaterThan(0));
        }

        [Test]
        public void TestLogic([Values(15)] decimal amount, [Values(0.6)] decimal rate)
        {
            // Arrange

            // Act
            var result = AmountHelper.Compute(amount, rate);

            // Assert
            Assert.That(result, Is.EqualTo(amount * rate));
        }
    }
}

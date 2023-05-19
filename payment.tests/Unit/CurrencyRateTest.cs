using Moq;
using payment.Interfaces.Operations;
using payment.Operations;
using payment.tests.Constants;

namespace payment.tests.Unit
{
    public class CurrencyRateTest
    {
        [Test]
        public void TestGetRateOfDefinedCurrencies()
        {
            // Arrange
            var currencyApiOperationMock = new Mock<ICurrencyApiOperation>();
            WhenCurrenciesAreDefinedThenRateIsDefined(currencyApiOperationMock);
            var currencyRateOperation = new CurrencyRateOperation(currencyApiOperationMock.Object);

            // Act
            var rate = currencyRateOperation.Get(CurrencyConstants.USD, CurrencyConstants.EUR);

            // Assert
            Assert.That(rate, Is.Not.Null);
            Assert.That(rate, Is.GreaterThan(0));
        }

        [Test]
        public void TestGetRateOfIdenticalCurrencies()
        {
            // Arrange
            var currencyRateOperation = new CurrencyRateOperation(null!);

            // Act
            var rate = currencyRateOperation.Get(CurrencyConstants.USD, CurrencyConstants.USD);

            // Assert
            Assert.That(rate, Is.EqualTo(1m));
        }

        [Test]
        public void TestGetUndefinedCurrenciesRate()
        {
            // Arrange
            var currencyApiOperationMock = new Mock<ICurrencyApiOperation>();
            WhenCurrencyIsUndefinedThenRateNull(currencyApiOperationMock);
            var currencyRateOperation = new CurrencyRateOperation(currencyApiOperationMock.Object);

            // Act
            var firstCurrencyIsUndefinedRate = currencyRateOperation.Get(CurrencyConstants.UndefinedCurrencyCode, CurrencyConstants.USD);
            var secondCurrencyIsUndefinedRate = currencyRateOperation.Get(CurrencyConstants.USD, CurrencyConstants.UndefinedCurrencyCode);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstCurrencyIsUndefinedRate, Is.Null);
                Assert.That(secondCurrencyIsUndefinedRate, Is.Null);
            });
        }

        private static void WhenCurrenciesAreDefinedThenRateIsDefined(Mock<ICurrencyApiOperation> mock)
        {
            mock.Setup(a => a.GetRate(It.IsAny<string>(), It.IsAny<string>())).Returns(RandomConstants.DefinedRate);
        }

        private static void WhenCurrencyIsUndefinedThenRateNull(Mock<ICurrencyApiOperation> mock)
        {
            mock.Setup(a => a.GetRate(CurrencyConstants.UndefinedCurrencyCode, It.IsAny<string>())).Returns((decimal?)null);
            mock.Setup(a => a.GetRate(It.IsAny<string>(), CurrencyConstants.UndefinedCurrencyCode)).Returns((decimal?)null);
        }
    }
}
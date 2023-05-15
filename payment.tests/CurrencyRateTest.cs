using Moq;
using payment.Interfaces.Operations;
using payment.Operations;
using payment.tests.Constants;

namespace payment.tests
{
    public class CurrencyRateTest
    {
        private ICurrencyRateOperation currencyRateOperation;

        [SetUp]
        public void Setup()
        {
            MockCurrencyRateOperation();
        }

        [Test]
        public void TestDifferent([Values("some_currency_code")] string currencyFromCode, [Values("come_another_currency_code")] string currencyToCode)
        {
            // Arrange

            // Act
            var rate = currencyRateOperation.Get(currencyFromCode, currencyToCode);

            // Assert
            Assert.That(rate, Is.Not.Null);
            Assert.That(rate, Is.GreaterThan(0));
        }

        [Test]
        public void TestEqual([Values("USD")] string currencyFromCode, [Values("USD")] string currencyToCode)
        {
            // Arrange

            // Act
            var rate = currencyRateOperation.Get(currencyFromCode, currencyToCode);

            // Assert
            Assert.That(rate, Is.EqualTo(1m));
        }

        [Test]
        public void TestUnknowCurrency([Values(CurrencyConstants.UnknownCurrencyCode)] string currencyFromCode, [Values(CurrencyConstants.EUR)] string currencyToCode)
        {
            // Arrange

            // Act
            var rate = currencyRateOperation.Get(currencyFromCode, currencyToCode);

            // Assert
            Assert.That(rate, Is.Null);
        }

        private void MockCurrencyRateOperation()
        {
            var mock = new Mock<ICurrencyApiOperation>();

            WhenAnyCurrenciesThenAnyAmount(mock);
            WhenAtLeastOneUnknowCurrencyThenNull(mock);

            currencyRateOperation = new CurrencyRateOperation(mock.Object);
        }

        private static void WhenAnyCurrenciesThenAnyAmount(Mock<ICurrencyApiOperation> mock)
        {
            mock.Setup(a => a.GetRate(It.IsAny<string>(), It.IsAny<string>())).Returns(0.94m);
        }

        private static void WhenAtLeastOneUnknowCurrencyThenNull(Mock<ICurrencyApiOperation> mock)
        {
            mock.Setup(a => a.GetRate(CurrencyConstants.UnknownCurrencyCode, It.IsAny<string>())).Returns((decimal?)null);
        }
    }
}
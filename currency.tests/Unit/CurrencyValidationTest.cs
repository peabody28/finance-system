using currency.Entities;
using currency.Interfaces.Entities;
using currency.Interfaces.Repositories;
using currency.tests.Constants;
using currency.Validations;
using Moq;
using NUnit.Framework;
using Validation.Helper.Extensions;

namespace currency.tests.Unit
{
    internal class CurrencyValidationTest
    {
        private static ICurrency CurrencyStub => new CurrencyEntity();

        [Test]
        public void TestExistingCurrencyCode()
        {
            // Arrange
            var currencyRepositoryMock = new Mock<ICurrencyRepository>();
            currencyRepositoryMock.Setup(m => m.Get(CurrencyConstants.ExistingCurrencyCode)).Returns(CurrencyStub);

            var currencyValidation = new CurrencyValidation(currencyRepositoryMock.Object);

            // Act
            var result = currencyValidation.Validate(CurrencyConstants.ExistingCurrencyCode);

            // Assert
            Assert.That(result.IsEmpty, Is.EqualTo(true));
            Assert.That(result.ErrorMessage, Is.Null);
        }

        [Test]
        public void TestUndefinedCurrencyCode()
        {
            // Arrange
            var currencyRepositoryMock = new Mock<ICurrencyRepository>();
            currencyRepositoryMock.Setup(m => m.Get(CurrencyConstants.UndefinedCurrencyCode)).Returns((ICurrency?)null);

            var currencyValidation = new CurrencyValidation(currencyRepositoryMock.Object);

            // Act
            var result = currencyValidation.Validate(CurrencyConstants.UndefinedCurrencyCode);

            // Assert
            AssertThatResultNotEmptyAndHaveMessage(result);
        }

        [Test]
        public void TestEmptyCurrencyCode()
        {
            // Arrange

            // Act
            var result = new CurrencyValidation(null!).Validate(string.Empty);

            // Assert
            AssertThatResultNotEmptyAndHaveMessage(result);
        }

        [Test]
        public void TestNullCurrencyCode()
        {
            // Arrange

            // Act
            var result = new CurrencyValidation(null!).Validate(null!);

            // Assert
            AssertThatResultNotEmptyAndHaveMessage(result);
        }

        private static void AssertThatResultNotEmptyAndHaveMessage(ValidationResult result)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result.IsEmpty, Is.EqualTo(false));
                Assert.That(result.ErrorMessage, Is.EqualTo("Currency code invalid"));
            });
        }
    }
}

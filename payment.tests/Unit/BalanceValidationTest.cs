using Moq;
using payment.Interfaces.Entities;
using payment.Interfaces.Operations;
using payment.tests.Constants;
using payment.Validations;
using Validation.Helper.Extensions;

namespace payment.tests.Unit
{
    internal class BalanceValidationTest
    {
        [Test]
        public void TestEmptyWalletForDebit()
        {
            // Arrange
            var balanceOperationMock = new Mock<IBalanceOperation>();
            SetWalletBalanceToZero(balanceOperationMock);
            var balanceValidation = new BalanceValidation(balanceOperationMock.Object);

            // Act
            var result = balanceValidation.ValidateWalletForDebit(It.IsAny<IWallet>(), AmountConstants.NotZeroAmount);

            // Assert
            AssertThatResultNotEmptyAndHaveMessage(result);
        }

        [Test]
        public void TestInsufficientAmountWalletForDebit()
        {
            // Arrange
            var balanceOperationMock = new Mock<IBalanceOperation>();
            SetWalletBalanceToNonEmptyValue(balanceOperationMock);
            var balanceValidation = new BalanceValidation(balanceOperationMock.Object);

            // Act
            var result = balanceValidation.ValidateWalletForDebit(It.IsAny<IWallet>(), AmountConstants.NotZeroAmount + 1);

            // Assert
            AssertThatResultNotEmptyAndHaveMessage(result);
        }

        [Test]
        public void TestRichManWalletForDebit()
        {
            // Arrange
            var balanceOperationMock = new Mock<IBalanceOperation>();
            SetWalletBalanceToMaxValue(balanceOperationMock);
            var balanceValidation = new BalanceValidation(balanceOperationMock.Object);

            // Act
            var result = balanceValidation.ValidateWalletForDebit(It.IsAny<IWallet>(), AmountConstants.NotZeroAmount);

            // Assert
            Assert.That(result.IsEmpty, Is.EqualTo(true));
            Assert.That(result.ErrorMessage, Is.Null);
        }

        private static void AssertThatResultNotEmptyAndHaveMessage(ValidationResult result)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result.IsEmpty, Is.EqualTo(false));
                Assert.That(result.ErrorMessage, Is.EqualTo("Insufficient funds"));
            });
        }

        private static void SetWalletBalanceToNonEmptyValue(Mock<IBalanceOperation> balanceOperationMock)
        {
            balanceOperationMock.Setup(m => m.Get(It.IsAny<IWallet>())).Returns(AmountConstants.NotZeroAmount);
        }

        private static void SetWalletBalanceToZero(Mock<IBalanceOperation> balanceOperationMock)
        {
            balanceOperationMock.Setup(m => m.Get(It.IsAny<IWallet>())).Returns(decimal.Zero);
        }

        private static void SetWalletBalanceToMaxValue(Mock<IBalanceOperation> balanceOperationMock)
        {
            balanceOperationMock.Setup(m => m.Get(It.IsAny<IWallet>())).Returns(decimal.MaxValue);
        }
    }
}

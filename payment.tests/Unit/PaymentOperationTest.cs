using Moq;
using payment.Interfaces.Entities;
using payment.Interfaces.Operations;
using payment.Interfaces.Repositories;
using payment.Operations;
using payment.tests.Constants;

namespace payment.tests.Unit
{
    internal class PaymentOperationTest
    {
        [Test]
        public void TestTransferBetweenUndefinedCurrencyWallets()
        {
            // Arrange
            var currencyRateOperationMock = new Mock<ICurrencyRateOperation>();
            WhenWalletCurrencyUndefinedThenRateNull(currencyRateOperationMock);

            var undefinedCurrencyWallet = UndefinedCurrencyWalletStub();
            var usdWallet = WalletStub(CurrencyConstants.USD);

            var paymentOperation = new PaymentOperation(null!, null!, currencyRateOperationMock.Object, null!, null!, null!);

            // Act && Assert
            Assert.Throws<ArgumentException>(() => paymentOperation.TryTransfer(undefinedCurrencyWallet, usdWallet, RandomConstants.AnyAmount));
            Assert.Throws<ArgumentException>(() => paymentOperation.TryTransfer(usdWallet, undefinedCurrencyWallet, RandomConstants.AnyAmount));
        }

        [Test]
        public void TestTransferBetweenDefinedCurrencyWallets()
        {
            // Arrange
            var currencyRateOperationMock = new Mock<ICurrencyRateOperation>();
            WhenWalletCurrencyDefinedThenRateDefined(currencyRateOperationMock);

            var walletFrom = WalletStub(CurrencyConstants.USD);
            var walletTo = WalletStub(CurrencyConstants.EUR);

            var paymentOperation = new PaymentOperation(PaymentRepositoryMock(), BalanceOperationTypeOperaitionStub(),
                currencyRateOperationMock.Object, null!, null!, null!);

            // Act
            var result = paymentOperation.TryTransfer(walletFrom, walletTo, RandomConstants.AnyAmount);

            // Assert
            Assert.That(result, Is.InstanceOf<bool>());
        }

        #region [ Stubs ]

        private static IWallet UndefinedCurrencyWalletStub()
        {
            var walletFromStub = new Mock<IWallet>();
            walletFromStub.Setup(a => a.Currency.Code).Returns(CurrencyConstants.UndefinedCurrencyCode);

            return walletFromStub.Object;
        }

        private static IWallet WalletStub(string currencyCode)
        {
            var walletStub = new Mock<IWallet>();
            walletStub.SetupAllProperties();
            walletStub.Setup(a => a.Currency.Code).Returns(currencyCode);

            return walletStub.Object;
        }

        private static IBalanceOperationTypeOperation BalanceOperationTypeOperaitionStub()
        {
            var balanceOperationTypeOperationStub = new Mock<IBalanceOperationTypeOperation>();
            balanceOperationTypeOperationStub.Setup(a => a.Debit).Returns((IBalanceOperationType?)null);
            balanceOperationTypeOperationStub.Setup(a => a.Credit).Returns((IBalanceOperationType?)null);

            return balanceOperationTypeOperationStub.Object;
        }

        #endregion

        #region [ Mock Members ]

        private static void WhenWalletCurrencyDefinedThenRateDefined(Mock<ICurrencyRateOperation> mock)
        {
            mock.Setup(a => a.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(RandomConstants.DefinedRate);
        }

        private static void WhenWalletCurrencyUndefinedThenRateNull(Mock<ICurrencyRateOperation> mock)
        {
            mock.Setup(a => a.Get(CurrencyConstants.UndefinedCurrencyCode, It.IsAny<string>())).Returns((decimal?)null);
            mock.Setup(a => a.Get(It.IsAny<string>(), CurrencyConstants.UndefinedCurrencyCode)).Returns((decimal?)null);
        }

        private static IPaymentRepository PaymentRepositoryMock()
        {
            var mock = new Mock<IPaymentRepository>();

            MockTransactionPassing(mock);
            MockPaymentCreating(mock);

            return mock.Object;
        }

        private static void MockTransactionPassing(Mock<IPaymentRepository> mock)
        {
            mock.SetupSequence(a => a.BeginTransaction()).Pass();
            mock.SetupSequence(a => a.CommitTransaction()).Pass();
            mock.SetupSequence(a => a.RollbackTransaction()).Pass();
        }

        private static void MockPaymentCreating(Mock<IPaymentRepository> mock)
        {
            mock.Setup(a => a.Create(It.IsAny<IWallet>(), It.IsAny<IBalanceOperationType>(), It.IsAny<decimal>())).Returns(It.IsNotNull<IPayment>());
        }

        #endregion
    }
}

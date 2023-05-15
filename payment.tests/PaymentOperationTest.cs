using Microsoft.Extensions.Logging;
using Moq;
using payment.Interfaces.Entities;
using payment.Interfaces.Operations;
using payment.Interfaces.Repositories;
using payment.Operations;
using payment.tests.Constants;

namespace payment.tests
{
    internal class PaymentOperationTest
    {
        private ICurrencyRateOperation currencyRateOperation;

        private IPaymentRepository paymentRepository;

        private IBalanceOperationTypeOperation balanceOperationTypeOperation;

        private IRabbitMqOperation rabbitMqOperation;

        private ILogger<PaymentOperation> logger;

        [SetUp]
        public void Setup()
        {
            MockCurrencyRateOperation();
            MockPaymentRepository();
            StubBalanceOperationTypeOperaition();
            MockRabbitMqOperation();
            StubLogger();
        }

        [Test]
        public void TestUnknowCurrency()
        {
            // Arrange
            var walletFrom = UnknowCurrencyWalletStub();
            var walletTo = WalletStub(CurrencyConstants.USD);

            var paymentOperation = new PaymentOperation(paymentRepository, balanceOperationTypeOperation, currencyRateOperation, rabbitMqOperation, null, logger);

            // Act && Assert
            Assert.Throws<ArgumentException>(() => paymentOperation.TryTransfer(walletFrom, walletTo, RandomConstants.AnyAmount));
        }

        [Test]
        public void TestSomething()
        {
            // Arrange
            var walletFrom = WalletStub(CurrencyConstants.USD);
            var walletTo = WalletStub(CurrencyConstants.EUR);

            var paymentOperation = new PaymentOperation(paymentRepository, balanceOperationTypeOperation, currencyRateOperation, rabbitMqOperation, null, logger);

            // Act
            var result = paymentOperation.TryTransfer(walletFrom, walletTo, RandomConstants.AnyAmount);

            // Assert
            Assert.That(result, Is.InstanceOf<bool>());
        }

        #region [ Stubs ]

        private static IWallet UnknowCurrencyWalletStub()
        {
            var walletFromStub = new Mock<IWallet>();
            walletFromStub.Setup(a => a.Number).Returns("some_unknow_currency_wallet");
            walletFromStub.Setup(a => a.Currency.Code).Returns(CurrencyConstants.UnknownCurrencyCode);

            return walletFromStub.Object;
        }

        private static IWallet WalletStub(string currencyCode)
        {
            var walletStub = new Mock<IWallet>();
            walletStub.SetupAllProperties();
            walletStub.Setup(a => a.Currency.Code).Returns(currencyCode);

            return walletStub.Object;
        }

        private void StubBalanceOperationTypeOperaition()
        {
            var balanceOperationTypeOperationStub = new Mock<IBalanceOperationTypeOperation>();
            balanceOperationTypeOperationStub.Setup(a => a.Debit).Returns((IBalanceOperationType?)null);
            balanceOperationTypeOperationStub.Setup(a => a.Credit).Returns((IBalanceOperationType?)null);
            balanceOperationTypeOperation = balanceOperationTypeOperationStub.Object;
        }

        private void StubLogger()
        {
            var loggerMock = new Mock<ILogger<PaymentOperation>>();
            logger = loggerMock.Object;
        }

        #endregion

        #region [ Mock Members ]

        private void MockCurrencyRateOperation()
        {
            var mock = new Mock<ICurrencyRateOperation>();

            MockAnyCurrency(mock);
            MockUnknowCurrencyRate(mock);

            currencyRateOperation = mock.Object;
        }

        private static void MockAnyCurrency(Mock<ICurrencyRateOperation> mock)
        {
            mock.Setup(a => a.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(0.94m);
        }

        private static void MockUnknowCurrencyRate(Mock<ICurrencyRateOperation> mock)
        {
            mock.Setup(a => a.Get(CurrencyConstants.UnknownCurrencyCode, It.IsAny<string>())).Returns((decimal?)null);
        }


        private void MockPaymentRepository()
        {
            var mock = new Mock<IPaymentRepository>();

            MockTransactionPassing(mock);
            MockPaymentCreating(mock);

            paymentRepository = mock.Object;
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

        private void MockRabbitMqOperation()
        {
            var mock = new Mock<IRabbitMqOperation>();

            MockRabbitMqSendMessagePassing(mock);

            rabbitMqOperation = mock.Object;
        }

        private void MockRabbitMqSendMessagePassing(Mock<IRabbitMqOperation> mock)
        {
            mock.SetupSequence(a => a.SendMessage(It.IsAny<It.IsAnyType>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Pass();
        }

        #endregion
    }
}

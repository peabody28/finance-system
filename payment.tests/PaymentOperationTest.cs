using Castle.Core.Logging;
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
        private const string UnknowCurrencyWalletNumber = "some_unknow_currency_wallet";

        private ICurrencyRateOperation currencyRateOperation;

        private IPaymentRepository paymentRepository;

        private IBalanceOperationTypeOperation balanceOperationTypeOperation;

        private IRabbitMqOperation rabbitMqOperation;

        private ILogger<PaymentOperation> logger;

        [SetUp]
        public void Setup()
        {
            var currencyRateOperationMock = new Mock<ICurrencyRateOperation>();
            currencyRateOperationMock.Setup(a => a.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(0.94m);
            currencyRateOperationMock.Setup(a => a.Get(CurrencyConstants.UnknownCurrencyCode, It.IsAny<string>())).Returns((decimal?)null);
            currencyRateOperation = currencyRateOperationMock.Object;

            var paymentRepositoryMock = new Mock<IPaymentRepository>();
            paymentRepositoryMock.SetupSequence(a => a.BeginTransaction()).Pass();
            paymentRepositoryMock.SetupSequence(a => a.CommitTransaction()).Pass();
            paymentRepositoryMock.SetupSequence(a => a.RollbackTransaction()).Pass();
            paymentRepositoryMock.Setup(a => a.Create(It.IsAny<IWallet>(), It.IsAny<IBalanceOperationType>(), It.IsAny<decimal>())).Returns(It.IsNotNull<IPayment>());
            paymentRepository = paymentRepositoryMock.Object;

            var balanceOperationTypeOperationStub = new Mock<IBalanceOperationTypeOperation>();
            balanceOperationTypeOperationStub.Setup(a => a.Debit).Returns((IBalanceOperationType?)null);
            balanceOperationTypeOperationStub.Setup(a => a.Credit).Returns((IBalanceOperationType?)null);
            balanceOperationTypeOperation = balanceOperationTypeOperationStub.Object;

            var rabbitMqMock = new Mock<IRabbitMqOperation>();
            rabbitMqMock.SetupSequence(a => a.SendMessage(It.IsAny<It.IsAnyType>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Pass();
            rabbitMqOperation = rabbitMqMock.Object;

            var loggerMock = new Mock<ILogger<PaymentOperation>>();
            loggerMock.SetupSequence(a => a.LogInformation(It.IsAny<string>(), It.IsAny<It.IsAnyType>()));
            logger = loggerMock.Object;
        }

        [Test]
        public void TestUnknowCurrency()
        {
            // Arrange
            var walletFromStub = new Mock<IWallet>();
            walletFromStub.Setup(a => a.Number).Returns(UnknowCurrencyWalletNumber);
            walletFromStub.Setup(a => a.Currency.Code).Returns(CurrencyConstants.UnknownCurrencyCode);

            var paymentOperation = new PaymentOperation(paymentRepository, balanceOperationTypeOperation, currencyRateOperation, rabbitMqOperation, null, logger);

            var walletToStub = new Mock<IWallet>();
            walletToStub.SetupAllProperties();
            walletToStub.Setup(a => a.Currency.Code).Returns(CurrencyConstants.USD);

            // Act && Assert
            Assert.Throws<ArgumentException>(() => paymentOperation.TryTransfer(walletFromStub.Object, walletToStub.Object, 2));
        }

        [Test]
        public void TestSomething()
        {
            // Arrange
            var walletFromStub = new Mock<IWallet>();
            walletFromStub.SetupAllProperties();
            walletFromStub.Setup(a => a.Currency.Code).Returns(CurrencyConstants.USD);

            var walletToStub = new Mock<IWallet>();
            walletToStub.SetupAllProperties();
            walletToStub.Setup(a => a.Currency.Code).Returns(CurrencyConstants.EUR);

            var paymentOperation = new PaymentOperation(paymentRepository, balanceOperationTypeOperation, currencyRateOperation, rabbitMqOperation, null, logger);

            // Act
            var result = paymentOperation.TryTransfer(walletFromStub.Object, walletToStub.Object, 2);

            // Assert
            Assert.That(result, Is.InstanceOf<bool>());
        }
    }
}

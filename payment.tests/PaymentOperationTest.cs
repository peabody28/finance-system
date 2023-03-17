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
        private IPaymentOperation paymentOperation;

        private const string UnknowCurrencyWalletNumber = "some_unknow_currency_wallet";

        [SetUp]
        public void Setup()
        {
            var walletApiOperationMock = new Mock<IWalletApiOperation>();
            walletApiOperationMock.Setup(a => a.CurrencyCode(It.IsAny<string>())).Returns(CurrencyConstants.USD);
            walletApiOperationMock.Setup(a => a.CurrencyCode(UnknowCurrencyWalletNumber)).Returns(CurrencyConstants.UnknownCurrencyCode);

            var currencyRateOperationMock = new Mock<ICurrencyRateOperation>();
            currencyRateOperationMock.Setup(a => a.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(0.94m);
            currencyRateOperationMock.Setup(a => a.Get(CurrencyConstants.UnknownCurrencyCode, It.IsAny<string>())).Returns((decimal?)null);

            var paymentRepositoryMock = new Mock<IPaymentRepository>();
            paymentRepositoryMock.SetupSequence(a => a.BeginTransaction()).Pass();
            paymentRepositoryMock.SetupSequence(a => a.CommitTransaction()).Pass();
            paymentRepositoryMock.SetupSequence(a => a.RollbackTransaction()).Pass();
            paymentRepositoryMock.Setup(a => a.Create(It.IsAny<IWallet>(), It.IsAny<IBalanceOperationType>(), It.IsAny<decimal>())).Returns(It.IsNotNull<IPayment>());

            var balanceOperationTypeOperationStub = new Mock<IBalanceOperationTypeOperation>();
            balanceOperationTypeOperationStub.Setup(a => a.Debit).Returns((IBalanceOperationType?)null);
            balanceOperationTypeOperationStub.Setup(a => a.Credit).Returns((IBalanceOperationType?)null);

            paymentOperation = new PaymentOperation(paymentRepositoryMock.Object, balanceOperationTypeOperationStub.Object, currencyRateOperationMock.Object, walletApiOperationMock.Object);
        }

        [Test]
        public void TestUnknowCurrency()
        {
            // Arrange
            var walletFromStub = new Mock<IWallet>();
            walletFromStub.Setup(a => a.Number).Returns(UnknowCurrencyWalletNumber);

            var walletToStub = new Mock<IWallet>();
            walletToStub.SetupAllProperties();

            // Act && Assert
            Assert.Throws<ArgumentException>(() => paymentOperation.Transfer(walletFromStub.Object, walletToStub.Object, 2));
        }

        [Test]
        public void TestSomething()
        {
            // Arrange
            var walletFromStub = new Mock<IWallet>();
            walletFromStub.SetupAllProperties();

            var walletToStub = new Mock<IWallet>();
            walletToStub.SetupAllProperties();

            // Act
            var result = paymentOperation.Transfer(walletFromStub.Object, walletToStub.Object, 2);

            // Assert
            Assert.That(result, Is.InstanceOf<bool>());
        }
    }
}

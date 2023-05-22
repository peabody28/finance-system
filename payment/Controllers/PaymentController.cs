using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using payment.Interfaces.Operations;
using payment.Interfaces.Repositories;
using payment.ModelBuilders;
using payment.Models.Payment;

namespace payment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IWalletRepository walletRepository;

        private readonly IPaymentTypeRepository paymentTypeRepository;

        private readonly IPaymentRepository paymentRepository;

        private readonly IPaymentOperation paymentOperation;

        public PaymentController(IWalletRepository walletRepository, IPaymentTypeRepository paymentTypeRepository, IPaymentRepository paymentRepository, IPaymentOperation paymentOperation)
        {
            this.walletRepository = walletRepository;
            this.paymentTypeRepository = paymentTypeRepository;
            this.paymentRepository = paymentRepository;
            this.paymentOperation = paymentOperation;
        }

        [Authorize]
        [HttpGet]
        [Route("{walletNumber?}")]
        public IEnumerable<PaymentModel> Get([FromRoute] PaymentsRequestModel model)
        {
            var wallet = !string.IsNullOrWhiteSpace(model.WalletNumber) ? walletRepository.Get(model.WalletNumber) : null;

            var payments = paymentRepository.Get(wallet);

            return payments.Select(PaymentModelBuilder.Build).OrderBy(p => p.WalletNumber);
        }

        [Authorize]
        [HttpPost]
        [Route("withdraw")]
        public HttpResponseMessage Withdraw(WithdrawModel model)
        {
            var wallet = walletRepository.Get(model.WalletNumber);
            var paymentType = paymentTypeRepository.Get(model.PaymentTypeCode);
            var payment = paymentOperation.Withdraw(wallet, paymentType, model.Amount);

            return new HttpResponseMessage(payment != null ? System.Net.HttpStatusCode.Created : System.Net.HttpStatusCode.BadRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("deposit")]
        public DepositPaymentUrlModel Deposit(DepositModel model)
        {
            var wallet = walletRepository.Get(model.WalletNumber);
            var paymentType = paymentTypeRepository.Get(model.PaymentTypeCode);

            var payment = paymentOperation.Deposit(wallet, paymentType, model.Amount, out var paymentUrl);

            return new DepositPaymentUrlModel { PaymentUrl = paymentUrl };
        }

        [Authorize]
        [HttpPost]
        [Route("transfer")]
        public HttpResponseMessage Transfer(TransferCreateModel model)
        {
            var walletFrom = walletRepository.Get(model.WalletNumberFrom);
            var walletTo = walletRepository.Get(model.WalletNumberTo);

            var isTransferCreated = paymentOperation.TryTransfer(walletFrom, walletTo, model.Amount);
            
            return new HttpResponseMessage(isTransferCreated ? System.Net.HttpStatusCode.Created : System.Net.HttpStatusCode.BadRequest);
        }
    }
}
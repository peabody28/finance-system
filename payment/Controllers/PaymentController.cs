using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using payment.Interfaces.Operations;
using payment.Interfaces.Repositories;
using payment.Models.Payment;

namespace payment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IWalletRepository walletRepository;

        private readonly IBalanceOperationTypeRepository balanceOperationTypeRepository;

        private readonly IPaymentRepository paymentRepository;

        private readonly IPaymentOperation paymentOperation;

        public PaymentController(IWalletRepository walletRepository, IBalanceOperationTypeRepository balanceOperationTypeRepository, IPaymentRepository paymentRepository, IPaymentOperation paymentOperation)
        {
            this.walletRepository = walletRepository;
            this.balanceOperationTypeRepository = balanceOperationTypeRepository;
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

            return payments.Select(p => new PaymentModel 
            {
                WalletNumber = p.Wallet.Number,
                BalanceOperationTypeCode = p.BalanceOperationType.Code,
                Amount = p.Amount
            }).OrderBy(p => p.WalletNumber);
        }

        [Authorize]
        [HttpPost]
        public HttpResponseMessage Create(PaymentCreateModel model)
        {
            var wallet = walletRepository.Get(model.WalletNumber);
            var balanceOperationType = balanceOperationTypeRepository.Get(model.BalanceOperationTypeCode);

            var isPaymentCreated = paymentOperation.TryCreate(wallet, balanceOperationType, model.Amount);

            return new HttpResponseMessage(isPaymentCreated ? System.Net.HttpStatusCode.Created : System.Net.HttpStatusCode.BadRequest);
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
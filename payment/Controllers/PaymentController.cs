using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public PaymentController(IWalletRepository walletRepository, IBalanceOperationTypeRepository balanceOperationTypeRepository, IPaymentRepository paymentRepository)
        {
            this.walletRepository = walletRepository;
            this.balanceOperationTypeRepository = balanceOperationTypeRepository;
            this.paymentRepository = paymentRepository;
        }

        [Authorize]
        [HttpPost]
        public HttpResponseMessage Create(PaymentCreateModel model)
        {
            var wallet = walletRepository.Get(model.WalletNumber) ?? walletRepository.Create(model.WalletNumber);
            var balanceOperationType = balanceOperationTypeRepository.Get(model.BalanceOperationTypeCode);

            var payment = paymentRepository.Create(wallet, balanceOperationType, model.Amount);

            return new HttpResponseMessage(System.Net.HttpStatusCode.Created);
        }
    }
}
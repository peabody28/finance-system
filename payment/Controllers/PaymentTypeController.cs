using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using payment.Interfaces.Repositories;
using payment.Models.PaymentType;

namespace payment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentTypeController : ControllerBase
    {
        private readonly IPaymentTypeRepository paymentTypeRepository;

        public PaymentTypeController(IPaymentTypeRepository paymentTypeRepository)
        {
            this.paymentTypeRepository = paymentTypeRepository;
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<PaymentTypeModel> Get()
        {
            var paymentTypes = paymentTypeRepository.Get();

            return paymentTypes.Select(pt => new PaymentTypeModel { Code = pt.Code });
        }
    }
}

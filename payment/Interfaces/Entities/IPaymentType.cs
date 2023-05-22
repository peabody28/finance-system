namespace payment.Interfaces.Entities
{
    public interface IPaymentType
    {
        Guid Id { get; set; }

        string Code { get; set; }
    }
}

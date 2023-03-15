namespace payment.Helpers
{
    public class AmountHelper
    {
        public static decimal Compute(decimal amount, decimal rate)
        {
            return amount * rate;
        }
    }
}

using wallet.Helpers;
using wallet.Interfaces.Operations;

namespace wallet.Operations
{
    public class WalletOperation : IWalletOperation
    {
        private readonly IConfiguration configuration;

        public WalletOperation(IConfiguration configuration)
        {
            this.configuration = configuration;

        }

        public string GenerateNumber()
        {
            var pattern = configuration.GetSection("WalletNumberPattern").Value;

            return RegExpHelper.GenerateString(pattern);
        }
    }
}

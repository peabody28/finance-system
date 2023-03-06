using Fare;

namespace wallet.Helpers
{
    public static class RegExpHelper
    {
        public static string GenerateString(string pattern)
        {
            var xeger = new Xeger(pattern);

            return xeger.Generate();
        }
    }
}

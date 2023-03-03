using System.Security.Cryptography;

namespace user.Helper
{
    public static class MD5Helper
    {
        public static string? Hash(string input)
        {
            if (input == null)
                return null;

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }
    }
}

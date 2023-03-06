using Newtonsoft.Json;

namespace user.Models.Auth
{
    public class TokenModel
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("expirationDate")]
        public DateTime ExpirationDate { get; set; }
    }
}

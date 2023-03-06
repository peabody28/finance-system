using Newtonsoft.Json;

namespace user.Models.Auth
{
    public class UserCredentialsModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}

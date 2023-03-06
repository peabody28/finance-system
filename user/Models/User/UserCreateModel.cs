using Newtonsoft.Json;
using user.Models.Auth;

namespace user.Models.User
{
    public class UserCreateModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("roleCode")]
        public string RoleCode { get; set; }
    }
}

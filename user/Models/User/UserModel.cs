using Newtonsoft.Json;

namespace user.Models.User
{
    public class UserModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("roleCode")]
        public string RoleCode { get; set; }

    }
}

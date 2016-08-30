namespace DynDns.Rest.Powershell.Request
{
    using Newtonsoft.Json;

    public class SessionRequest
    {
        [JsonProperty("customer_name")]
        public string CustomerName { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
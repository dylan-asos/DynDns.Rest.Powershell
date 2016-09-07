namespace DynDns.Rest.Powershell.Response.ResponseData
{
    using Newtonsoft.Json;

    public class ZoneChangesData
    {
        [JsonProperty("fqdn")]
        public string FullyQualifiedDomainName { get; set; }

        [JsonProperty("rdata_type")]
        public string RecordType { get; set; }

        [JsonProperty("ttl")]
        public string TimeToLive { get; set; }

        [JsonProperty("zone")]
        public string Zone { get; set; }

        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("serial")]
        public decimal Serial { get; set; }

        [JsonProperty("user_id")]
        public decimal UserId { get; set; }
    }
}
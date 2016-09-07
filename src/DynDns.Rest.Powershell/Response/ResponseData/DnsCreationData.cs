namespace DynDns.Rest.Powershell.Response.ResponseData
{
    using Newtonsoft.Json;

    public class DnsCreationData
    {
        [JsonProperty("fqdn")]
        public string FullyQualifiedDomainName { get; set; }

        [JsonProperty("record_type")]
        public string RecordType { get; set; }

        [JsonProperty("ttl")]
        public string TimeToLive { get; set; }

        [JsonProperty("zone")]
        public string Zone { get; set; }
    }
}
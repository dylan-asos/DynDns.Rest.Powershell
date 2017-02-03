namespace DynDns.Rest.Powershell.Request
{
    using System;

    using Newtonsoft.Json;

    public class CreateDnsEntryRequest
    {
        public CreateDnsEntryRequest(dynamic entryData)
        {
            if (entryData == null)
            {
                throw new ArgumentException("Argument is null or empty", "entryData");
            }

            RData = entryData;
        }

        [JsonProperty("rdata")]
        public dynamic RData { get; set; }

        [JsonProperty("ttl")]
        public string TimeToLive { get; set; }
    }
}
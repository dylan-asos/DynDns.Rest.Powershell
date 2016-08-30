namespace DynDns.Rest.Powershell.Response.ResponseData
{
    using Newtonsoft.Json;

    public class MessageData 
    {
        [JsonProperty("ERR_CD")]
        public string ErrorCode { get; set; }

        [JsonProperty("INFO")]
        public string Information { get; set; }

        [JsonProperty("LVL")]
        public string Level { get; set; }

        [JsonProperty("SOURCE")]
        public string Source { get; set; }
    }
}
namespace DynDns.Rest.Powershell.Response.ResponseData
{
    using Newtonsoft.Json;

    public class PublishZoneData 
    {
        [JsonProperty("serial")]
        public int Serial { get; set; }

        [JsonProperty("serial_style")]
        public string SerialStyle { get; set; }

        [JsonProperty("zone")]
        public string Zone { get; set; }

        [JsonProperty("zone_type")]
        public string ZoneType { get; set; }

        [JsonProperty("task_id")]
        public int TaskId { get; set; }
    }
}
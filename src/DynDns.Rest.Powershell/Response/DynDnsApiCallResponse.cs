namespace DynDns.Rest.Powershell.Response
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DynDns.Rest.Powershell.Response.ResponseData;

    using Newtonsoft.Json;

    public class DynDnsApiCallResponse<T>
    {
        [JsonProperty("job_id")]
        public decimal JobId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("msgs")]
        public List<MessageData> Messages { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        public bool Success
        {
            get
            {
                return Status == "success";
            }
        }

        public string GetErrorMessage()
        {
            var sb = new StringBuilder();
            foreach (var messageData in Messages)
            {
                sb.Append(string.Concat(messageData.ErrorCode, " - ", messageData.Information, Environment.NewLine));
            }

            return sb.ToString();
        }
    }
}
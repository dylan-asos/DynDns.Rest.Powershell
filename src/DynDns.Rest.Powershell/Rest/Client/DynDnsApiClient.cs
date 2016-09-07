namespace DynDns.Rest.Powershell.Rest.Client
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;

    using DynDns.Rest.Powershell.Response;
    using DynDns.Rest.Powershell.Response.ResponseData;

    using Newtonsoft.Json;

    public class DynDnsApiClient
    {
        private readonly HttpClient client;

        private readonly JsonSerializerSettings jsonSerializerSettings;

        private const string ApplicationJson = "application/json";

        public DynDnsApiClient(string baseUrl)
        {
            client = new HttpClient(new DynApiDelegatingHandler()) { BaseAddress = new Uri(baseUrl) };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationJson));

            jsonSerializerSettings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore, NullValueHandling = NullValueHandling.Ignore };

            ServicePointManager.Expect100Continue = false;
        }

        internal static SessionData DynDnsSession { get; set; }

        public DynDnsApiCallResponse Send(HttpRequestMessage request)
        {
            var httpResponseMessage = client.SendAsync(request).Result;

            string responseText = string.Empty;

            if (httpResponseMessage.Content != null)
            {
                responseText = httpResponseMessage.Content.ReadAsStringAsync().Result;
            }

            var response = JsonConvert.DeserializeObject<DynDnsApiCallResponse>(responseText, jsonSerializerSettings);
            response.StatusCode = httpResponseMessage.StatusCode;

            return response;
        }
    }
}
namespace DynDns.Rest.Powershell.Rest.Client
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;

    using DynDns.Rest.Powershell.Response.ResponseData;

    using Newtonsoft.Json;

    public class DynDnsApiClient
    {
        private readonly HttpClient client;

        private const string ApplicationJson = "application/json";

        public DynDnsApiClient(string baseUrl)
        {
            client = new HttpClient(new DynApiDelegatingHandler()) { BaseAddress = new Uri(baseUrl) };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationJson));
            ServicePointManager.Expect100Continue = false;
        }

        internal static SessionData DynDnsSession { get; set; }

        public T Send<T>(HttpRequestMessage request)
        {
            var httpResponseMessage = client.SendAsync(request).Result;

            string responseText = string.Empty;

            if (httpResponseMessage.Content != null)
            {
                responseText = httpResponseMessage.Content.ReadAsStringAsync().Result;
            }

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(responseText);
            }

            throw new DynDnsApiException(responseText);
        }
    }
}
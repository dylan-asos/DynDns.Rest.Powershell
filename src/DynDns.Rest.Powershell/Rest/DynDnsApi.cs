namespace DynDns.Rest.Powershell.Rest
{
    using System.Dynamic;
    using System.Net;
    using System.Net.Http;

    using DynDns.Rest.Powershell.Request;
    using DynDns.Rest.Powershell.Response;
    using DynDns.Rest.Powershell.Response.ResponseData;
    using DynDns.Rest.Powershell.Rest.Client;

    using Newtonsoft.Json.Linq;

    public class DynDnsApi
    {
        private readonly DynDnsApiClient client;

        public DynDnsApi()
        {
            client = new DynDnsApiClient("https://api.dynect.net/");
        }

        public bool HasValidSession
        {
            get
            {
                return DynDnsApiClient.DynDnsSession != null;
            }
        }


        public DynDnsApiCallResponse Login(string userName, string password, string customerName)
        {
            var sessionRequest = new SessionRequest { CustomerName = customerName, UserName = userName, Password = password };

            var request = DynRequestMessage(HttpMethod.Post, "REST/Session/");

            request.Content = sessionRequest.ToStringContent();

            var response = ExecuteDynDnsRequest(request);

            if (response.Success)
            {
                DynDnsApiClient.DynDnsSession = (response.Data as JObject).ToObject<SessionData>();
            }

            return response;
        }

        public DynDnsApiCallResponse Logout()
        {
            var request = DynRequestMessage(HttpMethod.Delete, "REST/Session/");

            var response = ExecuteDynDnsRequest(request);

            if (response.Success)
            {
                DynDnsApiClient.DynDnsSession = null;
            }

            return response;
        }

        public DynDnsApiCallResponse CreateCName(string node, string zone, string data)
        {
            string fqdn = string.Concat(node, ".", zone);

            var request = DynRequestMessage(HttpMethod.Post, string.Format("REST/CNAMERecord/{0}/{1}/", zone, fqdn));

            request.Content = new CreateDnsEntryRequest(new { cname = data }).ToStringContent();

            return ExecuteDynDnsRequest(request);
        }

        public DynDnsApiCallResponse CreateARecord(string node, string zone, string data)
        {
            string fqdn = string.Concat(node, ".", zone);

            var request = DynRequestMessage(HttpMethod.Post, string.Format("REST/ARecord/{0}/{1}/", zone, fqdn));

            request.Content = new CreateDnsEntryRequest(new { address = data }).ToStringContent();

            return ExecuteDynDnsRequest(request);
        }

        public bool DoesEntryAlreadyExist(string node, string zone)
        {
            var response = GetDnsEntry(node, zone);

            return response.StatusCode != HttpStatusCode.NotFound;
        }

        public DynDnsApiCallResponse PublishZone(string zone)
        {
            var request = DynRequestMessage(HttpMethod.Put, string.Format("REST/Zone/{0}/", zone));

            dynamic publishZoneRequest = new { publish = true };

            request.Content = ObjectExtensions.ToJsonContent(publishZoneRequest);

            return ExecuteDynDnsRequest(request);
        }

        public DynDnsApiCallResponse GetZoneChanges(string zone)
        {
            var request = DynRequestMessage(HttpMethod.Get, string.Format("REST/ZoneChanges/{0}/", zone));

            return ExecuteDynDnsRequest(request);
        }

        private HttpRequestMessage DynRequestMessage(HttpMethod method, string resource)
        {
            var request = new HttpRequestMessage(method, resource);

            return request;
        }

        private DynDnsApiCallResponse ExecuteDynDnsRequest(HttpRequestMessage request) 
        {
            return client.Send(request);
        }

        public DynDnsApiCallResponse GetDnsEntry(string node, string zone)
        {
            string fqdn = string.Concat(node, ".", zone);

            var request = DynRequestMessage(HttpMethod.Get, string.Format("REST/ANYRecord/{0}/{1}/", zone, fqdn));

            return ExecuteDynDnsRequest(request);            
        }
    }
}
namespace DynDns.Rest.Powershell.Rest
{
    using System.Linq;
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

        private const string CNameRecordType = "CNAMERecord";
        private const string ARecordType = "ARecord";

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

        public DynDnsApiCallResponse CreateCName(string node, string zone, string data, int ttl)
        {
            return UpdateDnsEntry(HttpMethod.Post, CNameRecordType, node, zone, data, ttl);
        }

        public DynDnsApiCallResponse UpdateCName(string node, string zone, string data, int ttl)
        {
            return UpdateDnsEntry(HttpMethod.Put, CNameRecordType, node, zone, data, ttl);
        }

        private DynDnsApiCallResponse UpdateDnsEntry(HttpMethod method, string recordType, string node, string zone, string data, int ttl)
        {
            var fqdn = GetFullyQualifiedEntry(node, zone);

            var request = DynRequestMessage(method, string.Format("REST/{0}/{1}/{2}/", recordType, zone, fqdn));

            request.Content = new CreateDnsEntryRequest(new { cname = data }) { TimeToLive = ttl > 0 ? ttl.ToString() : "0" }.ToStringContent();

            return ExecuteDynDnsRequest(request);
        }

        public DynDnsApiCallResponse CreateARecord(string node, string zone, string data, int ttl)
        {
            return UpdateDnsEntry(HttpMethod.Post, ARecordType, node, zone, data, ttl);
        }

        public DynDnsApiCallResponse UpdateARecord(string node, string zone, string data, int ttl)
        {
            return UpdateDnsEntry(HttpMethod.Put, ARecordType, node, zone, data, ttl);
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
            var fqdn = GetFullyQualifiedEntry(node, zone);

            var request = DynRequestMessage(HttpMethod.Get, string.Format("REST/ANYRecord/{0}/{1}/", zone, fqdn));

            return ExecuteDynDnsRequest(request);
        }

        private static string GetFullyQualifiedEntry(string node, string zone)
        {
            return string.Concat(node, ".", zone);
        }

        public DynDnsApiCallResponse RemoveDnsEntry(string node, string zone)
        {
            var getEntryResponse = GetDnsEntry(node, zone);

            if (getEntryResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return getEntryResponse;
            }

            var entry = getEntryResponse.Data as JArray;
            var recordToDelete = entry.Values().FirstOrDefault().ToObject<string>();

            var request = DynRequestMessage(HttpMethod.Delete, recordToDelete);

            return ExecuteDynDnsRequest(request);
        }
    }
}
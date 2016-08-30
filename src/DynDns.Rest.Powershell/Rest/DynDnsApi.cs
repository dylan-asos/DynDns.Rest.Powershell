namespace DynDns.Rest.Powershell.Rest
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using DynDns.Rest.Powershell.Request;
    using DynDns.Rest.Powershell.Response;
    using DynDns.Rest.Powershell.Response.ResponseData;
    using DynDns.Rest.Powershell.Rest.Client;

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


        public DynDnsApiCallResponse<SessionData> Login(string userName, string password, string customerName)
        {
            var sessionRequest = new SessionRequest { CustomerName = customerName, UserName = userName, Password = password };

            var request = DynRequestMessage(HttpMethod.Post, "REST/Session/");

            request.Content = sessionRequest.ToStringContent();

            var response = ExecuteDynDnsRequest<DynDnsApiCallResponse<SessionData>>(request);

            if (response.Success)
            {
                DynDnsApiClient.DynDnsSession = response.Data;
            }

            return response;
        }

        public DynDnsApiCallResponse<LogoutData> Logout()
        {
            var request = DynRequestMessage(HttpMethod.Delete, "REST/Session/");

            var response = ExecuteDynDnsRequest<DynDnsApiCallResponse<LogoutData>>(request);

            if (response.Success)
            {
                DynDnsApiClient.DynDnsSession = null;
            }

            return response;
        }

        public DynDnsApiCallResponse<DnsCreationData> CreateCName(string node, string zone, string data)
        {
            string fqdn = string.Concat(node, ".", zone);

            var request = DynRequestMessage(HttpMethod.Post, string.Format("REST/CNAMERecord/{0}/{1}/", zone, fqdn));

            request.Content = new CreateDnsEntryRequest(new { cname = fqdn }).ToStringContent();

            return ExecuteDynDnsRequest<DynDnsApiCallResponse<DnsCreationData>>(request);
        }

        public DynDnsApiCallResponse<DnsCreationData> CreateARecord(string node, string zone, string data)
        {
            string fqdn = string.Concat(node, ".", zone);

            var request = DynRequestMessage(HttpMethod.Post, string.Format("REST/ARecord/{0}/{1}/", zone, fqdn));

            request.Content = new CreateDnsEntryRequest(new { address = fqdn }).ToStringContent();

            return ExecuteDynDnsRequest<DynDnsApiCallResponse<DnsCreationData>>(request);
        }

        public bool DoesEntryAlreadyExist(string node, string zone)
        {
            var response = GetDnsEntry(node, zone);

            return response.Data.Any();
        }

        public DynDnsApiCallResponse<PublishZoneData> PublishZone(string zone)
        {
            var request = DynRequestMessage(HttpMethod.Put, string.Format("REST/Zone/{0}/", zone));

            dynamic publishZoneRequest = new { publish = true };

            request.Content = ObjectExtensions.ToJsonContent(publishZoneRequest);

            return ExecuteDynDnsRequest<DynDnsApiCallResponse<PublishZoneData>>(request);
        }

        public DynDnsApiCallResponse<List<ZoneChangesData>> GetZoneChanges(string zone)
        {
            var request = DynRequestMessage(HttpMethod.Get, string.Format("REST/ZoneChanges/{0}/", zone));

            return ExecuteDynDnsRequest<DynDnsApiCallResponse<List<ZoneChangesData>>>(request);
        }

        private HttpRequestMessage DynRequestMessage(HttpMethod method, string resource)
        {
            var request = new HttpRequestMessage(method, resource);

            return request;
        }

        private T ExecuteDynDnsRequest<T>(HttpRequestMessage request) 
        {
            var response = client.Send<T>(request);

            return response;
        }

        public DynDnsApiCallResponse<List<string>>  GetDnsEntry(string node, string zone)
        {
            string fqdn = string.Concat(node, ".", zone);

            var request = DynRequestMessage(HttpMethod.Get, string.Format("REST/ANYRecord/{0}/{1}/", zone, fqdn));

            return ExecuteDynDnsRequest<DynDnsApiCallResponse<List<string>>>(request);            
        }
    }
}
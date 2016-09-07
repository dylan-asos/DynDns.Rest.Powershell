namespace DynDns.Rest.Powershell.Rest.Client
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    public class DynApiDelegatingHandler : DelegatingHandler
    {
        private const string ContentType = "Content-Type";

        public DynApiDelegatingHandler()
            : this(new HttpClientHandler())
        {
        }

        public DynApiDelegatingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (DynDnsApiClient.DynDnsSession != null)
            {
                request.Headers.Add("Auth-Token", DynDnsApiClient.DynDnsSession.Token);
            }

            SetContentTypeHeaderForAllRequests(request);

            return await base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        ///     Total hack to get around every resource request to Dyn Dns requring a Content-Type
        ///     header - HttpClient requests by default don't allow setting the Conetent-Type on
        ///     GET and DELETE requests.
        /// </summary>
        /// <param name="request"></param>
        private void SetContentTypeHeaderForAllRequests(HttpRequestMessage request)
        {
            var invalidHeaders = (HashSet<string>)typeof(HttpHeaders).GetField("invalidHeaders", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(request.Headers);

            invalidHeaders.Remove(ContentType);
            request.Headers.Remove(ContentType);

            request.Headers.Add(ContentType, "application/json");
        }
    }
}
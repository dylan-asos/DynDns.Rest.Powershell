namespace DynDns.Rest.Powershell.Rest.Client
{
    using System.Net.Http;
    using System.Text;

    using Newtonsoft.Json;

    internal static class ObjectExtensions
    {
        private const string JsonMediaType = "application/json";

        public static StringContent ToStringContent(this object instance)
        {
            var jsonContent = JsonConvert.SerializeObject(instance);

            return new StringContent(jsonContent, Encoding.UTF8, JsonMediaType);
        }

        public static StringContent ToJsonContent(dynamic instance)
        {
            var jsonContent = JsonConvert.SerializeObject(instance);

            return new StringContent(jsonContent, Encoding.UTF8, JsonMediaType);
        }
    }
}
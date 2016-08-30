namespace DynDns.Rest.Powershell
{
    using System;

    public class DynDnsApiException : Exception
    {
        public DynDnsApiException()
        {
        }

        public DynDnsApiException(string message)
            : base(message)
        {
        }

        public DynDnsApiException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
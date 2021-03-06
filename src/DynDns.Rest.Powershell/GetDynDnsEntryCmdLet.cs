namespace DynDns.Rest.Powershell
{
    using System.Management.Automation;

    using DynDns.Rest.Powershell.Response;

    [Cmdlet(VerbsCommon.Get, "DynDnsEntry")]
    public class GetDynDnsEntryCmdLet : DynDnsPsCmdLet
    {
        [Parameter(Mandatory = true, HelpMessage = "The entry node")]
        public string Node { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "The zone of the entry")]
        public string Zone { get; set; }

        protected override DynDnsApiCallResponse CallDynDnsApi()
        {
            return ApiClient.GetDnsEntry(Node, Zone);
        }
    }
}
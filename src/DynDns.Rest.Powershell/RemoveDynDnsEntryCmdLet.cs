namespace DynDns.Rest.Powershell
{
    using System.Management.Automation;

    using DynDns.Rest.Powershell.Response;

    [Cmdlet(VerbsCommon.Remove, "DynDnsEntry")]
    public class RemoveDynDnsEntryCmdLet : DynDnsPsCmdLet
    {
        [Parameter(Mandatory = true, HelpMessage = "The zone for the DNS entry")]
        public string Zone { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "The node to remove in the zone")]
        public string Node { get; set; }

        protected override DynDnsApiCallResponse CallDynDnsApi()
        {
            return ApiClient.RemoveDnsEntry(Node, Zone);
        }
    }
}
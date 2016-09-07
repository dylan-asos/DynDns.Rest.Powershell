namespace DynDns.Rest.Powershell
{
    using System.Management.Automation;

    using DynDns.Rest.Powershell.Response;

    [Cmdlet(VerbsCommon.Get, "DynDnsZoneChanges")]
    public class GetDynDnsZoneChangesCmdLet : DynDnsPsCmdLet
    {
        [Parameter(Mandatory = true, HelpMessage = "The zone changes to publish")]
        public string Zone { get; set; }

        protected override DynDnsApiCallResponse CallDynDnsApi()
        {
            return ApiClient.GetZoneChanges(Zone);
        }
    }
}
namespace DynDns.Rest.Powershell
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using DynDns.Rest.Powershell.Response;
    using DynDns.Rest.Powershell.Response.ResponseData;

    [Cmdlet(VerbsCommon.Get, "DynDnsZoneChanges")]
    public class GetDynDnsZoneChangesCmdLet : DynDnsPsCmdLet<List<ZoneChangesData>>
    {
        [Parameter(Mandatory = true, HelpMessage = "The zone changes to publish")]
        public string Zone { get; set; }

        protected override DynDnsApiCallResponse<List<ZoneChangesData>> CallDynDnsApi()
        {
            return ApiClient.GetZoneChanges(Zone);
        }
    }
}
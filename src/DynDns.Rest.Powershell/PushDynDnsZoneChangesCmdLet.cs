namespace DynDns.Rest.Powershell
{
    using System.Management.Automation;

    using DynDns.Rest.Powershell.Response;

    [Cmdlet(VerbsCommon.Push, "DynDnsZoneChanges", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
    public class PushDynDnsZoneChangesCmdLet : DynDnsPsCmdLet
    {
        [Parameter(Mandatory = true, HelpMessage = "The zone changes to publish")]
        public string Zone { get; set; }

        protected override ConfirmationDetails ConfirmationDetails
        {
            get
            {
                return new ConfirmationDetails { Caption = string.Format("Push zone changes to {0}", Zone), Query = "Are you sure you want to publish the zone changes?", Target = Zone };
            }
        }

        protected override DynDnsApiCallResponse CallDynDnsApi()
        {
            return ApiClient.PublishZone(Zone);
        }
    }
}
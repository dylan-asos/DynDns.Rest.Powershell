namespace DynDns.Rest.Powershell
{
    using System.Management.Automation;

    using DynDns.Rest.Powershell.Response;

    [Cmdlet(VerbsCommon.Close, "DynDnsSession")]
    public class CloseDynDnsSessionCmdLet : DynDnsPsCmdLet
    {
        protected override DynDnsApiCallResponse CallDynDnsApi()
        {
            return ApiClient.Logout();
        }
    }
}
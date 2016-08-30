namespace DynDns.Rest.Powershell
{
    using System.Management.Automation;

    using DynDns.Rest.Powershell.Response;
    using DynDns.Rest.Powershell.Response.ResponseData;

    [Cmdlet(VerbsCommon.Close, "DynDnsSession")]
    public class CloseDynDnsSessionCmdLet : DynDnsPsCmdLet<LogoutData>
    {
        protected override DynDnsApiCallResponse<LogoutData> CallDynDnsApi()
        {
            return ApiClient.Logout();
        }
    }
}
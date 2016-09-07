namespace DynDns.Rest.Powershell
{
    using System.Management.Automation;

    using DynDns.Rest.Powershell.Response;

    [Cmdlet(VerbsCommon.Get, "NewDynDnsSession")]
    public class NewDynDnsSessionCmdLet : DynDnsPsCmdLet
    {
        [Parameter(Mandatory = true, HelpMessage = "The user name to connect as")]
        [Alias("user")]
        public string UserName { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "The password for the user")]
        [Alias("pwd")]
        public string Password { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "The customer name that the user is associated with")]
        [Alias("customer")]
        public string CustomerName { get; set; }

        protected override bool AuthenticationRequired
        {
            get
            {
                return false;
            }
        }

        protected override DynDnsApiCallResponse CallDynDnsApi()
        {
            return ApiClient.Login(UserName, Password, CustomerName);
        }
    }
}
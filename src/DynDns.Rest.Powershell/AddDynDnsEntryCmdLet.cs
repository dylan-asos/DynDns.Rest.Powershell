namespace DynDns.Rest.Powershell
{
    using System;
    using System.Management.Automation;

    using DynDns.Rest.Powershell.Request;
    using DynDns.Rest.Powershell.Response;

    [Cmdlet(VerbsCommon.Add, "DynDnsEntry")]
    public class AddDynDnsEntryCmdLet : DynDnsPsCmdLet
    {
        [Parameter(Mandatory = true, HelpMessage = "The zone for the DNS entry")]
        public string Zone { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "The environment number to create the in the remote desktop settings")]
        public string Node { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "The rdg file that holds your environment")]
        [Alias("rd")]
        public string RData { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "RecordType for DNS entry?")]
        [Alias("rt")]
        public RecordType RecordType { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Time to live")]
        [Alias("ttl")]
        public string TimeToLive { get; set; }

        protected override DynDnsApiCallResponse CallDynDnsApi()
        {
            if (ApiClient.DoesEntryAlreadyExist(Node, Zone))
            {
                WriteError(new ErrorRecord(new Exception(string.Format("Record {0} already exists in zone {1}", Node, Zone)), "CONFLICT_ERROR_RECORD_EXISTS", ErrorCategory.InvalidOperation, null));

                return null;
            }

            return RecordType == RecordType.CName ? ApiClient.CreateCName(Node, Zone, RData) : ApiClient.CreateARecord(Node, Zone, RData);
        }
    }
}
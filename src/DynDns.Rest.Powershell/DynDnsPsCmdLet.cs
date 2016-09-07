namespace DynDns.Rest.Powershell
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    using DynDns.Rest.Powershell.Response;
    using DynDns.Rest.Powershell.Rest;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Defines the base functionality for interacting with the DynDns REST API. Access to the API client, 
    /// </summary>
    /// <typeparam name="T">The data type returned by the call to the DynDns API.
    /// </typeparam>
    public class DynDnsPsCmdLet<T> : PSCmdlet
    {
        protected DynDnsApi ApiClient;

        public DynDnsPsCmdLet()
        {
            ApiClient = new DynDnsApi();
        }

        [Parameter]
        public SwitchParameter Force { get; set; }

        public virtual bool CheckAuthenticationState()
        {
            if (ApiClient.HasValidSession)
            {
                return true;
            }

            WriteError(new ErrorRecord(new PSSecurityException("You must login to DynDns first"), "NOT_LOGGED_IN", ErrorCategory.PermissionDenied, null));

            return false;
        }

        /// <summary>
        /// A <see cref="ConfirmationDetails"/> instance to use in any confirmation dialogues to display during a powershell operation 
        /// </summary>
        protected virtual ConfirmationDetails ConfirmationDetails
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// True if the client is required to have signed in to DynDns before performing an operation, otherwise False.
        /// </summary>
        protected virtual bool AuthenticationRequired
        {
            get
            {
                return true;
            }
        }

        protected virtual DynDnsApiCallResponse CallDynDnsApi()
        {
            return null;
        }

        protected override void ProcessRecord()
        {
            if (!CanPerformAction())
            {
                return;
            }

            if (AuthenticationRequired && !CheckAuthenticationState())
            {
                return;
            }

            try
            {
                var response = CallDynDnsApi();

                WriteDynDnsResponse(response);
            }
            catch (Exception exception)
            {
                WriteError(new ErrorRecord(exception, "API_CALL_ERROR", ErrorCategory.InvalidOperation, "An exception has been thrown during the API call"));
            }
        }

        private bool CanPerformAction()
        {
            if (ConfirmationDetails == null)
            {
                return true;
            }

            if (!ShouldProcess(ConfirmationDetails.Target))
            {
                return true;
            }

            return Force || ShouldContinue(ConfirmationDetails.Query, ConfirmationDetails.Caption);
        }

        protected virtual void WriteDynDnsResponse(DynDnsApiCallResponse response)
        {
            if (response == null)
            {
                return;
            }

            if (response.Success)
            {
                var output = response.Data as JObject;

                if (output != null)
                {
                    WriteObject(output.ToString(Formatting.Indented));
                }

                var arrayData = response.Data as JArray;
                if (arrayData != null)
                {
                    WriteObject(arrayData.ToString(Formatting.Indented));
                }
            }
            else
            {
                WriteError(new ErrorRecord(new Exception(response.GetErrorMessage()), response.Messages.FirstOrDefault().ErrorCode, ErrorCategory.InvalidResult, response.Data));
            }
        }
    }
}
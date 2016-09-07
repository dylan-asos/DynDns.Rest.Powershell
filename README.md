# DynDns.Rest.Powershell
Unofficial powershell wrapper around the DynDns Rest API -  A .NET Powershell library for working with the DynDns Rest API - https://help.dyn.com/rest/

## Usage
Below are some samples of usage. This is only a subset of the possible API operations available via the DynDns interface


### Get-NewDynDnsSession

Logs you in to DnsDns and starts a new session for the user

```powershell
Get-NewDynDnsSession -user dyndnsuser -pwd the-password -customer customer-name
```


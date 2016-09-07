# DynDns.Rest.Powershell
Unofficial powershell wrapper around the DynDns Rest API -  A .NET Powershell library for working with the DynDns Rest API - https://help.dyn.com/rest/

## Usage
Below are some samples of usage. This is only a subset of the possible API operations available via the DynDns interface

Usage follows the DynDns guidelines of, 

* Log-on
* Make Changes and updates
* Publish changes
* Log-out

### Get-NewDynDnsSession

Logs you in to DnsDns and starts a new session for the user

```powershell
Get-NewDynDnsSession -user dyndnsuser -pwd the-password -customer customer-name
```

You *must* start a new DynDns session before attempting operations, or an error is displayed

### Get-DynDnsEntry

Gets *any* matching record type for the specified criteria. 

```powershell
Get-DynDnsEntry -node some-dns-entry -zone some-zone.com
```

If record *some-dns-entry.some-zone.com* exists, the results are displayed

### Add-DynDnsEntry

Supports adding a Cname or ARecord to a DNS Zone

#### CName example

This would create CNAME *the-node-to-create.somezone.com* over *some.origin.net*

```powershell
Add-DynDnsEntry -zone somezone.com -node the-node-to-create -rt CName -rd some.origin.net
```

#### A Record example

This would create A Record *the-node-to-create.somezone.com* over *1.2.3.4*

```powershell
Add-DynDnsEntry -zone somezone.com -node the-node-to-create -rt ARecord -rd 1.2.3.4
```

### Get-DynDnsZoneChanges

Any outstanding changes waiting to be published are displayed

```powershell
Get-DynDnsZoneChanges -zone some-zone.com
```

### Push-DynDnsZoneChanges

Requires confirmation. Any outstanding changes to be published are updated in the zone

```powershell
Push-DynDnsZoneChanges -zone some-zone.com
```

### Close-DynDnsSession

Ends your session. If any changes were made that weren't commited using Push-DynDnsZoneChanges , they are abandoned and no updates are made






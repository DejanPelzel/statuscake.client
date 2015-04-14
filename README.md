# StatusCake .NET Api Client
A simple .NET client for communication with the StatusCake website monitoring service API.

#### Examples:

The StatusCakeClient library is incredibly simple to use. It supports two authentication options. You can either use the constructor to pass the username or password like so:
```c#
var statusCakeClient = new StatusCakeClient("MyUsername", "MyAccessKey");
```
or simply update your appSettings section in the configuration file with the following keys:
```c#
<appSettings>
    <add key="StatusCake.Client.Username" value="MyUsername"/>
    <add key="StatusCake.Client.AccessKey" value="MyAccessKey"/>
</appSettings>
```

Get test list:

```c#
var statusCakeClient = new StatusCakeClient();
await statusCakeClient.GetTestsAsync();
```

# StatusCake .NET Api Client
A simple .NET client for communication with the StatusCake website monitoring service API.

#### Examples:

Get test list:

```c#
var statusCakeClient = new StatusCakeClient();
await statusCakeClient.GetTestsAsync();
```

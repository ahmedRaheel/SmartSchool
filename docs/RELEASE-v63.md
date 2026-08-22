# v63 SignalR compile fix

- Removed duplicate Microsoft.AspNetCore.App from Communication.csproj; Directory.Build.props already supplies it to SmartSchool.Modules.*.
- Added explicit ASP.NET Core DI/routing imports in Module.cs.
- Added explicit SignalR/System imports in CommunicationHubs.cs.
- NotificationHub and ChatHub derive from Microsoft.AspNetCore.SignalR.Hub.
- No SignalR server NuGet package is required.

Delete bin/obj, restore, and rebuild. The missing Communication.dll metadata error is cascading from the Communication project build failure.

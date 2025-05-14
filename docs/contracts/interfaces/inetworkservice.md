# INetworkService Interface

## Description

The `INetworkService` interface defines the contract for a service that provides information about network interfaces and allows monitoring their status.

**Namespace:** `NetBindUI.Contracts.Interfaces.Network`  
**Assembly:** `NetBindUI.Contracts`

## Methods

### GetNetworkInterfacesAsync

```csharp
Task<IEnumerable<INetworkInterface>> GetNetworkInterfacesAsync();
Returns a collection of all available network interfaces.

Parameters:

None
Returns:

Task<IEnumerable<INetworkInterface>>: A task that represents the asynchronous operation. The task result contains a collection of INetworkInterface objects, each representing a network interface.
Exceptions:

System.Exception: If an error occurs while retrieving the network interfaces.
Example:

csharp
Copy Code
INetworkService networkService = new NetworkService();
var interfaces = await networkService.GetNetworkInterfacesAsync();
foreach (var i in interfaces)
{
    Console.WriteLine(i.Name);
}
GetInterfaceByIdAsync
csharp
Copy Code
Task<INetworkInterface?> GetInterfaceByIdAsync(string interfaceId);
Returns detailed information about a specific network interface.

Parameters:

interfaceId (string): The identifier of the network interface.
Returns:

Task<INetworkInterface?>: A task that represents the asynchronous operation. The task result contains an INetworkInterface object if the interface is found; otherwise, null.
Exceptions:

System.Exception: If an error occurs while retrieving the network interface.
Example:

csharp
Copy Code
INetworkService networkService = new NetworkService();
var interface = await networkService.GetInterfaceByIdAsync("Ethernet");
if (interface != null)
{
    Console.WriteLine(interface.Name);
}
GetActiveNetworkInterfacesAsync
csharp
Copy Code
Task<IEnumerable<INetworkInterface>> GetActiveNetworkInterfacesAsync();
Returns a collection of active network interfaces.

Parameters:

None
Returns:

Task<IEnumerable<INetworkInterface>>: A task that represents the asynchronous operation. The task result contains a collection of INetworkInterface objects, each representing an active network interface.
Exceptions:

System.Exception: If an error occurs while retrieving the network interfaces.
Example:

csharp
Copy Code
INetworkService networkService = new NetworkService();
var interfaces = await networkService.GetActiveNetworkInterfacesAsync();
foreach (var i in interfaces)
{
    Console.WriteLine(i.Name);
}
GetNetworkInterfacesByTypeAsync
csharp
Copy Code
Task<IEnumerable<INetworkInterface>> GetNetworkInterfacesByTypeAsync(NetworkInterfaceType type);
Returns a collection of network interfaces of a specific type.

Parameters:

type (NetworkInterfaceType): The type of network interface (e.g., Ethernet, Wireless).
Returns:

Task<IEnumerable<INetworkInterface>>: A task that represents the asynchronous operation. The task result contains a collection of INetworkInterface objects, each representing a network interface of the specified type.
Exceptions:

System.Exception: If an error occurs while retrieving the network interfaces.
Example:

csharp
Copy Code
INetworkService networkService = new NetworkService();
var interfaces = await networkService.GetNetworkInterfacesByTypeAsync(NetworkInterfaceType.Wireless80211);
foreach (var i in interfaces)
{
    Console.WriteLine(i.Name);
}
GetInterfaceStatisticsAsync
csharp
Copy Code
Task<NetworkInterfaceStatistics> GetInterfaceStatisticsAsync(string interfaceId);
Returns statistics for a specific network interface.

Parameters:

interfaceId (string): The identifier of the network interface.
Returns:

Task<NetworkInterfaceStatistics>: A task that represents the asynchronous operation. The task result contains a NetworkInterfaceStatistics object, containing statistics for the specified interface.
Exceptions:

System.ArgumentException: If the network interface with the specified ID is not found.
System.Exception: If an error occurs while retrieving the statistics.
Example:

csharp
Copy Code
INetworkService networkService = new NetworkService();
var statistics = await networkService.GetInterfaceStatisticsAsync("Ethernet");
Console.WriteLine(statistics.BytesReceived);
StartInterfaceMonitoringAsync
csharp
Copy Code
Task<bool> StartInterfaceMonitoringAsync();
Starts monitoring the status of network interfaces and raises events when changes occur.

Parameters:

None
Returns:

Task<bool>: A task that represents the asynchronous operation. The task result is true if monitoring started successfully; otherwise, false.
Exceptions:

System.Exception: If an error occurs while starting the monitoring.
Example:

csharp
Copy Code
INetworkService networkService = new NetworkService();
bool started = await networkService.StartInterfaceMonitoringAsync();
if (started)
{
    Console.WriteLine("Monitoring started");
}
StopInterfaceMonitoringAsync
csharp
Copy Code
Task StopInterfaceMonitoringAsync();
Stops monitoring network interface status.

Parameters:

None
Returns:

Task: A task that represents the asynchronous operation.
Exceptions:

System.Exception: If an error occurs while stopping the monitoring.
Example:

csharp
Copy Code
INetworkService networkService = new NetworkService();
await networkService.StopInterfaceMonitoringAsync();
Console.WriteLine("Monitoring stopped");
Events
NetworkInterfaceChanged
csharp
Copy Code
event EventHandler<NetworkInterfaceChangedEventArgs> NetworkInterfaceChanged;
Occurs when a network interface changes (e.g., when it connects to a network or disconnects from a network).

Event Arguments:

NetworkInterfaceChangedEventArgs: Provides data for the NetworkInterfaceChanged event.
Example:

csharp
Copy Code
INetworkService networkService = new NetworkService();
networkService.NetworkInterfaceChanged += (sender, e) =>
{
    Console.WriteLine($"Interface {e.InterfaceId} changed");
};
await networkService.StartInterfaceMonitoringAsync();
Dependencies
NetBindUI.Contracts.Interfaces.Network.INetworkInterface
NetBindUI.Contracts.Models.Network.NetworkInterfaceStatistics
NetBindUI.Contracts.Events.NetworkInterfaceChangedEventArgs
Notes
This interface provides a high-level abstraction for managing network interfaces.
Implementations of this interface should handle all platform-specific details.
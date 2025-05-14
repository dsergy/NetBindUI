using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using NetBindUI.Contracts.Events;
using NetBindUI.Contracts.Interfaces.Network;
using NetBindUI.Contracts.Models.Network;

namespace NetBindUI.Contracts.Interfaces.Network;

/// <summary>
/// Provides methods for working with network interfaces and obtaining network-related information
/// </summary>
public interface INetworkService
{
    /// <summary>
    /// Gets a list of all available network interfaces
    /// </summary>
    /// <returns>Collection of network interface information</returns>
    Task<IEnumerable<INetworkInterface>> GetNetworkInterfacesAsync();

    /// <summary>
    /// Gets detailed information about a specific network interface
    /// </summary>
    /// <param name="interfaceId">Network interface identifier</param>
    /// <returns>Detailed interface information or null if not found</returns>
    Task<INetworkInterface?> GetInterfaceByIdAsync(string interfaceId);

    /// <summary>
    /// Monitors the status of network interfaces and raises events when changes occur
    /// </summary>
    /// <returns>True if monitoring started successfully</returns>
    Task<bool> StartInterfaceMonitoringAsync();

    /// <summary>
    /// Stops monitoring network interface status
    /// </summary>
    Task StopInterfaceMonitoringAsync();

    /// <summary>
    /// Gets a list of active network interfaces
    /// </summary>
    /// <returns>Collection of active network interface information</returns>
    Task<IEnumerable<INetworkInterface>> GetActiveNetworkInterfacesAsync();

    /// <summary>
    /// Gets a list of network interfaces of a specific type
    /// </summary>
    /// <param name="type">Network interface type</param>
    /// <returns>Collection of network interface information</returns>
    Task<IEnumerable<INetworkInterface>> GetNetworkInterfacesByTypeAsync(NetworkInterfaceType type);

    /// <summary>
    /// Gets statistics for a specific network interface
    /// </summary>
    /// <param name="interfaceId">Network interface identifier</param>
    /// <returns>Network interface statistics</returns>
    Task<NetworkInterfaceStatistics> GetInterfaceStatisticsAsync(string interfaceId);

    /// <summary>
    /// Event raised when a network interface changes
    /// </summary>
    event EventHandler<NetworkInterfaceChangedEventArgs> NetworkInterfaceChanged;
}
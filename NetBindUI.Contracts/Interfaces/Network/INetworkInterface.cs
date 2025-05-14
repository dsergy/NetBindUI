using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using NetBindUI.Contracts.Models.Network;

namespace NetBindUI.Contracts.Interfaces.Network;

/// <summary>
/// Represents network interface information
/// </summary>
public interface INetworkInterface
{
    /// <summary>
    /// Unique identifier of the network interface
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Human-readable name of the network interface
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Description of the network interface
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Current status of the network interface
    /// </summary>
    NetworkInterfaceStatus Status { get; }

    /// <summary>
    /// List of IP addresses associated with this interface
    /// </summary>
    IReadOnlyList<IPAddress> IPAddresses { get; }

    /// <summary>
    /// Type of the network interface (Ethernet, WiFi, etc.)
    /// </summary>
    NetworkInterfaceType Type { get; }

    /// <summary>
    /// Media Access Control (MAC) address of the interface
    /// </summary>
    PhysicalAddress MacAddress { get; }

    /// <summary>
    /// Maximum Transmission Unit (MTU) of the interface
    /// </summary>
    int Mtu { get; }

    /// <summary>
    /// Link speed in bits per second
    /// </summary>
    long Speed { get; }

    /// <summary>
    /// Detailed operational status of the interface
    /// </summary>
    OperationalStatus OperationalStatus { get; }

    /// <summary>
    /// Index used by the operating system to identify the interface
    /// </summary>
    int InterfaceIndex { get; }

    /// <summary>
    /// List of gateway addresses used by the interface
    /// </summary>
    IReadOnlyList<GatewayIPAddressInformation> GatewayAddresses { get; }

    /// <summary>
    /// List of DNS servers used by the interface
    /// </summary>
    IReadOnlyList<IPAddress> DnsAddresses { get; }
}
using System;

namespace NetBindUI.Contracts.Events;

/// <summary>
/// Event arguments for network interface changes
/// </summary>
public class NetworkInterfaceChangedEventArgs : EventArgs
{
    /// <summary>
    /// Network interface identifier
    /// </summary>
    public string InterfaceId { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="interfaceId">Network interface identifier</param>
    public NetworkInterfaceChangedEventArgs(string interfaceId)
    {
        InterfaceId = interfaceId;
    }
}
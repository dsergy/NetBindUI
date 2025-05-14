namespace NetBindUI.Contracts.Models.Network;

/// <summary>
/// Represents network interface statistics information
/// </summary>
public class NetworkInterfaceStatistics
{
    /// <summary>
    /// Number of bytes received
    /// </summary>
    public long BytesReceived { get; }

    /// <summary>
    /// Number of bytes sent
    /// </summary>
    public long BytesSent { get; }

    /// <summary>
    /// Number of packets received
    /// </summary>
    public long PacketsReceived { get; }

    /// <summary>
    /// Number of packets sent
    /// </summary>
    public long PacketsSent { get; }

    /// <summary>
    /// Number of errors during receiving
    /// </summary>
    public long InErrors { get; }

    /// <summary>
    /// Number of errors during sending
    /// </summary>
    public long OutErrors { get; }

    /// <summary>
    /// Constructor for NetworkInterfaceStatistics
    /// </summary>
    public NetworkInterfaceStatistics(
        long bytesReceived,
        long bytesSent,
        long packetsReceived,
        long packetsSent,
        long inErrors,
        long outErrors)
    {
        BytesReceived = bytesReceived;
        BytesSent = bytesSent;
        PacketsReceived = packetsReceived;
        PacketsSent = packetsSent;
        InErrors = inErrors;
        OutErrors = outErrors;
    }
}
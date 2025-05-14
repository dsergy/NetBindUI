using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using NetBindUI.Contracts.Interfaces.Network;
using NetBindUI.Contracts.Models.Network;  // Добавляем этот using

namespace NetBindUI.Core.Models
{
    /// <summary>
    /// Represents detailed information about a network interface
    /// </summary>
    public class NetworkInterfaceInfo : INetworkInterface
    {
        private NetworkInterface _networkInterface;

        /// <summary>
        /// Initializes a new instance of the NetworkInterfaceInfo class
        /// </summary>
        /// <param name="networkInterface">The network interface to wrap</param>
        public NetworkInterfaceInfo(NetworkInterface networkInterface)
        {
            _networkInterface = networkInterface ?? throw new ArgumentNullException(nameof(networkInterface));
            Id = _networkInterface.Id;
            Update(networkInterface);
        }

        /// <summary>
        /// Updates the network interface information
        /// </summary>
        /// <param name="networkInterface">The network interface with updated information</param>
        public void Update(NetworkInterface networkInterface)
        {
            _networkInterface = networkInterface ?? throw new ArgumentNullException(nameof(networkInterface));

            // Update all properties
            Status = ConvertStatus(_networkInterface.OperationalStatus);
            Type = _networkInterface.NetworkInterfaceType;
            Speed = _networkInterface.Speed;
            Description = _networkInterface.Description;
            Name = _networkInterface.Name;
            MacAddress = _networkInterface.GetPhysicalAddress();
            OperationalStatus = _networkInterface.OperationalStatus;
            InterfaceIndex = _networkInterface.GetIPProperties().GetIPv4Properties()?.Index ?? 0;

            // Update IP addresses and network information
            var ipProperties = _networkInterface.GetIPProperties();

            IPAddresses = ipProperties.UnicastAddresses
                .Select(addr => addr.Address)
                .ToList()
                .AsReadOnly();

            GatewayAddresses = ipProperties.GatewayAddresses
                .ToList()
                .AsReadOnly();

            DnsAddresses = ipProperties.DnsAddresses
                .ToList()
                .AsReadOnly();

            // Update MTU for IPv4 interfaces
            try
            {
                Mtu = ipProperties.GetIPv4Properties()?.Mtu ?? 0;
            }
            catch
            {
                Mtu = 0;
            }
        }

        private static NetworkInterfaceStatus ConvertStatus(OperationalStatus status)
        {
            return status switch
            {
                OperationalStatus.Up => NetworkInterfaceStatus.Up,
                OperationalStatus.Down => NetworkInterfaceStatus.Down,
                _ => NetworkInterfaceStatus.Unknown
            };
        }

        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public string Description { get; private set; }

        /// <inheritdoc />
        public NetworkInterfaceStatus Status { get; private set; }

        /// <inheritdoc />
        public NetworkInterfaceType Type { get; private set; }

        /// <inheritdoc />
        public long Speed { get; private set; }

        /// <inheritdoc />
        public PhysicalAddress MacAddress { get; private set; }

        /// <inheritdoc />
        public int Mtu { get; private set; }

        /// <inheritdoc />
        public IReadOnlyList<IPAddress> IPAddresses { get; private set; }

        /// <inheritdoc />
        public OperationalStatus OperationalStatus { get; private set; }

        /// <inheritdoc />
        public int InterfaceIndex { get; private set; }

        /// <inheritdoc />
        public IReadOnlyList<GatewayIPAddressInformation> GatewayAddresses { get; private set; }

        /// <inheritdoc />
        public IReadOnlyList<IPAddress> DnsAddresses { get; private set; }
    }
}
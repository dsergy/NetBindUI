using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using NetBindUI.Contracts.Models.Network;
using NetBindUI.Contracts.Events;
using NetBindUI.Contracts.Interfaces.Network;
using NetBindUI.Core.Models;

namespace NetBindUI.Core.Services
{
    /// <summary>
    /// Implementation of the INetworkService interface
    /// </summary>
    public class NetworkService : INetworkService
    {
        private bool _isMonitoring = false;
        private readonly Dictionary<string, NetworkInterfaceInfo> _interfaceCache = new();

        /// <summary>
        /// Gets a list of all available network interfaces
        /// </summary>
        /// <returns>Collection of network interface information</returns>
        public async Task<IEnumerable<INetworkInterface>> GetNetworkInterfacesAsync()
        {
            return await Task.Run(() =>
            {
                var interfaces = NetworkInterface.GetAllNetworkInterfaces();
                var result = new List<INetworkInterface>();

                foreach (var ni in interfaces)
                {
                    if (!_interfaceCache.TryGetValue(ni.Id, out var cachedInterface))
                    {
                        cachedInterface = new NetworkInterfaceInfo(ni);
                        _interfaceCache[ni.Id] = cachedInterface;
                    }
                    else
                    {
                        cachedInterface.Update(ni);
                    }
                    result.Add(cachedInterface);
                }

                // Очищаем кэш от несуществующих интерфейсов
                var existingIds = interfaces.Select(ni => ni.Id).ToHashSet();
                var removedIds = _interfaceCache.Keys.Where(id => !existingIds.Contains(id)).ToList();
                foreach (var id in removedIds)
                {
                    _interfaceCache.Remove(id);
                }

                return result;
            });
        }

        /// <summary>
        /// Gets detailed information about a specific network interface
        /// </summary>
        /// <param name="interfaceId">Network interface identifier</param>
        /// <returns>Detailed interface information or null if not found</returns>
        public async Task<INetworkInterface?> GetInterfaceByIdAsync(string interfaceId)
        {
            return await Task.Run(() =>
            {
                if (_interfaceCache.TryGetValue(interfaceId, out var cachedInterface))
                {
                    var ni = NetworkInterface.GetAllNetworkInterfaces()
                        .FirstOrDefault(ni => ni.Id == interfaceId);

                    if (ni != null)
                    {
                        cachedInterface.Update(ni);
                        return cachedInterface;
                    }
                }

                var newNi = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(ni => ni.Id == interfaceId);

                if (newNi != null)
                {
                    var newInterface = new NetworkInterfaceInfo(newNi);
                    _interfaceCache[interfaceId] = newInterface;
                    return newInterface;
                }

                return null;
            });
        }

        /// <summary>
        /// Gets a list of active network interfaces
        /// </summary>
        /// <returns>Collection of active network interface information</returns>
        public async Task<IEnumerable<INetworkInterface>> GetActiveNetworkInterfacesAsync()
        {
            var allInterfaces = await GetNetworkInterfacesAsync();
            return allInterfaces.Where(ni => ni.Status == NetworkInterfaceStatus.Up);
        }

        /// <summary>
        /// Gets a list of network interfaces of a specific type
        /// </summary>
        /// <param name="type">Network interface type</param>
        /// <returns>Collection of network interface information</returns>
        public async Task<IEnumerable<INetworkInterface>> GetNetworkInterfacesByTypeAsync(NetworkInterfaceType type)
        {
            var allInterfaces = await GetNetworkInterfacesAsync();
            return allInterfaces.Where(ni => ni.Type == type);
        }

        /// <summary>
        /// Gets statistics for a specific network interface
        /// </summary>
        /// <param name="interfaceId">Network interface identifier</param>
        /// <returns>Network interface statistics</returns>
        public async Task<NetworkInterfaceStatistics> GetInterfaceStatisticsAsync(string interfaceId)
        {
            return await Task.Run(() =>
            {
                var ni = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(ni => ni.Id == interfaceId);

                if (ni == null)
                {
                    throw new ArgumentException($"Network interface with ID {interfaceId} not found");
                }

                var statistics = ni.GetIPStatistics();

                return new NetworkInterfaceStatistics(
                    statistics.BytesReceived,
                    statistics.BytesSent,
                    statistics.UnicastPacketsReceived,
                    statistics.UnicastPacketsSent,
                    statistics.IncomingPacketsDiscarded,
                    statistics.OutgoingPacketsDiscarded);
            });
        }

        /// <summary>
        /// Monitors the status of network interfaces and raises events when changes occur
        /// </summary>
        /// <returns>True if monitoring started successfully</returns>
        public Task<bool> StartInterfaceMonitoringAsync()
        {
            if (_isMonitoring)
            {
                return Task.FromResult(true);
            }

            NetworkChange.NetworkAvailabilityChanged += OnNetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged += OnNetworkAddressChanged;
            _isMonitoring = true;
            return Task.FromResult(true);
        }

        /// <summary>
        /// Stops monitoring network interface status
        /// </summary>
        public Task StopInterfaceMonitoringAsync()
        {
            if (!_isMonitoring)
            {
                return Task.CompletedTask;
            }

            NetworkChange.NetworkAvailabilityChanged -= OnNetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged -= OnNetworkAddressChanged;
            _isMonitoring = false;
            return Task.CompletedTask;
        }

        private void OnNetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            // Raise event for all interfaces
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                NetworkInterfaceChanged?.Invoke(this, new NetworkInterfaceChangedEventArgs(ni.Id));
            }
        }

        private void OnNetworkAddressChanged(object sender, EventArgs e)
        {
            // Raise event for all interfaces
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                NetworkInterfaceChanged?.Invoke(this, new NetworkInterfaceChangedEventArgs(ni.Id));
            }
        }

        /// <summary>
        /// Event raised when a network interface changes
        /// </summary>
        public event EventHandler<NetworkInterfaceChangedEventArgs> NetworkInterfaceChanged;
    }
}
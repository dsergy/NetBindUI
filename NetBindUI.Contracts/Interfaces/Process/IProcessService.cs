using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NetBindUI.Contracts.Events;

namespace NetBindUI.Contracts.Interfaces.Process
{
    /// <summary>
    /// Service for managing processes with network interface binding
    /// </summary>
    public interface IProcessService
    {
        /// <summary>
        /// Starts a process with network binding to specific interface
        /// </summary>
        /// <param name="executablePath">Path to the executable file</param>
        /// <param name="arguments">Command line arguments</param>
        /// <param name="interfaceId">Network interface identifier to bind to</param>
        /// <returns>Process information if started successfully, null otherwise</returns>
        Task<IProcessInfo?> StartProcessAsync(string executablePath, string? arguments, string interfaceId);

        /// <summary>
        /// Gets information about all processes started through this service
        /// </summary>
        /// <returns>Collection of process information</returns>
        Task<IEnumerable<IProcessInfo>> GetManagedProcessesAsync();

        /// <summary>
        /// Gets information about specific process
        /// </summary>
        /// <param name="processId">Process identifier</param>
        /// <returns>Process information if found, null otherwise</returns>
        Task<IProcessInfo?> GetProcessByIdAsync(int processId);

        /// <summary>
        /// Terminates specific process
        /// </summary>
        /// <param name="processId">Process identifier</param>
        /// <returns>True if process was terminated successfully</returns>
        Task<bool> TerminateProcessAsync(int processId);

        /// <summary>
        /// Event raised when managed process state changes (started, terminated, etc.)
        /// </summary>
        event EventHandler<ProcessStateChangedEventArgs> ProcessStateChanged;

        /// <summary>
        /// Binds a process to a specific network interface
        /// </summary>
        /// <param name="processId">The ID of the process to bind</param>
        /// <param name="interfaceId">The ID of the network interface</param>
        /// <returns>True if binding was successful, false otherwise</returns>
        Task<bool> BindProcessToInterfaceAsync(int processId, string interfaceId);

        /// <summary>
        /// Unbinds a process from its network interface
        /// </summary>
        /// <param name="processId">The ID of the process to unbind</param>
        /// <returns>True if unbinding was successful, false otherwise</returns>
        Task<bool> UnbindProcessAsync(int processId);
    }
}
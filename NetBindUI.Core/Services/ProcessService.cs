using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NetBindUI.Contracts.Interfaces.Process;
using NetBindUI.Core.Models;

namespace NetBindUI.Core.Services
{
    /// <summary>
    /// Implementation of the IProcessService interface
    /// </summary>
    public class ProcessService : IProcessService
    {
        private readonly Dictionary<int, ProcessInfo> _managedProcesses = new();

        /// <summary>
        /// Starts a process with network binding to specific interface
        /// </summary>
        /// <param name="executablePath">Path to the executable file</param>
        /// <param name="arguments">Command line arguments</param>
        /// <param name="interfaceId">Network interface identifier to bind to</param>
        /// <returns>Process information if started successfully, null otherwise</returns>
        public Task<IProcessInfo?> StartProcessAsync(string executablePath, string? arguments, string interfaceId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"StartProcessAsync: Path={executablePath}, Args={arguments}, Interface={interfaceId}");

                ProcessStartInfo startInfo = new(executablePath, arguments)
                {
                    UseShellExecute = true,
                };

                Process process = Process.Start(startInfo) ?? throw new Exception("Process not started");

                var processInfo = new ProcessInfo(
                    process.Id,
                    process.ProcessName,
                    executablePath,
                    arguments,
                    interfaceId,
                    ProcessState.Running,
                    process.StartTime);

                _managedProcesses[process.Id] = processInfo;
                ProcessStateChanged?.Invoke(this, new Contracts.Events.ProcessStateChangedEventArgs(process.Id, ProcessState.Running));

                System.Diagnostics.Debug.WriteLine($"Process started: ID={process.Id}, Name={process.ProcessName}");
                return Task.FromResult<IProcessInfo?>(processInfo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error starting process: {ex.Message}");
                return Task.FromResult<IProcessInfo?>(null);
            }
        }

        /// <summary>
        /// Gets information about all processes started through this service
        /// </summary>
        /// <returns>Collection of process information</returns>
        public Task<IEnumerable<IProcessInfo>> GetManagedProcessesAsync()
        {
            System.Diagnostics.Debug.WriteLine($"GetManagedProcessesAsync: returning {_managedProcesses.Count} processes");
            foreach (var process in _managedProcesses.Values)
            {
                System.Diagnostics.Debug.WriteLine($"Process: ID={process.ProcessId}, Name={process.ProcessName}, Interface={process.BoundInterfaceId}");
            }
            return Task.FromResult<IEnumerable<IProcessInfo>>(_managedProcesses.Values);
        }

        /// <summary>
        /// Gets information about specific process
        /// </summary>
        /// <param name="processId">Process identifier</param>
        /// <returns>Process information if found, null otherwise</returns>
        public Task<IProcessInfo?> GetProcessByIdAsync(int processId)
        {
            System.Diagnostics.Debug.WriteLine($"GetProcessByIdAsync: ProcessId={processId}");
            _managedProcesses.TryGetValue(processId, out var processInfo);
            return Task.FromResult<IProcessInfo?>(processInfo);
        }

        /// <summary>
        /// Terminates specific process
        /// </summary>
        /// <param name="processId">Process identifier</param>
        /// <returns>True if process was terminated successfully</returns>
        public async Task<bool> TerminateProcessAsync(int processId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"TerminateProcessAsync: ProcessId={processId}");
                Process? process = Process.GetProcessById(processId);
                if (process != null)
                {
                    process.Kill();
                    await process.WaitForExitAsync();

                    if (_managedProcesses.Remove(processId))
                    {
                        ProcessStateChanged?.Invoke(this, new Contracts.Events.ProcessStateChangedEventArgs(processId, ProcessState.Terminated));
                        System.Diagnostics.Debug.WriteLine($"Process terminated: ID={processId}");
                    }

                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Process not found: ID={processId}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error terminating process: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Binds a process to a specific network interface
        /// </summary>
        /// <param name="processId">The ID of the process to bind</param>
        /// <param name="interfaceId">The ID of the network interface</param>
        /// <returns>True if binding was successful, false otherwise</returns>
        public async Task<bool> BindProcessToInterfaceAsync(int processId, string interfaceId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"BindProcessToInterfaceAsync: ProcessId={processId}, InterfaceId={interfaceId}");

                if (_managedProcesses.TryGetValue(processId, out var processInfo))
                {
                    // TODO: Implement actual binding logic
                    processInfo.UpdateBinding(interfaceId);
                    ProcessStateChanged?.Invoke(this, new Contracts.Events.ProcessStateChangedEventArgs(processId, processInfo.State));
                    System.Diagnostics.Debug.WriteLine($"Process bound: ID={processId}, Interface={interfaceId}");
                    return true;
                }

                System.Diagnostics.Debug.WriteLine($"Process not found for binding: ID={processId}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in BindProcessToInterfaceAsync: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Unbinds a process from its network interface
        /// </summary>
        /// <param name="processId">The ID of the process to unbind</param>
        /// <returns>True if unbinding was successful, false otherwise</returns>
        public async Task<bool> UnbindProcessAsync(int processId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"UnbindProcessAsync: ProcessId={processId}");

                if (_managedProcesses.TryGetValue(processId, out var processInfo))
                {
                    // TODO: Implement actual unbinding logic
                    processInfo.UpdateBinding(null);
                    ProcessStateChanged?.Invoke(this, new Contracts.Events.ProcessStateChangedEventArgs(processId, processInfo.State));
                    System.Diagnostics.Debug.WriteLine($"Process unbound: ID={processId}");
                    return true;
                }

                System.Diagnostics.Debug.WriteLine($"Process not found for unbinding: ID={processId}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UnbindProcessAsync: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Event raised when managed process state changes
        /// </summary>
        public event EventHandler<Contracts.Events.ProcessStateChangedEventArgs>? ProcessStateChanged;
    }
}
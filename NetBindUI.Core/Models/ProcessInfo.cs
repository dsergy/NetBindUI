using System;
using NetBindUI.Contracts.Interfaces.Process;

namespace NetBindUI.Core.Models
{
    /// <summary>
    /// Represents information about a process with network binding
    /// </summary>
    public class ProcessInfo : IProcessInfo
    {
        /// <summary>
        /// Process identifier
        /// </summary>
        public int ProcessId { get; }

        /// <summary>
        /// Name of the process
        /// </summary>
        public string ProcessName { get; }

        /// <summary>
        /// Path to the executable file
        /// </summary>
        public string ExecutablePath { get; }

        /// <summary>
        /// Command line arguments used to start the process
        /// </summary>
        public string? Arguments { get; }

        /// <summary>
        /// Network interface identifier this process is bound to
        /// </summary>
        public string BoundInterfaceId { get; private set; }

        /// <summary>
        /// Current state of the process
        /// </summary>
        public ProcessState State { get; private set; }

        /// <summary>
        /// Process start time
        /// </summary>
        public DateTime StartTime { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessInfo(
            int processId,
            string processName,
            string executablePath,
            string? arguments,
            string boundInterfaceId,
            ProcessState state,
            DateTime startTime)
        {
            ProcessId = processId;
            ProcessName = processName;
            ExecutablePath = executablePath;
            Arguments = arguments;
            BoundInterfaceId = boundInterfaceId;
            State = state;
            StartTime = startTime;
        }

        /// <summary>
        /// Updates the network interface binding for this process
        /// </summary>
        /// <param name="interfaceId">The ID of the network interface to bind to, or null to unbind</param>
        public void UpdateBinding(string? interfaceId)
        {
            BoundInterfaceId = interfaceId ?? string.Empty;
        }

        /// <summary>
        /// Updates the state of this process
        /// </summary>
        /// <param name="newState">The new state of the process</param>
        public void UpdateState(ProcessState newState)
        {
            State = newState;
        }
    }
}
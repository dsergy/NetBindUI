namespace NetBindUI.Contracts.Interfaces.Process
{
    /// <summary>
    /// Represents information about a process with network binding
    /// </summary>
    public interface IProcessInfo
    {
        /// <summary>
        /// Process identifier
        /// </summary>
        int ProcessId { get; }

        /// <summary>
        /// Name of the process
        /// </summary>
        string ProcessName { get; }

        /// <summary>
        /// Path to the executable file
        /// </summary>
        string ExecutablePath { get; }

        /// <summary>
        /// Command line arguments used to start the process
        /// </summary>
        string? Arguments { get; }

        /// <summary>
        /// Network interface identifier this process is bound to
        /// </summary>
        string BoundInterfaceId { get; }

        /// <summary>
        /// Current state of the process
        /// </summary>
        ProcessState State { get; }

        /// <summary>
        /// Process start time
        /// </summary>
        DateTime StartTime { get; }
    }

    /// <summary>
    /// Represents the state of a process
    /// </summary>
    public enum ProcessState
    {
        /// <summary>
        /// Process is running normally
        /// </summary>
        Running,

        /// <summary>
        /// Process is suspended
        /// </summary>
        Suspended,

        /// <summary>
        /// Process has terminated
        /// </summary>
        Terminated
    }
}
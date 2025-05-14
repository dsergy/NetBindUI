namespace NetBindUI.Contracts.Models.Process
{
    /// <summary>
    /// Represents the state of a process
    /// </summary>
    public enum ProcessState
    {
        /// <summary>
        /// Process is running
        /// </summary>
        Running,

        /// <summary>
        /// Process has been terminated
        /// </summary>
        Terminated,

        /// <summary>
        /// Process state is unknown
        /// </summary>
        Unknown
    }
}
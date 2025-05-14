using System;
using NetBindUI.Contracts.Interfaces.Process;

namespace NetBindUI.Contracts.Events;

/// <summary>
/// Event arguments for process state changes
/// </summary>
public class ProcessStateChangedEventArgs : EventArgs
{
    /// <summary>
    /// Process identifier
    /// </summary>
    public int ProcessId { get; }

    /// <summary>
    /// New process state
    /// </summary>
    public ProcessState NewState { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="processId">Process identifier</param>
    /// <param name="newState">New process state</param>
    public ProcessStateChangedEventArgs(int processId, ProcessState newState)
    {
        ProcessId = processId;
        NewState = newState;
    }
}
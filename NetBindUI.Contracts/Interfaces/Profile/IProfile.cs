namespace NetBindUI.Contracts.Interfaces.Profile;

/// <summary>
/// Represents application binding profile information
/// </summary>
public interface IProfile
{
    /// <summary>
    /// Unique identifier of the profile
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Name of the profile
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Path to the executable file
    /// </summary>
    string ExecutablePath { get; }

    /// <summary>
    /// Command line arguments used to start the process
    /// </summary>
    string? Arguments { get; }

    /// <summary>
    /// Network interface identifier to bind to
    /// </summary>
    string InterfaceId { get; }
}
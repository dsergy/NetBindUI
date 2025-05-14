using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NetBindUI.Contracts.Interfaces.Profile;

namespace NetBindUI.Core.Models;

/// <summary>
/// Represents application binding profile information
/// </summary>
public class Profile : IProfile
{
    /// <summary>
    /// Unique identifier of the profile
    /// </summary>
    [JsonPropertyName("id")]
    [Required]
    public string Id { get; set; }

    /// <summary>
    /// Name of the profile
    /// </summary>
    [JsonPropertyName("name")]
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Path to the executable file
    /// </summary>
    [JsonPropertyName("executablePath")]
    [Required]
    public string ExecutablePath { get; set; }

    /// <summary>
    /// Command line arguments used to start the process
    /// </summary>
    [JsonPropertyName("arguments")]
    public string? Arguments { get; set; }

    /// <summary>
    /// Network interface identifier to bind to
    /// </summary>
    [JsonPropertyName("interfaceId")]
    [Required]
    public string InterfaceId { get; set; }
}
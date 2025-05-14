using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetBindUI.Contracts.Interfaces.Profile;

/// <summary>
/// Service for managing application binding profiles
/// </summary>
public interface IProfileService
{
    /// <summary>
    /// Loads all available profiles
    /// </summary>
    /// <returns>Collection of profiles</returns>
    Task<IEnumerable<IProfile>> GetProfilesAsync();

    /// <summary>
    /// Loads specific profile by its identifier
    /// </summary>
    /// <param name="profileId">Profile identifier</param>
    /// <returns>Profile information if found, null otherwise</returns>
    Task<IProfile?> GetProfileByIdAsync(string profileId);

    /// <summary>
    /// Creates new profile
    /// </summary>
    /// <param name="profile">Profile information</param>
    /// <returns>True if profile was created successfully</returns>
    Task<bool> CreateProfileAsync(IProfile profile);

    /// <summary>
    /// Updates existing profile
    /// </summary>
    /// <param name="profile">Profile information</param>
    /// <returns>True if profile was updated successfully</returns>
    Task<bool> UpdateProfileAsync(IProfile profile);

    /// <summary>
    /// Deletes specific profile
    /// </summary>
    /// <param name="profileId">Profile identifier</param>
    /// <returns>True if profile was deleted successfully</returns>
    Task<bool> DeleteProfileAsync(string profileId);
}
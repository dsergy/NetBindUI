using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using NetBindUI.Contracts.Interfaces.Profile;
using NetBindUI.Core.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetBindUI.Core.Services;

/// <summary>
/// Implementation of the IProfileService interface
/// </summary>
public class ProfileService : IProfileService
{
    private readonly string _profilesFilePath = "profiles.json";
    private List<IProfile> _profiles = new();

    public ProfileService()
    {
        LoadProfilesAsync().Wait();
    }

    /// <summary>
    /// Loads all available profiles
    /// </summary>
    /// <returns>Collection of profiles</returns>
    public Task<IEnumerable<IProfile>> GetProfilesAsync()
    {
        return Task.FromResult<IEnumerable<IProfile>>(_profiles);
    }

    /// <summary>
    /// Loads specific profile by its identifier
    /// </summary>
    /// <param name="profileId">Profile identifier</param>
    /// <returns>Profile information if found, null otherwise</returns>
    public Task<IProfile?> GetProfileByIdAsync(string profileId)
    {
        var profile = _profiles.FirstOrDefault(p => p.Id == profileId);
        return Task.FromResult<IProfile?>(profile);
    }

    /// <summary>
    /// Creates new profile
    /// </summary>
    /// <param name="profile">Profile information</param>
    /// <returns>True if profile was created successfully</returns>
    public async Task<bool> CreateProfileAsync(IProfile profile)
    {
        if (_profiles.Any(p => p.Id == profile.Id))
        {
            return false;
        }

        _profiles.Add(profile);
        await SaveProfilesAsync();
        return true;
    }

    /// <summary>
    /// Updates existing profile
    /// </summary>
    /// <param name="profile">Profile information</param>
    /// <returns>True if profile was updated successfully</returns>
    public async Task<bool> UpdateProfileAsync(IProfile profile)
    {
        var existingProfile = _profiles.FirstOrDefault(p => p.Id == profile.Id);
        if (existingProfile == null)
        {
            return false;
        }

        _profiles.Remove(existingProfile);
        _profiles.Add(profile);
        await SaveProfilesAsync();
        return true;
    }

    /// <summary>
    /// Deletes specific profile
    /// </summary>
    /// <param name="profileId">Profile identifier</param>
    /// <returns>True if profile was deleted successfully</returns>
    public async Task<bool> DeleteProfileAsync(string profileId)
    {
        var profile = _profiles.FirstOrDefault(p => p.Id == profileId);
        if (profile == null)
        {
            return false;
        }

        _profiles.Remove(profile);
        await SaveProfilesAsync();
        return true;
    }

    private async Task LoadProfilesAsync()
    {
        try
        {
            if (File.Exists(_profilesFilePath))
            {
                string jsonString = await File.ReadAllTextAsync(_profilesFilePath);
                var profiles = JsonSerializer.Deserialize<List<Profile>>(jsonString);
                if (profiles != null)
                {
                    _profiles = profiles.Cast<IProfile>().ToList();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading profiles: {ex.Message}");
            _profiles = new List<IProfile>();
        }
    }

    private async Task SaveProfilesAsync()
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(_profiles, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_profilesFilePath, jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving profiles: {ex.Message}");
        }
    }
}
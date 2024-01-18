namespace MsiPostProfile;

/// <summary>
/// The service for interacting with the MsiPost database.
/// </summary>
public interface IMsiProfileService
{

    /// <summary>
    /// Set the given profile as active.
    /// </summary>
    /// <param name="profile">The profile to set as active</param>
    /// <returns></returns>
    public Task SetActiveAsync(Profile profile);

    /// <summary>
    /// Create a new profile with the given unique identifier.
    /// </summary>
    /// <param name="id">The unqiue identifier of the minecraft account</param>
    /// <returns></returns>
    public Task<Profile> CreateProfileAsync(Guid id);

    /// <summary>
    /// Get all profiles.
    /// </summary>
    /// <returns>All registered profiles</returns>
    public Task<IEnumerable<Profile>> GetProfilesAsync();

    /// <summary>
    /// Get the profile with the given unique identifier.
    /// </summary>
    /// <param name="id">The unqiue identifier of the minecraft account</param>
    /// <returns>The found profile, null if none found.</returns>
    public Task<Profile?> GetProfileAsync(Guid id);

    /// <summary>
    /// Get the profile with the given name.
    /// </summary>
    /// <param name="username">The username</param>
    /// <returns>The found profile, null if none found.</returns>
    public Task<Profile?> GetProfileByNameAsync(string username);
}

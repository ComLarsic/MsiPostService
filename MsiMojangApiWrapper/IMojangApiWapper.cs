using MsiMojangApiWrapper.DTO;

namespace MsiMojangApiWrapper;

/// <summary>
/// A wrapper for the Mojang API.
/// </summary>
public interface IMojangApiWrapper
{
    /// <summary>
    /// Get the profile for the given UUID.
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Task<MojangProfileDTO?> GetProfileAsync(Guid uuid);

    /// <summary>
    /// Get the minecraft user for the given username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public Task<MinecraftUserDTO?> GetMinecraftUserAsync(string username);

    /// <summary>
    /// Get the skin url for the given UUID.
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns>The url to the skin</returns>
    public Task<string?> GetSkinUrlAsync(Guid uuid);
}

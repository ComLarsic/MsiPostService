using System.Net;
using System.Text.Json;
using MsiMojangApiWrapper.DTO;

namespace MsiMojangApiWrapper;

/// <summary>
/// The wrapper for interacting with the Mojang API.
/// </summary>
public static class MojangApiWrapper
{
    /// <summary>
    /// The URL for the Mojang session API.
    /// </summary>
    private readonly static string MojangSessionServerUrl = "https://sessionserver.mojang.com/session/minecraft/";

    /// <summary>
    /// The URL for the Mojang API.
    /// </summary>
    private readonly static string MojangApiUrl = "https://api.mojang.com/";

    /// <summary>
    /// Get the profile for the given UUID.
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task<MojangProfileDTO?> GetProfileAsync(Guid uuid)
    {
        using HttpClient client = new();
        var response = await client.GetAsync($"{MojangSessionServerUrl}/profile/{uuid}");
        if (!response.IsSuccessStatusCode)
            return null;
        if (response.StatusCode == HttpStatusCode.NoContent)
            return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MojangProfileDTO>(content);
    }

    /// <summary>
    /// Get the minecraft user for the given username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public static async Task<MinecraftUserDTO?> GetMinecraftUserAsync(string username)
    {
        using HttpClient client = new();
        var response = await client.GetAsync($"{MojangApiUrl}/users/profiles/minecraft/{username}");
        if (!response.IsSuccessStatusCode)
            return null;

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MinecraftUserDTO>(content);
    }
}

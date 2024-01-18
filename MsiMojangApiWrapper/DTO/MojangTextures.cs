using System.Text.Json.Serialization;

namespace MsiMojangApiWrapper;

/// <summary>
/// The DTO for a Mojang profile.
/// </summary>
[Serializable]
public struct MojangProfilePropertySkinDTO
{
    /// <summary>
    /// The timestamp for this property.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

}

/// <summary>
/// The DTO for a Mojang profile texture.
/// </summary>
[Serializable]
public struct MojangProfilePropertyTexturesDTO
{
    [JsonPropertyName("SKIN")]
    public MojangProfilePropertySkinDTO? Skin { get; set; }
}
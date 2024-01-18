using System.Text.Json.Serialization;

namespace MsiMojangApiWrapper;

/// <summary>
/// The DTO for a Mojang profile property.
/// </summary>
public struct MojangProfilePropertyValueDTO
{
    /// <summary>
    /// The timestamp for this property.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    /// <summary>
    /// The profile id
    /// </summary>
    [JsonPropertyName("profileId")]
    public string? ProfileId { get; set; }

    /// <summary>
    /// The profile name
    /// </summary>
    [JsonPropertyName("profileName")]
    public string? ProfileName { get; set; }

    /// <summary>
    /// The texture value
    /// </summary>
    [JsonPropertyName("textures")]
    public MojangProfilePropertyTexturesDTO? Textures { get; set; }

}

using System.Text.Json.Serialization;

namespace MsiMojangApiWrapper.DTO;

/// <summary>
/// The DTO for a Mojang profile.
/// </summary>
public struct MojangProfileDTO
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("properties")]
    public MojangProfilePropertyDTO[]? Properties { get; set; }
}

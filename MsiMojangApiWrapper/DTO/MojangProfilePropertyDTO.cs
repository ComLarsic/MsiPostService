using System.Text.Json.Serialization;

namespace MsiMojangApiWrapper.DTO;


/// <summary>
/// The DTO for a Mojang profile property.
/// </summary>
public struct MojangProfilePropertyDTO
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}

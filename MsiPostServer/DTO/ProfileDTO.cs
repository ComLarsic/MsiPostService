using System.Text.Json.Serialization;

namespace MsiPostServer.DTO;

/// <summary>
/// The Data Transfer Object of a profile
/// </summary>
[Serializable]
public struct ProfileDTO
{
    [JsonPropertyName("uuid")]
    public Guid Uuid { get; set; }
    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }
    [JsonPropertyName("skin_url")]
    public string SkinUrl { get; set; }
}

using System.Text.Json.Serialization;

namespace MsiPostServer.DTO;

/// <summary>
/// The Data Transfer Object of a profile
/// </summary>
[Serializable]
public struct ProfileDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }
}

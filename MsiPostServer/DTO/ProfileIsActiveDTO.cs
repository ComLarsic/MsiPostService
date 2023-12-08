using System.Text.Json.Serialization;

namespace MsiPostServer.DTO;

/// <summary>
/// The Data Transfer Object of a is active response
/// </summary>
[Serializable]
public struct ProfileIsActiveDTO
{
    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }
}

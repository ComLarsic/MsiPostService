using System.Text.Json.Serialization;

namespace MsiPostServer.DTO;

/// <summary>
/// The Data Transfer Object for a post create response
/// </summary>
[Serializable]
public struct CreatePostResponseDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}

using System.Text.Json.Serialization;

namespace MsiPostServer.DTO;

/// <summary>
/// DTO for editing a post
/// </summary>
[Serializable]
public struct EditPostDTO
{
    [JsonPropertyName("post")]
    public Guid Post { get; set; }

    [JsonPropertyName("profile")]
    public string Text { get; set; }
}

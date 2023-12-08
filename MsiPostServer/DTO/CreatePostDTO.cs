using System.Text.Json.Serialization;

namespace Namespace;

/// <summary>
/// The Data Transfer Object for creating a post
/// </summary>
[Serializable]
public struct CreatePostDTO
{
    [JsonPropertyName("profile")]
    public Guid Profile { get; set; }
    [JsonPropertyName("text")]
    public string Text { get; set; }
}

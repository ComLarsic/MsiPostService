using System.Text.Json.Serialization;

namespace MsiPosts.DTO;

/// <summary>
/// A post on the MsiPost server.
/// </summary>
[Serializable]
public struct PostDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("profile_id")]
    public Guid ProfileId { get; set; }
    [JsonPropertyName("text")]
    public string? Text { get; set; }
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("likes")]
    public List<LikeDTO> Likes { get; set; }
}

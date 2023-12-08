namespace MsiPosts.DTO;

/// <summary>
/// A post on the MsiPost server.
/// </summary>
public struct PostDTO
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public string? Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<LikeDTO> Likes { get; set; }
}

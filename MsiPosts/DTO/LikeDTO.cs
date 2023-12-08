namespace MsiPosts.DTO;

/// <summary>
/// A like on the MsiPost post.
/// </summary>
public struct LikeDTO
{
    public int Id { get; set; }
    public Guid PostId { get; set; }
    public Guid ProfileId { get; set; }
}

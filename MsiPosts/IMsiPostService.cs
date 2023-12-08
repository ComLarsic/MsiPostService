using MsiPosts.DTO;
using MsiPostUtility;

namespace MsiPosts;

/// <summary>
/// The service that handles msi posts
/// </summary>
public interface IMsiPostService
{
    /// <summary>
    /// Get a post
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<PostDTO?> GetPostAsync(Guid id);

    /// <summary>
    /// Create a post
    /// </summary>
    /// <param name="profile"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public Task<Guid> CreatePostAsync(Guid profile, string text);

    /// <summary>
    /// Edit a post
    /// </summary>
    /// <param name="id"></param>
    /// <param name="text"></param>
    public Task EditPostAsync(Guid id, string text);

    /// <summary>
    /// Delete a post
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task DeletePostAsync(Guid id);

    /// <summary>
    /// Get the posts of a user
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<PostDTO>> GetPostsOfProfileAsync(Guid profile);

    /// <summary>
    /// Get the posts of a user paginated
    /// </summary>
    /// <returns></returns>
    public Task<PagedResponse<PostDTO>> GetPostsOfProfileAsync(Guid profile, int page, int pageSize);

}

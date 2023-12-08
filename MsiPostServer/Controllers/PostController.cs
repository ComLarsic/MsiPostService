using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using MsiPostProfile;
using MsiPosts;
using MsiPosts.DTO;
using MsiPostServer.DTO;
using Namespace;

namespace MsiPostServer.Controllers;

/// <summary>
/// The controller that handles posts
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class PostController : Controller
{
    private readonly IMsiPostService _msiPostService;
    private readonly IMsiProfileService _msiProfileService;

    public PostController(IMsiPostService msiPostService, IMsiProfileService msiProfileService)
    {
        _msiPostService = msiPostService;
        _msiProfileService = msiProfileService;
    }

    /// <summary>
    /// Create a post
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<CreatePostResponseDTO>> CreatePost([FromBody] CreatePostDTO dto)
    {
        var profile = await _msiProfileService.GetProfileAsync(dto.Profile);
        if (profile == null)
            return BadRequest("Invalid profile id");
        var id = await _msiPostService.CreatePostAsync(dto.Profile, dto.Text);
        return new CreatePostResponseDTO { Id = id };
    }

    /// <summary>
    /// Edit a post
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult> EditPost([FromBody] EditPostDTO dto)
    {
        var post = await _msiPostService.GetPostAsync(dto.Post);
        if (post == null)
            return BadRequest("Invalid post id");
        await _msiPostService.EditPostAsync(dto.Post, dto.Text);
        return Ok();
    }


    /// <summary>
    /// Get a post
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PostDTO>> GetPost(Guid id)
    {
        var post = await _msiPostService.GetPostAsync(id);
        if (post == null)
            return NotFound();
        return new PostDTO
        {
            Id = post.Value.Id,
            Text = post.Value.Text,
            ProfileId = post.Value.ProfileId,
            CreatedAt = post.Value.CreatedAt,
            Likes = post.Value.Likes.Select(l => new LikeDTO
            {
                Id = l.Id,
                PostId = l.PostId,
                ProfileId = l.ProfileId,
            }).ToList(),
        };
    }
}

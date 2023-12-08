using Microsoft.AspNetCore.Mvc;
using MsiPostServer.DTO;
using MsiPostProfile;
using MsiPostUtility;
using MsiPosts.DTO;
using MsiPosts;

namespace MsiPostServer.Controllers;

/// <summary>
/// The controller for player profiles.
/// </summary>
[Route("api/[controller]")]
public class ProfileController : Controller
{
    private readonly IMsiProfileService _profileService;
    private readonly IMsiPostService _postService;

    public ProfileController(IMsiProfileService profileService, IMsiPostService postService)
    {
        _profileService = profileService;
        _postService = postService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProfileDTO profileCreateDTO)
    {
        // Create the profile.
        try
        {
            await _profileService.CreateProfileAsync(profileCreateDTO.Id);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<ProfileDTO>>> Get()
    {
        // Get all profiles.
        var result = await _profileService.GetProfilesAsync();
        return result.Select(p => new ProfileDTO
        {
            Id = p.Id,
            IsActive = p.IsActive,
        }).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileDTO>> Get(Guid id)
    {
        // Get the profile with the given id.
        var result = await _profileService.GetProfileAsync(id);
        if (result == null)
            return BadRequest();
        return new ProfileDTO
        {
            Id = result.Value.Id,
            IsActive = result.Value.IsActive
        };
    }

    /// <summary>
    /// Check if a profile is active.
    /// </summary>
    [HttpGet("isactive/{id}")]
    public async Task<ActionResult<ProfileIsActiveDTO>> IsActive(Guid id)
    {
        // Get the profile with the given id.
        var result = await _profileService.GetProfileAsync(id);
        if (result == null)
            return new ProfileIsActiveDTO
            {
                IsActive = false,
            };
        return new ProfileIsActiveDTO { IsActive = true };
    }

    /// <summary>
    /// Get all posts made by a profile
    /// </summary>
    [HttpGet("posts")]
    public async Task<ActionResult<PagedResponse<PostDTO>>> GetPosts([FromQuery] Guid profile, [FromQuery] int page, [FromQuery] int pageSize)
        => await _postService.GetPostsOfProfileAsync(profile, page, pageSize);
}


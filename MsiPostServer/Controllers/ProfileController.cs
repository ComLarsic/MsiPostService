using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using MinecraftPostServer.DTO;
using MsiPostOrm.Entity;
using MsiPostOrmService;
using MsiPostProfile;

namespace MinecraftPostServer;

/// <summary>
/// The controller for player profiles.
/// </summary>
[Route("api/[controller]")]
public class ProfileController : Controller
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
        => _profileService = profileService;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProfileCreateDTO profileCreateDTO)
    {
        // Create the profile.
        var result = await _profileService.CreateProfileAsync(profileCreateDTO.Id);
        if (!result)
            return BadRequest("Profile Id could not be found by the Mojang api.");
        return Ok();
    }

    [HttpGet]
    public IActionResult Get()
    {
        // Get all profiles.
        var result = _profileService.GetProfilesAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        // Get the profile with the given id.
        var result = _profileService.GetProfileAsync(id);
        return Ok(result);
    }
}



using Microsoft.EntityFrameworkCore;
using MsiMojangApiWrapper;
using MsiPostOrm.Entity;
using MsiPostOrmService;

namespace MsiPostProfile;

/// <summary>
/// The service for interacting with the MsiPost database.
/// </summary>
public class ProfileService : IProfileService
{
    private readonly IMsiPostOrmService _ormService;

    public ProfileService(IMsiPostOrmService ormService)
        => _ormService = ormService;

    public async Task<bool> CreateProfileAsync(Guid id)
    {
        /// Check if the id is a valid minecraft profile.
        var profile = await MojangApiWrapper.GetProfileAsync(id);
        if (profile == null)
            return false;

        var context = _ormService.Context();

        // Check if the profile already exists.
        var existingProfile = await context.Profiles.FirstOrDefaultAsync(entity => entity.Id == id);

        var profileEntity = new ProfileEntity
        {
            Id = id,
        };
        await context.AddAsync(profileEntity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Profile>> GetProfilesAsync()
    {
        var context = _ormService.Context();
        var entities = await context.Profiles.ToListAsync();
        return entities.Select(Profile.FromEntity);
    }

    public async Task<Profile?> GetProfileAsync(Guid id)
    {
        var context = _ormService.Context();
        var entity = await context.Profiles.FirstOrDefaultAsync(entity => entity.Id == id);
        if (entity == null) return null;
        return Profile.FromEntity(entity);
    }

    public async Task<Profile?> GetProfileByNameAsync(string username)
    {
        var userResult = await MojangApiWrapper.GetMinecraftUserAsync(username);
        if (userResult == null)
            return null;

        var context = _ormService.Context();
        var entity = await context.Profiles.FirstOrDefaultAsync(entity => entity.Id == userResult.Value.Id);
        if (entity == null)
            return null;

        return Profile.FromEntity(entity);
    }
}

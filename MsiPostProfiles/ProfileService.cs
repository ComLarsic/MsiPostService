
using Microsoft.EntityFrameworkCore;
using MsiMojangApiWrapper;
using MsiPostOrm.Entity;
using MsiPostOrmUtility;

namespace MsiPostProfile;

/// <summary>
/// The service for interacting with the MsiPost database.
/// </summary>
public class MsiProfileService : IMsiProfileService
{
    private readonly IMsiPostOrmService _ormService;
    private readonly IMojangApiWrapper _mojangApiWrapper;

    public MsiProfileService(IMsiPostOrmService ormService, IMojangApiWrapper mojangApiWrapper)
    {
        _ormService = ormService;
        _mojangApiWrapper = mojangApiWrapper;
    }

    public async Task CreateProfileAsync(Guid id)
    {
        /// Check if the id is a valid minecraft profile.
        var _ = await _mojangApiWrapper.GetProfileAsync(id) ?? throw new ArgumentException("The given id is not a valid minecraft profile.");
        await _ormService.Context(async db =>
        {
            await db.AddAsync(new ProfileEntity
            {
                Id = id,
                IsActive = true,
            });
            await db.SaveChangesAsync();
        });
    }

    public async Task<IEnumerable<Profile>> GetProfilesAsync()
        => await _ormService.Context(async db =>
            {
                var entities = await db.Profiles.ToListAsync();
                return entities.Select(Profile.FromEntity);
            });

    public async Task<Profile?> GetProfileAsync(Guid id)
        => await _ormService.Context<Profile?>(async db =>
        {
            var entity = await db.Profiles.FirstOrDefaultAsync(entity => entity.Id == id);
            if (entity == null) return null;
            return Profile.FromEntity(entity);
        });

    public async Task<Profile?> GetProfileByNameAsync(string username)
    {
        var userResult = await _mojangApiWrapper.GetMinecraftUserAsync(username);
        if (userResult == null)
            return null;

        return await _ormService.Context<Profile?>(async db =>
        {
            var entity = await db.Profiles.FirstOrDefaultAsync(entity => entity.Id == userResult.Value.Id);
            if (entity == null)
                return null;

            return Profile.FromEntity(entity);
        });
    }
}

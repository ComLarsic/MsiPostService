
using Microsoft.AspNetCore.Mvc;
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

    public async Task SetActiveAsync(Profile profile)
    {
        var entity = await _ormService.Context(async db =>
        {
            var entity = await db.Profiles.FirstOrDefaultAsync(entity => entity.Id == profile.Uuid);
            if (entity == null)
            {
                entity = new ProfileEntity
                {
                    Id = profile.Uuid,
                    IsActive = false
                };
                await db.AddAsync(entity);
            }
            return entity;
        });
        entity.IsActive = true;
        await _ormService.Context(async db =>
        {
            db.Profiles.Update(entity);
            await db.SaveChangesAsync();
        });
    }

    public async Task<Profile> CreateProfileAsync(Guid id)
    {
        /// Check if the id is a valid minecraft profile.
        var _ = await _mojangApiWrapper.GetProfileAsync(id) ?? throw new ArgumentException("The given id is not a valid minecraft profile.");
        await _ormService.Context(async db =>
        {
            var entity = db.Profiles.FirstOrDefault(entity => entity.Id == id);
            if (entity != null)
            {
                entity.IsActive = true;
                db.Profiles.Update(entity);
            }
            else
            {
                entity = new ProfileEntity
                {
                    Id = id,
                    IsActive = true
                };
                await db.AddAsync(entity);
            }

            await db.SaveChangesAsync();
        });
        return new Profile
        {
            Uuid = id,
            IsActive = true
        };
    }

    public async Task<IEnumerable<Profile>> GetProfilesAsync()
    {
        var profiles = await _ormService.Context(async db =>
        {
            var entities = await db.Profiles.ToListAsync();
            return entities.Select(Profile.FromEntity);
        });
        var tasks = profiles.ToList().Select(profile => GetSkinUrlAsync(profile, profile.Uuid));
        return await Task.WhenAll(tasks);
    }

    public async Task<Profile?> GetProfileAsync(Guid id)
    {
        var profileResult = await _ormService.Context<Profile?>(async db =>
        {
            var entity = await db.Profiles.FirstOrDefaultAsync(entity => entity.Id == id);
            if (entity == null) return null;
            return Profile.FromEntity(entity);
        });
        if (profileResult == null)
            return null;
        return await GetSkinUrlAsync(profileResult.Value, id);
    }

    public async Task<Profile?> GetProfileByNameAsync(string username)
    {
        var userResult = await _mojangApiWrapper.GetMinecraftUserAsync(username);
        if (userResult == null)
            return null;

        var profileResult = await _ormService.Context<Profile?>(async db =>
        {
            var entity = await db.Profiles.FirstOrDefaultAsync(entity => entity.Id == userResult.Value.Id);
            if (entity == null)
                return null;

            return Profile.FromEntity(entity);
        });
        if (profileResult == null)
            return null;
        var profile = profileResult.Value;
        var skinUrl = await _mojangApiWrapper.GetSkinUrlAsync(profile.Uuid);
        if (skinUrl != null)
            profile.SkinUrl = skinUrl;
        return profile;
    }

    /// <summary>
    /// Get the skin url of a profile and return the profile.
    /// </summary>
    /// <param name="mojangApiWrapper"></param>
    /// <param name="profile"></param>
    /// <param name="uuid"></param>
    /// <returns></returns>
    private async Task<Profile> GetSkinUrlAsync(Profile profile, Guid uuid)
    {
        var skinUrl = await _mojangApiWrapper.GetSkinUrlAsync(uuid);
        if (skinUrl != null)
            profile.SkinUrl = skinUrl;
        return profile;
    }
}

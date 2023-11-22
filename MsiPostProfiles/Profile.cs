using System.Net.NetworkInformation;
using MsiPostOrm.Entity;

namespace MsiPostProfile;

/// <summary>
/// A profile associated with a minecraft account.
/// </summary>
public struct Profile
{
    /// <summary>
    /// The unique identifier for this profile.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Whether or not the minecraft account has been claimed by a user.
    /// </summary>
    public bool IsActive { get; set; }

    

    /// <summary>
    /// Create a new profile instance from its entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static Profile FromEntity(ProfileEntity entity)
        => new()
        {
            Id = entity.Id,
            IsActive = entity.IsActive
        };
}

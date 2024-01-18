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
    public Guid Uuid { get; set; }

    /// <summary>
    /// Whether or not the minecraft account has been claimed by a user.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The skin url of the minecraft account.
    /// </summary>
    public string SkinUrl { get; set; }


    /// <summary>
    /// Create a new profile instance from its entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static Profile FromEntity(ProfileEntity entity)
        => new()
        {
            Uuid = entity.Id,
            IsActive = entity.IsActive
        };
}

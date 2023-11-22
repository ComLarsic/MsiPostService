namespace MsiPostOrm.Entity;

/// <summary>
/// A profile associated with a minecraft account.
/// </summary>
public class ProfileEntity
{
    /// <summary>
    /// The unique identifier for this profile.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Whether or not the minecraft account has used this site before.
    /// </summary>
    public bool IsActive { get; set; }
}

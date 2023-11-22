namespace MsiMojangApiWrapper.DTO;

/// <summary>
/// The DTO for a minecraft user.
/// </summary>
public struct MinecraftUserDTO
{
    /// <summary>
    /// The unique identifier for this profile.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Whether or not the minecraft account has used this site before.
    /// </summary>
    public string? Name { get; set; }
}

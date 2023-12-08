namespace MsiPostServer.DTO;

/// <summary>
/// The DTO for a profile create request.
/// </summary>
public struct ProfileCreateDTO
{
    /// <summary>
    /// The unique identifier for this profile.
    /// </summary>
    public Guid Id { get; set; }
}

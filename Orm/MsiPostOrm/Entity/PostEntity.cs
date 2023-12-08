namespace MsiPostOrm.Entity;

/// <summary>
/// A post in the database
/// </summary>
public class PostEntity
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public string? Text { get; set; }
    public DateTime CreatedAt { get; set; }
}

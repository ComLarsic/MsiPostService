namespace MsiPostOrm.Entity;

/// <summary>
/// A like link table in the database
/// </summary>
public class LikeEntity
{
    public int Id { get; set; }
    public Guid PostId { get; set; }
    public Guid ProfileId { get; set; }
}

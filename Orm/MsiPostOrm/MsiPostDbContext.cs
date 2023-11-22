using Microsoft.EntityFrameworkCore;
using MsiPostOrm.Entity;

namespace MsiPostOrm;

/// <summary>
/// The database context for the MsiPost database.
/// </summary>
public class MsiPostDbContext : DbContext
{
    #region Models
    public DbSet<ProfileEntity> Profiles { get; set; }
    #endregion
}
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
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<LikeEntity> Likes { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProfileEntity>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<PostEntity>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<LikeEntity>()
            .HasKey(l => l.Id);
    }
}
using Microsoft.EntityFrameworkCore;
using MsiPostOrm;

namespace MsiPostOrmSqlite;

/// <summary>
/// The database context for the MsiPost database.
/// </summary>
public class MsiPostSqliteContext : MsiPostDbContext
{
    // If in debug mode, assign the database file to the temp directory.
#if DEBUG
    private static readonly string DB_PATH = "../Orm/MsiPostOrmSqlite/msipost.db";
#else
    private static readonly string DB_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "msipost.db");
#endif

    /// <summary>
    /// The path of the database file
    /// </summary>
    public string DbPath { get; private set; } = DB_PATH;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}");

}

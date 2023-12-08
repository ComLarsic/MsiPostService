using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using MsiPostOrm;

namespace MsiPostOrmSqlite;

/// <summary>
/// The database context for the MsiPost database.
/// </summary>
public class MsiPostSqliteContext : MsiPostDbContext
{
    /// <summary>
    /// The connection string
    /// </summary>
    public string ConnectionString { get; private set; }

    private MsiPostSqliteContext()
        => ConnectionString = "";

    [ActivatorUtilitiesConstructor]
    public MsiPostSqliteContext(IConfiguration configuration)
        => ConnectionString = configuration.GetSection("MsiPost:Sqlite")
            .GetValue<string>("ConnectionString")
                ?? throw new Exception("No connection string provided");

    public MsiPostSqliteContext(string connectionString)
        => ConnectionString = connectionString;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite(ConnectionString);
}

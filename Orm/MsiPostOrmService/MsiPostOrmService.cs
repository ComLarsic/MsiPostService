using Microsoft.EntityFrameworkCore;
using MsiPostOrm;
using MsiPostOrmSqlite;

namespace MsiPostOrmService;

/// <summary>
/// The service for interacting with the MsiPost database.
/// </summary>
public class MsiPostOrmService : IMsiPostOrmService
{
    // The database context for the MsiPost database.
    private MsiPostDbContext _dbContext;

    public MsiPostOrmService(MsiPostOrmBackend backend)
        => _dbContext = backend switch
        {
            MsiPostOrmBackend.Sqlite => new MsiPostSqliteContext(),
            _ => throw new NotImplementedException(),
        };

    /// <summary>
    /// The database context for the MsiPost database.
    /// </summary>
    public MsiPostDbContext Context()
        => _dbContext;
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MsiPostOrm;
using MsiPostOrmSqlite;

namespace MsiPostOrmUtility;

/// <summary>
/// The service for interacting with the MsiPost database.
/// </summary>
public class MsiPostOrmService : IMsiPostOrmService
{

    private readonly IServiceProvider _serviceProvider;
    private readonly MsiPostOrmBackend _backend;

    public MsiPostOrmService(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        var config = configuration.GetSection("MsiPost")
            .Get<MsiPostOrmConfig>() ?? throw new Exception("Failed to load MsiPost config");

        _backend = config.Backend;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Get the database context
    /// </summary>
    /// <returns></returns>
    public async Task Context(Func<MsiPostDbContext, Task> func)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = _backend switch
        {
            MsiPostOrmBackend.Sqlite => scope.ServiceProvider.GetRequiredService<MsiPostSqliteContext>(),
            _ => throw new NotImplementedException(),
        };
        await func(db);
    }

    /// <summary>
    /// Get the database context in a scope
    /// </summary>
    /// <returns></returns>
    public Task<TResult> Context<TResult>(Func<MsiPostDbContext, Task<TResult>> func)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = _backend switch
        {
            MsiPostOrmBackend.Sqlite => scope.ServiceProvider.GetRequiredService<MsiPostSqliteContext>(),
            _ => throw new NotImplementedException(),
        };
        return func(db);
    }

    /// <summary>
    /// Create the db context
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>s
    /// <exception cref="Exception"></exception>
    /// <exception cref="NotImplementedException"></exception>
    public static void CreateDbContext(IConfiguration configuration, IServiceCollection services)
    {
        var config = configuration.GetSection("MsiPost")
            .Get<MsiPostOrmConfig>() ?? throw new Exception("Failed to load MsiPost config");

        switch (config.Backend)
        {
            case MsiPostOrmBackend.Sqlite:
                services.AddDbContext<MsiPostSqliteContext>();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Creates a new dbcontext that uses an in-memory database.
    /// </summary>
    /// <param name="backend"></param>
    /// <returns></returns>
    public static void CreateInMemory(MsiPostOrmBackend backend, IServiceCollection services)
    {
        switch (backend)
        {
            case MsiPostOrmBackend.Sqlite:
                services.AddDbContext<MsiPostSqliteContext>(options =>
                    options.UseSqlite("Data Source=:memory:"));
                break;
            default:
                throw new NotImplementedException();
        }
    }
}

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;

namespace MsiPostOrmUtility;

/// <summary>
/// The service that sets-up the orm on startup
/// </summary>
public interface IMsiPostOrmHostedService : IHostedService { }

/// <summary>
/// The service that sets-up the orm on startup
/// </summary>
public class MsiPostOrmHostedService : IMsiPostOrmHostedService
{
    private readonly IMsiPostOrmService _ormService;

    public MsiPostOrmHostedService(IMsiPostOrmService ormService)
    {
        _ormService = ormService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
        => await _ormService.Context(db =>
        {
            // Build the tables if needed
            var dbCreator = (RelationalDatabaseCreator)db.Database.GetService<IDatabaseCreator>();

            if (Environment.GetEnvironmentVariable("MOCK_DB") != "true")
            {
                // Im sorry i have to :(
                try
                {
                    if (dbCreator.HasTables())
                        return Task.CompletedTask;
                    dbCreator.CreateTables();
                }
                catch
                {
                    if (dbCreator.HasTables())
                        return Task.CompletedTask;
                    dbCreator.CreateTables();
                }
            }
            return Task.CompletedTask;
        });

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;

namespace MsiPostOrmUtility;

/// <summary>
/// The service that sets-up the orm on startup
/// </summary>
public class MsiPostOrmHostedService : IHostedService
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
            if (!dbCreator.HasTables())
                dbCreator.CreateTables();
            return Task.CompletedTask;
        });

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
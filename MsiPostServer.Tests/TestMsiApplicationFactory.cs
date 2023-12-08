using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using MsiPostOrm;
using MsiPostOrmSqlite;
using MsiPostOrmUtility;

namespace MsiPostServer.Tests;

/// <summary>
/// Test class for <see cref="WebApplicationFactory<Programs>"/>.
/// </summary>
public class TestMsiApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    /// <summary>
    /// Creates a <see cref="IHostBuilder"/> used to set up the <see cref="IHost"/> for the test server.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/> used to configure the test server.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Add mock database
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<MsiPostSqliteContext>));

            services.Remove(dbContextDescriptor ?? throw new InvalidOperationException());
            MsiPostOrmService.CreateInMemory(MsiPostOrmBackend.Sqlite, services);
        });
    }
}

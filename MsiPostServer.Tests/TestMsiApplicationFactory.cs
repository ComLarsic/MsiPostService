using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using MsiMojangApiWrapper;
using MsiMojangApiWrapper.DTO;
using MsiPostOrm;
using MsiPostOrmSqlite;
using MsiPostOrmUtility;
using MsiPostProfile;
using MsiPosts;
using MsiPosts.DTO;
using Newtonsoft.Json.Bson;

namespace MsiPostServer.Tests;

/// <summary>
/// Test class for <see cref="WebApplicationFactory<Programs>"/>.
/// </summary>
public class TestMsiApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    // Flag to mock the msi database
    private bool _mockDatabase = false;
    // Flag to mock the MojangApiWrapper
    private bool _mockMojangApiWrapper = false;
    // Flag to mock msi profile service
    private bool _mockMsiProfileService = false;
    // Flag to mock msi post service
    private bool _mockMsiPostService = false;

    /// <summary>
    /// Creates a <see cref="IHostBuilder"/> used to set up the <see cref="IHost"/> for the test server.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/> used to configure the test server.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        builder.ConfigureServices(services =>
        {
            if (_mockDatabase)
            {
                // Add mock database
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<MsiPostSqliteContext>));

                services.Remove(dbContextDescriptor ?? throw new InvalidOperationException());
                services.AddSingleton(CreateMockDatabase().Object);

                Environment.SetEnvironmentVariable("MOCK_DB", "true");
            }
            else
            {
                // Add in-memory database
                var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<MsiPostSqliteContext>));

                services.Remove(dbContextDescriptor ?? throw new InvalidOperationException());
                MsiPostOrmService.CreateInMemory(MsiPostOrmBackend.Sqlite, services);
            }
            // Add mock IMojangApiWrapper
            if (_mockMojangApiWrapper)
            {
                var mojangApiWrapperDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(IMojangApiWrapper));

                services.Remove(mojangApiWrapperDescriptor ?? throw new InvalidOperationException());
                services.AddSingleton(CreateMockMojangApiWrapper().Object);
            }

            // Add mock IMsiProfileService
            if (_mockMsiProfileService)
            {
                var msiProfileServiceDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(IMsiProfileService));

                services.Remove(msiProfileServiceDescriptor ?? throw new InvalidOperationException());
                services.AddSingleton(CreateMockMsiProfileService().Object);
            }

            // Add mock IMsiPostService
            if (_mockMsiPostService)
            {
                var msiPostServiceDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(IMsiPostService));

                services.Remove(msiPostServiceDescriptor ?? throw new InvalidOperationException());
                services.AddSingleton(CreateMockMsiPostService().Object);
            }
        });
    }

    /// <summary>
    /// Create a mock database
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static Mock<MsiPostDbContext> CreateMockDatabase()
    {
        var mock = new Mock<MsiPostDbContext>();
        return mock;
    }

    /// <summary>
    /// Create a mock IMojangApiWrapper
    /// </summary>
    /// <returns></returns>
    private static Mock<IMojangApiWrapper> CreateMockMojangApiWrapper()
    {
        var mock = new Mock<IMojangApiWrapper>();
        mock.Setup(m => m.GetProfileAsync(It.IsAny<Guid>())).ReturnsAsync(new MojangProfileDTO
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test"
        });
        return mock;
    }

    /// <summary>
    /// Create a mock IMsiProfileService
    /// </summary>
    /// <returns></returns>
    private static Mock<IMsiProfileService> CreateMockMsiProfileService()
    {
        var mock = new Mock<IMsiProfileService>();
        mock.Setup(m => m.GetProfileAsync(It.IsAny<Guid>())).ReturnsAsync(new Profile
        {
            Id = Guid.NewGuid(),
        });
        return mock;
    }

    /// <summary>
    /// Create a mock IMsiPostService
    /// </summary>
    /// <returns></returns>
    private static Mock<IMsiPostService> CreateMockMsiPostService()
    {
        var mock = new Mock<IMsiPostService>();
        mock.Setup(m => m.GetPostAsync(It.IsAny<Guid>())).ReturnsAsync(new PostDTO
        {
            Id = Guid.NewGuid(),
            ProfileId = Guid.NewGuid(),
            Text = "Test",
            CreatedAt = DateTime.Now
        });
        return mock;
    }

    /// <summary>
    /// Enable mocking of the database
    /// </summary>
    /// <returns></returns>
    public TestMsiApplicationFactory<TProgram> WithMockDatabase()
    {
        _mockDatabase = true;
        return this;
    }

    /// <summary>
    /// Enable mocking of the MojangApiWrapper
    /// </summary>
    /// <returns></returns>
    public TestMsiApplicationFactory<TProgram> WithMockMojangApiWrapper()
    {
        _mockMojangApiWrapper = true;
        return this;
    }

    /// <summary>
    /// Enable mocking of the MsiProfileService
    /// </summary>
    /// <returns></returns>
    public TestMsiApplicationFactory<TProgram> WithMockMsiProfileService()
    {
        _mockMsiProfileService = true;
        return this;
    }

    /// <summary>
    /// Enable mocking of the MsiPostService
    /// </summary>
    /// <returns></returns>
    public TestMsiApplicationFactory<TProgram> WithMockMsiPostService()
    {
        _mockMsiPostService = true;
        return this;
    }
}

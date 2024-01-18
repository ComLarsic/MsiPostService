using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using Moq;
using MsiMojangApiWrapper;
using MsiMojangApiWrapper.DTO;
using MsiPostOrm;
using MsiPostOrm.Entity;
using MsiPostOrmSqlite;
using MsiPostOrmUtility;
using MsiPostProfile;
using SQLitePCL;
using Xunit.Sdk;

namespace MsiPostServer.Tests.Unit;

/// <summary>
/// The unit tests for the profile service.
/// </summary>
public class ProfileTests
{
    protected readonly DbContextMock<MsiPostDbContext> MockDbContext;
    protected readonly Mock<IMsiPostOrmService> MockMsiPostOrmService;
    protected readonly Mock<IMojangApiWrapper> MockMojangApiWrapper;
    protected readonly MsiProfileService ProfileService;


    /// <summary>
    /// The dummy profile.
    /// </summary>
    protected readonly Profile Profile;

    /// <summary>
    /// The dummy profile entity.
    /// </summary>
    protected readonly ProfileEntity ProfileEntity;

    public ProfileTests()
    {
        MockDbContext = new DbContextMock<MsiPostDbContext>();
        MockMsiPostOrmService = new Mock<IMsiPostOrmService>();
        MockMojangApiWrapper = new Mock<IMojangApiWrapper>();
        ProfileService = new MsiProfileService(MockMsiPostOrmService.Object, MockMojangApiWrapper.Object);

        // Dummy data
        Profile = new Profile
        {
            Uuid = Guid.NewGuid(),
            IsActive = true
        };

        ProfileEntity = new ProfileEntity
        {
            Id = Profile.Uuid,
            IsActive = Profile.IsActive
        };

        // Mocks
        MockDbContext.CreateDbSetMock(db => db.Profiles, new List<ProfileEntity> { });
        MockMsiPostOrmService.Setup(service => service.Context(It.IsAny<Func<MsiPostDbContext, Task>>()))
            .Returns((Func<MsiPostDbContext, Task> func) => func(MockDbContext.Object));
        MockMsiPostOrmService.Setup(service => service.Context(It.IsAny<Func<MsiPostDbContext, Task<Profile?>>>()))
            .Returns((Func<MsiPostDbContext, Task<Profile?>> func) => func(MockDbContext.Object));
        MockMojangApiWrapper.Setup(service => service.GetProfileAsync(Profile.Uuid))
            .ReturnsAsync(new MojangProfileDTO
            {
                Id = Profile.Uuid.ToString(),
                Name = "",
                Properties = []
            });
    }

    /// <summary>
    /// Unit test for creating a profile.
    /// </summary>
    [Fact]
    public void CreateProfile()
    {
        // Act
        var task = ProfileService.CreateProfileAsync(Profile.Uuid);
        task.Wait();
        var result = task.Result;

        // Assert
        Assert.Equal(Profile.Uuid, result.Uuid);
    }

    /// <summary>
    /// Unit tests for fetching a profile
    /// </summary>
    [Fact]
    public void GetProfile()
    {
        // Act
        var createTask = ProfileService.CreateProfileAsync(Profile.Uuid);
        createTask.Wait();

        var getTask = ProfileService.GetProfileAsync(Profile.Uuid);
        getTask.Wait();

        MockMsiPostOrmService.Verify(service => service.Context(It.IsAny<Func<MsiPostDbContext, Task<Profile?>>>()), Times.Once);
    }
}
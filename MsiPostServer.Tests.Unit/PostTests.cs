using EntityFrameworkCoreMock;
using Moq;
using MsiMojangApiWrapper;
using MsiMojangApiWrapper.DTO;
using MsiPostOrm;
using MsiPostOrm.Entity;
using MsiPostOrmUtility;
using MsiPostProfile;
using MsiPosts;
using Xunit.Sdk;

namespace MsiPostServer.Tests.Unit;

/// <summary>
/// The unit tests for the post service
/// </summary>
public class PostTests
{
    protected readonly DbContextMock<MsiPostDbContext> MockDbContext;
    protected readonly Mock<IMsiPostOrmService> MockMsiPostOrmService;
    protected readonly Mock<IMojangApiWrapper> MockMojangApiWrapper;
    protected readonly MsiProfileService ProfileService;
    protected readonly MsiPostService PostService;

    /// <summary>
    /// The dummy profile.
    /// </summary>
    protected readonly Profile Profile;

    /// <summary>
    /// The dummy profile entity.
    /// </summary>
    protected readonly ProfileEntity ProfileEntity;

    /// <summary>
    /// The dummy post text.
    /// </summary>
    protected readonly string PostText;

    /// <summary>
    /// The dummy post entity.
    /// </summary>
    protected readonly PostEntity PostEntity;

    public PostTests()
    {
        MockDbContext = new DbContextMock<MsiPostDbContext>();
        MockMsiPostOrmService = new Mock<IMsiPostOrmService>();
        MockMojangApiWrapper = new Mock<IMojangApiWrapper>();
        ProfileService = new MsiProfileService(MockMsiPostOrmService.Object, MockMojangApiWrapper.Object);
        PostService = new MsiPostService(MockMsiPostOrmService.Object);

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

        PostText = "This is a post";

        PostEntity = new PostEntity
        {
            Id = Guid.NewGuid(),
            ProfileId = Profile.Uuid,
            Text = PostText,
            CreatedAt = DateTime.UtcNow
        };

        // Mocks
        MockDbContext.CreateDbSetMock(db => db.Profiles, new List<ProfileEntity> { });
        MockDbContext.CreateDbSetMock(db => db.Posts, new List<PostEntity> { });
        MockDbContext.CreateDbSetMock(db => db.Likes, new List<LikeEntity> { });

        Task result = Task.CompletedTask;
        MockMsiPostOrmService.Setup(service => service.Context(It.IsAny<Func<MsiPostDbContext, Task>>()))
            .Returns((Func<MsiPostDbContext, Task> func) => func(MockDbContext.Object));
        MockMsiPostOrmService.Setup(service => service.Context(It.IsAny<Func<MsiPostDbContext, Task<Guid>>>()))
            .Returns((Func<MsiPostDbContext, Task<Guid>> func) => func(MockDbContext.Object));
        MockMojangApiWrapper.Setup(service => service.GetProfileAsync(Profile.Uuid))
            .ReturnsAsync(new MojangProfileDTO
            {
                Id = Profile.Uuid.ToString(),
                Name = "",
                Properties = []
            });
    }

    /// <summary>
    /// Unit test for creating a post.
    /// </summary>
    [Fact]
    public void CreatePost()
    {
        // Arrange
        var profileId = Profile.Uuid;
        var text = PostText;

        // Act
        var postIdTask = PostService.CreatePostAsync(profileId, text);
        postIdTask.Wait();
        var postId = postIdTask.Result;

        // Assert
        Assert.NotEqual(Guid.Empty, postId);
    }

    /// <summary>
    /// Unit test for deleting a post.
    /// </summary>
    [Fact]
    public void DeletePost()
    {
        // Arrange
        var postId = PostEntity.Id;

        // Act
        var task = PostService.DeletePostAsync(postId);
        task.Wait();

        // Assert

        // Verify the database has been accessed
        MockMsiPostOrmService.Verify(service => service.Context(It.IsAny<Func<MsiPostDbContext, Task>>()), Times.Once);
    }
}
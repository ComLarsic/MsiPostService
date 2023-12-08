using System.Text;
using System.Text.Json;
using MsiPostServer.DTO;

namespace MsiPostServer.Tests;

/// <summary>
/// The tests for the profile controller.
/// </summary>
/// <param name="webApplicationFactory"></param>
public class ProfileTests(TestMsiApplicationFactory<Program> webApplicationFactory)
    : IClassFixture<TestMsiApplicationFactory<Program>>
{
    private readonly TestMsiApplicationFactory<Program> _webApplicationFactory = webApplicationFactory;

    /// <summary>
    /// Test for creating a profile.
    /// This tests creates a profile and checks if the profile is created.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateProfile()
    {
        // Add mock services
        _webApplicationFactory
            .WithMockMojangApiWrapper()
            .WithMockMsiPostService();

        var createProfileDTO = new CreateProfileDTO
        {
            Id = Guid.NewGuid(),
        };

        var client = _webApplicationFactory.CreateClient();
        var dtoSerialized = JsonSerializer.Serialize(createProfileDTO);
        var postResponse = await client.PostAsync("/api/profile", new StringContent(dtoSerialized.ToString(), Encoding.UTF8, "application/json"));
        postResponse.EnsureSuccessStatusCode();

        var response = await client.GetAsync("/api/profile/" + createProfileDTO.Id);

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var profileResult = JsonSerializer.Deserialize<ProfileDTO>(responseString);
        Assert.Equal(createProfileDTO.Id, profileResult.Id);
    }

    /// <summary>
    /// Test for getting all profiles.
    /// This tests adds a couple of profiles and checks if the profiles are returned.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetProfiles()
    {
        // Add mock services
        _webApplicationFactory
            .WithMockMojangApiWrapper()
            .WithMockMsiPostService();

        var client = _webApplicationFactory.CreateClient();

        var ids = new List<Guid>();
        for (int i = 0; i < 5; i++)
        {
            var createProfileDTO = new CreateProfileDTO
            {
                Id = Guid.NewGuid(),
            };


            var dtoSerialized = JsonSerializer.Serialize(createProfileDTO);
            var postResponse = await client.PostAsync("/api/profile", new StringContent(dtoSerialized.ToString(), Encoding.UTF8, "application/json"));
            postResponse.EnsureSuccessStatusCode();
            ids.Add(createProfileDTO.Id);
        }

        var response = await client.GetAsync("/api/profile");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var profileResult = JsonSerializer.Deserialize<List<ProfileDTO>>(responseString) ?? [];

        Assert.Equal(ids.Count, profileResult.Count);
        foreach (var id in ids)
            Assert.Contains(profileResult, p => p.Id == id);
    }
}

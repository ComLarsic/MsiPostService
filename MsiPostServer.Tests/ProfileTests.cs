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
    private TestMsiApplicationFactory<Program> _webApplicationFactory = webApplicationFactory;

    /// <summary>
    /// Test for creating a profile.
    /// This tests creates a profile and checks if the profile is created.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public void CreateProfile()
    {
        // Add mock services
        _webApplicationFactory = new TestMsiApplicationFactory<Program>()
            .WithMockMojangApiWrapper()
            .WithMockMsiPostService();

        var createProfileDTO = new CreateProfileDTO
        {
            Id = Guid.NewGuid(),
        };

        var client = _webApplicationFactory.CreateClient();
        var dtoSerialized = JsonSerializer.Serialize(createProfileDTO);
        var postResponse = client.PostAsync("/api/profile", new StringContent(dtoSerialized.ToString(), Encoding.UTF8, "application/json")).Result;
        postResponse.EnsureSuccessStatusCode();

        var response = client.GetAsync("/api/profile/" + createProfileDTO.Id).Result;

        response.EnsureSuccessStatusCode();
        var responseString = response.Content.ReadAsStringAsync().Result;
        var profileResult = JsonSerializer.Deserialize<ProfileDTO>(responseString);
        Assert.Equal(createProfileDTO.Id, profileResult.Id);
    }

    /// <summary>
    /// Test for getting all profiles.
    /// This tests adds a couple of profiles and checks if the profiles are returned.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public void GetProfiles()
    {
        // Add mock services
        _webApplicationFactory = new TestMsiApplicationFactory<Program>()
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
            var postResponse = client.PostAsync("/api/profile", new StringContent(dtoSerialized.ToString(), Encoding.UTF8, "application/json")).Result;
            postResponse.EnsureSuccessStatusCode();
            ids.Add(createProfileDTO.Id);
        }

        var response = client.GetAsync("/api/profile").Result;
        response.EnsureSuccessStatusCode();

        var responseString = response.Content.ReadAsStringAsync().Result;
        var profileResult = JsonSerializer.Deserialize<List<ProfileDTO>>(responseString) ?? [];

        foreach (var id in ids)
            Assert.Contains(profileResult, p => p.Id == id);
    }
}

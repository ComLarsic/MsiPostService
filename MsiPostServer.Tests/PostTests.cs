using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using MsiMojangApiWrapper;
using MsiMojangApiWrapper.DTO;
using MsiPosts.DTO;
using MsiPostServer.DTO;
using Namespace;

namespace MsiPostServer.Tests;

/// <summary>
/// The testing for posts
/// </summary>
public class PostTests(TestMsiApplicationFactory<Program> webApplicationFactory)
    : IClassFixture<TestMsiApplicationFactory<Program>>
{
    private TestMsiApplicationFactory<Program> _webApplicationFactory = webApplicationFactory;



    /// <summary>
    /// Test for getting all post   s.
    /// This tests adds a couple of posts and checks if the posts are returned.
    /// </summary>
    /// <returns></returns>
    [Theory]
    [InlineData("Hello, World!")]
    [InlineData("Bye, World!")]
    [InlineData("Hello, World! Bye, World!")]
    [InlineData("The quick brown fox jumped over the lazy dog")]
    public void GetPosts(string post)
    {
        // Add mock services
        _webApplicationFactory = new TestMsiApplicationFactory<Program>()
            .WithMockMojangApiWrapper()
            .WithMockMsiProfileService();

        var createPostDTO = new CreatePostDTO
        {
            Profile = Guid.NewGuid(),
            Text = post
        };

        var client = _webApplicationFactory.CreateClient();
        var dtoSerialized = JsonSerializer.Serialize(createPostDTO);
        var postResponse = client.PostAsync("/api/post", new StringContent(dtoSerialized.ToString(), Encoding.UTF8, "application/json")).Result;
        postResponse.EnsureSuccessStatusCode();

        var postResponseString = postResponse.Content.ReadAsStringAsync().Result;
        var createPostResponseDTO = JsonSerializer.Deserialize<CreatePostResponseDTO>(postResponseString);

        var response = client.GetAsync("/api/post/" + createPostResponseDTO.Id).Result;

        response.EnsureSuccessStatusCode();
        var responseString = response.Content.ReadAsStringAsync().Result;
        var postResult = JsonSerializer.Deserialize<PostDTO>(responseString);
        Assert.Equal(createPostDTO.Text, postResult.Text);
    }

    /// <summary>
    /// Test for editing a post.
    /// This tests adds a post, edits it and checks if the post is edited.
    /// </summary>
    /// <returns></returns>
    [Theory]
    [InlineData("Hello, World!", "Bye, World!")]
    public void EditPost(string post, string editedPost)
    {
        // Add mock services
        _webApplicationFactory = new TestMsiApplicationFactory<Program>()
            .WithMockMojangApiWrapper()
            .WithMockMsiProfileService();

        var createPostDTO = new CreatePostDTO
        {
            Profile = Guid.NewGuid(),
            Text = post
        };

        var client = _webApplicationFactory.CreateClient();
        var dtoSerialized = JsonSerializer.Serialize(createPostDTO);
        var postResponse = client.PostAsync("/api/post", new StringContent(dtoSerialized.ToString(), Encoding.UTF8, "application/json")).Result;
        postResponse.EnsureSuccessStatusCode();

        var postResponseString = postResponse.Content.ReadAsStringAsync().Result;
        var createPostResponseDTO = JsonSerializer.Deserialize<CreatePostResponseDTO>(postResponseString);

        var editPostDTO = new EditPostDTO
        {
            Post = createPostResponseDTO.Id,
            Text = editedPost
        };

        var editDtoSerialized = JsonSerializer.Serialize(editPostDTO);
        var editResponse = client.PutAsync("/api/post", new StringContent(editDtoSerialized.ToString(), Encoding.UTF8, "application/json")).Result;
        editResponse.EnsureSuccessStatusCode();

        var response = client.GetAsync("/api/post/" + createPostResponseDTO.Id).Result;

        response.EnsureSuccessStatusCode();
        var responseString = response.Content.ReadAsStringAsync().Result;
        var postResult = JsonSerializer.Deserialize<PostDTO>(responseString);
        Assert.Equal(editedPost, postResult.Text);
    }
}

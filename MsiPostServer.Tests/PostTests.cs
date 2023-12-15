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
public class PostTests
{
    /// <summary>
    /// Test for getting all posts.
    /// This tests adds a couple of posts and checks if the posts are returned.
    /// </summary>
    /// <returns></returns>
    [Theory]
    [InlineData("Hello, World!")]
    [InlineData("Bye, World!")]
    [InlineData("Hello, World! Bye, World!")]
    [InlineData("The quick brown fox jumped over the lazy dog")]
    public async Task GetPosts(string post)
    {
        var webApplicationFactory = new TestMsiApplicationFactory<Program>();

        // Add mock services
        webApplicationFactory
            .WithMockMojangApiWrapper()
            .WithMockMsiProfileService();

        var createPostDTO = new CreatePostDTO
        {
            Profile = Guid.NewGuid(),
            Text = post
        };

        var client = webApplicationFactory.CreateClient();
        var dtoSerialized = JsonSerializer.Serialize(createPostDTO);
        var postResponse = await client.PostAsync("/api/post", new StringContent(dtoSerialized.ToString(), Encoding.UTF8, "application/json"));
        postResponse.EnsureSuccessStatusCode();

        var postResponseString = await postResponse.Content.ReadAsStringAsync();
        var createPostResponseDTO = JsonSerializer.Deserialize<CreatePostResponseDTO>(postResponseString);

        var response = await client.GetAsync("/api/post/" + createPostResponseDTO.Id);

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
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
    public async Task EditPost(string post, string editedPost)
    {
        var webApplicationFactory = new TestMsiApplicationFactory<Program>();

        // Add mock services
        webApplicationFactory
            .WithMockMojangApiWrapper()
            .WithMockMsiProfileService();

        var createPostDTO = new CreatePostDTO
        {
            Profile = Guid.NewGuid(),
            Text = post
        };

        var client = webApplicationFactory.CreateClient();
        var dtoSerialized = JsonSerializer.Serialize(createPostDTO);
        var postResponse = await client.PostAsync("/api/post", new StringContent(dtoSerialized.ToString(), Encoding.UTF8, "application/json"));
        postResponse.EnsureSuccessStatusCode();

        var postResponseString = await postResponse.Content.ReadAsStringAsync();
        var createPostResponseDTO = JsonSerializer.Deserialize<CreatePostResponseDTO>(postResponseString);

        var editPostDTO = new EditPostDTO
        {
            Post = createPostResponseDTO.Id,
            Text = editedPost
        };

        var editDtoSerialized = JsonSerializer.Serialize(editPostDTO);
        var editResponse = await client.PutAsync("/api/post", new StringContent(editDtoSerialized.ToString(), Encoding.UTF8, "application/json"));
        editResponse.EnsureSuccessStatusCode();

        var response = await client.GetAsync("/api/post/" + createPostResponseDTO.Id);

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var postResult = JsonSerializer.Deserialize<PostDTO>(responseString);
        Assert.Equal(editedPost, postResult.Text);
    }
}

using Microsoft.AspNetCore.Mvc.Testing;

namespace MsiPostServer.Tests;

/// <summary>
/// The testing for posts
/// </summary>
public class PostTests(TestMsiApplicationFactory<Program> webApplicationFactory)
    : IClassFixture<TestMsiApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory = webApplicationFactory;

    
}

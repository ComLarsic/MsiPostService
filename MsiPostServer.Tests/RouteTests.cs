using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace MsiPostServer.Tests;

/// <summary>
/// Tests for the routes
/// </summary>
public class RouteTests
{

    /// <summary>
    /// Test that none of the routes cause an internal server error when requested
    /// </summary>
    [Fact]
    public async void NoInternalError()
    {
        var webApplicationFactory = new TestMsiApplicationFactory<Program>();
        var endpoints = webApplicationFactory.Services
            .GetServices<EndpointDataSource>()
            .SelectMany(es => es.Endpoints)
            .OfType<RouteEndpoint>();

        using HttpClient client = webApplicationFactory.CreateClient();

        foreach (var endpoint in endpoints)
        {
            var response = await client.GetAsync(endpoint.RoutePattern.RawText);
            // Asserts that the response is not an internal server error by making sure it is not in the 500 range
            Assert.NotInRange((int)response.StatusCode, 500, 599);
        }
    }
}
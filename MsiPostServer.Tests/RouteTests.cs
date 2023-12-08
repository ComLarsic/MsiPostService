using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MsiPostOrmUtility;

namespace MsiPostServer.Tests;

/// <summary>
/// Tests for the routes
/// </summary>
public class RouteTests(TestMsiApplicationFactory<Program> webApplicationFactory)
    : IClassFixture<TestMsiApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory = webApplicationFactory;

    /// <summary>
    /// Test that none of the routes cause an internal server error when requested
    /// </summary>
    [Fact]
    public async void NoInternalError()
    {
        var endpoints = _webApplicationFactory.Services
            .GetServices<EndpointDataSource>()
            .SelectMany(es => es.Endpoints)
            .OfType<RouteEndpoint>();

        using HttpClient client = _webApplicationFactory.CreateClient();

        foreach (var endpoint in endpoints)
        {
            var response = await client.GetAsync(endpoint.RoutePattern.RawText);
            // Asserts that the response is not an internal server error by making sure it is not in the 500 range
            Console.WriteLine($"Testing {endpoint.RoutePattern.RawText}");
            Assert.NotInRange((int)response.StatusCode, 500, 599);
        }
    }
}
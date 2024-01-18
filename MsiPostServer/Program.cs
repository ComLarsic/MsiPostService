using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MsiMojangApiWrapper;
using MsiPostOrm;
using MsiPostOrmUtility;
using MsiPostProfile;
using MsiPosts;

/// <summary>
/// Setup the CORS policy.
/// </summary>
static void SetupCors(WebApplicationBuilder builder)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
        {
            builder.AllowAnyOrigin()
                .WithMethods("GET", "POST", "PUT", "DELETE")
                .AllowCredentials()
                .WithHeaders("Accept", "Content-Type", "Origin", "Authentication-Bearer");
        });
    });
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

MsiPostOrmService.CreateDbContext(builder.Configuration, builder.Services);
builder.Services.AddSingleton<IMsiPostOrmService, MsiPostOrmService>();
builder.Services.AddSingleton<IMojangApiWrapper, MojangApiWrapper>();
builder.Services.AddSingleton<IMsiProfileService, MsiProfileService>();
builder.Services.AddSingleton<IMsiPostService, MsiPostService>();

builder.Services.AddHostedService<IMsiPostOrmHostedService>(s =>
{
    var ormService = s.GetRequiredService<IMsiPostOrmService>();
    return new MsiPostOrmHostedService(ormService);
});

SetupCors(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

/// <summary>
/// Expose the implicitly defined Program class to the rest of the project.
/// This allows us to access the VERSION constant from other files, and allows for unit/integration testing.
/// </summary>
public partial class Program
{
    /// <summary>
    /// The version of the server.
    /// </summary>
    public const string VERSION = "0.0.1";
}

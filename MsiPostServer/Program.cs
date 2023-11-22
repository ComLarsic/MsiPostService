using MsiPostOrmService;
using MsiPostProfile;

// The backend to use for interacting with the MsiPost database.
const MsiPostOrmBackend ORM_BACKEND = MsiPostOrmBackend.Sqlite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the database context
builder.Services.AddSingleton<IMsiPostOrmService>(new MsiPostOrmService.MsiPostOrmService(ORM_BACKEND));

builder.Services.AddSingleton<IProfileService, ProfileService>();

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

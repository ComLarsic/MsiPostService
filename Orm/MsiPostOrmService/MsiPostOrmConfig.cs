using System.Text.Json.Serialization;
using MsiPostOrmSqlite;

namespace MsiPostOrmUtility;

/// <summary>
/// The configuration for the MsiPost database.
/// </summary>
[Serializable]
public class MsiPostOrmConfig
{
    [JsonPropertyName("Backend")]
    public MsiPostOrmBackend Backend { get; set; }

    [JsonPropertyName("Sqlite")]
    public MsiPostSqliteConfig? Sqlite { get; set; }
}
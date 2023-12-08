namespace MsiPostOrmSqlite;

/// <summary>
/// The config for the sqlite backend 
/// </summary>
public class MsiPostSqliteConfig
{
    /// <summary>
    /// The connection string
    /// </summary>
    public required string ConnectionString { get; set; }
}

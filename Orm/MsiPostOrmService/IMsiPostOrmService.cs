using MsiPostOrm;

namespace MsiPostOrmUtility;

/// <summary>
/// The service for interacting with the MsiPost database.
/// </summary>
public interface IMsiPostOrmService
{
    /// <summary>
    /// Get the database context in a scope
    /// </summary>
    /// <returns></returns>
    public Task Context(Func<MsiPostDbContext, Task> func);

    /// <summary>
    /// Get the database context in a scope
    /// </summary>
    /// <returns></returns>
    public Task<TResult> Context<TResult>(Func<MsiPostDbContext, Task<TResult>> func);
}

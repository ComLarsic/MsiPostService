using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using MsiPostOrm;

namespace MsiPostOrmService;

/// <summary>
/// The service for interacting with the MsiPost database.
/// </summary>
public interface IMsiPostOrmService
{
    /// <summary>
    /// The database context for the MsiPost database.
    /// </summary>
    public MsiPostDbContext Context();
}

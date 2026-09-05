using System.Data.Common;

namespace SmartSchool.Application.Persistence;

/// <summary>
/// Opens database connections for optimized Dapper read queries.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>Opens a database connection using the configured persistence provider.</summary>
    Task<DbConnection> OpenConnectionAsync(
        CancellationToken cancellationToken = default);
}

using SmartSchool.Modules.Audit.Models;

namespace SmartSchool.Modules.Audit.Persistence;

/// <summary>
/// Defines command persistence operations for AuditLogEntity.
/// </summary>
public interface IAuditLogCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        AuditLogEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        AuditLogEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        AuditLogEntity entity,
        CancellationToken cancellationToken);
}

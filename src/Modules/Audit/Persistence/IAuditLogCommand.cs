using SmartSchool.Modules.Audit.Models;

namespace SmartSchool.Modules.Audit.Persistence;

public interface IAuditLogCommand
{
    Task AddAsync(
        AuditLog entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        AuditLog entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        AuditLog entity,
        CancellationToken cancellationToken);
}

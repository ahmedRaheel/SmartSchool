using SmartSchool.Modules.Audit.Models;

namespace SmartSchool.Modules.Audit.Persistence;

/// <summary>
/// Write-side persistence for AuditLogEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AuditLogCommand : IAuditLogCommand
{
    public Task AddAsync(
        AuditLogEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AuditLogEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        AuditLogEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AuditLogEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        AuditLogEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AuditLogEntity delete persistence has not been connected to the module DbContext.");
    }
}

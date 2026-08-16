using SmartSchool.Modules.Audit.Models;

namespace SmartSchool.Modules.Audit.Persistence;

/// <summary>
/// Write-side persistence for AuditLog.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AuditLogCommand : IAuditLogCommand
{
    public Task AddAsync(
        AuditLog entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AuditLog create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        AuditLog entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AuditLog update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        AuditLog entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AuditLog delete persistence has not been connected to the module DbContext.");
    }
}

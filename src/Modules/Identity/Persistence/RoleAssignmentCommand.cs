using SmartSchool.Modules.Identity.Models;

namespace SmartSchool.Modules.Identity.Persistence;

/// <summary>
/// Write-side persistence for RoleAssignmentEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class RoleAssignmentCommand : IRoleAssignmentCommand
{
    public Task AddAsync(
        RoleAssignmentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "RoleAssignmentEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        RoleAssignmentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "RoleAssignmentEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        RoleAssignmentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "RoleAssignmentEntity delete persistence has not been connected to the module DbContext.");
    }
}

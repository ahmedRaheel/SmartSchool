using SmartSchool.Modules.Identity.Models;

namespace SmartSchool.Modules.Identity.Persistence;

/// <summary>
/// Write-side persistence for RoleAssignment.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class RoleAssignmentCommand : IRoleAssignmentCommand
{
    public Task AddAsync(
        RoleAssignment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "RoleAssignment create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        RoleAssignment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "RoleAssignment update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        RoleAssignment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "RoleAssignment delete persistence has not been connected to the module DbContext.");
    }
}

using SmartSchool.Modules.Identity.Models;

namespace SmartSchool.Modules.Identity.Persistence;

public interface IRoleAssignmentCommand
{
    Task AddAsync(
        RoleAssignment entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        RoleAssignment entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        RoleAssignment entity,
        CancellationToken cancellationToken);
}

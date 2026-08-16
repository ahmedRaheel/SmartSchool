using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence;

/// <summary>
/// Write-side persistence for Department.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class DepartmentCommand : IDepartmentCommand
{
    public Task AddAsync(
        Department entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Department create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Department entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Department update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Department entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Department delete persistence has not been connected to the module DbContext.");
    }
}

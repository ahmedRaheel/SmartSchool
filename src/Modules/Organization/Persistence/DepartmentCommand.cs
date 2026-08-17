using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence;

/// <summary>
/// Write-side persistence for DepartmentEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class DepartmentCommand : IDepartmentCommand
{
    public Task AddAsync(
        DepartmentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DepartmentEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        DepartmentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DepartmentEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        DepartmentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DepartmentEntity delete persistence has not been connected to the module DbContext.");
    }
}

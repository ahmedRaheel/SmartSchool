using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence;

/// <summary>
/// Write-side persistence for SchoolEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class SchoolCommand : ISchoolCommand
{
    public Task AddAsync(
        SchoolEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SchoolEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        SchoolEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SchoolEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        SchoolEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SchoolEntity delete persistence has not been connected to the module DbContext.");
    }
}

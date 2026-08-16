using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence;

/// <summary>
/// Write-side persistence for School.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class SchoolCommand : ISchoolCommand
{
    public Task AddAsync(
        School entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "School create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        School entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "School update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        School entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "School delete persistence has not been connected to the module DbContext.");
    }
}

using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Write-side persistence for ScholarshipEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ScholarshipCommand : IScholarshipCommand
{
    public Task AddAsync(
        ScholarshipEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ScholarshipEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ScholarshipEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ScholarshipEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ScholarshipEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ScholarshipEntity delete persistence has not been connected to the module DbContext.");
    }
}

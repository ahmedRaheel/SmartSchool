using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Write-side persistence for Scholarship.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ScholarshipCommand : IScholarshipCommand
{
    public Task AddAsync(
        Scholarship entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Scholarship create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Scholarship entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Scholarship update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Scholarship entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Scholarship delete persistence has not been connected to the module DbContext.");
    }
}

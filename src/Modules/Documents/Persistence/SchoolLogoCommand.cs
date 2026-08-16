using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Write-side persistence for SchoolLogo.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class SchoolLogoCommand : ISchoolLogoCommand
{
    public Task AddAsync(
        SchoolLogo entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SchoolLogo create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        SchoolLogo entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SchoolLogo update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        SchoolLogo entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SchoolLogo delete persistence has not been connected to the module DbContext.");
    }
}

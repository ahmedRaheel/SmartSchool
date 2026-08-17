using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Write-side persistence for SchoolLogoEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class SchoolLogoCommand : ISchoolLogoCommand
{
    public Task AddAsync(
        SchoolLogoEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SchoolLogoEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        SchoolLogoEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SchoolLogoEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        SchoolLogoEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SchoolLogoEntity delete persistence has not been connected to the module DbContext.");
    }
}

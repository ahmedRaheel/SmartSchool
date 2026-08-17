using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for GradeLevelEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class GradeLevelCommand : IGradeLevelCommand
{
    public Task AddAsync(
        GradeLevelEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GradeLevelEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        GradeLevelEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GradeLevelEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        GradeLevelEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GradeLevelEntity delete persistence has not been connected to the module DbContext.");
    }
}

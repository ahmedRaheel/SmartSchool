using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Write-side persistence for ApplicationEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ApplicationCommand : IApplicationCommand
{
    public Task AddAsync(
        ApplicationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ApplicationEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ApplicationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ApplicationEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ApplicationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ApplicationEntity delete persistence has not been connected to the module DbContext.");
    }
}

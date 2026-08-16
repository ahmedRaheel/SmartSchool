using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Write-side persistence for Application.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ApplicationCommand : IApplicationCommand
{
    public Task AddAsync(
        Application entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Application create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Application entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Application update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Application entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Application delete persistence has not been connected to the module DbContext.");
    }
}

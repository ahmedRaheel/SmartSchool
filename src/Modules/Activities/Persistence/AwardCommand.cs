using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// Write-side persistence for Award.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AwardCommand : IAwardCommand
{
    public Task AddAsync(
        Award entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Award create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Award entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Award update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Award entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Award delete persistence has not been connected to the module DbContext.");
    }
}

using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Write-side persistence for HumanHandoff.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class HumanHandoffCommand : IHumanHandoffCommand
{
    public Task AddAsync(
        HumanHandoff entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "HumanHandoff create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        HumanHandoff entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "HumanHandoff update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        HumanHandoff entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "HumanHandoff delete persistence has not been connected to the module DbContext.");
    }
}

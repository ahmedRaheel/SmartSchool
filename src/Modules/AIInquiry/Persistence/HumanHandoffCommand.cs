using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Write-side persistence for HumanHandoffEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class HumanHandoffCommand : IHumanHandoffCommand
{
    public Task AddAsync(
        HumanHandoffEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "HumanHandoffEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        HumanHandoffEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "HumanHandoffEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        HumanHandoffEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "HumanHandoffEntity delete persistence has not been connected to the module DbContext.");
    }
}

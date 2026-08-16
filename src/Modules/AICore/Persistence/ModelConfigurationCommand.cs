using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Write-side persistence for ModelConfiguration.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ModelConfigurationCommand : IModelConfigurationCommand
{
    public Task AddAsync(
        ModelConfiguration entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ModelConfiguration create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ModelConfiguration entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ModelConfiguration update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ModelConfiguration entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ModelConfiguration delete persistence has not been connected to the module DbContext.");
    }
}

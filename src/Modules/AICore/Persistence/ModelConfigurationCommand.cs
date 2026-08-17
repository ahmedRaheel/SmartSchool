using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Write-side persistence for ModelConfigurationEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ModelConfigurationCommand : IModelConfigurationCommand
{
    public Task AddAsync(
        ModelConfigurationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ModelConfigurationEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ModelConfigurationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ModelConfigurationEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ModelConfigurationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ModelConfigurationEntity delete persistence has not been connected to the module DbContext.");
    }
}

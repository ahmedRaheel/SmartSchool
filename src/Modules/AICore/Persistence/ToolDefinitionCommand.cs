using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Write-side persistence for ToolDefinitionEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ToolDefinitionCommand : IToolDefinitionCommand
{
    public Task AddAsync(
        ToolDefinitionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ToolDefinitionEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ToolDefinitionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ToolDefinitionEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ToolDefinitionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ToolDefinitionEntity delete persistence has not been connected to the module DbContext.");
    }
}

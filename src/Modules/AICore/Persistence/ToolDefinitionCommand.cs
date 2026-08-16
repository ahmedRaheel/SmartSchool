using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Write-side persistence for ToolDefinition.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ToolDefinitionCommand : IToolDefinitionCommand
{
    public Task AddAsync(
        ToolDefinition entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ToolDefinition create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ToolDefinition entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ToolDefinition update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ToolDefinition entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ToolDefinition delete persistence has not been connected to the module DbContext.");
    }
}

using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

public interface IToolDefinitionCommand
{
    Task AddAsync(
        ToolDefinition entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        ToolDefinition entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        ToolDefinition entity,
        CancellationToken cancellationToken);
}

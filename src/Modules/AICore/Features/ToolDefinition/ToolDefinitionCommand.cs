using SmartSchool.Modules.AICore.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Features.ToolDefinition;

/// <summary>
/// Executes database writes for <see cref="ToolDefinitionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ToolDefinitionCommand(IAICoreDbContext dbContext) : IToolDefinitionCommand
{
    public async Task AddAsync(
        ToolDefinitionEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.ToolDefinitions
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        ToolDefinitionEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.ToolDefinitions
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        ToolDefinitionEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.ToolDefinitions
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

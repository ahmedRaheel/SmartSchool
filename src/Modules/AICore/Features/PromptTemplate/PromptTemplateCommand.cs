using SmartSchool.Modules.AICore.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Features.PromptTemplate;

/// <summary>
/// Executes database writes for <see cref="PromptTemplateEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PromptTemplateCommand(IAICoreDbContext dbContext) : IPromptTemplateCommand
{
    public async Task AddAsync(
        PromptTemplateEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.PromptTemplates
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        PromptTemplateEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.PromptTemplates
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        PromptTemplateEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.PromptTemplates
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

using SmartSchool.Modules.AITutor.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Features.GeneratedQuiz;

/// <summary>
/// Executes database writes for <see cref="GeneratedQuizEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class GeneratedQuizCommand(IAITutorDbContext dbContext) : IGeneratedQuizCommand
{
    public async Task AddAsync(
        GeneratedQuizEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.GeneratedQuizs
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        GeneratedQuizEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.GeneratedQuizs
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        GeneratedQuizEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.GeneratedQuizs
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

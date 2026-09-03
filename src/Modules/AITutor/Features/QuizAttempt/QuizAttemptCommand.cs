using SmartSchool.Modules.AITutor.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Features.QuizAttempt;

/// <summary>
/// Executes database writes for <see cref="QuizAttemptEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class QuizAttemptCommand(IAITutorDbContext dbContext) : IQuizAttemptCommand
{
    public async Task AddAsync(
        QuizAttemptEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.QuizAttempts
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        QuizAttemptEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.QuizAttempts
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        QuizAttemptEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.QuizAttempts
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

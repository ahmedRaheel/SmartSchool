using SmartSchool.Modules.Examinations.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Features.Exam;

/// <summary>
/// Executes database writes for <see cref="ExamEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ExamCommand(IExaminationsDbContext dbContext) : IExamCommand
{
    public async Task AddAsync(
        ExamEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.Exams
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        ExamEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Exams
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        ExamEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Exams
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

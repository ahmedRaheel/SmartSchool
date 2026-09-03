using SmartSchool.Modules.Examinations.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Features.StudentExamResult;

/// <summary>
/// Executes database writes for <see cref="StudentExamResultEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StudentExamResultCommand(IExaminationsDbContext dbContext) : IStudentExamResultCommand
{
    public async Task AddAsync(
        StudentExamResultEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.StudentExamResults
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        StudentExamResultEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.StudentExamResults
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        StudentExamResultEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.StudentExamResults
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

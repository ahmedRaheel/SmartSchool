using SmartSchool.Modules.Students.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Features.Enrollment;

/// <summary>
/// Executes database writes for <see cref="EnrollmentEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class EnrollmentCommand(IStudentsDbContext dbContext) : IEnrollmentCommand
{
    public async Task AddAsync(
        EnrollmentEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.Enrollments
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        EnrollmentEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Enrollments
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        EnrollmentEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Enrollments
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

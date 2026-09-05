using SmartSchool.Modules.Examinations.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Features.GradeScale;

/// <summary>
/// Executes database writes for <see cref="GradeScaleEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class GradeScaleCommand(IExaminationsDbContext dbContext) : IGradeScaleCommand
{
    public async Task AddAsync(
        GradeScaleEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.GradeScales
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        GradeScaleEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.GradeScales
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        GradeScaleEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.GradeScales
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

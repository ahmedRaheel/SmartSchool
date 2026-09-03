using SmartSchool.Modules.AIPrediction.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.StudentIntervention;

/// <summary>
/// Executes database writes for <see cref="StudentInterventionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StudentInterventionCommand(IAIPredictionDbContext dbContext) : IStudentInterventionCommand
{
    public async Task AddAsync(
        StudentInterventionEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.StudentInterventions
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        StudentInterventionEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.StudentInterventions
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        StudentInterventionEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.StudentInterventions
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

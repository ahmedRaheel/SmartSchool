using SmartSchool.Modules.AIPrediction.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionModel;

/// <summary>
/// Executes database writes for <see cref="PredictionModelEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PredictionModelCommand(IAIPredictionDbContext dbContext) : IPredictionModelCommand
{
    public async Task AddAsync(
        PredictionModelEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.PredictionModels
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        PredictionModelEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.PredictionModels
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        PredictionModelEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.PredictionModels
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

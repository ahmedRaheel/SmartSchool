using SmartSchool.Modules.Transport.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Features.Route;

/// <summary>
/// Executes database writes for <see cref="RouteEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class RouteCommand(ITransportDbContext dbContext) : IRouteCommand
{
    public async Task AddAsync(
        RouteEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.Routes
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        RouteEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Routes
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        RouteEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Routes
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

using SmartSchool.Modules.Transport.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Features.StudentTransport;

/// <summary>
/// Executes database writes for <see cref="StudentTransportEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StudentTransportCommand(ITransportDbContext dbContext) : IStudentTransportCommand
{
    public async Task AddAsync(
        StudentTransportEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.StudentTransports
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        StudentTransportEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.StudentTransports
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        StudentTransportEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.StudentTransports
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

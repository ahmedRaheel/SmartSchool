using SmartSchool.Modules.HR.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Features.LeaveRequest;

/// <summary>
/// Executes database writes for <see cref="LeaveRequestEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class LeaveRequestCommand(IHRDbContext dbContext) : ILeaveRequestCommand
{
    public async Task AddAsync(
        LeaveRequestEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.LeaveRequests
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        LeaveRequestEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.LeaveRequests
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        LeaveRequestEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.LeaveRequests
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

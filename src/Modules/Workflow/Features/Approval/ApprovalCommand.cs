using SmartSchool.Modules.Workflow.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Features.Approval;

/// <summary>
/// Executes database writes for <see cref="ApprovalEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ApprovalCommand(IWorkflowDbContext dbContext) : IApprovalCommand
{
    public async Task AddAsync(
        ApprovalEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.Approvals
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        ApprovalEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Approvals
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        ApprovalEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Approvals
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

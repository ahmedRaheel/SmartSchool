using SmartSchool.Modules.Workflow.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Features.WorkflowInstance;

/// <summary>
/// Executes database writes for <see cref="WorkflowInstanceEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class WorkflowInstanceCommand(IWorkflowDbContext dbContext) : IWorkflowInstanceCommand
{
    public async Task AddAsync(
        WorkflowInstanceEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.WorkflowInstances
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        WorkflowInstanceEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.WorkflowInstances
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        WorkflowInstanceEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.WorkflowInstances
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

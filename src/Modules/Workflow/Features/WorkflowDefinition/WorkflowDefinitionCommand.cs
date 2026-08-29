using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Features.WorkflowDefinition;

/// <summary>
/// Executes database writes for <see cref="WorkflowDefinitionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class WorkflowDefinitionCommand(IApplicationDbContext dbContext) : IWorkflowDefinitionCommand
{
	public async Task AddAsync(
		WorkflowDefinitionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<WorkflowDefinitionEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		WorkflowDefinitionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<WorkflowDefinitionEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		WorkflowDefinitionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<WorkflowDefinitionEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}

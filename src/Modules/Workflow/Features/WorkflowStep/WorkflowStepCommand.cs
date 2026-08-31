using SmartSchool.Modules.Workflow.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Features.WorkflowStep;

/// <summary>
/// Executes database writes for <see cref="WorkflowStepEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class WorkflowStepCommand(IWorkflowDbContext dbContext) : IWorkflowStepCommand
{
	public async Task AddAsync(
		WorkflowStepEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.WorkflowSteps
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		WorkflowStepEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.WorkflowSteps
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		WorkflowStepEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.WorkflowSteps
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}

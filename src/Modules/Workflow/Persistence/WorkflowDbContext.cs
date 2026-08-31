using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

public interface IWorkflowDbContext
{
	DatabaseFacade Database { get; }

	DbSet<ApprovalEntity> Approvals { get; }
	DbSet<WorkflowDefinitionEntity> WorkflowDefinitions { get; }
	DbSet<WorkflowInstanceEntity> WorkflowInstances { get; }
	DbSet<WorkflowStepEntity> WorkflowSteps { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class WorkflowDbContext(IApplicationDbContext dbContext) : IWorkflowDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<ApprovalEntity> Approvals => dbContext.Set<ApprovalEntity>();
	public DbSet<WorkflowDefinitionEntity> WorkflowDefinitions => dbContext.Set<WorkflowDefinitionEntity>();
	public DbSet<WorkflowInstanceEntity> WorkflowInstances => dbContext.Set<WorkflowInstanceEntity>();
	public DbSet<WorkflowStepEntity> WorkflowSteps => dbContext.Set<WorkflowStepEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}

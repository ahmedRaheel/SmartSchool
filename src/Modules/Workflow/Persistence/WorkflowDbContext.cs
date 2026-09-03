using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
/// EF Core unit-of-work owned by the Workflow module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class WorkflowDbContext(DbContextOptions<WorkflowDbContext> options)
    : DbContext(options), IWorkflowDbContext
{
    public DbSet<ApprovalEntity> Approvals => Set<ApprovalEntity>();
    public DbSet<WorkflowDefinitionEntity> WorkflowDefinitions => Set<WorkflowDefinitionEntity>();
    public DbSet<WorkflowInstanceEntity> WorkflowInstances => Set<WorkflowInstanceEntity>();
    public DbSet<WorkflowStepEntity> WorkflowSteps => Set<WorkflowStepEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WorkflowDbContext).Assembly,
            type => type.Namespace is not null
                && type.Namespace.StartsWith("SmartSchool.Modules.Workflow.Persistence.Configurations", StringComparison.Ordinal));
    }
}

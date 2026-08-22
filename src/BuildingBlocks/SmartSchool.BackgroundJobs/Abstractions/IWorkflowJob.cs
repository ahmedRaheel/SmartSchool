namespace SmartSchool.BackgroundJobs.Abstractions;

/// <summary>Represents an idempotent background workflow step.</summary>
public interface IWorkflowJob
{
    /// <summary>Executes the workflow for a tenant.</summary>
    Task ExecuteAsync(Guid tenantId, CancellationToken cancellationToken);
}

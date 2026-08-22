using SmartSchool.BackgroundJobs.Abstractions;
using Microsoft.Extensions.Logging;

namespace SmartSchool.BackgroundJobs.Jobs;

/// <summary>Processes admission submitted, accepted, rejected and terminated workflow notifications.</summary>
public sealed class AdmissionWorkflowJob(ILogger<AdmissionWorkflowJob> logger) : IWorkflowJob
{
    /// <inheritdoc />
    public Task ExecuteAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Executing admission workflow for tenant {TenantId}.",
            tenantId);

        // The concrete feature command/query is invoked here.
        // Keep this job orchestration-only and idempotent.
        return Task.CompletedTask;
    }
}

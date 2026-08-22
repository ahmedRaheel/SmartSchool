using SmartSchool.BackgroundJobs.Abstractions;
using Microsoft.Extensions.Logging;

namespace SmartSchool.BackgroundJobs.Jobs;

/// <summary>Processes result-publication notifications and downstream refresh work.</summary>
public sealed class ResultPublicationJob(ILogger<ResultPublicationJob> logger) : IWorkflowJob
{
    /// <inheritdoc />
    public Task ExecuteAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Executing result publication workflow for tenant {TenantId}.",
            tenantId);

        // The concrete feature command/query is invoked here.
        // Keep this job orchestration-only and idempotent.
        return Task.CompletedTask;
    }
}

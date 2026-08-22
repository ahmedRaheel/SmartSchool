using SmartSchool.BackgroundJobs.Abstractions;
using Microsoft.Extensions.Logging;

namespace SmartSchool.BackgroundJobs.Jobs;

/// <summary>Publishes exam-start and exam-reminder notifications.</summary>
public sealed class ExamNotificationJob(ILogger<ExamNotificationJob> logger) : IWorkflowJob
{
    /// <inheritdoc />
    public Task ExecuteAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Executing exam notification workflow for tenant {TenantId}.",
            tenantId);

        // The concrete feature command/query is invoked here.
        // Keep this job orchestration-only and idempotent.
        return Task.CompletedTask;
    }
}

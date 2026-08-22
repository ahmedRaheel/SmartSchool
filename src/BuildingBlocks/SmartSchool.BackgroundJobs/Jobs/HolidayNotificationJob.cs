using SmartSchool.BackgroundJobs.Abstractions;
using Microsoft.Extensions.Logging;

namespace SmartSchool.BackgroundJobs.Jobs;

/// <summary>Publishes Eid, summer, winter and public-holiday notifications.</summary>
public sealed class HolidayNotificationJob(ILogger<HolidayNotificationJob> logger) : IWorkflowJob
{
    /// <inheritdoc />
    public Task ExecuteAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Executing holiday workflow for tenant {TenantId}.",
            tenantId);

        // The concrete feature command/query is invoked here.
        // Keep this job orchestration-only and idempotent.
        return Task.CompletedTask;
    }
}

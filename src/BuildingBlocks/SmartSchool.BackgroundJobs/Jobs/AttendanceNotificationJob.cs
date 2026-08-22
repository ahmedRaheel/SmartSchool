using SmartSchool.BackgroundJobs.Abstractions;
using Microsoft.Extensions.Logging;

namespace SmartSchool.BackgroundJobs.Jobs;

/// <summary>Processes absence, late-arrival and attendance-summary notifications.</summary>
public sealed class AttendanceNotificationJob(ILogger<AttendanceNotificationJob> logger) : IWorkflowJob
{
    /// <inheritdoc />
    public Task ExecuteAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Executing attendance workflow for tenant {TenantId}.",
            tenantId);

        // The concrete feature command/query is invoked here.
        // Keep this job orchestration-only and idempotent.
        return Task.CompletedTask;
    }
}

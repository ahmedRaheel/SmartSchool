using Hangfire;
using SmartSchool.BackgroundJobs.Jobs;

namespace SmartSchool.BackgroundJobs.Workflows;

/// <summary>Registers recurring SmartSchool automation workflows.</summary>
public sealed class WorkflowScheduler(IRecurringJobManager recurringJobs)
{
    /// <summary>Registers recurring jobs using stable identifiers.</summary>
    public void Register()
    {
        recurringJobs.AddOrUpdate<FeeReminderJob>(
            "fees-reminders",
            job => job.ExecuteAsync(Guid.Empty, CancellationToken.None),
            Cron.Daily(6));

        recurringJobs.AddOrUpdate<ExamNotificationJob>(
            "exam-notifications",
            job => job.ExecuteAsync(Guid.Empty, CancellationToken.None),
            "*/15 * * * *");

        recurringJobs.AddOrUpdate<HolidayNotificationJob>(
            "holiday-notifications",
            job => job.ExecuteAsync(Guid.Empty, CancellationToken.None),
            Cron.Daily(7));

        recurringJobs.AddOrUpdate<TimetableNotificationJob>(
            "timetable-notifications",
            job => job.ExecuteAsync(Guid.Empty, CancellationToken.None),
            "*/10 * * * *");

        recurringJobs.AddOrUpdate<AttendanceNotificationJob>(
            "attendance-notifications",
            job => job.ExecuteAsync(Guid.Empty, CancellationToken.None),
            Cron.Daily(15));

        recurringJobs.AddOrUpdate<NotificationDispatchJob>(
            "notification-dispatch",
            job => job.ExecuteAsync(Guid.Empty, CancellationToken.None),
            "*/5 * * * *");
    }
}

using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.BackgroundJobs.Configuration;
using SmartSchool.BackgroundJobs.Jobs;
using SmartSchool.BackgroundJobs.Workflows;

namespace SmartSchool.BackgroundJobs.Extensions;

/// <summary>Registers Hangfire and SmartSchool workflow jobs.</summary>
public static class BackgroundJobExtensions
{
    /// <summary>Adds background-job infrastructure.</summary>
    public static IServiceCollection AddSmartSchoolBackgroundJobs(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration
            .GetSection(BackgroundJobOptions.SectionName)
            .Get<BackgroundJobOptions>() ?? new BackgroundJobOptions();

        services.Configure<BackgroundJobOptions>(
            configuration.GetSection(BackgroundJobOptions.SectionName));

        var provider = configuration["Database:Provider"] ?? "PostgreSql";
        var connectionString = configuration.GetConnectionString("SmartSchool")
            ?? throw new InvalidOperationException("SmartSchool connection string is required.");

        services.AddHangfire(hangfire =>
        {
            if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
            {
                hangfire.UseSqlServerStorage(connectionString);
            }
            else
            {
                hangfire.UsePostgreSqlStorage(options =>
                    options.UseNpgsqlConnection(connectionString));
            }
        });

        services.AddHangfireServer(server => server.WorkerCount = options.WorkerCount);

        services.AddScoped<ExamNotificationJob>();
        services.AddScoped<FeeReminderJob>();
        services.AddScoped<HolidayNotificationJob>();
        services.AddScoped<TimetableNotificationJob>();
        services.AddScoped<LeaveWorkflowJob>();
        services.AddScoped<AdmissionWorkflowJob>();
        services.AddScoped<EventNotificationJob>();
        services.AddScoped<ResultPublicationJob>();
        services.AddScoped<AttendanceNotificationJob>();
        services.AddScoped<RagKnowledgeIngestionJob>();
        services.AddScoped<NotificationDispatchJob>();
        services.AddScoped<WorkflowScheduler>();

        return services;
    }

    /// <summary>Enables Hangfire middleware and recurring workflows.</summary>
    public static WebApplication UseSmartSchoolBackgroundJobs(this WebApplication app)
    {
        var options = app.Configuration
            .GetSection(BackgroundJobOptions.SectionName)
            .Get<BackgroundJobOptions>() ?? new BackgroundJobOptions();

        if (!options.Enabled)
        {
            return app;
        }

        if (options.DashboardEnabled)
        {
            app.UseHangfireDashboard(options.DashboardPath);
        }

        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<WorkflowScheduler>().Register();

        return app;
    }
}

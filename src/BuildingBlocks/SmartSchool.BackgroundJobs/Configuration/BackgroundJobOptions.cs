namespace SmartSchool.BackgroundJobs.Configuration;

/// <summary>Configures SmartSchool background processing.</summary>
public sealed class BackgroundJobOptions
{
    /// <summary>Gets the configuration section name.</summary>
    public const string SectionName = "BackgroundJobs";

    /// <summary>Gets or sets whether background processing is enabled.</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Gets or sets whether the Hangfire dashboard is enabled.</summary>
    public bool DashboardEnabled { get; set; } = true;

    /// <summary>Gets or sets the dashboard route.</summary>
    public string DashboardPath { get; set; } = "/hangfire";

    /// <summary>Gets or sets the worker count.</summary>
    public int WorkerCount { get; set; } = 4;
}

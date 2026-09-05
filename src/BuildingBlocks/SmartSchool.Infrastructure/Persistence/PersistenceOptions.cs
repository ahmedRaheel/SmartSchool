namespace SmartSchool.Infrastructure.Persistence;

/// <summary>
/// Configures the operational persistence provider.
/// </summary>
public sealed class PersistenceOptions
{
    public const string SectionName = "Persistence";

    public PersistenceProvider Provider { get; init; } = PersistenceProvider.Mock;

    public string ConnectionStringName { get; init; } = "SmartSchool";

    public bool EnableSensitiveDataLogging { get; init; }
}

public enum PersistenceProvider
{
    Mock,
    PostgreSql,
    SqlServer
}

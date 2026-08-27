namespace SmartSchool.Application.Persistence;

/// <summary>
/// Generates concurrency-safe, human-readable business identifiers.
/// </summary>
public interface IBusinessNumberGenerator
{
    Task<string> NextAsync(
        string sequenceName,
        string prefix,
        Guid? tenantId,
        int padding,
        CancellationToken cancellationToken = default);
}

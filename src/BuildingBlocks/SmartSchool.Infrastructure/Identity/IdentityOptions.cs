namespace SmartSchool.Infrastructure.Identity;

/// <summary>
/// Configures authentication for local mock development or an external OIDC identity server.
/// </summary>
public sealed class MockIdentityOptions
{
    public const string SectionName = "Identity";

    public IdentityProvider Provider { get; init; } = IdentityProvider.Mock;

    public string Authority { get; init; } = string.Empty;

    public string Audience { get; init; } = "smartschool-api";

    public bool RequireHttpsMetadata { get; init; } = true;
}

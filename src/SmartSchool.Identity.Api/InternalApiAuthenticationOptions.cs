namespace SmartSchool.Identity.Api;

public sealed class InternalApiAuthenticationOptions
{
    public const string SectionName = "InternalApiAuthentication";

    public const string SchemeName = "SmartSchoolApiBearer";

    public string Authority { get; init; } = string.Empty;

    public string RequiredScope { get; init; } = string.Empty;

    public bool RequireHttpsMetadata { get; init; } = true;
}

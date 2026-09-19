namespace SmartSchool.Identity.Api;

public sealed class InternalApiAuthenticationOptions
{
    public const string SectionName = "InternalApiAuthentication";

    public const string SchemeName = "SmartSchoolApiBearer";

    public string Authority { get; init; } = string.Empty;

    /// <summary>
    /// Expected JWT issuer. This is intentionally separate from Authority so a
    /// container can fetch discovery metadata through an internal address while
    /// validating tokens whose public issuer is http://localhost:7101.
    /// </summary>
    public string ValidIssuer { get; init; } = string.Empty;

    public string RequiredScope { get; init; } = string.Empty;

    public bool RequireHttpsMetadata { get; init; } = true;
}

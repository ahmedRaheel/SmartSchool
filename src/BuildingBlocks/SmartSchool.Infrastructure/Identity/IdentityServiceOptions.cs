namespace SmartSchool.Infrastructure.Identity;

public sealed class IdentityServiceOptions
{
	public const string SectionName = "IdentityService";
	public string BaseUrl { get; init; } = string.Empty;
	public string ClientId { get; init; } = "smartschool-api-service";
	public string ClientSecret { get; init; } = string.Empty;
	public string Scope { get; init; } = "smartschool.identity.manage";
}

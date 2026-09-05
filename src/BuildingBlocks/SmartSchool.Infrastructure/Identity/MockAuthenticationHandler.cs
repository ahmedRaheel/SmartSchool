using System.Threading.Tasks;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SmartSchool.Infrastructure.Identity;

/// <summary>
/// Development-only authentication handler that creates a predictable administrator identity.
/// </summary>
public sealed class MockAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory loggerFactory,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(
        options,
        loggerFactory,
        encoder)
{
    public const string SchemeName = "SmartSchoolMock";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "demo-admin"),
            new Claim(ClaimTypes.Name, "SmartSchool Administrator"),
            new Claim(ClaimTypes.Email, "admin@smartschool.demo"),
            new Claim(ClaimTypes.Role, "Administrator"),
            new Claim("tenant_id", "11111111-1111-1111-1111-111111111111")
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

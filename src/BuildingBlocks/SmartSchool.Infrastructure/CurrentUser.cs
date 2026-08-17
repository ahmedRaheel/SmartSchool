using System.Security.Claims;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Infrastructure.Identity;

public interface ICurrentUser
{
    Guid UserId { get; }
    Guid TenantId { get; }
    Guid SchoolId { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}

public sealed class CurrentUser(
    IHttpContextAccessor httpContextAccessor)
    : ICurrentUser
{
    private ClaimsPrincipal User =>
        httpContextAccessor.HttpContext?.User
        ?? new ClaimsPrincipal();

    public bool IsAuthenticated =>
        User.Identity?.IsAuthenticated == true;

    public Guid UserId =>
        GetRequiredGuidClaim(SmartSchoolClaims.UserId);

    public Guid TenantId =>
        GetRequiredGuidClaim(SmartSchoolClaims.TenantId);

    public Guid SchoolId =>
        GetRequiredGuidClaim(SmartSchoolClaims.SchoolId);

    public bool IsInRole(string role) =>
        User.IsInRole(role);

    private Guid GetRequiredGuidClaim(string claimType)
    {
        var value = User.FindFirstValue(claimType);

        if (!Guid.TryParse(value, out var result))
        {
            throw new UnauthorizedAccessException(
                $"Required identity claim '{claimType}' is missing.");
        }

        return result;
    }
}

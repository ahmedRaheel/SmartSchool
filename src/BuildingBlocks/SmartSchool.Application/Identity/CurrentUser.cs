using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Application.Identity;

/// <summary>
/// Reads the authenticated SmartSchool business context from JWT claims.
/// </summary>
public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal Principal =>
        httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();

    public bool IsAuthenticated => Principal.Identity?.IsAuthenticated == true;

    public Guid UserId => GetRequiredGuid(SmartSchoolClaims.UserId);

    public Guid? TenantId => GetOptionalGuid(SmartSchoolClaims.TenantId);

    public Guid? SchoolId => GetOptionalGuid(SmartSchoolClaims.SchoolId);

    public Guid? BranchId => GetOptionalGuid(SmartSchoolClaims.BranchId);

    public Guid? StudentId => GetOptionalGuid(SmartSchoolClaims.StudentId);

    public Guid? TeacherId => GetOptionalGuid(SmartSchoolClaims.TeacherId);

    public Guid? DriverId => GetOptionalGuid(SmartSchoolClaims.DriverId);

    public Guid? ExaminerId => GetOptionalGuid(SmartSchoolClaims.ExaminerId);

    public Guid? EmployeeId => GetOptionalGuid(SmartSchoolClaims.EmployeeId);

    public string? FirstName => GetValue(SmartSchoolClaims.FirstName);

    public string? LastName => GetValue(SmartSchoolClaims.LastName);

    public string? Email =>
        GetValue(ClaimTypes.Email) ?? GetValue("email");

    public IReadOnlyCollection<string> Roles => Principal
        .FindAll(SmartSchoolClaims.Role)
        .Select(claim => claim.Value)
        .Concat(Principal.FindAll(ClaimTypes.Role).Select(claim => claim.Value))
        .Where(role => !string.IsNullOrWhiteSpace(role))
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToArray();

    public bool IsSuperAdmin => IsInRole(SmartSchoolRoles.SuperAdmin);

    public bool IsImpersonated =>
        string.Equals(
            GetValue(SmartSchoolClaims.Impersonated),
            "true",
            StringComparison.OrdinalIgnoreCase);

    public bool IsInRole(string role)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(role);

        return Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
    }

    private string? GetValue(string claimType)
    {
        return Principal.FindFirstValue(claimType);
    }

    private Guid GetRequiredGuid(string claimType)
    {
        return GetOptionalGuid(claimType)
            ?? throw new UnauthorizedAccessException(
                $"Required access-token claim '{claimType}' is missing or invalid.");
    }

    private Guid? GetOptionalGuid(string claimType)
    {
        var value = GetValue(claimType);

        return Guid.TryParse(value, out var id)
            ? id
            : null;
    }
}

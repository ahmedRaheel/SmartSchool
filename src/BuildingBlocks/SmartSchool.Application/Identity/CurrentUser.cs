using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Application.Identity;

/// <summary>
/// Reads the current SmartSchool user exclusively from the authenticated
/// <see cref="HttpContext.User"/> claims principal.
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

    public string? DisplayName => GetValue(SmartSchoolClaims.DisplayName);

    public string? Email => GetValue(SmartSchoolClaims.Email);

    public string? AccountType => GetValue(SmartSchoolClaims.AccountType);

    public bool MustChangePassword => GetBoolean(SmartSchoolClaims.MustChangePassword);

    public IReadOnlyCollection<string> Roles => Principal
        .FindAll(SmartSchoolClaims.Role)
        .Select(claim => claim.Value)
        .Concat(Principal.FindAll(ClaimTypes.Role).Select(claim => claim.Value))
        .Where(role => !string.IsNullOrWhiteSpace(role))
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToArray();

    public bool IsSuperAdmin => IsInRole(SmartSchoolRoles.SuperAdmin);

    public bool IsImpersonated => GetBoolean(SmartSchoolClaims.Impersonated);

    public Guid? ImpersonatorUserId => GetOptionalGuid(SmartSchoolClaims.ImpersonatorSubject);

    public bool IsInRole(string role)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(role);

        return Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
    }

    private string? GetValue(string claimType)
    {
        var value = Principal.FindFirstValue(claimType);

        return string.IsNullOrWhiteSpace(value)
            ? null
            : value;
    }

    private Guid GetRequiredGuid(string claimType)
    {
        var value = GetOptionalGuid(claimType);

        if (!value.HasValue)
        {
            throw new UnauthorizedAccessException(
                $"Required access-token claim '{claimType}' is missing or invalid.");
        }

        return value.Value;
    }

    private Guid? GetOptionalGuid(string claimType)
    {
        var value = GetValue(claimType);

        if (!Guid.TryParse(value, out var id))
        {
            return null;
        }

        return id;
    }

    private bool GetBoolean(string claimType)
    {
        var value = GetValue(claimType);

        return bool.TryParse(value, out var result) && result;
    }
}

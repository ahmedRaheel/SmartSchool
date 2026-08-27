namespace SmartSchool.Application.Identity;

/// <summary>
/// Exposes the authenticated SmartSchool user context carried by the access token.
/// </summary>
public interface ICurrentUser
{
    bool IsAuthenticated { get; }

    Guid UserId { get; }

    Guid? TenantId { get; }

    Guid? SchoolId { get; }

    Guid? BranchId { get; }

    Guid? StudentId { get; }

    Guid? TeacherId { get; }

    Guid? DriverId { get; }

    Guid? ExaminerId { get; }

    Guid? EmployeeId { get; }

    string? FirstName { get; }

    string? LastName { get; }

    string? DisplayName { get; }

    string? Email { get; }

    string? AccountType { get; }

    bool MustChangePassword { get; }

    IReadOnlyCollection<string> Roles { get; }

    bool IsSuperAdmin { get; }

    bool IsImpersonated { get; }

    Guid? ImpersonatorUserId { get; }

    bool IsInRole(string role);
}

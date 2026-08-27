namespace SmartSchool.Application.Identity;

/// <summary>
/// Provides the authenticated SmartSchool business context from access-token claims.
/// </summary>
public interface ICurrentUser
{
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

    string? Email { get; }

    IReadOnlyCollection<string> Roles { get; }

    bool IsAuthenticated { get; }

    bool IsSuperAdmin { get; }

    bool IsImpersonated { get; }

    bool IsInRole(string role);
}

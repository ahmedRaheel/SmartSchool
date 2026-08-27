using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Infrastructure.Identity;

/// <summary>Authenticated business context. Tenant and actor scope come from the access token.</summary>
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
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}

public sealed class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private ClaimsPrincipal User => accessor.HttpContext?.User ?? new ClaimsPrincipal();
    public bool IsAuthenticated => User.Identity?.IsAuthenticated == true;
    public Guid UserId => Required(SmartSchoolClaims.UserId);
    public Guid? TenantId => Optional(SmartSchoolClaims.TenantId);
    public Guid? SchoolId => Optional(SmartSchoolClaims.SchoolId);
    public Guid? BranchId => Optional(SmartSchoolClaims.BranchId);
    public Guid? StudentId => Optional(SmartSchoolClaims.StudentId);
    public Guid? TeacherId => Optional(SmartSchoolClaims.TeacherId);
    public Guid? DriverId => Optional(SmartSchoolClaims.DriverId);
    public Guid? ExaminerId => Optional(SmartSchoolClaims.ExaminerId);
    public Guid? EmployeeId => Optional(SmartSchoolClaims.EmployeeId);
    public string? FirstName => User.FindFirstValue(SmartSchoolClaims.FirstName);
    public string? LastName => User.FindFirstValue(SmartSchoolClaims.LastName);
    public bool IsInRole(string role) => User.IsInRole(role);
    private Guid Required(string type) => Optional(type) ?? throw new UnauthorizedAccessException($"Required identity claim '{type}' is missing.");
    private Guid? Optional(string type) => Guid.TryParse(User.FindFirstValue(type), out var id) ? id : null;
}

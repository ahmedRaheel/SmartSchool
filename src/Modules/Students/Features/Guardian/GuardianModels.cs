namespace SmartSchool.Modules.Students.Features.Guardian;

/// <summary>Request model aligned to student.guardian.</summary>
public sealed record GuardianRequest(Guid TenantId, Guid? UserId, string FullName,
    string? CnicNumber, string? Email, string? Phone);

/// <summary>Guardian response aligned to student.guardian.</summary>
public sealed record GuardianResponse(Guid TenantId, Guid Id, Guid? UserId, string FullName,
    string? CnicNumber, string? Email, string? Phone);

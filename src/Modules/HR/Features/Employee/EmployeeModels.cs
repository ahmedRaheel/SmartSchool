namespace SmartSchool.Modules.HR.Features.Employee;

/// <summary>Request model aligned to hr.employee.</summary>
public sealed record EmployeeRequest(Guid TenantId, Guid? UserId, string? EmployeeNumber,
    string FirstName, string? LastName, string? CnicNumber, byte[]? Photo,
    string? PhotoContentType, string? PhotoFileName, string? Email, string? Phone,
    DateOnly HireDate, string EmploymentTypeCode, string Status, Guid? SourceCandidateId);

/// <summary>Detailed employee response; used by GetById.</summary>
public sealed record EmployeeResponse(Guid TenantId, Guid Id, Guid? UserId, string? EmployeeNumber,
    string FirstName, string? LastName, string? CnicNumber, byte[]? Photo,
    string? PhotoContentType, string? PhotoFileName, string? Email, string? Phone,
    DateOnly HireDate, string EmploymentTypeCode, string Status, Guid? SourceCandidateId);

/// <summary>Lightweight employee response without photograph bytes.</summary>
public sealed record EmployeePageResponse(Guid TenantId, Guid Id, string? EmployeeNumber,
    string FirstName, string? LastName, string? CnicNumber, string? Email, string? Phone,
    DateOnly HireDate, string EmploymentTypeCode, string Status);

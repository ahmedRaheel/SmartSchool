using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Models;

public sealed class EmployeeExperienceEntity : Entity
{
    private EmployeeExperienceEntity() { }
    public Guid EmployeeExperienceId { get; private set; } = Guid.NewGuid();
    public Guid EmployeeId { get; private set; }
    public string Employer { get; private set; } = string.Empty;
    public string JobTitle { get; private set; } = string.Empty;
    public DateOnly StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public string? Responsibilities { get; private set; }
    public static EmployeeExperienceEntity Create(Guid tenantId, Guid employeeId, string employer, string jobTitle, DateOnly startDate, DateOnly? endDate, string? responsibilities) => new()
    { TenantId=tenantId, EmployeeId=employeeId, Employer=employer.Trim(), JobTitle=jobTitle.Trim(), StartDate=startDate, EndDate=endDate, Responsibilities=responsibilities?.Trim() };
}

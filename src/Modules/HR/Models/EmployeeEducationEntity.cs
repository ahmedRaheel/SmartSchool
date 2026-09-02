using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Models;

public sealed class EmployeeEducationEntity : Entity
{
    private EmployeeEducationEntity() { }
    public Guid EmployeeEducationId { get; private set; } = Guid.NewGuid();
    public Guid EmployeeId { get; private set; }
    public string Qualification { get; private set; } = string.Empty;
    public string? Institute { get; private set; }
    public string? FieldOfStudy { get; private set; }
    public DateOnly? StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public string? Grade { get; private set; }
    public bool IsHighest { get; private set; }
    public static EmployeeEducationEntity Create(Guid tenantId, Guid employeeId, string qualification, string? institute, string? fieldOfStudy, DateOnly? startDate, DateOnly? endDate, string? grade, bool isHighest) => new()
    { TenantId=tenantId, EmployeeId=employeeId, Qualification=qualification.Trim(), Institute=institute?.Trim(), FieldOfStudy=fieldOfStudy?.Trim(), StartDate=startDate, EndDate=endDate, Grade=grade?.Trim(), IsHighest=isHighest };
}

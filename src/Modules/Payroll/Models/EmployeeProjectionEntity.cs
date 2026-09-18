namespace SmartSchool.Modules.Payroll.Models;

public sealed class EmployeeProjectionEntity
{
    public Guid EmployeeId { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid BranchId { get; private set; }
    public string? EmployeeNumber { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string? LastName { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private EmployeeProjectionEntity() { }
}

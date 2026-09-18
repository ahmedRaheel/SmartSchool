namespace SmartSchool.Modules.Payroll.Models;

public sealed class JobGradeProjectionEntity
{
    public Guid JobGradeId { get; private set; }
    public Guid TenantId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private JobGradeProjectionEntity() { }
}

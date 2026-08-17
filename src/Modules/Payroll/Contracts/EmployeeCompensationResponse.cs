namespace SmartSchool.Modules.Payroll.Contracts;

public sealed record EmployeeCompensationResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static EmployeeCompensationResponse FromEntity(
        Models.EmployeeCompensation entity)
    {
        return new EmployeeCompensationResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

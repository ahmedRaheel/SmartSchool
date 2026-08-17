namespace SmartSchool.Modules.Payroll.Contracts;

public sealed record PayrollRunResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static PayrollRunResponse FromEntity(
        Models.PayrollRun entity)
    {
        return new PayrollRunResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

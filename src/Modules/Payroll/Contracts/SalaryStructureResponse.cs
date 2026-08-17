namespace SmartSchool.Modules.Payroll.Contracts;

public sealed record SalaryStructureResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static SalaryStructureResponse FromEntity(
        Models.SalaryStructure entity)
    {
        return new SalaryStructureResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

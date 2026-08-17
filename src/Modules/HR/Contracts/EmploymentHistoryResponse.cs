namespace SmartSchool.Modules.HR.Contracts;

public sealed record EmploymentHistoryResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static EmploymentHistoryResponse FromEntity(
        Models.EmploymentHistory entity)
    {
        return new EmploymentHistoryResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

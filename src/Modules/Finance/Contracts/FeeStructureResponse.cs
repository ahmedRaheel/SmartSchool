namespace SmartSchool.Modules.Finance.Contracts;

public sealed record FeeStructureResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static FeeStructureResponse FromEntity(
        Models.FeeStructure entity)
    {
        return new FeeStructureResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

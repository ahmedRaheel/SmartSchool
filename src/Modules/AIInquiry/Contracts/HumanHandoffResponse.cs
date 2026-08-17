namespace SmartSchool.Modules.AIInquiry.Contracts;

public sealed record HumanHandoffResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static HumanHandoffResponse FromEntity(
        Models.HumanHandoff entity)
    {
        return new HumanHandoffResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

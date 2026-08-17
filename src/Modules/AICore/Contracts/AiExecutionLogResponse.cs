namespace SmartSchool.Modules.AICore.Contracts;

public sealed record AiExecutionLogResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static AiExecutionLogResponse FromEntity(
        Models.AiExecutionLog entity)
    {
        return new AiExecutionLogResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

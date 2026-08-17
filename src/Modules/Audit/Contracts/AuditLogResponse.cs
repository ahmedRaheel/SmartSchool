namespace SmartSchool.Modules.Audit.Contracts;

public sealed record AuditLogResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static AuditLogResponse FromEntity(
        Models.AuditLog entity)
    {
        return new AuditLogResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

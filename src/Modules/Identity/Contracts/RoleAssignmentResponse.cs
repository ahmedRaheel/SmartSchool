namespace SmartSchool.Modules.Identity.Contracts;

public sealed record RoleAssignmentResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static RoleAssignmentResponse FromEntity(
        Models.RoleAssignment entity)
    {
        return new RoleAssignmentResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

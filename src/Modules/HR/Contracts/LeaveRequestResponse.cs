namespace SmartSchool.Modules.HR.Contracts;

public sealed record LeaveRequestResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static LeaveRequestResponse FromEntity(
        Models.LeaveRequest entity)
    {
        return new LeaveRequestResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

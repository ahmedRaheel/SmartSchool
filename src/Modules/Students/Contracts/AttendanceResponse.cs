namespace SmartSchool.Modules.Students.Contracts;

public sealed record AttendanceResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static AttendanceResponse FromEntity(
        Models.Attendance entity)
    {
        return new AttendanceResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

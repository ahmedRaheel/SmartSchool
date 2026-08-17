namespace SmartSchool.Modules.Academics.Contracts;

public sealed record TeacherAssignmentResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static TeacherAssignmentResponse FromEntity(
        Models.TeacherAssignment entity)
    {
        return new TeacherAssignmentResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

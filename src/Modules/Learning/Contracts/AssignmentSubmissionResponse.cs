namespace SmartSchool.Modules.Learning.Contracts;

public sealed record AssignmentSubmissionResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static AssignmentSubmissionResponse FromEntity(
        Models.AssignmentSubmission entity)
    {
        return new AssignmentSubmissionResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

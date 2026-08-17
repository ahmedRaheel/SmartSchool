namespace SmartSchool.Modules.AITutor.Contracts;

public sealed record QuizAttemptResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static QuizAttemptResponse FromEntity(
        Models.QuizAttempt entity)
    {
        return new QuizAttemptResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

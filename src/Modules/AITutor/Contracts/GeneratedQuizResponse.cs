namespace SmartSchool.Modules.AITutor.Contracts;

public sealed record GeneratedQuizResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static GeneratedQuizResponse FromEntity(
        Models.GeneratedQuiz entity)
    {
        return new GeneratedQuizResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

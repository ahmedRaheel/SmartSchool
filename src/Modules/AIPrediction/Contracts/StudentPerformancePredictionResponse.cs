namespace SmartSchool.Modules.AIPrediction.Contracts;

public sealed record StudentPerformancePredictionResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static StudentPerformancePredictionResponse FromEntity(
        Models.StudentPerformancePrediction entity)
    {
        return new StudentPerformancePredictionResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

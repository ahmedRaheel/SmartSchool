namespace SmartSchool.Modules.AIPrediction.Contracts;

public sealed record PredictionEvaluationResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static PredictionEvaluationResponse FromEntity(
        Models.PredictionEvaluation entity)
    {
        return new PredictionEvaluationResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

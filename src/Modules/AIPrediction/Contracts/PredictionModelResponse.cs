namespace SmartSchool.Modules.AIPrediction.Contracts;

public sealed record PredictionModelResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static PredictionModelResponse FromEntity(
        Models.PredictionModel entity)
    {
        return new PredictionModelResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

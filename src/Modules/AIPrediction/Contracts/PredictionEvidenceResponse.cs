namespace SmartSchool.Modules.AIPrediction.Contracts;

public sealed record PredictionEvidenceResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static PredictionEvidenceResponse FromEntity(
        Models.PredictionEvidence entity)
    {
        return new PredictionEvidenceResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

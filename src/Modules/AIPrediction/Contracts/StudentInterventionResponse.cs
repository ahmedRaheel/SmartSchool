namespace SmartSchool.Modules.AIPrediction.Contracts;

public sealed record StudentInterventionResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static StudentInterventionResponse FromEntity(
        Models.StudentIntervention entity)
    {
        return new StudentInterventionResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

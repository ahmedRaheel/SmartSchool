namespace SmartSchool.Modules.Admissions.Contracts;

public sealed record AdmissionDecisionResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static AdmissionDecisionResponse FromEntity(
        Models.AdmissionDecision entity)
    {
        return new AdmissionDecisionResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

namespace SmartSchool.Modules.Admissions.Contracts;

public sealed record ApplicantResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static ApplicantResponse FromEntity(
        Models.Applicant entity)
    {
        return new ApplicantResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

namespace SmartSchool.Modules.Admissions.Contracts;

public sealed record InquiryResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static InquiryResponse FromEntity(
        Models.Inquiry entity)
    {
        return new InquiryResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

namespace SmartSchool.Modules.AIInquiry.Contracts;

public sealed record InquiryMessageResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static InquiryMessageResponse FromEntity(
        Models.InquiryMessage entity)
    {
        return new InquiryMessageResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

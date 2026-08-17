namespace SmartSchool.Modules.AIInquiry.Contracts;

public sealed record InquiryConversationResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static InquiryConversationResponse FromEntity(
        Models.InquiryConversation entity)
    {
        return new InquiryConversationResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

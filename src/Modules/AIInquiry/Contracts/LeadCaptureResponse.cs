namespace SmartSchool.Modules.AIInquiry.Contracts;

public sealed record LeadCaptureResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static LeadCaptureResponse FromEntity(
        Models.LeadCapture entity)
    {
        return new LeadCaptureResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

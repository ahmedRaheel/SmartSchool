namespace SmartSchool.Modules.Documents.Contracts;

public sealed record GeneratedDocumentResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static GeneratedDocumentResponse FromEntity(
        Models.GeneratedDocument entity)
    {
        return new GeneratedDocumentResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

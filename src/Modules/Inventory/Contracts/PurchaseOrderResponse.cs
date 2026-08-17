namespace SmartSchool.Modules.Inventory.Contracts;

public sealed record PurchaseOrderResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static PurchaseOrderResponse FromEntity(
        Models.PurchaseOrder entity)
    {
        return new PurchaseOrderResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

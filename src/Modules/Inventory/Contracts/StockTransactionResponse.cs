namespace SmartSchool.Modules.Inventory.Contracts;

public sealed record StockTransactionResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static StockTransactionResponse FromEntity(
        Models.StockTransaction entity)
    {
        return new StockTransactionResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}

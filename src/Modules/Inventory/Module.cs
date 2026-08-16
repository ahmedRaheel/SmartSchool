using FluentValidation;
using SmartSchool.Modules.Inventory.Features.Item;
using SmartSchool.Modules.Inventory.Features.PurchaseOrder;
using SmartSchool.Modules.Inventory.Features.StockTransaction;

namespace SmartSchool.Modules.Inventory;

public static class Module
{
    public static IServiceCollection AddInventoryModule(
        this IServiceCollection services)
    {
        services.AddScoped<CreateItem.Handler>();
        services.AddScoped<GetItemById.Handler>();
        services.AddScoped<GetItemPage.Handler>();
        services.AddScoped<UpdateItem.Handler>();
        services.AddScoped<DeleteItem.Handler>();
        services.AddScoped<IValidator<CreateItem.Request>, CreateItem.Validator>();
        services.AddScoped<IValidator<UpdateItem.Request>, UpdateItem.Validator>();
        services.AddScoped<CreatePurchaseOrder.Handler>();
        services.AddScoped<GetPurchaseOrderById.Handler>();
        services.AddScoped<GetPurchaseOrderPage.Handler>();
        services.AddScoped<UpdatePurchaseOrder.Handler>();
        services.AddScoped<DeletePurchaseOrder.Handler>();
        services.AddScoped<IValidator<CreatePurchaseOrder.Request>, CreatePurchaseOrder.Validator>();
        services.AddScoped<IValidator<UpdatePurchaseOrder.Request>, UpdatePurchaseOrder.Validator>();
        services.AddScoped<CreateStockTransaction.Handler>();
        services.AddScoped<GetStockTransactionById.Handler>();
        services.AddScoped<GetStockTransactionPage.Handler>();
        services.AddScoped<UpdateStockTransaction.Handler>();
        services.AddScoped<DeleteStockTransaction.Handler>();
        services.AddScoped<IValidator<CreateStockTransaction.Request>, CreateStockTransaction.Validator>();
        services.AddScoped<IValidator<UpdateStockTransaction.Request>, UpdateStockTransaction.Validator>();

        return services;
    }

    public static IEndpointRouteBuilder MapInventoryEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateItem.MapEndpoint(endpoints);
        GetItemById.MapEndpoint(endpoints);
        GetItemPage.MapEndpoint(endpoints);
        UpdateItem.MapEndpoint(endpoints);
        DeleteItem.MapEndpoint(endpoints);
        CreatePurchaseOrder.MapEndpoint(endpoints);
        GetPurchaseOrderById.MapEndpoint(endpoints);
        GetPurchaseOrderPage.MapEndpoint(endpoints);
        UpdatePurchaseOrder.MapEndpoint(endpoints);
        DeletePurchaseOrder.MapEndpoint(endpoints);
        CreateStockTransaction.MapEndpoint(endpoints);
        GetStockTransactionById.MapEndpoint(endpoints);
        GetStockTransactionPage.MapEndpoint(endpoints);
        UpdateStockTransaction.MapEndpoint(endpoints);
        DeleteStockTransaction.MapEndpoint(endpoints);

        return endpoints;
    }
}

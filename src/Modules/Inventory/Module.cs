using SmartSchool.Modules.Inventory.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Inventory.Persistence;
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
        services.AddScoped<IItemQuery, ItemQuery>();
        services.AddScoped<IItemCommand, ItemCommand>();
        services.AddScoped<IPurchaseOrderQuery, PurchaseOrderQuery>();
        services.AddScoped<IPurchaseOrderCommand, PurchaseOrderCommand>();
        services.AddScoped<IStockTransactionQuery, StockTransactionQuery>();
        services.AddScoped<IStockTransactionCommand, StockTransactionCommand>();
        services.AddScoped<IValidator<CreateItem.Request>, CreateItem.Validator>();
        services.AddScoped<IValidator<UpdateItem.Request>, UpdateItem.Validator>();
        services.AddScoped<IValidator<CreatePurchaseOrder.Request>, CreatePurchaseOrder.Validator>();
        services.AddScoped<IValidator<UpdatePurchaseOrder.Request>, UpdatePurchaseOrder.Validator>();
        services.AddScoped<IValidator<CreateStockTransaction.Request>, CreateStockTransaction.Validator>();
        services.AddScoped<IValidator<UpdateStockTransaction.Request>, UpdateStockTransaction.Validator>();


        services.AddScoped<IRequestHandler<CreateItem.Request, Result<ItemResponse>>, CreateItem.Handler>();
        services.AddScoped<IRequestHandler<GetItemById.Query, Result<ItemResponse>>, GetItemById.Handler>();
        services.AddScoped<IRequestHandler<GetItemPage.Query, Result<PagedResult<ItemResponse>>>, GetItemPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateItem.Request, Result<ItemResponse>>, UpdateItem.Handler>();
        services.AddScoped<IRequestHandler<DeleteItem.Command, Result<DeleteItem.Response>>, DeleteItem.Handler>();
        services.AddScoped<IRequestHandler<CreatePurchaseOrder.Request, Result<PurchaseOrderResponse>>, CreatePurchaseOrder.Handler>();
        services.AddScoped<IRequestHandler<GetPurchaseOrderById.Query, Result<PurchaseOrderResponse>>, GetPurchaseOrderById.Handler>();
        services.AddScoped<IRequestHandler<GetPurchaseOrderPage.Query, Result<PagedResult<PurchaseOrderResponse>>>, GetPurchaseOrderPage.Handler>();
        services.AddScoped<IRequestHandler<UpdatePurchaseOrder.Request, Result<PurchaseOrderResponse>>, UpdatePurchaseOrder.Handler>();
        services.AddScoped<IRequestHandler<DeletePurchaseOrder.Command, Result<DeletePurchaseOrder.Response>>, DeletePurchaseOrder.Handler>();
        services.AddScoped<IRequestHandler<CreateStockTransaction.Request, Result<StockTransactionResponse>>, CreateStockTransaction.Handler>();
        services.AddScoped<IRequestHandler<GetStockTransactionById.Query, Result<StockTransactionResponse>>, GetStockTransactionById.Handler>();
        services.AddScoped<IRequestHandler<GetStockTransactionPage.Query, Result<PagedResult<StockTransactionResponse>>>, GetStockTransactionPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateStockTransaction.Request, Result<StockTransactionResponse>>, UpdateStockTransaction.Handler>();
        services.AddScoped<IRequestHandler<DeleteStockTransaction.Command, Result<DeleteStockTransaction.Response>>, DeleteStockTransaction.Handler>();

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

using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Inventory.Features.Item;
using SmartSchool.Modules.Inventory.Persistence;
using SmartSchool.SharedKernel;

using SmartSchool.Modules.Inventory.Features.PurchaseOrder;
using SmartSchool.Modules.Inventory.Features.StockTransaction;
namespace SmartSchool.Modules.Inventory;

public static class Module
{
	public static IServiceCollection AddInventoryModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<IItemQuery, ItemQuery>();
		services.AddScoped<IItemCommand, ItemCommand>();
		services.AddScoped<IPurchaseOrderCommand, PurchaseOrderCommand>();
		services.AddScoped<IPurchaseOrderQuery, PurchaseOrderQuery>();
		services.AddScoped<IStockTransactionCommand, StockTransactionCommand>();
		services.AddScoped<IStockTransactionQuery, StockTransactionQuery>();

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
		CreateStockTransaction.MapEndpoint(endpoints);
		DeletePurchaseOrder.MapEndpoint(endpoints);
		DeleteStockTransaction.MapEndpoint(endpoints);
		GetPurchaseOrderById.MapEndpoint(endpoints);
		GetPurchaseOrderPage.MapEndpoint(endpoints);
		GetStockTransactionById.MapEndpoint(endpoints);
		GetStockTransactionPage.MapEndpoint(endpoints);
		UpdatePurchaseOrder.MapEndpoint(endpoints);
		UpdateStockTransaction.MapEndpoint(endpoints);

		return endpoints;
	}
}

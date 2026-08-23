using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Inventory.Features.Item;
using SmartSchool.Modules.Inventory.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Inventory;

public static class Module
{
	public static IServiceCollection AddInventoryModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<IItemQuery, ItemQuery>();
		services.AddScoped<IItemCommand, ItemCommand>();

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

		return endpoints;
	}
}

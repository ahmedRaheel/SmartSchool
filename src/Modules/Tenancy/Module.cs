using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Tenancy.Features.CampusBranding;
using SmartSchool.Modules.Tenancy.Features.Tenant;
using SmartSchool.Modules.Tenancy.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Tenancy;

public static class Module
{
	public static IServiceCollection AddTenancyModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<ICampusBrandingQuery, CampusBrandingQuery>();
		services.AddScoped<ICampusBrandingCommand, CampusBrandingCommand>();
		services.AddScoped<ITenantQuery, TenantQuery>();
		services.AddScoped<ITenantCommand, TenantCommand>();
		services.AddScoped<ISubscriptionCommand, SubscriptionCommand>();
		services.AddScoped<ISubscriptionQuery, SubscriptionQuery>();
		return services;
	}

	public static IEndpointRouteBuilder MapTenancyEndpoints(
		this IEndpointRouteBuilder endpoints)
	{
		CreateCampusBranding.MapEndpoint(endpoints);
		GetCampusBrandingById.MapEndpoint(endpoints);
		GetCampusBrandingPage.MapEndpoint(endpoints);
		UpdateCampusBranding.MapEndpoint(endpoints);
		DeleteCampusBranding.MapEndpoint(endpoints);
		CreateTenant.MapEndpoint(endpoints);
		GetTenantById.MapEndpoint(endpoints);
		GetTenantPage.MapEndpoint(endpoints);
		UpdateTenant.MapEndpoint(endpoints);
		DeleteTenant.MapEndpoint(endpoints);

		return endpoints;
	}
}

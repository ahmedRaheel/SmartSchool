using SmartSchool.Modules.Tenancy.Persistence;
using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Tenancy.Features.CampusBranding;
using SmartSchool.Modules.Tenancy.Features.Tenant;
using SmartSchool.SharedKernel;

using SmartSchool.Modules.Tenancy.Features.Subscription;

namespace SmartSchool.Modules.Tenancy;

public static class Module
{
	public static IServiceCollection AddTenancyModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<ITenancyDbContext, TenancyDbContext>();

        services.AddFeaturePersistence(typeof(Module).Assembly);
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

		CreateSubscription.MapEndpoint(endpoints);
		DeleteSubscription.MapEndpoint(endpoints);
		GetSubscriptionById.MapEndpoint(endpoints);
		GetSubscriptionPage.MapEndpoint(endpoints);
		UpdateSubscription.MapEndpoint(endpoints);

		return endpoints;
	}
}

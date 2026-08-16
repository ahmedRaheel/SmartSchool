using FluentValidation;
using SmartSchool.Modules.Tenancy.Features.CampusBranding;
using SmartSchool.Modules.Tenancy.Features.Subscription;
using SmartSchool.Modules.Tenancy.Features.Tenant;

namespace SmartSchool.Modules.Tenancy;

public static class Module
{
    public static IServiceCollection AddTenancyModule(
        this IServiceCollection services)
    {
        services.AddScoped<CreateCampusBranding.Handler>();
        services.AddScoped<GetCampusBrandingById.Handler>();
        services.AddScoped<GetCampusBrandingPage.Handler>();
        services.AddScoped<UpdateCampusBranding.Handler>();
        services.AddScoped<DeleteCampusBranding.Handler>();
        services.AddScoped<IValidator<CreateCampusBranding.Request>, CreateCampusBranding.Validator>();
        services.AddScoped<IValidator<UpdateCampusBranding.Request>, UpdateCampusBranding.Validator>();
        services.AddScoped<CreateSubscription.Handler>();
        services.AddScoped<GetSubscriptionById.Handler>();
        services.AddScoped<GetSubscriptionPage.Handler>();
        services.AddScoped<UpdateSubscription.Handler>();
        services.AddScoped<DeleteSubscription.Handler>();
        services.AddScoped<IValidator<CreateSubscription.Request>, CreateSubscription.Validator>();
        services.AddScoped<IValidator<UpdateSubscription.Request>, UpdateSubscription.Validator>();
        services.AddScoped<CreateTenant.Handler>();
        services.AddScoped<GetTenantById.Handler>();
        services.AddScoped<GetTenantPage.Handler>();
        services.AddScoped<UpdateTenant.Handler>();
        services.AddScoped<DeleteTenant.Handler>();
        services.AddScoped<IValidator<CreateTenant.Request>, CreateTenant.Validator>();
        services.AddScoped<IValidator<UpdateTenant.Request>, UpdateTenant.Validator>();

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
        CreateSubscription.MapEndpoint(endpoints);
        GetSubscriptionById.MapEndpoint(endpoints);
        GetSubscriptionPage.MapEndpoint(endpoints);
        UpdateSubscription.MapEndpoint(endpoints);
        DeleteSubscription.MapEndpoint(endpoints);
        CreateTenant.MapEndpoint(endpoints);
        GetTenantById.MapEndpoint(endpoints);
        GetTenantPage.MapEndpoint(endpoints);
        UpdateTenant.MapEndpoint(endpoints);
        DeleteTenant.MapEndpoint(endpoints);

        return endpoints;
    }
}

using SmartSchool.Modules.Tenancy.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Tenancy.Persistence;
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
        services.AddScoped<ICampusBrandingQuery, CampusBrandingQuery>();
        services.AddScoped<ICampusBrandingCommand, CampusBrandingCommand>();
        services.AddScoped<ISubscriptionQuery, SubscriptionQuery>();
        services.AddScoped<ISubscriptionCommand, SubscriptionCommand>();
        services.AddScoped<ITenantQuery, TenantQuery>();
        services.AddScoped<ITenantCommand, TenantCommand>();
        services.AddScoped<IValidator<CreateCampusBranding.Request>, CreateCampusBranding.Validator>();
        services.AddScoped<IValidator<UpdateCampusBranding.Request>, UpdateCampusBranding.Validator>();
        services.AddScoped<IValidator<CreateSubscription.Request>, CreateSubscription.Validator>();
        services.AddScoped<IValidator<UpdateSubscription.Request>, UpdateSubscription.Validator>();
        services.AddScoped<IValidator<CreateTenant.Request>, CreateTenant.Validator>();
        services.AddScoped<IValidator<UpdateTenant.Request>, UpdateTenant.Validator>();


        services.AddScoped<IRequestHandler<CreateCampusBranding.Request, Result<CampusBrandingResponse>>, CreateCampusBranding.Handler>();
        services.AddScoped<IRequestHandler<GetCampusBrandingById.Query, Result<CampusBrandingResponse>>, GetCampusBrandingById.Handler>();
        services.AddScoped<IRequestHandler<GetCampusBrandingPage.Query, Result<PagedResult<CampusBrandingResponse>>>, GetCampusBrandingPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateCampusBranding.Request, Result<CampusBrandingResponse>>, UpdateCampusBranding.Handler>();
        services.AddScoped<IRequestHandler<DeleteCampusBranding.Command, Result<DeleteCampusBranding.Response>>, DeleteCampusBranding.Handler>();
        services.AddScoped<IRequestHandler<CreateSubscription.Request, Result<SubscriptionResponse>>, CreateSubscription.Handler>();
        services.AddScoped<IRequestHandler<GetSubscriptionById.Query, Result<SubscriptionResponse>>, GetSubscriptionById.Handler>();
        services.AddScoped<IRequestHandler<GetSubscriptionPage.Query, Result<PagedResult<SubscriptionResponse>>>, GetSubscriptionPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateSubscription.Request, Result<SubscriptionResponse>>, UpdateSubscription.Handler>();
        services.AddScoped<IRequestHandler<DeleteSubscription.Command, Result<DeleteSubscription.Response>>, DeleteSubscription.Handler>();
        services.AddScoped<IRequestHandler<CreateTenant.Request, Result<TenantResponse>>, CreateTenant.Handler>();
        services.AddScoped<IRequestHandler<GetTenantById.Query, Result<TenantResponse>>, GetTenantById.Handler>();
        services.AddScoped<IRequestHandler<GetTenantPage.Query, Result<PagedResult<TenantResponse>>>, GetTenantPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateTenant.Request, Result<TenantResponse>>, UpdateTenant.Handler>();
        services.AddScoped<IRequestHandler<DeleteTenant.Command, Result<DeleteTenant.Response>>, DeleteTenant.Handler>();

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

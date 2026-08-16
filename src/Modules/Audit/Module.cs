using FluentValidation;
using SmartSchool.Modules.Audit.Features.AuditLog;

namespace SmartSchool.Modules.Audit;

public static class Module
{
    public static IServiceCollection AddAuditModule(
        this IServiceCollection services)
    {
        services.AddScoped<CreateAuditLog.Handler>();
        services.AddScoped<GetAuditLogById.Handler>();
        services.AddScoped<GetAuditLogPage.Handler>();
        services.AddScoped<UpdateAuditLog.Handler>();
        services.AddScoped<DeleteAuditLog.Handler>();
        services.AddScoped<IValidator<CreateAuditLog.Request>, CreateAuditLog.Validator>();
        services.AddScoped<IValidator<UpdateAuditLog.Request>, UpdateAuditLog.Validator>();

        return services;
    }

    public static IEndpointRouteBuilder MapAuditEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateAuditLog.MapEndpoint(endpoints);
        GetAuditLogById.MapEndpoint(endpoints);
        GetAuditLogPage.MapEndpoint(endpoints);
        UpdateAuditLog.MapEndpoint(endpoints);
        DeleteAuditLog.MapEndpoint(endpoints);

        return endpoints;
    }
}

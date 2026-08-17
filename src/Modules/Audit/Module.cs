
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Audit.Persistence;

using SmartSchool.Modules.Audit.Features.AuditLog;

namespace SmartSchool.Modules.Audit;

public static class Module
{
    public static IServiceCollection AddAuditModule(
        this IServiceCollection services)
    {
        services.AddScoped<IAuditLogQuery, AuditLogQuery>();
        services.AddScoped<IAuditLogCommand, AuditLogCommand>();

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

using SmartSchool.Modules.Audit.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Audit.Persistence;
using FluentValidation;
using SmartSchool.Modules.Audit.Features.AuditLog;

namespace SmartSchool.Modules.Audit;

public static class Module
{
    public static IServiceCollection AddAuditModule(
        this IServiceCollection services)
    {
        services.AddScoped<IAuditLogQuery, AuditLogQuery>();
        services.AddScoped<IAuditLogCommand, AuditLogCommand>();
        services.AddScoped<IValidator<CreateAuditLog.Request>, CreateAuditLog.Validator>();
        services.AddScoped<IValidator<UpdateAuditLog.Request>, UpdateAuditLog.Validator>();


        services.AddScoped<IRequestHandler<CreateAuditLog.Request, Result<AuditLogResponse>>, CreateAuditLog.Handler>();
        services.AddScoped<IRequestHandler<GetAuditLogById.Query, Result<AuditLogResponse>>, GetAuditLogById.Handler>();
        services.AddScoped<IRequestHandler<GetAuditLogPage.Query, Result<PagedResult<AuditLogResponse>>>, GetAuditLogPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateAuditLog.Request, Result<AuditLogResponse>>, UpdateAuditLog.Handler>();
        services.AddScoped<IRequestHandler<DeleteAuditLog.Command, Result<DeleteAuditLog.Response>>, DeleteAuditLog.Handler>();

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

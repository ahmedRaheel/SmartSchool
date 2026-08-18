using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Audit.Features.AuditLog;
using SmartSchool.Modules.Audit.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Audit;

public static class Module
{
	public static IServiceCollection AddAuditModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
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

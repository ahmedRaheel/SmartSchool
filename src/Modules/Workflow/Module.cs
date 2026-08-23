using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Workflow.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow;

public static class Module
{
	public static IServiceCollection AddWorkflowModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);

		return services;
	}

	public static IEndpointRouteBuilder MapWorkflowEndpoints(
		this IEndpointRouteBuilder endpoints)
	{

		return endpoints;
	}
}

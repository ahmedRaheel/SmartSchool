using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions;

public static class Module
{
	public static IServiceCollection AddAdmissionsModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);

		return services;
	}

	public static IEndpointRouteBuilder MapAdmissionsEndpoints(
		this IEndpointRouteBuilder endpoints)
	{

		return endpoints;
	}
}

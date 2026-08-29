using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Modules.Organization.Features.Campus;
using SmartSchool.Modules.Organization.Features.Department;
using SmartSchool.Modules.Organization.Features.School;
using SmartSchool.Application.Messaging;
using SmartSchool.Application;




public static class Module
{
	public static IServiceCollection AddOrganizationModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);

        services.AddFeaturePersistence(typeof(Module).Assembly);
		return services;
	}

	public static IEndpointRouteBuilder MapOrganizationEndpoints(
		this IEndpointRouteBuilder endpoints)
	{
		CreateCampus.MapEndpoint(endpoints);
		BranchPolicyEndpoints.MapEndpoints(endpoints);
		GetCampusById.MapEndpoint(endpoints);
		GetCampusPage.MapEndpoint(endpoints);
		UpdateCampus.MapEndpoint(endpoints);
		DeleteCampus.MapEndpoint(endpoints);
		CreateDepartment.MapEndpoint(endpoints);
		GetDepartmentById.MapEndpoint(endpoints);
		GetDepartmentPage.MapEndpoint(endpoints);
		UpdateDepartment.MapEndpoint(endpoints);
		DeleteDepartment.MapEndpoint(endpoints);

		CreateSchool.MapEndpoint(endpoints);
		DeleteSchool.MapEndpoint(endpoints);
		GetSchoolById.MapEndpoint(endpoints);
		GetSchoolPage.MapEndpoint(endpoints);
		UpdateSchool.MapEndpoint(endpoints);

		return endpoints;
	}
}


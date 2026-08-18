
using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Features.Campus;
using SmartSchool.Modules.Organization.Features.Department;
using SmartSchool.Modules.Organization.Features.School;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization;

public static class Module
{
    public static IServiceCollection AddOrganizationModule(
        this IServiceCollection services)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);
        services.AddScoped<ICampusQuery, CampusQuery>();
        services.AddScoped<ICampusCommand, CampusCommand>();
        services.AddScoped<IDepartmentQuery, DepartmentQuery>();
        services.AddScoped<IDepartmentCommand, DepartmentCommand>();
        services.AddScoped<ISchoolQuery, SchoolQuery>();
        services.AddScoped<ISchoolCommand, SchoolCommand>();

        return services;
    }

    public static IEndpointRouteBuilder MapOrganizationEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateCampus.MapEndpoint(endpoints);
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
        GetSchoolById.MapEndpoint(endpoints);
        GetSchoolPage.MapEndpoint(endpoints);
        UpdateSchool.MapEndpoint(endpoints);
        DeleteSchool.MapEndpoint(endpoints);

        return endpoints;
    }
}

using FluentValidation;
using SmartSchool.Modules.Organization.Features.Campus;
using SmartSchool.Modules.Organization.Features.Department;
using SmartSchool.Modules.Organization.Features.School;

namespace SmartSchool.Modules.Organization;

public static class Module
{
    public static IServiceCollection AddOrganizationModule(
        this IServiceCollection services)
    {
        services.AddScoped<CreateCampus.Handler>();
        services.AddScoped<GetCampusById.Handler>();
        services.AddScoped<GetCampusPage.Handler>();
        services.AddScoped<UpdateCampus.Handler>();
        services.AddScoped<DeleteCampus.Handler>();
        services.AddScoped<IValidator<CreateCampus.Request>, CreateCampus.Validator>();
        services.AddScoped<IValidator<UpdateCampus.Request>, UpdateCampus.Validator>();
        services.AddScoped<CreateDepartment.Handler>();
        services.AddScoped<GetDepartmentById.Handler>();
        services.AddScoped<GetDepartmentPage.Handler>();
        services.AddScoped<UpdateDepartment.Handler>();
        services.AddScoped<DeleteDepartment.Handler>();
        services.AddScoped<IValidator<CreateDepartment.Request>, CreateDepartment.Validator>();
        services.AddScoped<IValidator<UpdateDepartment.Request>, UpdateDepartment.Validator>();
        services.AddScoped<CreateSchool.Handler>();
        services.AddScoped<GetSchoolById.Handler>();
        services.AddScoped<GetSchoolPage.Handler>();
        services.AddScoped<UpdateSchool.Handler>();
        services.AddScoped<DeleteSchool.Handler>();
        services.AddScoped<IValidator<CreateSchool.Request>, CreateSchool.Validator>();
        services.AddScoped<IValidator<UpdateSchool.Request>, UpdateSchool.Validator>();

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

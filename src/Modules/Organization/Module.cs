using SmartSchool.Modules.Organization.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Persistence;
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
        services.AddScoped<ICampusQuery, CampusQuery>();
        services.AddScoped<ICampusCommand, CampusCommand>();
        services.AddScoped<IDepartmentQuery, DepartmentQuery>();
        services.AddScoped<IDepartmentCommand, DepartmentCommand>();
        services.AddScoped<ISchoolQuery, SchoolQuery>();
        services.AddScoped<ISchoolCommand, SchoolCommand>();
        services.AddScoped<IValidator<CreateCampus.Request>, CreateCampus.Validator>();
        services.AddScoped<IValidator<UpdateCampus.Request>, UpdateCampus.Validator>();
        services.AddScoped<IValidator<CreateDepartment.Request>, CreateDepartment.Validator>();
        services.AddScoped<IValidator<UpdateDepartment.Request>, UpdateDepartment.Validator>();
        services.AddScoped<IValidator<CreateSchool.Request>, CreateSchool.Validator>();
        services.AddScoped<IValidator<UpdateSchool.Request>, UpdateSchool.Validator>();


        services.AddScoped<IRequestHandler<CreateCampus.Request, Result<CampusResponse>>, CreateCampus.Handler>();
        services.AddScoped<IRequestHandler<GetCampusById.Query, Result<CampusResponse>>, GetCampusById.Handler>();
        services.AddScoped<IRequestHandler<GetCampusPage.Query, Result<PagedResult<CampusResponse>>>, GetCampusPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateCampus.Request, Result<CampusResponse>>, UpdateCampus.Handler>();
        services.AddScoped<IRequestHandler<DeleteCampus.Command, Result<DeleteCampus.Response>>, DeleteCampus.Handler>();
        services.AddScoped<IRequestHandler<CreateDepartment.Request, Result<DepartmentResponse>>, CreateDepartment.Handler>();
        services.AddScoped<IRequestHandler<GetDepartmentById.Query, Result<DepartmentResponse>>, GetDepartmentById.Handler>();
        services.AddScoped<IRequestHandler<GetDepartmentPage.Query, Result<PagedResult<DepartmentResponse>>>, GetDepartmentPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateDepartment.Request, Result<DepartmentResponse>>, UpdateDepartment.Handler>();
        services.AddScoped<IRequestHandler<DeleteDepartment.Command, Result<DeleteDepartment.Response>>, DeleteDepartment.Handler>();
        services.AddScoped<IRequestHandler<CreateSchool.Request, Result<SchoolResponse>>, CreateSchool.Handler>();
        services.AddScoped<IRequestHandler<GetSchoolById.Query, Result<SchoolResponse>>, GetSchoolById.Handler>();
        services.AddScoped<IRequestHandler<GetSchoolPage.Query, Result<PagedResult<SchoolResponse>>>, GetSchoolPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateSchool.Request, Result<SchoolResponse>>, UpdateSchool.Handler>();
        services.AddScoped<IRequestHandler<DeleteSchool.Command, Result<DeleteSchool.Response>>, DeleteSchool.Handler>();

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

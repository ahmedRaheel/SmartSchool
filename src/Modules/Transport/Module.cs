using FluentValidation;
using SmartSchool.Modules.Transport.Features.Route;
using SmartSchool.Modules.Transport.Features.Stop;
using SmartSchool.Modules.Transport.Features.StudentTransport;
using SmartSchool.Modules.Transport.Features.Vehicle;

namespace SmartSchool.Modules.Transport;

public static class Module
{
    public static IServiceCollection AddTransportModule(
        this IServiceCollection services)
    {
        services.AddScoped<CreateRoute.Handler>();
        services.AddScoped<GetRouteById.Handler>();
        services.AddScoped<GetRoutePage.Handler>();
        services.AddScoped<UpdateRoute.Handler>();
        services.AddScoped<DeleteRoute.Handler>();
        services.AddScoped<IValidator<CreateRoute.Request>, CreateRoute.Validator>();
        services.AddScoped<IValidator<UpdateRoute.Request>, UpdateRoute.Validator>();
        services.AddScoped<CreateStop.Handler>();
        services.AddScoped<GetStopById.Handler>();
        services.AddScoped<GetStopPage.Handler>();
        services.AddScoped<UpdateStop.Handler>();
        services.AddScoped<DeleteStop.Handler>();
        services.AddScoped<IValidator<CreateStop.Request>, CreateStop.Validator>();
        services.AddScoped<IValidator<UpdateStop.Request>, UpdateStop.Validator>();
        services.AddScoped<CreateStudentTransport.Handler>();
        services.AddScoped<GetStudentTransportById.Handler>();
        services.AddScoped<GetStudentTransportPage.Handler>();
        services.AddScoped<UpdateStudentTransport.Handler>();
        services.AddScoped<DeleteStudentTransport.Handler>();
        services.AddScoped<IValidator<CreateStudentTransport.Request>, CreateStudentTransport.Validator>();
        services.AddScoped<IValidator<UpdateStudentTransport.Request>, UpdateStudentTransport.Validator>();
        services.AddScoped<CreateVehicle.Handler>();
        services.AddScoped<GetVehicleById.Handler>();
        services.AddScoped<GetVehiclePage.Handler>();
        services.AddScoped<UpdateVehicle.Handler>();
        services.AddScoped<DeleteVehicle.Handler>();
        services.AddScoped<IValidator<CreateVehicle.Request>, CreateVehicle.Validator>();
        services.AddScoped<IValidator<UpdateVehicle.Request>, UpdateVehicle.Validator>();

        return services;
    }

    public static IEndpointRouteBuilder MapTransportEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateRoute.MapEndpoint(endpoints);
        GetRouteById.MapEndpoint(endpoints);
        GetRoutePage.MapEndpoint(endpoints);
        UpdateRoute.MapEndpoint(endpoints);
        DeleteRoute.MapEndpoint(endpoints);
        CreateStop.MapEndpoint(endpoints);
        GetStopById.MapEndpoint(endpoints);
        GetStopPage.MapEndpoint(endpoints);
        UpdateStop.MapEndpoint(endpoints);
        DeleteStop.MapEndpoint(endpoints);
        CreateStudentTransport.MapEndpoint(endpoints);
        GetStudentTransportById.MapEndpoint(endpoints);
        GetStudentTransportPage.MapEndpoint(endpoints);
        UpdateStudentTransport.MapEndpoint(endpoints);
        DeleteStudentTransport.MapEndpoint(endpoints);
        CreateVehicle.MapEndpoint(endpoints);
        GetVehicleById.MapEndpoint(endpoints);
        GetVehiclePage.MapEndpoint(endpoints);
        UpdateVehicle.MapEndpoint(endpoints);
        DeleteVehicle.MapEndpoint(endpoints);

        return endpoints;
    }
}

using SmartSchool.Modules.Transport.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Transport.Persistence;
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
        services.AddScoped<IRouteQuery, RouteQuery>();
        services.AddScoped<IRouteCommand, RouteCommand>();
        services.AddScoped<IStopQuery, StopQuery>();
        services.AddScoped<IStopCommand, StopCommand>();
        services.AddScoped<IStudentTransportQuery, StudentTransportQuery>();
        services.AddScoped<IStudentTransportCommand, StudentTransportCommand>();
        services.AddScoped<IVehicleQuery, VehicleQuery>();
        services.AddScoped<IVehicleCommand, VehicleCommand>();
        services.AddScoped<IValidator<CreateRoute.Request>, CreateRoute.Validator>();
        services.AddScoped<IValidator<UpdateRoute.Request>, UpdateRoute.Validator>();
        services.AddScoped<IValidator<CreateStop.Request>, CreateStop.Validator>();
        services.AddScoped<IValidator<UpdateStop.Request>, UpdateStop.Validator>();
        services.AddScoped<IValidator<CreateStudentTransport.Request>, CreateStudentTransport.Validator>();
        services.AddScoped<IValidator<UpdateStudentTransport.Request>, UpdateStudentTransport.Validator>();
        services.AddScoped<IValidator<CreateVehicle.Request>, CreateVehicle.Validator>();
        services.AddScoped<IValidator<UpdateVehicle.Request>, UpdateVehicle.Validator>();


        services.AddScoped<IRequestHandler<CreateRoute.Request, Result<RouteResponse>>, CreateRoute.Handler>();
        services.AddScoped<IRequestHandler<GetRouteById.Query, Result<RouteResponse>>, GetRouteById.Handler>();
        services.AddScoped<IRequestHandler<GetRoutePage.Query, Result<PagedResult<RouteResponse>>>, GetRoutePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateRoute.Request, Result<RouteResponse>>, UpdateRoute.Handler>();
        services.AddScoped<IRequestHandler<DeleteRoute.Command, Result<DeleteRoute.Response>>, DeleteRoute.Handler>();
        services.AddScoped<IRequestHandler<CreateStop.Request, Result<StopResponse>>, CreateStop.Handler>();
        services.AddScoped<IRequestHandler<GetStopById.Query, Result<StopResponse>>, GetStopById.Handler>();
        services.AddScoped<IRequestHandler<GetStopPage.Query, Result<PagedResult<StopResponse>>>, GetStopPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateStop.Request, Result<StopResponse>>, UpdateStop.Handler>();
        services.AddScoped<IRequestHandler<DeleteStop.Command, Result<DeleteStop.Response>>, DeleteStop.Handler>();
        services.AddScoped<IRequestHandler<CreateStudentTransport.Request, Result<StudentTransportResponse>>, CreateStudentTransport.Handler>();
        services.AddScoped<IRequestHandler<GetStudentTransportById.Query, Result<StudentTransportResponse>>, GetStudentTransportById.Handler>();
        services.AddScoped<IRequestHandler<GetStudentTransportPage.Query, Result<PagedResult<StudentTransportResponse>>>, GetStudentTransportPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateStudentTransport.Request, Result<StudentTransportResponse>>, UpdateStudentTransport.Handler>();
        services.AddScoped<IRequestHandler<DeleteStudentTransport.Command, Result<DeleteStudentTransport.Response>>, DeleteStudentTransport.Handler>();
        services.AddScoped<IRequestHandler<CreateVehicle.Request, Result<VehicleResponse>>, CreateVehicle.Handler>();
        services.AddScoped<IRequestHandler<GetVehicleById.Query, Result<VehicleResponse>>, GetVehicleById.Handler>();
        services.AddScoped<IRequestHandler<GetVehiclePage.Query, Result<PagedResult<VehicleResponse>>>, GetVehiclePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateVehicle.Request, Result<VehicleResponse>>, UpdateVehicle.Handler>();
        services.AddScoped<IRequestHandler<DeleteVehicle.Command, Result<DeleteVehicle.Response>>, DeleteVehicle.Handler>();

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

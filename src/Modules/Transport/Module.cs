using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Transport.Features.Route;
using SmartSchool.Modules.Transport.Features.Stop;
using SmartSchool.Modules.Transport.Features.StudentTransport;
using SmartSchool.Modules.Transport.Features.Vehicle;
using SmartSchool.Modules.Transport.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Transport;

public static class Module
{
	public static IServiceCollection AddTransportModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<IRouteQuery, RouteQuery>();
		services.AddScoped<IRouteCommand, RouteCommand>();
		services.AddScoped<IStopQuery, StopQuery>();
		services.AddScoped<IStopCommand, StopCommand>();
		services.AddScoped<IStudentTransportQuery, StudentTransportQuery>();
		services.AddScoped<IStudentTransportCommand, StudentTransportCommand>();
		services.AddScoped<IVehicleQuery, VehicleQuery>();
		services.AddScoped<IVehicleCommand, VehicleCommand>();

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

using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Transport.Features.Route;
using SmartSchool.Modules.Transport.Features.Vehicle;
using SmartSchool.SharedKernel;

using SmartSchool.Modules.Transport.Features.Stop;
using SmartSchool.Modules.Transport.Features.StudentTransport;
namespace SmartSchool.Modules.Transport;

public static class Module
{
	public static IServiceCollection AddTransportModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);

        services.AddFeaturePersistence(typeof(Module).Assembly);
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
		CreateVehicle.MapEndpoint(endpoints);
		GetVehicleById.MapEndpoint(endpoints);
		GetVehiclePage.MapEndpoint(endpoints);
		UpdateVehicle.MapEndpoint(endpoints);
		DeleteVehicle.MapEndpoint(endpoints);

		CreateStop.MapEndpoint(endpoints);
		CreateStudentTransport.MapEndpoint(endpoints);
		DeleteStop.MapEndpoint(endpoints);
		DeleteStudentTransport.MapEndpoint(endpoints);
		GetStopById.MapEndpoint(endpoints);
		GetStopPage.MapEndpoint(endpoints);
		GetStudentTransportById.MapEndpoint(endpoints);
		GetStudentTransportPage.MapEndpoint(endpoints);
		UpdateStop.MapEndpoint(endpoints);
		UpdateStudentTransport.MapEndpoint(endpoints);

		return endpoints;
	}
}

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Reference.Features.Lookups;
using SmartSchool.Modules.Reference.Persistence;

namespace SmartSchool.Modules.Reference;

public static class Module
{
	public static IServiceCollection AddReferenceModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<
			IReferenceDbContext,
			ReferenceDbContext>();

		services.AddFeaturePersistence(
			typeof(Module).Assembly);

		return services;
	}

	public static IEndpointRouteBuilder MapReferenceEndpoints(
		this IEndpointRouteBuilder endpoints)
	{
		GetLookupTypes.MapEndpoint(endpoints);
		GetLookupValues.MapEndpoint(endpoints);
		GetAllLookups.MapEndpoint(endpoints);
		GetGeography.MapEndpoint(endpoints);
		CreateLookup.MapEndpoint(endpoints);
		UpdateLookup.MapEndpoint(endpoints);
		DeleteLookup.MapEndpoint(endpoints);

		return endpoints;
	}
}

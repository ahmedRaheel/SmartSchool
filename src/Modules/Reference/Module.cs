using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Reference.Features.Lookups;


namespace SmartSchool.Modules.Reference;

public static class Module
{
	public static IServiceCollection AddReferenceModule(this IServiceCollection services)
	{
		services.AddFeatureDataAccess(typeof(Module).Assembly);
		return services;
	}

	public static IEndpointRouteBuilder MapReferenceEndpoints(this IEndpointRouteBuilder endpoints)
	{
		GetLookupTypes.MapEndpoint(endpoints);
		GetLookupValues.MapEndpoint(endpoints);
		GetAllLookups.MapEndpoint(endpoints);
		GetGeography.MapEndpoint(endpoints);
		return endpoints;
	}
}

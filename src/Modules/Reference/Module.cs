using SmartSchool.Modules.Reference.Features.Lookups;
using SmartSchool.Modules.Reference.Persistence;

namespace SmartSchool.Modules.Reference;

public static class Module
{
	public static IServiceCollection AddReferenceModule(this IServiceCollection services)
	{
		services.AddScoped<ILookupQuery, LookupQuery>();
		return services;
	}

	public static IEndpointRouteBuilder MapReferenceEndpoints(this IEndpointRouteBuilder endpoints)
	{
		GetLookupTypes.MapEndpoint(endpoints);
		GetLookupValues.MapEndpoint(endpoints);
		GetAllLookups.MapEndpoint(endpoints);
		return endpoints;
	}
}

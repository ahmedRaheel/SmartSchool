using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Reference.Features.Lookups;
using SmartSchool.Modules.Reference.Persistence;

namespace SmartSchool.Modules.Reference;

public static class Module
{
    public static IServiceCollection AddReferenceModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);
        services.AddModuleDbContext<ReferenceDbContext, IReferenceDbContext>(
            configuration,
            ModuleConstants.Schema);

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
        GetCountries.MapEndpoint(endpoints);
        GetProvinces.MapEndpoint(endpoints);
        GetCities.MapEndpoint(endpoints);
        CreateLookup.MapEndpoint(endpoints);
        UpdateLookup.MapEndpoint(endpoints);
        DeleteLookup.MapEndpoint(endpoints);

        return endpoints;
    }
}

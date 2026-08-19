using SmartSchool.Modules.Reference.Persistence;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class GetAllLookups
{
	public static void MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet("/api/lookups",
			async (ILookupQuery query, CancellationToken cancellationToken) =>
				Results.Ok(await query.GetAllAsync(cancellationToken)))
			.WithTags("Lookups")
			.WithName("GetAllLookups");
	}
}

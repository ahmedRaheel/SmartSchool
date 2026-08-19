using SmartSchool.Modules.Reference.Persistence;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class GetLookupTypes
{
	public static void MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet("/api/lookups/types",
			async (ILookupQuery query, CancellationToken cancellationToken) =>
				Results.Ok(await query.GetTypesAsync(cancellationToken)))
			.WithTags("Lookups")
			.WithName("GetLookupTypes");
	}
}

using SmartSchool.Modules.Reference.Persistence;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class GetLookupValues
{
	public static void MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet("/api/lookups/{typeCode}",
			async (string typeCode, ILookupQuery query, CancellationToken cancellationToken) =>
				Results.Ok(await query.GetValuesAsync(typeCode, cancellationToken)))
			.WithTags("Lookups")
			.WithName("GetLookupValues");
	}
}

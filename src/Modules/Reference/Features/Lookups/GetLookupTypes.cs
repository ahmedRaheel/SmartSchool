using SmartSchool.Application.Http;
using SmartSchool.SharedKernel;
using SmartSchool.Modules.Reference.Persistence;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class GetLookupTypes
{
	public static void MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet("/api/lookups/types",
			async (ILookupQuery query, CancellationToken cancellationToken) =>
			{
				var lookupTypes = await query.GetTypesAsync(cancellationToken);
				return Result<object>.Success(lookupTypes).ToHttpResult();
			})
			.WithTags("Lookups")
			.WithName("GetLookupTypes");
	}
}

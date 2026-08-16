using SmartSchool.Modules.Examinations;
using SmartSchool.Modules.Examinations.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Examinations.Features.GradeScale;

public static class GetGradeScalePage
{
    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25);

    public sealed class Handler(
        IGradeScaleQuery query)
    {
        public async Task<Result<PagedResult<GradeScale>>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(
                query.Page,
                query.PageSize);

            var result = await query.GetPageAsync(
                query.TenantId,
                pageRequest.NormalizedPage,
                pageRequest.NormalizedPageSize,
                cancellationToken);

            return Result<PagedResult<GradeScale>>.Success(result);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "grade-scale"),
                async (
                    Guid tenantId,
                    int page,
                    int pageSize,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var query = new Query(
                        tenantId,
                        page,
                        pageSize);

                    var result = await handler.HandleAsync(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetGradeScalePage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}

using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Contracts;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.ClassPerformanceInsight;

public static class GetClassPerformanceInsightById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ClassPerformanceInsightResponse>>;

    public sealed class Handler(IClassPerformanceInsightQuery entityQuery)
        : IRequestHandler<Query, Result<ClassPerformanceInsightResponse>>
    {
        public async Task<Result<ClassPerformanceInsightResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ClassPerformanceInsightResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(ClassPerformanceInsight))));
            }
            return Result<ClassPerformanceInsightResponse>.Success(ClassPerformanceInsightResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "class-performance-insight"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ClassPerformanceInsightResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetClassPerformanceInsightById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

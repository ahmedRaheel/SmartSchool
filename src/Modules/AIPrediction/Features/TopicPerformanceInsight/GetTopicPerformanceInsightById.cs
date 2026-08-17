using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Contracts;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.TopicPerformanceInsight;

public static class GetTopicPerformanceInsightById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<TopicPerformanceInsightResponse>>;

    public sealed class Handler(ITopicPerformanceInsightQuery entityQuery)
        : IRequestHandler<Query, Result<TopicPerformanceInsightResponse>>
    {
        public async Task<Result<TopicPerformanceInsightResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<TopicPerformanceInsightResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(TopicPerformanceInsight))));
            }
            return Result<TopicPerformanceInsightResponse>.Success(TopicPerformanceInsightResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "topic-performance-insight"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<TopicPerformanceInsightResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetTopicPerformanceInsightById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

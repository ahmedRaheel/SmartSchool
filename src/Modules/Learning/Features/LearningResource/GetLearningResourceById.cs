using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Learning.Contracts;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.Modules.Learning.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Learning.Features.LearningResource;

public static class GetLearningResourceById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<LearningResourceResponse>>;

    public sealed class Handler(ILearningResourceQuery entityQuery)
        : IRequestHandler<Query, Result<LearningResourceResponse>>
    {
        public async Task<Result<LearningResourceResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<LearningResourceResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(LearningResource))));
            }
            return Result<LearningResourceResponse>.Success(LearningResourceResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "learning-resource"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<LearningResourceResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetLearningResourceById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

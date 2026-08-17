using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Contracts;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.Modules.AITutor.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AITutor.Features.TutorMessage;

public static class GetTutorMessageById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<TutorMessageResponse>>;

    public sealed class Handler(ITutorMessageQuery entityQuery)
        : IRequestHandler<Query, Result<TutorMessageResponse>>
    {
        public async Task<Result<TutorMessageResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<TutorMessageResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(TutorMessage))));
            }
            return Result<TutorMessageResponse>.Success(TutorMessageResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "tutor-message"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<TutorMessageResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetTutorMessageById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

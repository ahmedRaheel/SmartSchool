using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Contracts;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.Modules.AITutor.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AITutor.Features.TutorConversation;

public static class GetTutorConversationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<TutorConversationResponse>>;

    public sealed class Handler(ITutorConversationQuery entityQuery)
        : IRequestHandler<Query, Result<TutorConversationResponse>>
    {
        public async Task<Result<TutorConversationResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<TutorConversationResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(TutorConversation))));
            }
            return Result<TutorConversationResponse>.Success(TutorConversationResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "tutor-conversation"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<TutorConversationResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetTutorConversationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

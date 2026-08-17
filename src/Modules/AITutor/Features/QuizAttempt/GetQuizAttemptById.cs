using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Contracts;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.Modules.AITutor.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AITutor.Features.QuizAttempt;

public static class GetQuizAttemptById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<QuizAttemptResponse>>;

    public sealed class Handler(IQuizAttemptQuery entityQuery)
        : IRequestHandler<Query, Result<QuizAttemptResponse>>
    {
        public async Task<Result<QuizAttemptResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<QuizAttemptResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(QuizAttempt))));
            }
            return Result<QuizAttemptResponse>.Success(QuizAttemptResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "quiz-attempt"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<QuizAttemptResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetQuizAttemptById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

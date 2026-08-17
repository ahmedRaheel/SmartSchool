using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Contracts;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.Modules.AITutor.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AITutor.Features.GeneratedQuiz;

public static class GetGeneratedQuizById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<GeneratedQuizResponse>>;

    public sealed class Handler(IGeneratedQuizQuery entityQuery)
        : IRequestHandler<Query, Result<GeneratedQuizResponse>>
    {
        public async Task<Result<GeneratedQuizResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<GeneratedQuizResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(GeneratedQuiz))));
            }
            return Result<GeneratedQuizResponse>.Success(GeneratedQuizResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "generated-quiz"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<GeneratedQuizResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetGeneratedQuizById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Features.GeneratedQuiz;

public static class GetGeneratedQuizById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<GeneratedQuiz> repository)
    {
        public async Task<Result<GeneratedQuiz>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<GeneratedQuiz>.Failure(
                    Error.NotFound("GeneratedQuiz was not found."));
            }

            return Result<GeneratedQuiz>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/aitutor/generated-quiz/{id:guid}",
                async (
                    Guid id,
                    Guid tenantId,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var query = new Query(tenantId, id);

                    var result = await handler.HandleAsync(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetGeneratedQuizById")
            .WithTags("AITutor")
            .RequireAuthorization();

        return endpoints;
    }
}

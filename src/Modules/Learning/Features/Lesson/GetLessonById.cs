using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Learning.Features.Lesson;

public static class GetLessonById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<Lesson> repository)
    {
        public async Task<Result<Lesson>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<Lesson>.Failure(
                    Error.NotFound("Lesson was not found."));
            }

            return Result<Lesson>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/learning/lesson/{id:guid}",
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
            .WithName("GetLessonById")
            .WithTags("Learning")
            .RequireAuthorization();

        return endpoints;
    }
}

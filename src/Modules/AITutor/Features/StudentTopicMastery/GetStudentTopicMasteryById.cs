using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Features.StudentTopicMastery;

public static class GetStudentTopicMasteryById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<StudentTopicMastery> repository)
    {
        public async Task<Result<StudentTopicMastery>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<StudentTopicMastery>.Failure(
                    Error.NotFound("StudentTopicMastery was not found."));
            }

            return Result<StudentTopicMastery>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/aitutor/student-topic-mastery/{id:guid}",
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
            .WithName("GetStudentTopicMasteryById")
            .WithTags("AITutor")
            .RequireAuthorization();

        return endpoints;
    }
}

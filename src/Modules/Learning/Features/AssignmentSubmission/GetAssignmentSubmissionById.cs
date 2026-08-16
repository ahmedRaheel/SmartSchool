using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Learning.Features.AssignmentSubmission;

public static class GetAssignmentSubmissionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<AssignmentSubmission> repository)
    {
        public async Task<Result<AssignmentSubmission>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<AssignmentSubmission>.Failure(
                    Error.NotFound("AssignmentSubmission was not found."));
            }

            return Result<AssignmentSubmission>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/learning/assignment-submission/{id:guid}",
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
            .WithName("GetAssignmentSubmissionById")
            .WithTags("Learning")
            .RequireAuthorization();

        return endpoints;
    }
}

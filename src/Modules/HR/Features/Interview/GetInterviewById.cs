using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Features.Interview;

public static class GetInterviewById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<Interview> repository)
    {
        public async Task<Result<Interview>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<Interview>.Failure(
                    Error.NotFound("Interview was not found."));
            }

            return Result<Interview>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/hr/interview/{id:guid}",
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
            .WithName("GetInterviewById")
            .WithTags("HR")
            .RequireAuthorization();

        return endpoints;
    }
}

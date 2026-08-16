using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Features.Resume;

public static class GetResumeById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<Resume> repository)
    {
        public async Task<Result<Resume>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<Resume>.Failure(
                    Error.NotFound("Resume was not found."));
            }

            return Result<Resume>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/hr/resume/{id:guid}",
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
            .WithName("GetResumeById")
            .WithTags("HR")
            .RequireAuthorization();

        return endpoints;
    }
}

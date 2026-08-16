using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIParent.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIParent.Features.ParentToolExecution;

public static class GetParentToolExecutionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<ParentToolExecution> repository)
    {
        public async Task<Result<ParentToolExecution>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<ParentToolExecution>.Failure(
                    Error.NotFound("ParentToolExecution was not found."));
            }

            return Result<ParentToolExecution>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/aiparent/parent-tool-execution/{id:guid}",
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
            .WithName("GetParentToolExecutionById")
            .WithTags("AIParent")
            .RequireAuthorization();

        return endpoints;
    }
}

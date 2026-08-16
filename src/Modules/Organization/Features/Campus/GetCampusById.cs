using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Features.Campus;

public static class GetCampusById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<Campus> repository)
    {
        public async Task<Result<Campus>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<Campus>.Failure(
                    Error.NotFound("Campus was not found."));
            }

            return Result<Campus>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/organization/campus/{id:guid}",
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
            .WithName("GetCampusById")
            .WithTags("Organization")
            .RequireAuthorization();

        return endpoints;
    }
}

using SmartSchool.Modules.Organization;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Campus;

public static class GetCampusById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        ICampusQuery query)
    {
        public async Task<Result<Campus>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<Campus>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Campus))));
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
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}

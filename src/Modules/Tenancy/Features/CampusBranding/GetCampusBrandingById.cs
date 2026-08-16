using SmartSchool.Modules.Tenancy;
using SmartSchool.Modules.Tenancy.Persistence;
using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Tenancy.Features.CampusBranding;

public static class GetCampusBrandingById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        ICampusBrandingQuery query)
    {
        public async Task<Result<CampusBranding>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<CampusBranding>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(CampusBranding))));
            }

            return Result<CampusBranding>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/tenancy/campus-branding/{id:guid}",
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
            .WithName("GetCampusBrandingById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}

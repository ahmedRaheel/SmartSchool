using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Tenancy.Contracts;
using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.Modules.Tenancy.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Tenancy.Features.CampusBranding;

public static class GetCampusBrandingById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<CampusBrandingResponse>>;

    public sealed class Handler(ICampusBrandingQuery entityQuery)
        : IRequestHandler<Query, Result<CampusBrandingResponse>>
    {
        public async Task<Result<CampusBrandingResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<CampusBrandingResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(CampusBranding))));
            }
            return Result<CampusBrandingResponse>.Success(CampusBrandingResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "campus-branding"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<CampusBrandingResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetCampusBrandingById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

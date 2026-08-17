using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Contracts;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Campus;

public static class GetCampusById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<CampusResponse>>;

    public sealed class Handler(ICampusQuery entityQuery)
        : IRequestHandler<Query, Result<CampusResponse>>
    {
        public async Task<Result<CampusResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<CampusResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Campus))));
            }
            return Result<CampusResponse>.Success(CampusResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "campus"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<CampusResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetCampusById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

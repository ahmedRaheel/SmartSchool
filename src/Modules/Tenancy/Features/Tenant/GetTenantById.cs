using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Tenancy.Contracts;
using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.Modules.Tenancy.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Tenancy.Features.Tenant;

public static class GetTenantById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<TenantResponse>>;

    public sealed class Handler(ITenantQuery entityQuery)
        : IRequestHandler<Query, Result<TenantResponse>>
    {
        public async Task<Result<TenantResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<TenantResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Tenant))));
            }
            return Result<TenantResponse>.Success(TenantResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "tenant"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<TenantResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetTenantById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

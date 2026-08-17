using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Activities.Contracts;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.Award;

public static class GetAwardById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<AwardResponse>>;

    public sealed class Handler(IAwardQuery entityQuery)
        : IRequestHandler<Query, Result<AwardResponse>>
    {
        public async Task<Result<AwardResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<AwardResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Award))));
            }
            return Result<AwardResponse>.Success(AwardResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "award"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<AwardResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetAwardById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

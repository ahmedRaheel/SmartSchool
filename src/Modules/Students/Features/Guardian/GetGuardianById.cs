using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Contracts;
using SmartSchool.Modules.Students.Models;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Guardian;

public static class GetGuardianById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<GuardianResponse>>;

    public sealed class Handler(IGuardianQuery entityQuery)
        : IRequestHandler<Query, Result<GuardianResponse>>
    {
        public async Task<Result<GuardianResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<GuardianResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Guardian))));
            }
            return Result<GuardianResponse>.Success(GuardianResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "guardian"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<GuardianResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetGuardianById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

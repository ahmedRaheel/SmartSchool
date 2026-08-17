using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Identity.Contracts;
using SmartSchool.Modules.Identity.Models;
using SmartSchool.Modules.Identity.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Identity.Features.RoleAssignment;

public static class GetRoleAssignmentById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<RoleAssignmentResponse>>;

    public sealed class Handler(IRoleAssignmentQuery entityQuery)
        : IRequestHandler<Query, Result<RoleAssignmentResponse>>
    {
        public async Task<Result<RoleAssignmentResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<RoleAssignmentResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(RoleAssignment))));
            }
            return Result<RoleAssignmentResponse>.Success(RoleAssignmentResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "role-assignment"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<RoleAssignmentResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetRoleAssignmentById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

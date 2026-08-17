using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Contracts;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.LeaveRequest;

public static class GetLeaveRequestById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<LeaveRequestResponse>>;

    public sealed class Handler(ILeaveRequestQuery entityQuery)
        : IRequestHandler<Query, Result<LeaveRequestResponse>>
    {
        public async Task<Result<LeaveRequestResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<LeaveRequestResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(LeaveRequest))));
            }
            return Result<LeaveRequestResponse>.Success(LeaveRequestResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "leave-request"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<LeaveRequestResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetLeaveRequestById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

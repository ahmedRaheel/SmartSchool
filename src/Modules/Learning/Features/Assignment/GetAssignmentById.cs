using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Learning.Contracts;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.Modules.Learning.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Learning.Features.Assignment;

public static class GetAssignmentById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<AssignmentResponse>>;

    public sealed class Handler(IAssignmentQuery entityQuery)
        : IRequestHandler<Query, Result<AssignmentResponse>>
    {
        public async Task<Result<AssignmentResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<AssignmentResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Assignment))));
            }
            return Result<AssignmentResponse>.Success(AssignmentResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "assignment"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<AssignmentResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetAssignmentById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

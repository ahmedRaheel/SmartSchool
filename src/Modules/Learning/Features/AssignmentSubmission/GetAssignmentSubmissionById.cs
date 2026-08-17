using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Learning.Contracts;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.Modules.Learning.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Learning.Features.AssignmentSubmission;

public static class GetAssignmentSubmissionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<AssignmentSubmissionResponse>>;

    public sealed class Handler(IAssignmentSubmissionQuery entityQuery)
        : IRequestHandler<Query, Result<AssignmentSubmissionResponse>>
    {
        public async Task<Result<AssignmentSubmissionResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<AssignmentSubmissionResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(AssignmentSubmission))));
            }
            return Result<AssignmentSubmissionResponse>.Success(AssignmentSubmissionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "assignment-submission"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<AssignmentSubmissionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetAssignmentSubmissionById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

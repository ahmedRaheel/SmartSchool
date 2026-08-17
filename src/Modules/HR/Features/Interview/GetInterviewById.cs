using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Contracts;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Interview;

public static class GetInterviewById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<InterviewResponse>>;

    public sealed class Handler(IInterviewQuery entityQuery)
        : IRequestHandler<Query, Result<InterviewResponse>>
    {
        public async Task<Result<InterviewResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<InterviewResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Interview))));
            }
            return Result<InterviewResponse>.Success(InterviewResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "interview"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<InterviewResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetInterviewById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

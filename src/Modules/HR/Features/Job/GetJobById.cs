using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Contracts;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Job;

public static class GetJobById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<JobResponse>>;

    public sealed class Handler(IJobQuery entityQuery)
        : IRequestHandler<Query, Result<JobResponse>>
    {
        public async Task<Result<JobResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<JobResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Job))));
            }
            return Result<JobResponse>.Success(JobResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "job"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<JobResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetJobById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

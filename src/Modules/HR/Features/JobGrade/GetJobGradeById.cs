using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Contracts;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.JobGrade;

public static class GetJobGradeById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<JobGradeResponse>>;

    public sealed class Handler(IJobGradeQuery entityQuery)
        : IRequestHandler<Query, Result<JobGradeResponse>>
    {
        public async Task<Result<JobGradeResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<JobGradeResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(JobGrade))));
            }
            return Result<JobGradeResponse>.Success(JobGradeResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "job-grade"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<JobGradeResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetJobGradeById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

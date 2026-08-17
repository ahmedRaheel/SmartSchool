using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Contracts;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Resume;

public static class GetResumeById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ResumeResponse>>;

    public sealed class Handler(IResumeQuery entityQuery)
        : IRequestHandler<Query, Result<ResumeResponse>>
    {
        public async Task<Result<ResumeResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ResumeResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Resume))));
            }
            return Result<ResumeResponse>.Success(ResumeResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "resume"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ResumeResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetResumeById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

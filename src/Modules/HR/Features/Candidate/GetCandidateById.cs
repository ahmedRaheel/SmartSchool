using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Contracts;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Candidate;

public static class GetCandidateById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<CandidateResponse>>;

    public sealed class Handler(ICandidateQuery entityQuery)
        : IRequestHandler<Query, Result<CandidateResponse>>
    {
        public async Task<Result<CandidateResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<CandidateResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Candidate))));
            }
            return Result<CandidateResponse>.Success(CandidateResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "candidate"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<CandidateResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetCandidateById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

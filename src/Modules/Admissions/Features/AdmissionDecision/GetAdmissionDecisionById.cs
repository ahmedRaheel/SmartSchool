using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Admissions.Contracts;
using SmartSchool.Modules.Admissions.Models;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features.AdmissionDecision;

public static class GetAdmissionDecisionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<AdmissionDecisionResponse>>;

    public sealed class Handler(IAdmissionDecisionQuery entityQuery)
        : IRequestHandler<Query, Result<AdmissionDecisionResponse>>
    {
        public async Task<Result<AdmissionDecisionResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<AdmissionDecisionResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(AdmissionDecision))));
            }
            return Result<AdmissionDecisionResponse>.Success(AdmissionDecisionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "admission-decision"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<AdmissionDecisionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetAdmissionDecisionById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

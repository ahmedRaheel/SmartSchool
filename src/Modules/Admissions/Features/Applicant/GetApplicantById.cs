using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Admissions.Contracts;
using SmartSchool.Modules.Admissions.Models;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features.Applicant;

public static class GetApplicantById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ApplicantResponse>>;

    public sealed class Handler(IApplicantQuery entityQuery)
        : IRequestHandler<Query, Result<ApplicantResponse>>
    {
        public async Task<Result<ApplicantResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ApplicantResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Applicant))));
            }
            return Result<ApplicantResponse>.Success(ApplicantResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "applicant"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ApplicantResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetApplicantById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Finance.Contracts;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.Modules.Finance.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Finance.Features.Scholarship;

public static class GetScholarshipById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ScholarshipResponse>>;

    public sealed class Handler(IScholarshipQuery entityQuery)
        : IRequestHandler<Query, Result<ScholarshipResponse>>
    {
        public async Task<Result<ScholarshipResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ScholarshipResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Scholarship))));
            }
            return Result<ScholarshipResponse>.Success(ScholarshipResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "scholarship"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ScholarshipResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetScholarshipById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

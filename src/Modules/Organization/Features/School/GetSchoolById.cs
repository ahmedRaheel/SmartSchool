using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Contracts;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.School;

public static class GetSchoolById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<SchoolResponse>>;

    public sealed class Handler(ISchoolQuery entityQuery)
        : IRequestHandler<Query, Result<SchoolResponse>>
    {
        public async Task<Result<SchoolResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<SchoolResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(School))));
            }
            return Result<SchoolResponse>.Success(SchoolResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "school"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<SchoolResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetSchoolById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

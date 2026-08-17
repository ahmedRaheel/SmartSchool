using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Documents.Contracts;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Documents.Features.SchoolLogo;

public static class GetSchoolLogoById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<SchoolLogoResponse>>;

    public sealed class Handler(ISchoolLogoQuery entityQuery)
        : IRequestHandler<Query, Result<SchoolLogoResponse>>
    {
        public async Task<Result<SchoolLogoResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<SchoolLogoResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(SchoolLogo))));
            }
            return Result<SchoolLogoResponse>.Success(SchoolLogoResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "school-logo"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<SchoolLogoResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetSchoolLogoById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

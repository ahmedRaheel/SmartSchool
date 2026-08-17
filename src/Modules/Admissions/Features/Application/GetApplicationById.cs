using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Admissions.Contracts;
using SmartSchool.Modules.Admissions.Models;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features.Application;

public static class GetApplicationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ApplicationResponse>>;

    public sealed class Handler(IApplicationQuery entityQuery)
        : IRequestHandler<Query, Result<ApplicationResponse>>
    {
        public async Task<Result<ApplicationResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ApplicationResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Application))));
            }
            return Result<ApplicationResponse>.Success(ApplicationResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "application"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ApplicationResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetApplicationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

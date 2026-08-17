using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Contracts;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.AcademicSystem;

public static class GetAcademicSystemById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<AcademicSystemResponse>>;

    public sealed class Handler(IAcademicSystemQuery entityQuery)
        : IRequestHandler<Query, Result<AcademicSystemResponse>>
    {
        public async Task<Result<AcademicSystemResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<AcademicSystemResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(AcademicSystem))));
            }
            return Result<AcademicSystemResponse>.Success(AcademicSystemResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "academic-system"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<AcademicSystemResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetAcademicSystemById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

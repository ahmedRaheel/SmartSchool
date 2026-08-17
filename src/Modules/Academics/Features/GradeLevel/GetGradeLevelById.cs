using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Contracts;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.GradeLevel;

public static class GetGradeLevelById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<GradeLevelResponse>>;

    public sealed class Handler(IGradeLevelQuery entityQuery)
        : IRequestHandler<Query, Result<GradeLevelResponse>>
    {
        public async Task<Result<GradeLevelResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<GradeLevelResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(GradeLevel))));
            }
            return Result<GradeLevelResponse>.Success(GradeLevelResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "grade-level"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<GradeLevelResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetGradeLevelById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

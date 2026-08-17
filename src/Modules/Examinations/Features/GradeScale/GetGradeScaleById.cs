using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Examinations.Contracts;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.Modules.Examinations.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Examinations.Features.GradeScale;

public static class GetGradeScaleById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<GradeScaleResponse>>;

    public sealed class Handler(IGradeScaleQuery entityQuery)
        : IRequestHandler<Query, Result<GradeScaleResponse>>
    {
        public async Task<Result<GradeScaleResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<GradeScaleResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(GradeScale))));
            }
            return Result<GradeScaleResponse>.Success(GradeScaleResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "grade-scale"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<GradeScaleResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetGradeScaleById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

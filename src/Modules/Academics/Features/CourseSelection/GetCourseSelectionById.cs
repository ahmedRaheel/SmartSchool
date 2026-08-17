using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Contracts;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.CourseSelection;

public static class GetCourseSelectionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<CourseSelectionResponse>>;

    public sealed class Handler(ICourseSelectionQuery entityQuery)
        : IRequestHandler<Query, Result<CourseSelectionResponse>>
    {
        public async Task<Result<CourseSelectionResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<CourseSelectionResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(CourseSelection))));
            }
            return Result<CourseSelectionResponse>.Success(CourseSelectionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "course-selection"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<CourseSelectionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetCourseSelectionById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

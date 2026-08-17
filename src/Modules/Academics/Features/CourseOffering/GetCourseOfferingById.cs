using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Contracts;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.CourseOffering;

public static class GetCourseOfferingById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<CourseOfferingResponse>>;

    public sealed class Handler(ICourseOfferingQuery entityQuery)
        : IRequestHandler<Query, Result<CourseOfferingResponse>>
    {
        public async Task<Result<CourseOfferingResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<CourseOfferingResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(CourseOffering))));
            }
            return Result<CourseOfferingResponse>.Success(CourseOfferingResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "course-offering"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<CourseOfferingResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetCourseOfferingById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

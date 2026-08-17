using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Learning.Contracts;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.Modules.Learning.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Learning.Features.Lesson;

public static class GetLessonById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<LessonResponse>>;

    public sealed class Handler(ILessonQuery entityQuery)
        : IRequestHandler<Query, Result<LessonResponse>>
    {
        public async Task<Result<LessonResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<LessonResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Lesson))));
            }
            return Result<LessonResponse>.Success(LessonResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "lesson"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<LessonResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetLessonById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

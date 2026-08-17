using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Contracts;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.Modules.AITutor.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AITutor.Features.StudentTopicMastery;

public static class GetStudentTopicMasteryById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<StudentTopicMasteryResponse>>;

    public sealed class Handler(IStudentTopicMasteryQuery entityQuery)
        : IRequestHandler<Query, Result<StudentTopicMasteryResponse>>
    {
        public async Task<Result<StudentTopicMasteryResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<StudentTopicMasteryResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(StudentTopicMastery))));
            }
            return Result<StudentTopicMasteryResponse>.Success(StudentTopicMasteryResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student-topic-mastery"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<StudentTopicMasteryResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentTopicMasteryById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

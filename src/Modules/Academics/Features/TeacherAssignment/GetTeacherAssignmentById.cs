using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Contracts;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.TeacherAssignment;

public static class GetTeacherAssignmentById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<TeacherAssignmentResponse>>;

    public sealed class Handler(ITeacherAssignmentQuery entityQuery)
        : IRequestHandler<Query, Result<TeacherAssignmentResponse>>
    {
        public async Task<Result<TeacherAssignmentResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<TeacherAssignmentResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(TeacherAssignment))));
            }
            return Result<TeacherAssignmentResponse>.Success(TeacherAssignmentResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "teacher-assignment"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<TeacherAssignmentResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetTeacherAssignmentById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

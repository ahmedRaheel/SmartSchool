using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Contracts;
using SmartSchool.Modules.Students.Models;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.StudentGuardian;

public static class GetStudentGuardianById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<StudentGuardianResponse>>;

    public sealed class Handler(IStudentGuardianQuery entityQuery)
        : IRequestHandler<Query, Result<StudentGuardianResponse>>
    {
        public async Task<Result<StudentGuardianResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<StudentGuardianResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(StudentGuardian))));
            }
            return Result<StudentGuardianResponse>.Success(StudentGuardianResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student-guardian"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<StudentGuardianResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentGuardianById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

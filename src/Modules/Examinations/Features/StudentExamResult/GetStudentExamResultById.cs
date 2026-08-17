using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Examinations.Contracts;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.Modules.Examinations.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Examinations.Features.StudentExamResult;

public static class GetStudentExamResultById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<StudentExamResultResponse>>;

    public sealed class Handler(IStudentExamResultQuery entityQuery)
        : IRequestHandler<Query, Result<StudentExamResultResponse>>
    {
        public async Task<Result<StudentExamResultResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<StudentExamResultResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(StudentExamResult))));
            }
            return Result<StudentExamResultResponse>.Success(StudentExamResultResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student-exam-result"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<StudentExamResultResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentExamResultById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}

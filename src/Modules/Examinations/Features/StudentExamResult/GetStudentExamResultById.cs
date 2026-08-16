using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Features.StudentExamResult;

public static class GetStudentExamResultById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<StudentExamResult> repository)
    {
        public async Task<Result<StudentExamResult>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<StudentExamResult>.Failure(
                    Error.NotFound("StudentExamResult was not found."));
            }

            return Result<StudentExamResult>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/examinations/student-exam-result/{id:guid}",
                async (
                    Guid id,
                    Guid tenantId,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var query = new Query(tenantId, id);

                    var result = await handler.HandleAsync(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetStudentExamResultById")
            .WithTags("Examinations")
            .RequireAuthorization();

        return endpoints;
    }
}

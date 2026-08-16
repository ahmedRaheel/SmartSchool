using SmartSchool.Application.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Features.StudentGuardian;

public static class GetStudentGuardianPage
{
    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25);

    public sealed class Handler(
        IRepository<StudentGuardian> repository)
    {
        public async Task<Result<PagedResult<StudentGuardian>>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(
                query.Page,
                query.PageSize);

            var result = await repository.GetPageAsync(
                query.TenantId,
                pageRequest.NormalizedPage,
                pageRequest.NormalizedPageSize,
                cancellationToken);

            return Result<PagedResult<StudentGuardian>>.Success(result);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/students/student-guardian",
                async (
                    Guid tenantId,
                    int page,
                    int pageSize,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var query = new Query(
                        tenantId,
                        page,
                        pageSize);

                    var result = await handler.HandleAsync(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetStudentGuardianPage")
            .WithTags("Students")
            .RequireAuthorization();

        return endpoints;
    }
}

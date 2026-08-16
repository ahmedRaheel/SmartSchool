using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Features.Attendance;

public static class GetAttendanceById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<Attendance> repository)
    {
        public async Task<Result<Attendance>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<Attendance>.Failure(
                    Error.NotFound("Attendance was not found."));
            }

            return Result<Attendance>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/students/attendance/{id:guid}",
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
            .WithName("GetAttendanceById")
            .WithTags("Students")
            .RequireAuthorization();

        return endpoints;
    }
}

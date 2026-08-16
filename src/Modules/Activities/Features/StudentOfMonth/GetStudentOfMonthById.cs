using SmartSchool.Modules.Activities;
using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.StudentOfMonth;

public static class GetStudentOfMonthById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IStudentOfMonthQuery query)
    {
        public async Task<Result<StudentOfMonth>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<StudentOfMonth>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(StudentOfMonth))));
            }

            return Result<StudentOfMonth>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/activities/student-of-month/{id:guid}",
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
            .WithName("GetStudentOfMonthById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}

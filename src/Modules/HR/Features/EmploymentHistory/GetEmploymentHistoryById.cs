using SmartSchool.Modules.HR;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.EmploymentHistory;

public static class GetEmploymentHistoryById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IEmploymentHistoryQuery query)
    {
        public async Task<Result<EmploymentHistory>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<EmploymentHistory>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(EmploymentHistory))));
            }

            return Result<EmploymentHistory>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/hr/employment-history/{id:guid}",
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
            .WithName("GetEmploymentHistoryById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}

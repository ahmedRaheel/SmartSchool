using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Payroll.Features.EmployeeCompensation;

public static class GetEmployeeCompensationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<EmployeeCompensation> repository)
    {
        public async Task<Result<EmployeeCompensation>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<EmployeeCompensation>.Failure(
                    Error.NotFound("EmployeeCompensation was not found."));
            }

            return Result<EmployeeCompensation>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/payroll/employee-compensation/{id:guid}",
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
            .WithName("GetEmployeeCompensationById")
            .WithTags("Payroll")
            .RequireAuthorization();

        return endpoints;
    }
}

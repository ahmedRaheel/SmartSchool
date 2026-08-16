using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Payroll.Features.Payslip;

public static class GetPayslipById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<Payslip> repository)
    {
        public async Task<Result<Payslip>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<Payslip>.Failure(
                    Error.NotFound("Payslip was not found."));
            }

            return Result<Payslip>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/payroll/payslip/{id:guid}",
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
            .WithName("GetPayslipById")
            .WithTags("Payroll")
            .RequireAuthorization();

        return endpoints;
    }
}

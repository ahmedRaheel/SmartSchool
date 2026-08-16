using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Payroll.Features.SalaryStructure;

public static class GetSalaryStructureById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<SalaryStructure> repository)
    {
        public async Task<Result<SalaryStructure>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<SalaryStructure>.Failure(
                    Error.NotFound("SalaryStructure was not found."));
            }

            return Result<SalaryStructure>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/payroll/salary-structure/{id:guid}",
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
            .WithName("GetSalaryStructureById")
            .WithTags("Payroll")
            .RequireAuthorization();

        return endpoints;
    }
}

using SmartSchool.Modules.Payroll;
using SmartSchool.Modules.Payroll.Persistence;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Payroll.Features.EmployeeCompensation;

public static class DeleteEmployeeCompensation
{
    public sealed record Command(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IEmployeeCompensationQuery query,
        IEmployeeCompensationCommand command)
    {
        public async Task<Result<bool>> HandleAsync(
            Command command,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                command.TenantId,
                command.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<bool>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(EmployeeCompensation))));
            }

            await command.DeleteAsync(
                entity,
                cancellationToken);

            return Result<bool>.Success(true);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/api/payroll/employee-compensation/{id:guid}",
                async (
                    Guid id,
                    Guid tenantId,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var command = new Command(
                        tenantId,
                        id);

                    var result = await handler.HandleAsync(
                        command,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("DeleteEmployeeCompensation")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}

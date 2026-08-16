using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Audit.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Audit.Features.AuditLog;

public static class DeleteAuditLog
{
    public sealed record Command(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<AuditLog> repository)
    {
        public async Task<Result<bool>> HandleAsync(
            Command command,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                command.TenantId,
                command.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<bool>.Failure(
                    Error.NotFound("AuditLog was not found."));
            }

            repository.Remove(entity);

            await repository.SaveChangesAsync(
                cancellationToken);

            return Result<bool>.Success(true);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/api/audit/audit-log/{id:guid}",
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
            .WithName("DeleteAuditLog")
            .WithTags("Audit")
            .RequireAuthorization();

        return endpoints;
    }
}

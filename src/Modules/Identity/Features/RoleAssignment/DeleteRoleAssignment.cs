using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Identity.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Identity.Features.RoleAssignment;

public static class DeleteRoleAssignment
{
    public sealed record Command(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<RoleAssignment> repository)
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
                    Error.NotFound("RoleAssignment was not found."));
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
                "/api/identity/role-assignment/{id:guid}",
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
            .WithName("DeleteRoleAssignment")
            .WithTags("Identity")
            .RequireAuthorization();

        return endpoints;
    }
}

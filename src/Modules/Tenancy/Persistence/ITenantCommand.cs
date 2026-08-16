using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Persistence;

public interface ITenantCommand
{
    Task AddAsync(
        Tenant entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Tenant entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Tenant entity,
        CancellationToken cancellationToken);
}

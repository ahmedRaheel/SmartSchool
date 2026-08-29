using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Tenancy.Models;
namespace SmartSchool.Modules.Tenancy.Persistence;
public sealed class TenantContactCommand(IApplicationDbContext dbContext) : ITenantContactCommand
{
    public async Task AddAsync(TenantContactEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.Set<TenantContactEntity>().AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

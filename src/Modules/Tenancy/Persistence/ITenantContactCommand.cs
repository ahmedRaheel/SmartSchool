using SmartSchool.Modules.Tenancy.Models;
namespace SmartSchool.Modules.Tenancy.Persistence;
public interface ITenantContactCommand { Task AddAsync(TenantContactEntity entity, CancellationToken cancellationToken); }

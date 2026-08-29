using SmartSchool.Modules.Tenancy.Models;
namespace SmartSchool.Modules.Tenancy.Features.DataAccess.TenantContact;
public interface ITenantContactCommand { Task AddAsync(TenantContactEntity entity, CancellationToken cancellationToken); }

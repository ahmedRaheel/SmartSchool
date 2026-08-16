using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Persistence;

public interface ICampusBrandingCommand
{
    Task AddAsync(
        CampusBranding entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        CampusBranding entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        CampusBranding entity,
        CancellationToken cancellationToken);
}

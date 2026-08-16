using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

public interface IModelConfigurationCommand
{
    Task AddAsync(
        ModelConfiguration entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        ModelConfiguration entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        ModelConfiguration entity,
        CancellationToken cancellationToken);
}

using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

public interface IAiExecutionLogCommand
{
    Task AddAsync(
        AiExecutionLog entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        AiExecutionLog entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        AiExecutionLog entity,
        CancellationToken cancellationToken);
}

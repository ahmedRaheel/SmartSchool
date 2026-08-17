using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Write-side persistence for AiExecutionLogEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AiExecutionLogCommand : IAiExecutionLogCommand
{
    public Task AddAsync(
        AiExecutionLogEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AiExecutionLogEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        AiExecutionLogEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AiExecutionLogEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        AiExecutionLogEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AiExecutionLogEntity delete persistence has not been connected to the module DbContext.");
    }
}

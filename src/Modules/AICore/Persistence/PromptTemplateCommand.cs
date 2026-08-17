using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Write-side persistence for PromptTemplateEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PromptTemplateCommand : IPromptTemplateCommand
{
    public Task AddAsync(
        PromptTemplateEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PromptTemplateEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        PromptTemplateEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PromptTemplateEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        PromptTemplateEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PromptTemplateEntity delete persistence has not been connected to the module DbContext.");
    }
}

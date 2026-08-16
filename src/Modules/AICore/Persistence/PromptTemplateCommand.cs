using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Write-side persistence for PromptTemplate.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PromptTemplateCommand : IPromptTemplateCommand
{
    public Task AddAsync(
        PromptTemplate entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PromptTemplate create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        PromptTemplate entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PromptTemplate update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        PromptTemplate entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PromptTemplate delete persistence has not been connected to the module DbContext.");
    }
}

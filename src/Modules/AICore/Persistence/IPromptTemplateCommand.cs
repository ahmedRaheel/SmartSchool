using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

public interface IPromptTemplateCommand
{
    Task AddAsync(
        PromptTemplate entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        PromptTemplate entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        PromptTemplate entity,
        CancellationToken cancellationToken);
}

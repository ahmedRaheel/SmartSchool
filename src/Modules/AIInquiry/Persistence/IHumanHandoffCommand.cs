using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

public interface IHumanHandoffCommand
{
    Task AddAsync(
        HumanHandoff entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        HumanHandoff entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        HumanHandoff entity,
        CancellationToken cancellationToken);
}

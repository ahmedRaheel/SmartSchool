using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

public interface IMessageReceiptCommand
{
    Task AddAsync(
        MessageReceipt entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        MessageReceipt entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        MessageReceipt entity,
        CancellationToken cancellationToken);
}

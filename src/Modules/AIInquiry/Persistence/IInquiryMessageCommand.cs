using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

public interface IInquiryMessageCommand
{
    Task AddAsync(
        InquiryMessage entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        InquiryMessage entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        InquiryMessage entity,
        CancellationToken cancellationToken);
}

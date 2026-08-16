using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

public interface IInquiryConversationCommand
{
    Task AddAsync(
        InquiryConversation entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        InquiryConversation entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        InquiryConversation entity,
        CancellationToken cancellationToken);
}

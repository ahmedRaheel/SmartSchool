using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIInquiry.Persistence;

public interface IInquiryMessageQuery
{
    Task<InquiryMessage?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<InquiryMessage>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken);
}

using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

public interface IInquiryCommand
{
    Task AddAsync(
        Inquiry entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Inquiry entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Inquiry entity,
        CancellationToken cancellationToken);
}

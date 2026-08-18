using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// EF-backed write persistence for InquiryEntity.
/// </summary>
public sealed class InquiryCommand(IEfMockStore store) : IInquiryCommand
{
	public Task AddAsync(InquiryEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(InquiryEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(InquiryEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}

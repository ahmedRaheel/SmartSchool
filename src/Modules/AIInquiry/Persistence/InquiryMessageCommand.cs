using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Executes database writes for <see cref="InquiryMessageEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class InquiryMessageCommand(IApplicationDbContext dbContext) : IInquiryMessageCommand
{
	public async Task AddAsync(
		InquiryMessageEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<InquiryMessageEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		InquiryMessageEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<InquiryMessageEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		InquiryMessageEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<InquiryMessageEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}

using SmartSchool.Modules.AIInquiry.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Executes database writes for <see cref="InquiryMessageEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class InquiryMessageCommand(IAIInquiryDbContext dbContext) : IInquiryMessageCommand
{
	public async Task AddAsync(
		InquiryMessageEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.InquiryMessages
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		InquiryMessageEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.InquiryMessages
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		InquiryMessageEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.InquiryMessages
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}

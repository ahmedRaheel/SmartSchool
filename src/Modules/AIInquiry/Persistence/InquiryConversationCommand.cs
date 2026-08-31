using SmartSchool.Modules.AIInquiry.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Executes database writes for <see cref="InquiryConversationEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class InquiryConversationCommand(IAIInquiryDbContext dbContext) : IInquiryConversationCommand
{
	public async Task AddAsync(
		InquiryConversationEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.InquiryConversations
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		InquiryConversationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.InquiryConversations
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		InquiryConversationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.InquiryConversations
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}

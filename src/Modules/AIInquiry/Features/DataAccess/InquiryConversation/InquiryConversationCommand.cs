using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;

using SmartSchool.Modules.AIInquiry.Features.InquiryConversation;

namespace SmartSchool.Modules.AIInquiry.Features.DataAccess.EnquiryConversation;

/// <summary>
/// Executes database writes for <see cref="InquiryConversationEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class InquiryConversationCommand(IApplicationDbContext dbContext) : IInquiryConversationCommand
{
	public async Task AddAsync(
		InquiryConversationEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<InquiryConversationEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		InquiryConversationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<InquiryConversationEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		InquiryConversationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<InquiryConversationEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}

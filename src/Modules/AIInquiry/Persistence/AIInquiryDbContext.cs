using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

public interface IAIInquiryDbContext
{
	DatabaseFacade Database { get; }

	DbSet<HumanHandoffEntity> HumanHandoffs { get; }
	DbSet<InquiryConversationEntity> InquiryConversations { get; }
	DbSet<InquiryMessageEntity> InquiryMessages { get; }
	DbSet<LeadCaptureEntity> LeadCaptures { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class AIInquiryDbContext(IApplicationDbContext dbContext) : IAIInquiryDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<HumanHandoffEntity> HumanHandoffs => dbContext.Set<HumanHandoffEntity>();
	public DbSet<InquiryConversationEntity> InquiryConversations => dbContext.Set<InquiryConversationEntity>();
	public DbSet<InquiryMessageEntity> InquiryMessages => dbContext.Set<InquiryMessageEntity>();
	public DbSet<LeadCaptureEntity> LeadCaptures => dbContext.Set<LeadCaptureEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}

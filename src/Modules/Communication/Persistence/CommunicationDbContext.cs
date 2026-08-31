using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

public interface ICommunicationDbContext
{
	DatabaseFacade Database { get; }

	DbSet<ChatAttachmentEntity> ChatAttachments { get; }
	DbSet<ChatConversationEntity> ChatConversations { get; }
	DbSet<ChatMessageEntity> ChatMessages { get; }
	DbSet<ChatParticipantEntity> ChatParticipants { get; }
	DbSet<ConversationEntity> Conversations { get; }
	DbSet<ConversationParticipantEntity> ConversationParticipants { get; }
	DbSet<MessageEntity> Messages { get; }
	DbSet<MessageReceiptEntity> MessageReceipts { get; }
	DbSet<NotificationEntity> Notifications { get; }
	DbSet<NotificationPreferenceEntity> NotificationPreferences { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class CommunicationDbContext(IApplicationDbContext dbContext) : ICommunicationDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<ChatAttachmentEntity> ChatAttachments => dbContext.Set<ChatAttachmentEntity>();
	public DbSet<ChatConversationEntity> ChatConversations => dbContext.Set<ChatConversationEntity>();
	public DbSet<ChatMessageEntity> ChatMessages => dbContext.Set<ChatMessageEntity>();
	public DbSet<ChatParticipantEntity> ChatParticipants => dbContext.Set<ChatParticipantEntity>();
	public DbSet<ConversationEntity> Conversations => dbContext.Set<ConversationEntity>();
	public DbSet<ConversationParticipantEntity> ConversationParticipants => dbContext.Set<ConversationParticipantEntity>();
	public DbSet<MessageEntity> Messages => dbContext.Set<MessageEntity>();
	public DbSet<MessageReceiptEntity> MessageReceipts => dbContext.Set<MessageReceiptEntity>();
	public DbSet<NotificationEntity> Notifications => dbContext.Set<NotificationEntity>();
	public DbSet<NotificationPreferenceEntity> NotificationPreferences => dbContext.Set<NotificationPreferenceEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
/// EF Core unit-of-work owned by the Communication module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class CommunicationDbContext(DbContextOptions<CommunicationDbContext> options)
	: DbContext(options), ICommunicationDbContext
{
	public DbSet<ChatAttachmentEntity> ChatAttachments => Set<ChatAttachmentEntity>();
	public DbSet<ChatConversationEntity> ChatConversations => Set<ChatConversationEntity>();
	public DbSet<ChatMessageEntity> ChatMessages => Set<ChatMessageEntity>();
	public DbSet<ChatParticipantEntity> ChatParticipants => Set<ChatParticipantEntity>();
	public DbSet<ConversationEntity> Conversations => Set<ConversationEntity>();
	public DbSet<ConversationParticipantEntity> ConversationParticipants => Set<ConversationParticipantEntity>();
	public DbSet<MessageEntity> Messages => Set<MessageEntity>();
	public DbSet<MessageReceiptEntity> MessageReceipts => Set<MessageReceiptEntity>();
	public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();
	public DbSet<NotificationPreferenceEntity> NotificationPreferences => Set<NotificationPreferenceEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(CommunicationDbContext).Assembly,
			type => type.Namespace is not null
				&& type.Namespace.StartsWith("SmartSchool.Modules.Communication.Persistence.Configurations", StringComparison.Ordinal));
	}
}

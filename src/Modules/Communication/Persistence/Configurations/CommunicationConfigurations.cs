using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence.Configurations;

/// <summary>Conversation mapping.</summary>
public sealed class ChatConversationConfiguration : IEntityTypeConfiguration<ChatConversationEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ChatConversationEntity> builder)
    {
        builder.ToTable("ChatConversations", "Communication");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.ConversationType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.RelatedEntityType).HasMaxLength(100);
        builder.HasIndex(x => new { x.TenantId, x.CreatedByUserId });
    }
}

/// <summary>Participant mapping.</summary>
public sealed class ChatParticipantConfiguration : IEntityTypeConfiguration<ChatParticipantEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ChatParticipantEntity> builder)
    {
        builder.ToTable("ChatParticipants", "Communication");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Role).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.ConversationId, x.UserId }).IsUnique();
    }
}

/// <summary>Message mapping.</summary>
public sealed class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessageEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ChatMessageEntity> builder)
    {
        builder.ToTable("ChatMessages", "Communication");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.MessageType).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(4000).IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.ConversationId, x.SentAt });
    }
}

/// <summary>Attachment mapping.</summary>
public sealed class ChatAttachmentConfiguration : IEntityTypeConfiguration<ChatAttachmentEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ChatAttachmentEntity> builder)
    {
        builder.ToTable("ChatAttachments", "Communication");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FileName).HasMaxLength(255).IsRequired();
        builder.Property(x => x.ContentType).HasMaxLength(150).IsRequired();
        builder.Property(x => x.StorageKey).HasMaxLength(500).IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.MessageId });
    }
}

/// <summary>Notification mapping.</summary>
public sealed class NotificationConfiguration : IEntityTypeConfiguration<NotificationEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<NotificationEntity> builder)
    {
        builder.ToTable("Notifications", "Communication");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(80);
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.RelatedEntityType).HasMaxLength(100);
        builder.Property(x => x.ActionUrl).HasMaxLength(500);
        builder.Property(x => x.Priority).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.RecipientUserId, x.IsRead, x.OccurredAt });
    }
}

/// <summary>Notification preference mapping.</summary>
public sealed class NotificationPreferenceConfiguration : IEntityTypeConfiguration<NotificationPreferenceEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<NotificationPreferenceEntity> builder)
    {
        builder.ToTable("NotificationPreferences", "Communication");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.NotificationType).HasConversion<string>().HasMaxLength(80);
        builder.HasIndex(x => new { x.TenantId, x.UserId, x.NotificationType }).IsUnique();
    }
}

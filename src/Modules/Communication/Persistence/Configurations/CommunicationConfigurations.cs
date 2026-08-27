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
        builder.HasKey(x => x.ChatConversationId);
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.ConversationType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.RelatedEntityType).HasMaxLength(100);
        builder.HasIndex(x => new { x.TenantId, x.CreatedByUserId });
    }
}

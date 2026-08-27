using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence.Configurations;

/// <summary>Message mapping.</summary>
public sealed class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessageEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ChatMessageEntity> builder)
    {
        builder.ToTable("ChatMessages", "Communication");
        builder.HasKey(x => x.ChatMessageId);
        builder.Property(x => x.MessageType).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(4000).IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.ConversationId, x.SentAt });
    }
}
